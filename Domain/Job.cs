namespace Domain
{
    public readonly struct Job
    {
        public Job(int duration, int ready)
        {
            Duration = duration;
            Ready = ready;
        }

        public int Duration { get; }
        public int Ready { get; }

        public override string ToString()
        {
            return $"Job(duration={Duration}, ready={Ready})";
        }
    }
}
