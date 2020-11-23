using Domain;

namespace Persistence
{
    public class InstanceWriter
    {
        public void Write(Instance instance, string filePath)
        {
            using var file = new System.IO.StreamWriter(filePath);
            file.WriteLine(instance.Size);
            foreach (var job in instance.Jobs)
            {
                file.WriteLine($"{job.Duration} {job.Ready} {job.Deadline} {job.Weight}");
            }
        }
    }
}
