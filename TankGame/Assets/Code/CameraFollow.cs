using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class CameraFollow : MonoBehaviour, ICameraFollow
    {

#region Serialized fields
        [SerializeField, Tooltip("Distance in world units between camera and target.")]
        private float _distance;

        [SerializeField, Tooltip("The angle in degrees between camera direction and vertical.")]
        private float _angle;

        [SerializeField, Tooltip("The target transform. ")]
        private Transform _target;

        [SerializeField, Tooltip("Whether camera looks at the target along z-axis or from behind it.")]
        private SetBehind _setBehind;

        #endregion

#region The required interface methods
        // Look like crap, apparently you need to add extra code for Unity
        // to prettify enums. Not going to do it.
        private enum SetBehind
        {
            along_z_axis,
            from_behind
        }

        // Public interface method to set the angle.
        public void SetAngle(float angle)
        {
            _angle = angle;
        }

        // Public interface method to set the distance.
        public void SetDistance(float distance)
        {
            _distance = distance;
        }

        // Public interface method to set the target.
        public void SetTarget(Transform targetTransform)
        {
            _target = targetTransform;
        }

        #endregion

#region Objects for the update function

        // Olds allow skipping redundant math.
        // Others just skip redundant object creation once per frame.
        // There should be no performance difference, just a preference.

        // Placeholders for current and previous target position and direction.       
        private Vector3 _oldPos, _oldForward, _Pos, _Forward;

        // Holder for the temp for camera position.
        private Vector3 _cameraPos;

#endregion

        // Using LateUpdate because Lauri said, using it removes stutter, it did.
        void LateUpdate()
        {

            _Pos = _target.position;
            _Forward = _target.forward;

            // Skip moving camera if nothing relevant changed.
            if (_Pos == _oldPos
                && (_Forward == _oldForward || _setBehind == SetBehind.along_z_axis))
                return;

            // The compulsory trigonometry.
            // The distance is the hypothenuse of a straight angle triagle,
            // The height and horizontal distance from target are then
            // the sine and cosine multiplied by distance because they are
            // ratios of the sides of the triangle to hypothenuse.
            float angle = Mathf.Deg2Rad * (_angle);
            float horizontal = Mathf.Sin(angle) * _distance;
            float y = Mathf.Cos(angle) * _distance;

            // As camera follows the target, the target position is the base.
            _cameraPos = _Pos;

            if (_setBehind == SetBehind.from_behind)
            {
                // From behind target is derived from target forward.
                // The y would mess up the height so is removed.
                // When normalized and multiplied horizontal distance gives
                // the relative horizontal coordinates.
                Vector3 direction = _Forward;                
                direction.y = 0;                
                direction.Normalize();
                direction = -direction * horizontal;
          
                // Apply the offsets.
                _cameraPos.x += direction.x;
                _cameraPos.y += y;
                _cameraPos.z += direction.z;

            }
            else
            {
                // Apply the height and horizontal distance to z.
                _cameraPos.y += y;
                _cameraPos.z -= horizontal;
                
            }

            // Put calculated position to camera and point it at the target.
            gameObject.transform.position = _cameraPos;
            gameObject.transform.LookAt(_target);

            // Store target position and direction.
            _oldPos = _Pos;
            _oldForward = _Forward;

        }
    }
}
