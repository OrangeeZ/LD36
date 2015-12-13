using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(RemoteDataLoader))]
public class RemoteDalaLoaderEditor : Editor
{
	SerializedProperty _url;
	SerializedProperty _pageId;
	SerializedProperty _type;
	SerializedProperty _postfix;

	int _selectedType;
	string[] _types;

	void OnEnable()
	{
		_url = serializedObject.FindProperty("_url");
		_pageId = serializedObject.FindProperty("_pageId");
		_type = serializedObject.FindProperty("type");
		_postfix = serializedObject.FindProperty("postfix");

		var types = typeof(ICsvConfigurable).Assembly.GetExportedTypes();
		_types = types.Where(t => t.IsSubclassOf(typeof(ScriptableObject))).Select(t => t.Name).ToArray();
		_selectedType = ArrayUtility.IndexOf(_types, _type.stringValue);
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(_url);
		EditorGUILayout.PropertyField(_pageId);
		EditorGUILayout.PropertyField(_postfix);

		EditorGUILayout.PropertyField(_type);

		int selected = EditorGUILayout.Popup(_selectedType, _types);
		if (selected != _selectedType) {
			_selectedType = selected;
			if (_selectedType != -1) {
				_type.stringValue = _types[_selectedType];
			}
		}

		//base.OnInspectorGUI ();
		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("Load")) {
			((RemoteDataLoader)target).LoadRemoteData();
		}
	}
}

