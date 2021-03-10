namespace TrainEngine.TrainTrack
{
    public class Station
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public bool IsLastStation { set; get; } = true;
    }
}