using System.Linq;
using Domain;

namespace Persistence
{
    public class InstanceWriter
    {
        public void Write(Instance instance, string filePath)
        {
            using var file = new System.IO.StreamWriter(filePath);
            file.WriteLine(instance.Size);
            file.WriteLine(string.Join(' ', instance.Machines.Select(m => m.SpeedFactor)));
            foreach (var job in instance.Jobs)
            {
                file.WriteLine($"{job.Duration} {job.Ready}");
            }
        }
    }
}
