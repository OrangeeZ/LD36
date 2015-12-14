using UnityEngine;
using System.Collections;

public class EnemyCharacterPawn : CharacterPawn {

	[SerializeField]
	private NavMeshAgent _navMeshAgent;

	public override void SetDestination( Vector3 destination ) {

		_navMeshAgent.SetDestination( destination );
	}

	public override float GetDistanceToDestination() {

		return _navMeshAgent.remainingDistance;
	}

	public override void ClearDestination() {
		
		//_navMeshAgent.Stop();
		_navMeshAgent.destination = transform.position;
	}

}
