using AutoTest.CodeGenerator.Helpers;

namespace AutoTest.CodeGenerator.Models
{
    public class Project
    {
        public string ProjectName { get; }
        public IEnumerable<Namespace> Namespaces { get; }

        public IEnumerable<string> References { get; set; }

        public Project(string projectName, IEnumerable<Namespace> namespaces)
        {
            ProjectName = projectName;
            Namespaces = namespaces ?? new List<Namespace>();
            References = new List<string>();
        }

        public void Generate(string path)
        {
            var attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                if (Directory.Exists(path))
                {
                    path += $"\\{ProjectName}.csproj";
                }
                else
                {
                    throw new Exception("Directory does not exist!");// TODO
                }
            }
            else
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
            }
            File.WriteAllText(path, GenerateProjectXml());
        }

        // TODO define .net version
        private string GenerateProjectXml()
        {
            var projectConfig = "PropertyGroup".FormatXmlContent("TargetFramework".FormatXmlContent("net6.0"));
            var references = string.Join(Environment.NewLine.Repeat(2), References.Select(@ref => "ItemGroup".FormatXmlContent("ProjectReference".FormatXmlContent(string.Empty, ("Include", @ref)))));
            var projectContent = string.Join(Environment.NewLine.Repeat(2), projectConfig, references);
            return "Project".FormatXmlContent(projectContent, ("Sdk", "Microsoft.NET.Sdk"));
        }
    }
}
