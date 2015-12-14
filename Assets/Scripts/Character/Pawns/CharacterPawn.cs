using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterPawn : CharacterPawnBase {

	public bool canFollowDestination;

	public GameObject turret;

	[SerializeField]
	private float _gunYOffset = 0.5f;

	[SerializeField]
	private float _weight = 1f;

	[SerializeField]
	private float _startingHeight = 5;

	[SerializeField]
	private float _rotationToDirectionSpeed = 100;

	[SerializeField]
	private SimpleSphereCollider _sphereCollider;

	[SerializeField]
	private CharacterController _characterController;

	private Vector3? _destination;
	private bool _isGravityEnabled;
	private float _ySpeed;

	private Transform _turretTarget;

	[SerializeField]
	private Transform _subTransform;

	[SerializeField]
	private CharacterPawnLevelingController _levelingController;

	private void Update() {

		if ( _isGravityEnabled ) {

			_ySpeed += _weight * Time.deltaTime;
		}

		if ( _destination.HasValue && canFollowDestination ) {

			//var direction = planetTransform.GetDirectionTo( _destination.Value );

			//planetTransform.Move( transform, Vector3.forward, speed * Time.deltaTime );

			//rotation = Quaternion.RotateTowards( transform.rotation, transform.rotation * Quaternion.FromToRotation( Vector3.forward, direction.Set( y: 0 ) ), _rotationToDirectionSpeed * Time.deltaTime );

			ApplyPunishingForce();
		}

		//if ( turret != null && _turretTarget != null ) {

		//	if ( character.pawn.turret != null ) {

		//		//var direction = GetDirectionTo( _turretTarget.position );

		//		character.pawn.turret.transform.localRotation = Quaternion.FromToRotation( Vector3.forward, direction );
		//	}
		//}

	}

	public Vector3 GetWeaponPosition() {

		return position + Vector3.up * _gunYOffset;
	}

	public override void MoveDirection( Vector3 direction ) {

		var directionDelta = direction * speed * Time.deltaTime;

		if ( _characterController == null ) {

			position += directionDelta;
		} else {

			_characterController.Move( directionDelta + Vector3.down * Time.deltaTime );
		}

		rotation = Quaternion.RotateTowards( rotation, Quaternion.LookRotation( direction, Vector3.up ), _rotationToDirectionSpeed * Time.deltaTime );
	}

	public override void SetDestination( Vector3 destination ) {

		_destination = destination;
	}

	public override float GetDistanceToDestination() {

		return _destination.HasValue ? Vector3.Distance( position, _destination.Value ) : float.NaN;
	}

	public override Vector3 GetDirectionTo( CharacterPawnBase otherPawn ) {

		return ( otherPawn.position - position ).normalized;
	}

	public virtual void ClearDestination() {

		_destination = null;
	}

	public void SetTurretTarget( Transform turretTarget ) {

		_turretTarget = turretTarget;
	}

	public void SetColor( Color baseColor ) {

		var renderers = GetComponentsInChildren<Renderer>();
		foreach ( var each in renderers ) {

			each.material.SetColor( "_Color", baseColor );
		}
	}

	public void SetGravityEnabled( bool value ) {

		_isGravityEnabled = value;

		if ( !value ) {

			_ySpeed = 0;
		}
	}

	public void SetActive( bool isActive ) {

		var collider = GetComponent<Collider>();

		//collider.enabled = isActive;
		enabled = isActive;
	}

	public override void MakeDead() {

		GetSphereSensor().enabled = false;
		GetComponent<Collider>().enabled = false;

		//GetComponentsInChildren<Rotator>().MapImmediate( Destroy );
		//GetComponentsInChildren<Renderer>().MapImmediate( _ => _.material.color += Color.black * 0.8f );
	}

	public void AddLevel( float scaleBonus ) {

		if ( _levelingController != null ) {

			_levelingController.AddLevel( scaleBonus );
		}
	}

	private void ApplyPunishingForce() {

		if ( _sphereCollider == null ) {

			return;
		}

		var punishingForce = Building.instances.Select( _ => _.sphereCollider )
			.Select( _ => _.CalculatePunishingForce( _sphereCollider ) )
			.Aggregate( Vector3.zero, ( total, each ) => each + total );

		transform.position += punishingForce;
	}

}