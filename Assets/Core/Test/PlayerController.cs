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

        private void Awake()
        {
            Context.Resolve<CinemachineVirtualCamera>().Follow = transform;
            _playerInputs = Context.Resolve<PlayerInputs.PlayerActions>();
        }

        private void FixedUpdate()
        {
            var direction = _playerInputs.Move.ReadValue<Vector2>();
            _body.velocity = new Vector3(direction.x, 0, direction.y) * _speed;
        }
    }
}