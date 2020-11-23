using System;
using System.Linq;
using Domain;

namespace SolutionVerifier
{
    public class SolutionVerifier
    {
        public int Verify(Instance instance, Solution solution)
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
            var size = solution.Size;
            var set = solution.JobPermutation.ToHashSet();
            return set.Min() == 1 && set.Max() == size && set.Count == size;
        }

        private int CalculateValue(Solution solution, Instance instance)
        {
            var value = 0;
            var clock = 0;
            foreach (var jobIndex in solution.JobPermutation)
            {
                var job = instance.Jobs[jobIndex - 1];
                clock = Math.Max(clock, job.Ready) + job.Duration;
                if (clock > job.Deadline) value += job.Weight;
            }

            return value;
        }
    }
}
