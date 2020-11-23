using System;
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
                var value = int.Parse(file.ReadLine() ?? string.Empty);
                var jobPermutation = file.ReadLine()?
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                return new Solution(value, jobPermutation);
            }
            catch (FormatException ex)
            {
                throw new FileFormatException($"Invalid file format, file={filePath}", ex);
            }
        }
    }
}
