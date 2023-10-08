using Aarthificial.Safekeeper.Attributes;
using UnityEditor;
using UnityEngine;

namespace Aarthificial.Safekeeper.Editor {
  [CustomPropertyDrawer(typeof(AssetLocationAttribute))]
  public class AssetLocationAttributePropertyDrawer : PropertyDrawer {
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
      if (attribute is not AssetLocationAttribute assetLocation) {
        return;
      }
      ApplyLocation(assetLocation, property);

      EditorGUI.BeginDisabledGroup(true);
      EditorGUI.PropertyField(position, property, label, true);
      EditorGUI.EndDisabledGroup();
    }

    public static void ApplyLocation(
      AssetLocationAttribute attribute,
      SerializedProperty property
    ) {
      var globalId =
        GlobalObjectId.GetGlobalObjectIdSlow(
          property.serializedObject.targetObject
        );
      var chunkId = property.FindPropertyRelative(nameof(SaveLocation.ChunkId));
      var objectId =
        property.FindPropertyRelative(nameof(SaveLocation.ObjectId));

      chunkId.stringValue = attribute.ChunkId;
      objectId.stringValue = globalId.assetGUID.ToString();
    }
  }
}
