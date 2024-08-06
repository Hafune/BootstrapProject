using System;
using Lib;
using UnityEngine;

namespace Core.Services
{
    [Serializable]
    public class GameplayStateService : MonoConstruct, IGlobalState
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
            if (!_globalStateService.ChangeActiveState(this))
                return;
            
            Debug.Log("GameplayStateService enabled");
            BuildCharacter();
        }

        public void DisableState() => RemoveCharacterAndControls();

        public void PauseInputs()
        {
            if (--_totalInputUsers == 0)
                _playerInputs.Disable();
        }

        public void ResumeInputs()
        {
            if (++_totalInputUsers == 1)
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
            _playerCharacter = Context.Instantiate(_playerCharacterPrefab, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(_playerCharacter);
            ResumeInputs();
        }
    }
}