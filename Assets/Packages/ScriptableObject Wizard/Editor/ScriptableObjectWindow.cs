using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace UnityEngine.ScriptableObjectWizard {

	internal class EndNameEdit : EndNameEditAction
	{
		#region implemented abstract members of EndNameEditAction
		public override void Action (int instanceId, string pathName, string resourceFile)
		{
			AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
		}

		#endregion
	}

	/// <summary>
	/// Scriptable object screen.
	/// </summary>
	public class ScriptableObjectWindow : EditorWindow
	{
		private int categoryIndex;
		private int classIndex;
		private string[] names;
	
		//private Type[] types;

		private Dictionary<string, string[]> categories = new Dictionary<string, string[]>();
		private Dictionary<string, Type[]> types = new Dictionary<string, Type[]>();

		private string[] categoryNames;

		public Type[] Types
		{ 
			//get { return types; }
			set
			{
				//types = value;
				//names = types.Select(t => t.FullName).ToArray();

				var grouping = value.GroupBy( delegate( Type _ ) {

					var attribute = _.GetCustomAttributes( inherit: true ).OfType<CategoryAttribute>().FirstOrDefault();

					return attribute == null ? "Default" : attribute.name;
				} );

				categories = grouping.ToDictionary( _ => _.Key, _ => _.Select( t => t.FullName ).ToArray() );
				types = grouping.ToDictionary( _ => _.Key, _ => _.ToArray() );

				categoryNames = categories.Keys.ToArray();
			}
		}

		void OnEnable() {

			categoryIndex = PlayerPrefs.GetInt( "ScriptableObjectWindow:categoryIndex", 0 );
			classIndex = PlayerPrefs.GetInt( "ScriptableObjectWindow:selectedIndex", 0 );
		}

		void OnDisable() {

			PlayerPrefs.SetInt( "ScriptableObjectWindow:categoryIndex", categoryIndex );
			PlayerPrefs.SetInt( "ScriptableObjectWindow:selectedIndex", classIndex );
		}
	
		public void OnGUI()
		{
			GUILayout.Label("ScriptableObject Class");
			
			EditorGUILayout.BeginHorizontal();

			categoryIndex = Mathf.Clamp( categoryIndex, 0, categories.Keys.Count );
			categoryIndex = EditorGUILayout.Popup( categoryIndex, categoryNames );

			classIndex = Mathf.Clamp( classIndex, 0, categories[categoryNames[categoryIndex]].Length );
			classIndex = EditorGUILayout.Popup( classIndex, categories[categoryNames[categoryIndex]] );
			
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Create")) {

				var type = types[categoryNames[categoryIndex]][classIndex];
				var asset = ScriptableObject.CreateInstance( type );
				ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
					asset.GetInstanceID(),
					ScriptableObject.CreateInstance<EndNameEdit>(),
					string.Format( "{0}.asset", type.FullName ),
					AssetPreview.GetMiniThumbnail(asset), 
					null);

				Close();
			}
		}
	}
}