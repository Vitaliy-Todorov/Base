using Scripts.Infrastructure.Services.InputService;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        private float _maxMoveSpeed;
        private IInputService _inputService;

        public void Construct(float maxMoveSpeed, IInputService inputService)
        {
            _maxMoveSpeed = maxMoveSpeed;
            _inputService = inputService;
        }

        void Update()
        {
            if (_rigidbody.velocity.magnitude < _maxMoveSpeed)
                Acceleration(_inputService.Axis);
        }

        private void Acceleration(Vector3 axis) =>
            _rigidbody.velocity += axis;
    }
}
