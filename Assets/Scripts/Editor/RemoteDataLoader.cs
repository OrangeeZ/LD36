using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu( fileName = "Remote Data Loader", menuName = "Data/Remote Data Loader" )]
public partial class RemoteDataLoader : ScriptableObject {

	[SerializeField]
	private string _url = "https://docs.google.com/spreadsheets/d/1vewdQjSxYpgDuyxaV5uC2182Y4kxRRVK3r6u-c_Whp8/export?format=csv";

	[SerializeField]
	private string _pageId = string.Empty;

	[SerializeField]
	private string type = string.Empty;

	[SerializeField]
	private string postfix = string.Empty;
}

#if UNITY_EDITOR
public partial class RemoteDataLoader {
	
	[ContextMenu( "Load remote data" )]
	public void LoadRemoteData() {

		string url = _url;
		if (!string.IsNullOrEmpty(_pageId)) {
			url = url + "&gid=" + _pageId;
		}

		GetCsvFromGoogleDocs.Get( url, type, postfix );
	}

	[MenuItem( "Tools/Remote Data Loader" )]
	private static void SelectRemoteDataLoader() {

		var loader = AssetDatabase.LoadAssetAtPath<RemoteDataLoader>( "Assets/Data/Remote Data Loader.asset" );
		if ( loader != null ) {

			Selection.activeObject = loader;
		}
	}

}

#endif