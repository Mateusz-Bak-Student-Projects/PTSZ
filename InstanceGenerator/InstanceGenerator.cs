using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace InstanceGenerator
{
    public class InstanceGenerator
    {
        private readonly InstanceGeneratorProperties properties;
        private readonly Random random;

        public InstanceGenerator(InstanceGeneratorProperties properties, Random random)
        {
            this.properties = properties;
            this.random = random;
        }

        public static Solution GenerateDummySolution(int size)
        {
            return new Solution(0, Enumerable.Range(1, size).ToArray());
        }

        public IEnumerable<Instance> GenerateAll()
        {
            var count = properties.InstanceSizes.Length;
            var instances = new Instance[count];
            for (var i = 0; i < count; i++)
            {
                instances[i] = GenerateInstance(properties.InstanceSizes[i]);
            }
            return instances;
        }

        public Instance GenerateInstance(int size)
        {
            var jobs = new Job[size];
            for (var i = 0; i < size; i++)
            {
                jobs[i] = GenerateJob(size);
            }
            return new Instance(jobs);
        }

        private Job GenerateJob(int instanceSize)
        {
            var duration = (int)properties.DurationDistribution.GenerateValue(random, instanceSize);
            var ready = (int)properties.ReadyTimeDistribution.GenerateValue(random, instanceSize);
            var deadline = ready + (int)(duration * properties.DeadlineOffsetFactorDistribution.GenerateValue(random, instanceSize));
            var weight = (int)properties.WeightDistribution.GenerateValue(random, instanceSize);
            return new Job(duration, ready, deadline, weight);
        }
    }
}
