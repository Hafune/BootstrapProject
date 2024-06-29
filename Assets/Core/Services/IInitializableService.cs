using Reflex;

namespace Core.Services
{
    public interface IInitializableService
    {
        public void InitializeService(Context context);
    }
}