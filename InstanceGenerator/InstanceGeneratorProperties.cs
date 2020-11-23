namespace InstanceGenerator
{
    public class InstanceGeneratorProperties
    {
        public int[] InstanceSizes { get; set; } = { };
        public string OutputFile { get; set; }
        public string DummySolutionFile { get; set; }
        public bool GenerateDummySolutions { get; set; }
        public ProbabilityDistribution DurationDistribution { get; set; }
        public ProbabilityDistribution ReadyTimeDistribution { get; set; }
        public ProbabilityDistribution DeadlineOffsetFactorDistribution { get; set; }
        public ProbabilityDistribution WeightDistribution { get; set; }
    }
}
