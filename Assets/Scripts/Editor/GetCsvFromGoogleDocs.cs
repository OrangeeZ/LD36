using System;
using System.IO;
using System.Threading;
using CsvHelper;
using UnityEditor;
using UnityEngine;

public static class GetCsvFromGoogleDocs {

	public static void Get( string url ) {

		EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", 0f);

		var www = new WWW( url );
		ContinuationManager.Add(() => { 
				EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", www.progress);
				return www.isDone; 
		}, 
		() => 
		{
			EditorUtility.ClearProgressBar();

			// Let's parse this CSV!
			TextReader sr = new StringReader( www.text );
			ParseCsv( sr );
		});
	}

	private static void ParseCsv( TextReader csvReader ) {

		//AssetDatabase.StartAssetEditing();

		var parser = new CsvParser( csvReader );
		var row = parser.Read(); // get first row and
		while ( row != null ) {
			// break when ther is no more lines
			// 'type' is a red flag, so...
			if ( row[0] != "type" ) {
				// if not a 'type'
				row = parser.Read(); // get next row and
				continue; // check it
			}

			// Ya! We have a type here so:
			//  1. chech we have a name for future object
			if ( row.Length < 3 ) {
				Debug.LogWarningFormat( "Object name for type {0} not found", row[1] );
				row = parser.Read();
				continue;
			}
			var instanceName = row[2];
			//  2. trying to get a type
			var type = Type.GetType( row[1] );
			if ( type == null ) {
				Debug.LogWarningFormat( "Type {0} not found", row[1] );
				row = parser.Read();
				continue;
			}

			var assetPath = Path.Combine( "Assets/Data/", type.Name );
			var assetPathWithName = assetPath + "/" + instanceName + ".asset";
            var instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>( assetPathWithName );

			if ( instance == null ) {

				instance = ScriptableObject.CreateInstance( type );

				Directory.CreateDirectory( assetPath ).Attributes = FileAttributes.Normal;
				AssetDatabase.CreateAsset( instance, assetPathWithName );
			}

			ParseFields( parser, type, instance );

			// Create an asset
		}

		//AssetDatabase.StopAssetEditing();

		//// Save all assets
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private static void ParseFields( CsvParser parser, Type type, ScriptableObject target ) {

		while ( true ) {
			// get next row and check it
			var row = parser.Read();

			if ( row == null ) {
				// if text is over...
				break; // break this sycle and upper while will breaked by condition
			}

			if ( row[0] == "type" ) {
				// if we get a type...
				break; // goto main while to check this type
			}

			var p = type.GetField( row[0] );
			try {
				p.SetValue( target, Convert.ChangeType( row[1], p.FieldType ) );
			}
			catch ( NullReferenceException ) {
				// We get Null in two cases:
				// 1. wrong fild name
				// 2. empty line
				// so...
				if ( row[0].Length > 0 ) {
					// log only if we get wrong name
					Debug.LogWarning( "Fild " + type.Name + "." + row[0] + " not found" );
				}
			}
			catch ( FormatException ) {
				Debug.LogWarning( "Fild " + type.Name + "." + row[0]
				                  + " wrong data '" + row[1] + "', "
				                  + p.FieldType.Name + " expected." );
			}
		}
	}

}