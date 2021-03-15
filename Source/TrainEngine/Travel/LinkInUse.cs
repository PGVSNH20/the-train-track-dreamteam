namespace TrainEngine.Travel
{
    internal class LinkInUse
    {
        public string LinkId { get; set; }
        public string Direction { get; set; }
        public int UsedByTrainId { get; set; }
    }
}