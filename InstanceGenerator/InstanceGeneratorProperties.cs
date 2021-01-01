namespace InstanceGenerator
{
    public class InstanceGeneratorProperties
    {
        public int[] InstanceSizes { get; set; } = { };
        public string OutputFile { get; set; }
        public string DummySolutionFile { get; set; }
        public bool GenerateDummySolutions { get; set; }
        public int NumberOfMachines { get; set; }
        public int MachineSpeedPrecision { get; set; }
        public ProbabilityDistribution MachineSpeedDistribution { get; set; }
        public ProbabilityDistribution DurationDistribution { get; set; }
        public ProbabilityDistribution ReadyTimeDistribution { get; set; }
    }
}
