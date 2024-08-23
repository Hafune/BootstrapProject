using Cinemachine;
using Lib;
using UnityEngine;

namespace Core.Test
{
    public class PlayerController : MonoConstruct
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private float _speed;
        private PlayerInputs.PlayerActions _playerInputs;
        private FixedJoystick _joystick;

        private void Start()
        {
            Context.Resolve<CinemachineVirtualCamera>().Follow = transform;
            _playerInputs = Context.Resolve<PlayerInputs.PlayerActions>();
            _joystick = Context.Resolve<FixedJoystick>();
        }

        private void FixedUpdate()
        {
            var direction = _playerInputs.Move.ReadValue<Vector2>();

            if (direction == Vector2.zero)
                direction = _joystick.Direction;
            
            _body.velocity = new Vector3(direction.x, 0, direction.y) * _speed;
        }
    }
}