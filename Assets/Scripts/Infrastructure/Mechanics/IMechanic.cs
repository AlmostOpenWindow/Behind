namespace Infrastructure.Mechanics
{
    public interface IMechanic : ISwitchEnableState
    {
    }

    public interface ISwitchEnableState
    {
        bool IsEnable { get; }
        void Enable();
        void Disable();
    }
}