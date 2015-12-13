using System;
using System.IO;
using System.Threading;
using CsvHelper;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public static class GetCsvFromGoogleDocs {

	public static void Get( string url, string type, string postfix ) {

		EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", 0f);
		Debug.Log("Loading csv from: " + url);
		var www = new WWW( url );
		ContinuationManager.Add(() => { 
				EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", www.progress);
				return www.isDone; 
		}, 
		() => {
				EditorUtility.ClearProgressBar();

				// Let's parse this CSV!
				TextReader sr = new StringReader( www.text );
				try {
					ParseCsv2( sr, type, postfix );
				} catch (Exception ex) {
					Debug.LogException(ex);
				}
		});
	}

	private static void ParseCsv2( TextReader csvReader, string type = null, string postfix = null ) {
		var parser = new CsvParser( csvReader );
		var row = parser.Read(); // get first row and

		if (string.IsNullOrEmpty(type)) {
			// Read Type info
			if (row[0] == "type") {
				type = row[1];

				row = parser.Read();
			} else {
				Debug.LogError("Worksheet must declare 'Type' in first wor");
				return;
			}
		}

		// Read fields
		string[] fieldNames;
		while (row != null && row[0] != "ID") {
			row = parser.Read();
		}
		if (row == null) {
			Debug.LogError("Can't find header!");
			return;
		}

		fieldNames = row;
		row = parser.Read();

		while (row != null) {
			if (row.Length < 2 || string.IsNullOrEmpty(row[0])) {
				row = parser.Read();
				continue;
			}

			string instanceName = csv.Utility.FixName( row[0], postfix);

			var instance = GetOrCreate(type, instanceName);

			if (instance is ICsvConfigurable) {
				ICsvConfigurable configurable = (ICsvConfigurable)instance;
				configurable.Configure(CreateValues(fieldNames, row));
			} else {
				ParseFields2(row, instance, fieldNames);
			}
			Debug.LogFormat("Data object '{0}' saved to \"{1}\"", instance.name, AssetDatabase.GetAssetPath(instance));

			row = parser.Read();
		}
	}

	private static csv.Values CreateValues(string[] fieldNames, string[] row)
	{
		var dict = new Dictionary<string, string>();
		for (int i = 1; i < row.Length; i++) {

			if ( dict.ContainsKey( fieldNames[i] ) ) {
				
				Debug.LogFormat( "They key is duplicate: {0}:{1}", fieldNames[i], row[i] );
				continue;
			}

			dict.Add(fieldNames[i], row[i]);
		}
		return new csv.Values(dict);
	}

	private static void ParseFields2(string[] row, ScriptableObject target, string[] fieldNames) {
		var type = target.GetType();

		for (int i = 1; i < row.Length; i++) {
			string fieldName = fieldNames[i];
			string fieldValue = row[i];

			var field = type.GetField(fieldName);
			if (field == null) {
				Debug.LogErrorFormat("Type {0} doesn't contains field {1}", type.Name, fieldName);
				return;
			}
			try {
				field.SetValue(target, Convert.ChangeType(fieldValue, field.FieldType));
			} catch (Exception ex) {
				Debug.LogErrorFormat("Can't set value {0} of field '{1}' to object '{1}'", fieldValue, fieldName, target.name, target);
				return;
			}
		}
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
			ScriptableObject instance = GetOrCreate (row[1], instanceName);

			ParseFields( parser, instance );

			// Create an asset
		}

		//AssetDatabase.StopAssetEditing();

		//// Save all assets
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	static ScriptableObject GetOrCreate (string typeName, string instanceName)
	{
		var assembly = typeof(ICanBeAffected).Assembly;
		var type = assembly.GetExportedTypes().First((x) => x.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
		if (type == null) {
			Debug.LogWarningFormat ("Type {0} not found", typeName);
			return null;
		}

		var assetPath = Path.Combine ("Assets/Data/Remote Data/", type.Name);
		var assetPathWithName = assetPath + "/" + instanceName + ".asset";

		var instance = AssetDatabase.LoadAssetAtPath<ScriptableObject> (assetPathWithName);
		//instance.name = instanceName;
		if (instance == null) {
			instance = ScriptableObject.CreateInstance (type);
			Directory.CreateDirectory (assetPath).Attributes = FileAttributes.Normal;
			AssetDatabase.CreateAsset (instance, assetPathWithName);
		}
		
		EditorUtility.SetDirty( instance );

		return instance;
	}

	private static void ParseFields( CsvParser parser, ScriptableObject target ) {
		var type = target.GetType();

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