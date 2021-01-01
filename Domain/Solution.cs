using System.Linq;

namespace Domain
{
    public readonly struct Solution
    {
        public Solution(double value, int[][] jobPermutation)
        {
            Value = value;
            JobPermutation = jobPermutation;
        }

        public double Value { get; }
        public int[][] JobPermutation { get; }

        public int[] Size => JobPermutation.Select(p => p.Length).ToArray();
    }
}
