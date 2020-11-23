namespace Domain
{
    public readonly struct Instance
    {
        public Instance(Job[] jobs)
        {
            Jobs = jobs;
        }

        public Job[] Jobs { get; }

        public int Size => Jobs.Length;

        public override string ToString()
        {
            return $"Instance(\n\t{string.Join(",\n\t", Jobs)}\n)";
        }
    }
}
