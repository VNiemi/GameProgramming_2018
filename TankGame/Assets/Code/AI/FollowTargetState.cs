using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Systems;

namespace TankGame.AI
{
    public class FollowTargetState : AIStateBase
    {


        public FollowTargetState(EnemyUnit owner) : base()
        {
            // Basic bookkeeping.
            State = AIStateType.FollowTarget;
            Owner = owner;

            // Has two possible transitions.
            AddTransition(AIStateType.Patrol);
            AddTransition(AIStateType.Shoot);


        }

        

        private bool ChangeState()
        {
            
            float _distanceSqr
                = (Owner.Target.transform.position - Owner.transform.position).sqrMagnitude;

            if (_distanceSqr > Owner.DetectEnemyDistance * Owner.DetectEnemyDistance)
            {
                Owner.PerformTransition(AIStateType.Patrol);
                return true;
               
            }
            
            if (_distanceSqr < Owner.ShootingDistance * Owner.ShootingDistance)
            {
                Owner.PerformTransition(AIStateType.Shoot);
                return true;
             
            }

            return false;
            
        }

        public override void Update()
        {
            if(!ChangeState())
            {
                Owner.Mover.Turn(Owner.Target.transform.position);
                Owner.Mover.Move(Owner.transform.forward);
            }
        }
    }
}
