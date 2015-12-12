using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceZone : MonoBehaviour {

    public static List<ForceZone> instances = new List<ForceZone>();

    public float radius;
    public float strength;

    public Vector3 GetForceVector( Vector3 position ) {

        var direction = ( transform.position - position ).normalized;

        var normalizedValue = 1f - ( position - transform.position ).magnitude / radius;

        return direction * normalizedValue.Clamped( 0, float.MaxValue ) * strength;
    }

    private void Start() {

        instances.Add( this );
    }

    private void OnDestroy() {

        instances.Remove( this );
    }

    private void OnDrawGizmos() {

        Gizmos.DrawWireSphere( transform.position, radius );
    }

}