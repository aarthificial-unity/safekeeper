using Aarthificial.Safekeeper.Attributes;
using UnityEditor;
using UnityEngine;

namespace Aarthificial.Safekeeper.Editor {
  [CustomPropertyDrawer(typeof(ObjectLocationAttribute))]
  public class ObjectLocationAttributePropertyDrawer : PropertyDrawer {
    public override float GetPropertyHeight(
      SerializedProperty property,
      GUIContent label
    ) {
      return EditorGUI.GetPropertyHeight(property, label);
    }

    public override void OnGUI(
      Rect position,
      SerializedProperty property,
      GUIContent label
    ) {
      if (attribute is not ObjectLocationAttribute objectLocation) {
        return;
      }
      ApplyLocation(objectLocation, property);

      EditorGUI.BeginDisabledGroup(true);
      EditorGUI.PropertyField(position, property, label, true);
      EditorGUI.EndDisabledGroup();
    }

    public static void ApplyLocation(
      ObjectLocationAttribute attribute,
      SerializedProperty property
    ) {
      var globalId =
        GlobalObjectId.GetGlobalObjectIdSlow(
          property.serializedObject.targetObject
        );
      var chunkId = property.FindPropertyRelative(nameof(SaveLocation.ChunkId));
      var objectId =
        property.FindPropertyRelative(nameof(SaveLocation.ObjectId));

      if (attribute.IsPrefab) {
        chunkId.stringValue = "prefabs";
        objectId.stringValue = globalId.targetObjectId.ToString();
      } else {
        chunkId.stringValue = globalId.assetGUID.ToString();
        objectId.stringValue =
          $"{globalId.targetObjectId}-{globalId.targetPrefabId}";
      }
    }
  }
}
