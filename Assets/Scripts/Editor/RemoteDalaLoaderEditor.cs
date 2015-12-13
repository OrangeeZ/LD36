using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RemoteDataLoader))]
public class RemoteDalaLoaderEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button("Load")) {
			((RemoteDataLoader)target).LoadRemoteData();
		}
	}
}

