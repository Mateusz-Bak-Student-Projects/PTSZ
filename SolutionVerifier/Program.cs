using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace SolutionVerifier
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            PersistenceProperties.SetCultureInfo();

            var properties = new SolutionVerifierProperties();
            configuration.Bind("SolutionVerifier", properties);
            var persistenceProperties = new PersistenceProperties();
            configuration.Bind("Persistence", persistenceProperties);

            var instanceFiles = Directory.GetFiles(persistenceProperties.InstanceDirectory);
            var solutionFiles = Directory.GetFiles(persistenceProperties.SolutionDirectory);
            if (solutionFiles.Length > instanceFiles.Length)
            {
                Console.WriteLine("Found more solution files than instance files, aborting");
                return;
            }

            using var file = new StreamWriter(properties.ResultsFile);
            file.WriteLine("instance,solution,value,expected");

            var instanceReader = new InstanceReader();
            var solutionReader = new SolutionReader();
            var verifier = new SolutionVerifier();

            for (var i = 0; i < solutionFiles.Length; i++)
            {
                var instance = instanceReader.Read(instanceFiles[i]);
                var solution = solutionReader.Read(solutionFiles[i]);

                var instanceFile = Path.GetFileName(instanceFiles[i]);
                var solutionFile = Path.GetFileName(solutionFiles[i]);
                Console.WriteLine($"{instanceFile} {solutionFile}");

                var result = verifier.Verify(instance, solution);
                file.WriteLine($"{instanceFile},{solutionFile},{solution.Value},{result}");
            }
        }
    }
}
