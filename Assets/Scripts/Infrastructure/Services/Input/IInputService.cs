using Services;
using StarterAssets;
using UnityEngine.InputSystem;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
#if ENABLE_INPUT_SYSTEM
        PlayerInput PlayerInput { get; }
#endif
        InputData Data { get; }
    }
}