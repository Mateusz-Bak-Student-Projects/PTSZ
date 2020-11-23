using Domain;

namespace Persistence
{
    public class SolutionWriter
    {
        public void Write(Solution solution, string filePath)
        {
            using var file = new System.IO.StreamWriter(filePath);
            file.WriteLine(solution.Value);
            file.WriteLine(string.Join(' ', solution.JobPermutation));
        }
    }
}
