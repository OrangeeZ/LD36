using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace csv {

	public static class Utility {

		public static string FixName( string name, string postfix = null ) {

			var builder = new StringBuilder();

			var wordStart = -1;
			var wordEnd = -1;
			var pushed = 0;
			// Convert to lower
			for ( var i = 0; i < name.Length; i++ ) {
				if ( char.IsUpper( name, i ) ) {
					if ( wordStart != -1 ) {
						if ( wordEnd != wordStart ) {
							// New word
							if ( builder.Length > 0 ) {
								builder.Append( "-" );
							}
							builder.Append( name.Substring( pushed, i - wordStart ).ToLower() );
							pushed = i;
						}
					} else if ( pushed < i ) {
						if ( builder.Length > 0 ) {
							builder.Append( "-" );
						}
						builder.Append( name.Substring( pushed, i ).ToLower() );
						pushed = i;
					}
					wordStart = i;
					wordEnd = i;
				} else {
					if ( wordStart != -1 ) {
						wordEnd = i;
					}
				}
			}

			if ( pushed < name.Length - 1 ) {
				if ( builder.Length > 0 ) {
					builder.Append( "-" );
				}
				builder.Append( name.Substring( pushed ).ToLower() );
			}

			var result = builder.ToString();
			if ( !string.IsNullOrEmpty( postfix ) ) {
				result += "-" + postfix;
			}
			return result;
		}

	}

	public class Values {

		private Dictionary<string, string> _values;

		public Values( Dictionary<string, string> values ) {
			_values = values;
		}

		public T Get<T>( string name ) {
			return Get<T>( _values[name] );
		}

		public T GetSafe<T>( string name ) {
			return Get( name, default ( T ) );
		}

		public T Get<T>( string name, T defaultValue ) {
			string strValue;
			if ( _values.TryGetValue( name, out strValue ) ) {
				return As<T>( strValue, defaultValue );
			}
			return defaultValue;
		}

		public T As<T>( string strValue, T defaultValue ) {
			try {
				return (T) System.Convert.ChangeType( strValue, typeof ( T ) );
			}
			catch ( System.Exception ex ) {
				return default ( T );
			}
		}

		public T[] GetScriptableObjects<T>( string name ) where T : ScriptableObject {

			var names = Get( name, string.Empty ).Split( ',', ' ' );

			return names.Select( _ => LoadScriptableObject<T>( _ ) ).Where( _ => _ != null ).ToArray();
		}

		public T GetScriptableObject<T>( string name ) where T : ScriptableObject {

			var assetName = Get( name, string.Empty );

			return LoadScriptableObject<T>( assetName );
		}

		private T LoadScriptableObject<T>( string name ) where T : ScriptableObject {

			name = Utility.FixName( name );

			if ( name.IsNullOrEmpty() ) {

				return null;
			}

			var guid = AssetDatabase.FindAssets( "t:" + typeof ( T ).Name + " " + name ).FirstOrDefault();
			if ( guid != null ) {

				var path = AssetDatabase.GUIDToAssetPath( guid );
				return AssetDatabase.LoadAssetAtPath<T>( path );
			}

			Debug.LogFormat( "Could not find {0}", "t:" + typeof ( T ).Name + " " + name );

			return null;
		}

	}

}

public interface ICsvConfigurable {

	void Configure( csv.Values values );

}