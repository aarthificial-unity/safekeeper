using Aarthificial.Safekeeper.Attributes;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aarthificial.Safekeeper.Editor {
  public class SaveLocationModificationProcessor : AssetModificationProcessor {
    private static string[] OnWillSaveAssets(string[] paths) {
      foreach (var path in paths) {
        switch (Path.GetExtension(path)) {
          case ".prefab":
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.assetPath == path) {
              ProcessStage(prefabStage);
            }
            break;
          case ".unity":
            ProcessStage(StageUtility.GetMainStage());
            break;
          case ".asset":
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (var asset in assets) {
              if (asset != null) {
                ProcessObject(asset);
              }
            }
            break;
        }
      }

      return paths;
    }

    private static void ProcessStage(Stage stage) {
      var components = stage.FindComponentsOfType<MonoBehaviour>();
      foreach (var component in components) {
        ProcessObject(component);
      }
    }

    private static void ProcessObject(Object component) {
      var fields = component.GetType()
        .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
      foreach (var fieldInfo in fields) {
        if (fieldInfo.IsStatic || fieldInfo.IsInitOnly || fieldInfo.IsLiteral) {
          continue;
        }

        if (Attribute.GetCustomAttribute(
            fieldInfo,
            typeof(ObjectLocationAttribute),
            false
          ) is ObjectLocationAttribute objectLocation) {
          var serializedObject = new SerializedObject(component);
          ObjectLocationAttributePropertyDrawer.ApplyLocation(
            objectLocation,
            serializedObject.FindProperty(fieldInfo.Name)
          );
          serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        if (Attribute.GetCustomAttribute(
            fieldInfo,
            typeof(AssetLocationAttribute),
            false
          ) is AssetLocationAttribute assetLocation) {
          var serializedObject = new SerializedObject(component);
          AssetLocationAttributePropertyDrawer.ApplyLocation(
            assetLocation,
            serializedObject.FindProperty(fieldInfo.Name)
          );
          serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
      }
    }
  }
}
