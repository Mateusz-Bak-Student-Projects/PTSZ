namespace Domain
{
    public readonly struct Instance
    {
        public Instance(Machine[] machines, Job[] jobs)
        {
            Machines = machines;
            Jobs = jobs;
        }

        public Machine[] Machines { get; }
        public Job[] Jobs { get; }

        public int Size => Jobs.Length;

        public override string ToString()
        {
            var machines = string.Join(",\n\t", Machines);
            var jobs = string.Join(",\n\t", Jobs);
            return $"Instance(\n\t{machines}\n\t{jobs}\n)";
        }
    }
}
