namespace TrainEngine.Trains
{
    public interface ITrainPosition
    {
        bool IsMoving { get; }
        string CurrentDestination { get; }
        bool IsAtStation { get; }
    }
}