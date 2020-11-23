using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Solver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            var properties = new SolverProperties();
            configuration.Bind("Solver", properties);
            var persistenceProperties = new PersistenceProperties();
            configuration.Bind("Persistence", persistenceProperties);

            var instanceFiles = Directory.GetFiles(persistenceProperties.InstanceDirectory);
            var instanceReader = new InstanceReader();
            var solutionWriter = new SolutionWriter();
            var solver = new Solver();

            using var metrics = new StreamWriter(properties.MetricsFile);
            metrics.WriteLine("solution,time");
            var stopwatch = new Stopwatch();

            foreach (var instanceFile in instanceFiles)
            {
                var instance = instanceReader.Read(instanceFile);
                stopwatch.Restart();
                var solution = solver.Solve(instance);
                stopwatch.Stop();
                var fileName = Path.GetFileName(instanceFile).Replace("in", "out");
                var filePath = Path.Join(persistenceProperties.SolutionDirectory, fileName);
                solutionWriter.Write(solution, filePath);
                metrics.WriteLine($"{fileName},{stopwatch.ElapsedMilliseconds}");
            }
        }
    }
}
