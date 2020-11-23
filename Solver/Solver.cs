using System;
using System.Collections.Generic;
using Domain;

namespace Solver
{
    public class Solver
    {
        private readonly struct IndexedJob
        {
            public int Index { get; }
            public Job Value { get; }

            public IndexedJob(int index, Job value)
            {
                Index = index;
                Value = value;
            }
        }

        public Solution Solve(Instance instance)
        {
            var indexedJobs = new IndexedJob[instance.Size];
            for (var i = 0; i < instance.Size; i++)
            {
                indexedJobs[i] = new IndexedJob(i + 1, instance.Jobs[i]);
            }
            Array.Sort(indexedJobs, (a, b) => -GetJobScore(a.Value).CompareTo(GetJobScore(b.Value)));
            var sequence = new SortedList<(int, int, int), IndexedJob>(instance.Size);
            var leftover = new List<IndexedJob>(instance.Size);
            foreach (var job in indexedJobs)
            {
                if (WillBeDelayed(sequence, job.Value))
                {
                    leftover.Add(job);
                }
                else
                {
                    sequence.Add((job.Value.Ready, -job.Value.Weight, job.Index), job);
                }
            }

            var permutation = new int[instance.Size];
            var k = 0;
            foreach (var job in sequence.Values)
            {
                permutation[k++] = job.Index;
            }

            foreach (var job in leftover)
            {
                permutation[k++] = job.Index;
            }
            return new Solution(CalculateValue(permutation, instance), permutation);
        }

        private static double GetJobScore(Job job)
        {
            return (double)job.Weight / job.Duration;
        }

        private static bool WillBeDelayed(SortedList<(int, int, int), IndexedJob> sequence, Job jobToInsert)
        {
            var clock = 0;
            var inserted = false;
            foreach (var indexedJob in sequence.Values)
            {
                var job = indexedJob.Value;
                if (!inserted && (jobToInsert.Ready < job.Ready ||
                                  jobToInsert.Ready == job.Ready && jobToInsert.Weight > job.Weight))
                {
                    inserted = true;
                    clock = Math.Max(clock, jobToInsert.Ready) + jobToInsert.Duration;
                    if (clock > jobToInsert.Deadline) return true;
                }
                clock = Math.Max(clock, job.Ready) + job.Duration;
                if (clock > job.Deadline) return true;
            }

            return false;
        }

        private int CalculateValue(IEnumerable<int> permutation, Instance instance)
        {
            var value = 0;
            var clock = 0;
            foreach (var jobIndex in permutation)
            {
                var job = instance.Jobs[jobIndex - 1];
                clock = Math.Max(clock, job.Ready) + job.Duration;
                if (clock > job.Deadline) value += job.Weight;
            }

            return value;
        }
    }
}
