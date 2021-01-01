using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace InstanceGenerator
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

            var properties = new InstanceGeneratorProperties();
            configuration.Bind("InstanceGenerator", properties);
            var persistenceProperties = new PersistenceProperties();
            configuration.Bind("Persistence", persistenceProperties);

            var instanceGenerator = new InstanceGenerator(properties, new Random());
            var instanceWriter = new InstanceWriter();
            var solutionWriter = new SolutionWriter();
            var instances = instanceGenerator.GenerateAll();

            foreach (var instance in instances)
            {
                var fileName = string.Format(properties.OutputFile, instance.Size);
                var filePath = Path.Join(persistenceProperties.InstanceDirectory, fileName);
                instanceWriter.Write(instance, filePath);

                if (properties.GenerateDummySolutions)
                {
                    var dummySolution = InstanceGenerator.GenerateDummySolution(instance.Size, properties.NumberOfMachines);
                    var solutionFileName = string.Format(properties.DummySolutionFile, instance.Size);
                    var solutionFilePath = Path.Join(persistenceProperties.SolutionDirectory, solutionFileName);
                    solutionWriter.Write(dummySolution, solutionFilePath);
                }
            }
        }
    }
}
