using Core.Services;
using Lib;

namespace Core.Views
{
    public class GameplayStateLauncher : MonoConstruct
    {
        private void Start() => Context.Resolve<GameplayStateService>().EnableState();
    }
}