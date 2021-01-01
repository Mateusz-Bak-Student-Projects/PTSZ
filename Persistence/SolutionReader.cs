using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Persistence
{
    public class SolutionReader
    {
        public Solution Read(string filePath)
        {
            using var file = new System.IO.StreamReader(filePath);
            try
            {
                var value = double.Parse(file.ReadLine() ?? string.Empty);
                var jobPermutation = new List<int[]>();
                while (!file.EndOfStream)
                {
                    var permutation = file.ReadLine()?
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
                    jobPermutation.Add(permutation);
                }
                return new Solution(value, jobPermutation.ToArray());
            }
            catch (FormatException ex)
            {
                throw new FileFormatException($"Invalid file format, file={filePath}", ex);
            }
        }
    }
}
