using System;
using UnityEngine;
using System.Collections;

namespace UnityEngine.ScriptableObjectWizard {

	[AttributeUsage( AttributeTargets.Class )]
	public class HideInWizardAttribute : Attribute { }

	[AttributeUsage( AttributeTargets.Class, Inherited = true, AllowMultiple = true )]
	public class CategoryAttribute : Attribute {

		public string name { get; private set; }

		public CategoryAttribute( string name ) {

			this.name = name;
		}
	}
}