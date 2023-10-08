using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Aarthificial.Safekeeper.Editor {
  public class SafekeeperEditor : EditorWindow {
    private static readonly Dictionary<string, string> _formattedData = new();
    private static readonly Dictionary<string, string> _chunkNames = new();

    private GUIStyle _listItemStyle;
    private GUIStyle _headerStyle;

    private int _currentIndex;
    private string _currentChunk;
    private string _currentLocation;

    private Vector2 _chunkScrollPos;
    private Vector2 _locationScrollPos;
    private Vector2 _dataScrollPos;

    [MenuItem("Tools/Safekeeper")]
    private static void ShowWindow() {
      var window = GetWindow<SafekeeperEditor>();
      window.Show();
    }

    private void OnEnable() {
      titleContent = new GUIContent {
        text = "Safekeeper",
        image = Resources.Load<Texture2D>("Textures/SafekeeperWindow"),
      };

      Repaint();
    }

    private SaveData _snapshot;

    private void OnGUI() {
      _listItemStyle ??= new GUIStyle(GUI.skin.button) {
        margin = new RectOffset(0, 0, 0, 0),
        alignment = TextAnchor.MiddleLeft,
        stretchWidth = true,
      };

      _headerStyle ??= new GUIStyle(GUI.skin.label) {
        fixedHeight = 32,
      };

      var saves = SaveControllerBase.ExistingSaves.Select(
          wr => wr.TryGetTarget(out var save) ? save : null
        )
        .Where(save => save != null)
        .ToArray();

      if (saves.Length == 0) {
        GUILayout.Box(
          "No existing save controllers found.\nOnce created, they'll appear in this window.",
          new GUIStyle(GUI.skin.label) {
            alignment = TextAnchor.MiddleCenter,
            stretchHeight = true,
          }
        );
        return;
      }

      EditorGUI.BeginChangeCheck();
      EditorGUILayout.Space();
      _currentIndex = EditorGUILayout.Popup(
        "Save Controller",
        _currentIndex,
        saves.Select(save => save.Name).ToArray()
      );
      EditorGUILayout.Space();
      if (EditorGUI.EndChangeCheck()) {
        _currentChunk = null;
        _currentLocation = null;
      }

      LayoutGUI(saves[Mathf.Clamp(_currentIndex, 0, saves.Length - 1)].Data);
    }

    private void LayoutGUI(ISaveData data) {
      if (Screen.width < Screen.height) {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.5f));
        ChunksGUI(data);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        LocationsGUI(data);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Height(Screen.height * 0.5f));
        DataGUI(data);
        EditorGUILayout.EndVertical();
      } else {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.3f));
        ChunksGUI(data);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.3f));
        LocationsGUI(data);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        DataGUI(data);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
      }
    }

    private void ChunksGUI(ISaveData data) {
      Header("Chunks");
      _chunkScrollPos = EditorGUILayout.BeginScrollView(_chunkScrollPos);

      foreach (var chunkId in data.GetChunkIds()) {
        _currentChunk ??= chunkId;

        if (!_chunkNames.TryGetValue(chunkId, out var chunkName)) {
          chunkName = AssetDatabase.GUIDToAssetPath(chunkId);
          if (string.IsNullOrEmpty(chunkName)) {
            chunkName = chunkId;
          }

          _chunkNames.Add(chunkId, chunkName);
        }

        if (ListItem(chunkId, chunkName, _currentChunk)) {
          _currentChunk = chunkId;
          _currentLocation = null;
        }
      }

      EditorGUILayout.EndScrollView();
    }

    private void LocationsGUI(ISaveData data) {
      Header("Locations");
      _locationScrollPos = EditorGUILayout.BeginScrollView(_locationScrollPos);

      if (!string.IsNullOrEmpty(_currentChunk)) {
        var chunk = data.GetChunk(_currentChunk);
        foreach (var location in chunk.Keys) {
          _currentLocation ??= location;
          if (ListItem(location, location, _currentLocation)) {
            _currentLocation = location;
          }
        }
      }

      EditorGUILayout.EndScrollView();
    }

    private void DataGUI(ISaveData data) {
      Header("Data");
      _dataScrollPos = EditorGUILayout.BeginScrollView(_dataScrollPos);

      if (!string.IsNullOrEmpty(_currentChunk) && _currentLocation != null) {
        var chunk = data.GetChunk(_currentChunk);
        if (chunk.TryGetValue(_currentLocation, out var value)) {
          if (!_formattedData.TryGetValue(value, out var formatted)) {
#if UNITY_NEWTONSOFT_JSON
            try {
              formatted = Newtonsoft.Json.Linq.JToken.Parse(value)
                .ToString(Newtonsoft.Json.Formatting.Indented);
              _formattedData.Add(value, formatted);
            } catch {
              formatted = value;
            }
#else
            formatted = value;
#endif
          }
          EditorGUILayout.TextArea(formatted, GUILayout.ExpandHeight(true));
        }
      }

      EditorGUILayout.EndScrollView();
    }

    private void Header(string label) {
      GUILayout.Box(label, _headerStyle);
    }

    private bool ListItem<T>(T id, string label, T activeId) {
      var clicked = GUILayout.Button(label, _listItemStyle);

      if (id.Equals(activeId)) {
        EditorGUI.DrawRect(
          GUILayoutUtility.GetLastRect(),
          new Color(1, 1, 1, 0.2f)
        );
      }

      return clicked;
    }
  }
}
