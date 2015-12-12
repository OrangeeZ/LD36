using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace csv
{
	public class Values
	{
		private Dictionary<string, string> _values;

		public Values (Dictionary<string, string> values)
		{
			_values = values;
		}

		public T Get<T>(string name)
		{
			return Get<T>(_values[name]);
		}

		public T GetSafe<T>(string name)
		{
			return Get(name, default(T));
		}

		public T Get<T>(string name, T defaultValue)
		{
			string strValue;
			if( _values.TryGetValue(name, out strValue))
			{
				return As<T>(strValue, defaultValue);
			}
			return defaultValue;
		}

		public T As<T>(string strValue, T defaultValue) 
		{
			try {
				return (T)System.Convert.ChangeType(strValue, typeof(T)); 
			} catch (System.Exception ex) {
				return default(T);
			}
		} 
	}
}

public interface ICsvConfigurable
{
	void Configure(csv.Values values);
}

