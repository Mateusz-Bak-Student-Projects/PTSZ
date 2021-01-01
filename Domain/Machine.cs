namespace Domain
{
    public readonly struct Machine
    {
        public Machine(double speedFactor)
        {
            SpeedFactor = speedFactor;
        }

        public double SpeedFactor { get; }

        public override string ToString()
        {
            return $"Machine(speed={SpeedFactor})";
        }
    }
}
