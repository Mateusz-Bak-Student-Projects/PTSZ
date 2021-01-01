using Domain;
using System;
using System.Collections.Generic;

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
            var sequence = new List<List<IndexedJob>>(instance.Machines.Length);
            for (var i = 0; i < instance.Machines.Length; i++)
            {
                sequence.Add(new List<IndexedJob>(instance.Size));
            }
            foreach (var job in indexedJobs)
            {
                var m = GetBestMachineForJob(sequence, job.Value, instance);
                AddToMachine(sequence, m, instance.Machines[m].SpeedFactor, job);
            }

            var permutation = new int[instance.Machines.Length][];
            for (var i = 0; i < instance.Machines.Length; i++)
            {
                permutation[i] = new int[sequence[i].Count];
                var j = 0;
                foreach (var job in sequence[i])
                {
                    permutation[i][j++] = job.Index;
                }
            }
            return new Solution(CalculateValue(permutation, instance), permutation);
        }

        private static double GetJobScore(Job job)
        {
            return job.Duration;
        }

        private bool GoesBefore(Job first, Job second, double clock)
        {
            if (first.Ready <= clock && second.Ready <= clock || first.Ready == second.Ready)
            {
                return first.Duration <= second.Duration;
            }
            return first.Ready < second.Ready;
        }

        private int GetBestMachineForJob(List<List<IndexedJob>> sequence, in Job jobToInsert, Instance instance)
        {
            var best = 0;
            var bestValue = 0.0;
            for (var insertTo = 0; insertTo < sequence.Count; insertTo++)
            {
                var value = 0.0;
                var inserted = false;
                for (var i = 0; i < sequence.Count; i++)
                {
                    var jobPermutation = sequence[i];
                    var inverseSpeedFactor = 1.0 / instance.Machines[i].SpeedFactor;
                    var clock = 0.0;
                    foreach (var indexedJob in jobPermutation)
                    {
                        var job = indexedJob.Value;
                        if (i == insertTo && !inserted && GoesBefore(jobToInsert, job, clock))
                        {
                            inserted = true;
                            clock = Math.Max(clock, jobToInsert.Ready) + jobToInsert.Duration * inverseSpeedFactor;
                            value += clock - jobToInsert.Ready;
                        }
                        clock = Math.Max(clock, job.Ready) + job.Duration * inverseSpeedFactor;
                        value += clock - job.Ready;
                    }
                    if (i == insertTo && !inserted)
                    {
                        inserted = true;
                        clock = Math.Max(clock, jobToInsert.Ready) + jobToInsert.Duration * inverseSpeedFactor;
                        value += clock - jobToInsert.Ready;
                    }
                }

                if (value < bestValue || bestValue == 0.0)
                {
                    bestValue = value;
                    best = insertTo;
                }
            }
            return best;
        }

        private void AddToMachine(List<List<IndexedJob>> sequence, int machineIndex, double speedFactor, in IndexedJob jobToInsert)
        {
            var jobPermutation = sequence[machineIndex];
            var inverseSpeedFactor = 1.0 / speedFactor;
            var clock = 0.0;
            for (var i = 0; i < jobPermutation.Count; i++)
            {
                var job = jobPermutation[i].Value;
                if (GoesBefore(jobToInsert.Value, job, clock))
                {
                    jobPermutation.Insert(i, jobToInsert);
                    sequence[machineIndex] = jobPermutation;
                    return;
                }
                clock = Math.Max(clock, job.Ready) + job.Duration * inverseSpeedFactor;
            }
            jobPermutation.Add(jobToInsert);
            sequence[machineIndex] = jobPermutation;
        }

        private double CalculateValue(int[][] permutation, Instance instance)
        {
            var value = 0.0;
            for (var i = 0; i < instance.Machines.Length; i++)
            {
                var jobPermutation = permutation[i];
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
