using AutoTest.CodeGenerator.Enums;

namespace AutoTest.CodeGenerator.Models
{
    public class Method
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<string> Annotations { get; set; } = Enumerable.Empty<string>();

        public virtual IEnumerable<MethodModifiers> Modifiers { get; set; } = Enumerable.Empty<MethodModifiers>();

        public virtual string ReturnType { get; set; }

        public virtual IEnumerable<(string Name, string Type)> Parameters { get; set; } = Enumerable.Empty<(string Name, string Type)>();

        public virtual string Body { get; set; }
    }
}
