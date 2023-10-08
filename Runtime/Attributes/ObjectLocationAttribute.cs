using UnityEngine;

namespace Aarthificial.Safekeeper.Attributes {
  /// <summary>
  /// Generates a <see cref="SaveLocation"/> for the <see cref="MonoBehaviour"/>
  /// that owns this property.
  /// </summary>
  ///
  /// <remarks>
  /// The GUID of the scene that this object belongs to will be used as the
  /// chunk ID. The object ID will be extracted from <see cref="UnityEditor.GlobalObjectId"/>.
  /// This ensures that the location is unique to the object.
  /// 
  /// If <see cref="IsPrefab"/> is true, the chunk ID will be set to "prefabs"
  /// and the prefab ID will be used as the object ID.
  /// In this case, the location will be shared among all instances of the prefab.
  /// </remarks>
  ///
  /// <example>
  /// <code>
  /// public class MyComponent : MonoBehaviour { 
  ///   [ObjectLocation]
  ///   public SaveLocation Location;
  /// }
  /// </code> 
  /// </example>
  public class ObjectLocationAttribute : PropertyAttribute {
    public readonly bool IsPrefab;

    public ObjectLocationAttribute(bool isPrefab = false) {
      IsPrefab = isPrefab;
    }
  }
}
