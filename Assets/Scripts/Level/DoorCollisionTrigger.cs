using UnityEngine;

namespace Assets.Scripts.Level
{
    public class DoorCollisionTrigger : MonoBehaviour
    {
        [SerializeField]
        private DoorAnimationTrigger _doorAnimationTrigger;

        private BoxCollider _boxCollider;
        private void OnCollisionEnter(Collision collision)
        {
            var projectile = collision.collider.GetComponent<Projectile>();
            if(projectile == null ||
                projectile.Owner!= GameplayController.Instance.PlayerSpawner.character)
                return;
            _doorAnimationTrigger.SetDoorState(true);
        }

    }
}
