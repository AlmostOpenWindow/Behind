using Services;

namespace Infrastructure.Time
{
    public interface ITimeService : IService
    {
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
        float TotalGamePlayTime { get; }
        float TimeScale { get; set; }
    }
}