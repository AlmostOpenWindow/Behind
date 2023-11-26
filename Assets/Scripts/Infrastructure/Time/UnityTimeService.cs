namespace Infrastructure.Time
{
    public class UnityTimeService : ITimeService
    {
        public float TimeScale { get; set; } = 1f;
        
        public float DeltaTime
            => UnityEngine.Time.deltaTime * TimeScale;

        public float FixedDeltaTime
            => UnityEngine.Time.fixedDeltaTime * TimeScale;
        
        public float TotalGamePlayTime
            => UnityEngine.Time.time;
    }
}