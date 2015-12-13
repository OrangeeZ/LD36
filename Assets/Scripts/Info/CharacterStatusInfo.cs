using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;

[CreateAssetMenu( menuName = "Create/Status Info" )]
public class CharacterStatusInfo : ScriptableObject, ICsvConfigurable {

	//[SerializeField]
	public float MaxHealth;

	//[SerializeField]
	public float MoveSpeed;

	//[SerializeField]
	public float Damage;

	[SerializeField]
	private CharacterStatus status;

	public CharacterStatus GetInstance() {

		return new CharacterStatus( this ) {

			Agility = {Value = status.Agility.Value},
			Strength = {Value = status.Strength.Value}
		};
	}

	public virtual void Configure( csv.Values values ) {

		MaxHealth = values.Get( "MaxHP", MaxHealth );
		MoveSpeed = values.Get( "Speed", MoveSpeed );
		Damage = values.Get( "DMG", Damage );
	}

}