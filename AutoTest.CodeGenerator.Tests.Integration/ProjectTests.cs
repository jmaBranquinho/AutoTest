using AutoTest.Core.Models;
using System;
using System.IO;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Integration
{
    public class ProjectTests : IDisposable
    {
        private readonly string _path = Path.Combine(Directory.GetCurrentDirectory(), "tests");

        public ProjectTests()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Directory.Exists(_path))
            {
                Directory.Delete(_path, true);
            }
        }

        [Fact]
        public void GenerateNewProjectFile()
        {
            var project = new Project("Project123", namespaces: null);
            project.Generate(_path);
        }
    }
}
