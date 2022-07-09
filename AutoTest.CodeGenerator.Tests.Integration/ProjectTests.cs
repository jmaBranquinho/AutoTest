using AutoTest.CodeGenerator.Models;
using System.IO;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Integration
{
    public class ProjectTests
    {
        [Fact]
        public void GenerateNewProjectFile()
        {
            var project = new Project("Project123", null);
            project.Generate(Directory.GetCurrentDirectory());
        }
    }
}
