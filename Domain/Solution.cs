namespace Domain
{
    public readonly struct Solution
    {
        public Solution(int value, int[] jobPermutation)
        {
            Value = value;
            JobPermutation = jobPermutation;
        }

        public int Value { get; }
        public int[] JobPermutation { get; }

        public int Size => JobPermutation.Length;
    }
}
