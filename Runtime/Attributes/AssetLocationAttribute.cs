using UnityEngine;

namespace Aarthificial.Safekeeper.Attributes {
  /// <summary>
  /// Generates a <see cref="SaveLocation"/> for the ScriptableObject that owns this property.
  /// </summary>
  ///
  /// <remarks>
  /// The GUID of the ScriptableObject will be used as the object ID.
  /// The chunk ID can be set in the attribute's constructor and defaults to "assets".
  /// </remarks>
  ///
  /// <example>
  /// <code>
  /// public class MyScriptableObject : ScriptableObject {
  ///   [AssetLocation("my-assets")]
  ///   public SaveLocation Location;
  /// }
  /// </code> 
  /// </example>
  public class AssetLocationAttribute : PropertyAttribute {
    public readonly string ChunkId;

    public AssetLocationAttribute(string chunkId = "assets") {
      ChunkId = chunkId;
    }
  }
}
