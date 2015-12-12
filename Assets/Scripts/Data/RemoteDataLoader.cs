using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu( fileName = "Remote Data Loader", menuName = "Data/Remote Data Loader" )]
public partial class RemoteDataLoader : ScriptableObject {

	[SerializeField]
	private string _url = "https://docs.google.com/spreadsheets/d/1vewdQjSxYpgDuyxaV5uC2182Y4kxRRVK3r6u-c_Whp8/export?format=csv";

	[ContextMenu( "Load remote data" )]
	private void LoadRemoteData() {

		GetCsvFromGoogleDocs.Get( _url );
	}

}

#if UNITY_EDITOR
public partial class RemoteDataLoader {

	[MenuItem( "Tools/Remote Data Loader" )]
	private static void SelectRemoteDataLoader() {

		var loader = AssetDatabase.LoadAssetAtPath<RemoteDataLoader>( "Assets/Data/Remote Data Loader.asset" );
		if ( loader != null ) {

			Selection.activeObject = loader;
		}
	}

}

#endif