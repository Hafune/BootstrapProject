namespace Core
{
    public interface IGlobalState
    {
        void DisableState();
    }

    public class GlobalStateService
    {
        private IGlobalState _currentState;

        public bool ChangeActiveState(IGlobalState state)
        {
            if (_currentState == state)
                return false;

            _currentState?.DisableState();
            _currentState = state;
            return true;
        }
    }
}