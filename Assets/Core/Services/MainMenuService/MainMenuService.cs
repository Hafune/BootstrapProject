using System;
using Reflex;
using UnityEngine;

namespace Core.Services
{
    [Serializable]
    public class MainMenuService : IInitializableService, IGlobalState
    {
        private GlobalStateService _globalStateService;

        public void InitializeService(Context context)
        {
            _globalStateService = context.Resolve<GlobalStateService>();
        }

        public void EnableState()
        {
            _globalStateService.ChangeActiveState(this);
            Debug.Log("MainMenuService enabled");
        }

        public void DisableState() => Debug.Log("MainMenuService disabled");
    }
}