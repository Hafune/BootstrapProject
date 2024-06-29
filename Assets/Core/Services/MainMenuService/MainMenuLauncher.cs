using Core.Services;
using Lib;

namespace Core.Views
{
    public class MainMenuLauncher : MonoConstruct
    {
        private void Start() => Context.Resolve<MainMenuService>().EnableState();
    }
}