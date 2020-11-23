using System;
using System.Linq;
using Domain;

namespace Persistence
{
    public class InstanceReader
    {
        public Instance Read(string filePath)
        {
            using var file = new System.IO.StreamReader(filePath);
            try
            {
                var size = int.Parse(file.ReadLine() ?? string.Empty);
                var jobs = new Job[size];
                for (var i = 0; i < size; i++)
                {
                    var jobParams = file.ReadLine()?
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
                    if (jobParams == null || jobParams.Length != 4)
                        throw new FileFormatException($"Invalid file format, file={filePath}");
                    jobs[i] = new Job(jobParams[0], jobParams[1], jobParams[2], jobParams[3]);
                }
                return new Instance(jobs);
            }
            catch (FormatException ex)
            {
                throw new FileFormatException($"Invalid file format, file={filePath}", ex);
            }
        }
    }
}
