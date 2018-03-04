using TankGame.Systems;
using TankGame.WaypointSystem;
using UnityEngine;

namespace TankGame.AI
{
    public class ShootState : AIStateBase
    {
        /// <summary>
        /// Function to return the shooting distance. Allows shooting distance to change at runtime.
        /// </summary>
        private float SqrShootingDistance
        {
            get { return Owner.ShootingDistance * Owner.ShootingDistance; }
        }

        private float sqrDistanceToPlayer;

        public ShootState(EnemyUnit owner)
            : base(owner, AIStateType.Shoot)
        {
            AddTransition(AIStateType.Patrol);
            AddTransition(AIStateType.FollowTarget);
        }

        public override void Update()
        {
            // Update distance to player.
            sqrDistanceToPlayer = (Owner.transform.position - Owner.Target.transform.position).sqrMagnitude;

            if (!ChangeState())
            {
                // Move forward if not too close to the target.
                if (sqrDistanceToPlayer > SqrShootingDistance / 2) Owner.Mover.Move(Owner.transform.forward);

                // Turn towards the target, same as with follow state.
                Owner.Mover.Turn(Owner.Target.transform.position);

                // You can check if the target is in sights with
                // "Vector3.Angle(Owner.transform.forward, Owner.Target.transform.position - Owner.transform.position)"
                // and even compensate for target movement but it is probably not wanted here. The AI is supposed to be simple.

                // Weapon handles whether it can shoot or not, so just fire.
                Owner.Weapon.Shoot();

            }

        }

        private bool ChangeState()
        {

            // Check if the target is dead.
            if(!Owner.Target.gameObject.activeSelf)
            {
                return Owner.PerformTransition(AIStateType.Patrol);
            }

            // Go to follow mode, if target outside firing range.            
            if (sqrDistanceToPlayer > SqrShootingDistance)
            {
                return Owner.PerformTransition(AIStateType.FollowTarget);
            }

            // Otherwise return false.
            return false;
        }
    }
}