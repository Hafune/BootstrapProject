using System;
using Lib;
using Reflex;
using UnityEngine;

namespace Core.Services
{
    [Serializable]
    public class GameplayStateService : MonoConstruct, IAbstractGlobalState
    {
        [SerializeField] private Transform _playerCharacterPrefab;
        private Transform _playerCharacter;
        private GlobalStateService _globalStateService;
        private PlayerInputs.PlayerActions _playerInputs;
        private int _totalInputUsers;

        private void Awake()
        {
            _globalStateService = Context.Resolve<GlobalStateService>();
            _playerInputs = Context.Resolve<PlayerInputs.PlayerActions>();
        }

        public void EnableState()
        {
            _globalStateService.ChangeActiveState(this);
            Debug.Log("GameplayStateService enabled");
            BuildCharacter();
        }

        public void DisableState() => RemoveCharacterAndControls();

        public void PauseInputs()
        {
            if (--_totalInputUsers != 0)
                return;

            _playerInputs.Disable();
        }

        public void ResumeInputs()
        {
            if (++_totalInputUsers != 1)
                return;

            _playerInputs.Enable();
        }

        private void RemoveCharacterAndControls()
        {
            PauseInputs();
            Destroy(_playerCharacter.gameObject);
            _playerCharacter = null;
        }

        private void BuildCharacter()
        {
            if (_playerCharacter is not null)
                return;

            _playerCharacter = Context.Instantiate(_playerCharacterPrefab, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(_playerCharacter);
            ResumeInputs();
        }
    }
}