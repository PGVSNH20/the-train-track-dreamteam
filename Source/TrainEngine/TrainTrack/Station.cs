namespace TrainEngine.TrainTrack
{
    public class Station : ILinkNode
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public bool IsLastStation { set; get; }
    }
}