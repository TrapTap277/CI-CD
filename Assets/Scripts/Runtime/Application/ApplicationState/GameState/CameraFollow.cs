using UnityEngine;

namespace Application.GameState
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        private void Update()
        {
            if(_target != null)
                transform.position = Vector3.Slerp(transform.position, TargetPositionWithSavedZ(), Time.deltaTime * _speed);
        }

        private Vector3 TargetPositionWithSavedZ() =>
            new(_target.transform.position.x, _target.transform.position.y, transform.position.z);
    }
}