using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
	public class TransformMover : MonoBehaviour, IMover
	{
		private float _moveSpeed;
		private float _turnSpeed;

		public void Init( float moveSpeed, float turnSpeed )
		{
			_moveSpeed = moveSpeed;
			_turnSpeed = turnSpeed;
		}
		
		public void Turn( float amount )
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.y += amount * _turnSpeed * Time.deltaTime;
			transform.localEulerAngles = rotation;
		}

		public void Move( float amount )
		{
			Vector3 position = transform.position;
			Vector3 movement = transform.forward * amount * _moveSpeed * Time.deltaTime;
			position += movement;
			transform.position = position;
		}

        public void Move(Vector3 direction)
        {
            direction = direction.normalized;
            Vector3 position = transform.position + direction * _moveSpeed * Time.deltaTime;
            transform.position = position;
        }

        public void Turn(Vector3 target)
        {

            float direction = Vector3.SignedAngle(gameObject.transform.forward, target,Vector3.up);
            if (direction < 0) { Turn(-1); } else { Turn(1); }
            
            
        }
    }
}
