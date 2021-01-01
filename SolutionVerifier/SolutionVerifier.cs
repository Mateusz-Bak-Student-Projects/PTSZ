using System;
using System.Linq;
using Domain;

namespace SolutionVerifier
{
    public class SolutionVerifier
    {
        public double Verify(Instance instance, Solution solution)
        {
            if (!ValidateIntegrity(solution))
            {
                Console.WriteLine("Solution is not a valid permutation");
                return -1;
            }

            var calculatedValue = CalculateValue(solution, instance);
            if (solution.Value != calculatedValue)
            {
                Console.WriteLine($"Solution is incorrect: value={solution.Value}, expected={calculatedValue}");
                return calculatedValue;
            }

            Console.WriteLine($"Solution is correct: value={solution.Value}");
            return calculatedValue;
        }

        private bool ValidateIntegrity(Solution solution)
        {
            var size = solution.Size.Sum();
            var set = solution.JobPermutation
                .SelectMany(p => p)
                .ToHashSet();
            return set.Min() == 1 && set.Max() == size && set.Count == size;
        }

        private double CalculateValue(Solution solution, Instance instance)
        {
            var value = 0.0;
            for (var i = 0; i < instance.Machines.Length; i++)
            {
                var jobPermutation = solution.JobPermutation[i];
                var inverseSpeedFactor = 1.0 / instance.Machines[i].SpeedFactor;
                var clock = 0.0;
                foreach (var jobIndex in jobPermutation)
                {
                    var job = instance.Jobs[jobIndex - 1];
                    clock = Math.Max(clock, job.Ready) + job.Duration * inverseSpeedFactor;
                    value += clock - job.Ready;
                }
            }

            return value / instance.Size;
        }
    }
}
