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

        public static Solution GenerateDummySolution(int size, int numberOfMachines)
        {
            var jobsPerMachine = size / numberOfMachines;
            var permutation = Enumerable.Range(0, numberOfMachines)
                .Select(machine => Enumerable.Range(machine * jobsPerMachine + 1, jobsPerMachine).ToArray())
                .ToArray();
            return new Solution(0, permutation);
        }

        public IEnumerable<Instance> GenerateAll()
        {
            var count = properties.InstanceSizes.Length;
            var instances = new Instance[count];
            for (var i = 0; i < count; i++)
            {
                instances[i] = GenerateInstance(properties.InstanceSizes[i], properties.NumberOfMachines);
            }
            return instances;
        }

        public Instance GenerateInstance(int size, int numberOfMachines)
        {
            var machines = new Machine[numberOfMachines];
            machines[0] = new Machine(1.0);
            for (var i = 1; i < numberOfMachines; i++)
            {
                machines[i] = GenerateMachine(size);
            }
            var jobs = new Job[size];
            for (var i = 0; i < size; i++)
            {
                jobs[i] = GenerateJob(size);
            }
            return new Instance(machines, jobs);
        }

        private Machine GenerateMachine(int instanceSize)
        {
            var speedFactor = properties.MachineSpeedDistribution.GenerateValue(random, instanceSize);
            var roundedSpeedFactor = Math.Round(speedFactor, properties.MachineSpeedPrecision);
            return new Machine(roundedSpeedFactor);
        }

        private Job GenerateJob(int instanceSize)
        {
            var duration = (int)properties.DurationDistribution.GenerateValue(random, instanceSize);
            var ready = (int)properties.ReadyTimeDistribution.GenerateValue(random, instanceSize);
            return new Job(duration, ready);
        }
    }
}
