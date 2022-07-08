using AutoTest.CodeGenerator.Enums;

namespace AutoTest.CodeGenerator.Models
{
    public class Class
    {
        public string Name { get; set; }

        public IEnumerable<string> Imports { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> Annotations { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<ClassModifiers> Modifiers { get; set; } = Enumerable.Empty<ClassModifiers>();

        public IEnumerable<(string Name, string Type, bool IsInjected)> Parameters { get; set; } = Enumerable.Empty<(string Name, string Type, bool IsInjected)>();

        public IEnumerable<Method> Methods { get; set; } = Enumerable.Empty<Method>();
    }
}
