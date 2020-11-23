namespace Domain
{
    public readonly struct Job
    {
        public Job(int duration, int ready, int deadline, int weight)
        {
            Duration = duration;
            Ready = ready;
            Deadline = deadline;
            Weight = weight;
        }

        public int Duration { get; }
        public int Ready { get; }
        public int Deadline { get; }
        public int Weight { get; }

        public override string ToString()
        {
            return $"Job(duration={Duration}, ready={Ready}, deadline={Deadline}, weight={Weight})";
        }
    }
}
