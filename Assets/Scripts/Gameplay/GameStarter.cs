using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {

	[SerializeField]
	private Object[] _scenesToLoad;

	// Use this for initialization
	void Start () {

		foreach ( var each in _scenesToLoad ) {
			
			SceneManager.LoadScene( each.name, LoadSceneMode.Additive );
		}

		SceneManager.UnloadScene( "Loader" );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
