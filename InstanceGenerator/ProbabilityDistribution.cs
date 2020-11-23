using System;

namespace InstanceGenerator
{
    public struct ProbabilityDistribution
    {
        public enum DistributionType
        {
            Uniform,
            Normal
        }

        /// <summary>
        /// Distribution type - uniform or normal
        /// </summary>
        public DistributionType Type { get; set; }

        /// <summary>
        /// Mean value
        /// </summary>
        public Linear Mean { get; set; }

        /// <summary>
        /// For uniform distribution defines its bounds: [Mean-Scale, Mean+Scale]
        /// For normal distribution defines its standard deviation
        /// </summary>
        public Linear Scale { get; set; }

        /// <summary>
        /// Lower bound for generated values
        /// </summary>
        public Linear ClampMin { get; set; }

        /// <summary>
        /// Upper bound for generated values
        /// </summary>
        public Linear ClampMax { get; set; }

        public double GenerateValue(Random generator, double parameter = 0.0)
        {
            var mean = Mean.Value(parameter);
            var scale = Scale.Value(parameter);
            var clampMin = ClampMin.Value(parameter);
            var clampMax = ClampMax.Value(parameter);

            var random = GetRandomValue(generator, mean, scale);
            return Math.Clamp(random, clampMin, clampMax);
        }

        private double GetRandomValue(Random generator, double mean, double scale)
        {
            return Type switch
            {
                DistributionType.Uniform => SampleUniform(generator, mean - scale, mean + scale),
                DistributionType.Normal => SampleGaussian(generator, mean, scale),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static double SampleUniform(Random generator, double min, double max)
        {
            var random = generator.NextDouble();
            var width = max - min;
            return min + random * width;
        }

        private static double SampleGaussian(Random generator, double mean, double stddev)
        {
            var x1 = 1 - generator.NextDouble();
            var x2 = 1 - generator.NextDouble();
            var y = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y * stddev + mean;
        }

        public override string ToString()
        {
            return $"ProbabilityDistribution(Type={Type}, Mean={Mean}, Scale={Scale}, ClampMin={ClampMin}, ClampMax={ClampMax})";
        }
    }
}
