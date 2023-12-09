namespace Infrastructure.Mechanics.Routines
{
    public interface IRoutineMechanics : IMechanic
    {
        void AddRoutine(Routine routine);
        void RemoveRoutine(Routine routine);
    }
}