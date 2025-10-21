using Core.Extensions;
using UnityEngine;

namespace Application.GameState
{
    public class InputController : MonoBehaviour
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        [SerializeField] private float _movementSpeed = 20;
        [SerializeField] private float _rotationSpeed;

        private Vector3 _direction;

        private void Update()
        {
            // EventBus.RaiseEvent<IObjectDistanceUpdater>(updater => updater.UpdateDistance(Vector2.Distance()));
            
            var horizontal = Input.GetAxisRaw(Horizontal);
            var vertical = Input.GetAxisRaw(Vertical);
            var direction = new Vector3(horizontal, vertical).normalized;

            if(direction != Vector3.zero && _direction != -direction)
            {
                _direction = direction;
                transform.SetX(transform.position.x + direction.x * (_movementSpeed * Time.deltaTime));
                transform.SetY(transform.position.y + direction.y * (_movementSpeed * Time.deltaTime));
                
                RotateTank();
            }
        }

        private void RotateTank()
        {
            var angle = Mathf.Atan2(-_direction.x, _direction.y) * Mathf.Rad2Deg;
            var lookRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}