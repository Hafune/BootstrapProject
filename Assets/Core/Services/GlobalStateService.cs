namespace Core
{
    public interface IAbstractGlobalState
    {
        void DisableState();
    }

    public class GlobalStateService
    {
        private IAbstractGlobalState _currentState;

        public bool ChangeActiveState(IAbstractGlobalState state)
        {
            if (_currentState == state)
                return false;

            _currentState?.DisableState();
            _currentState = state;
            return true;
        }
    }
}