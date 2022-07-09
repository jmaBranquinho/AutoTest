namespace AutoTest.CodeGenerator.Models
{
    public class Namespace
    {
        public Namespace Parent { get; set; }
        public string NamespaceName { get; set; }
        public IEnumerable<Class> Classes { get; set; }

        public Namespace(string namespaceName, IEnumerable<Class> classes, Namespace parent)
        {
            NamespaceName = namespaceName;
            Classes = classes;
            Parent = parent;
        }

        public Namespace(string namespaceName, IEnumerable<Class> classes)
        {
            NamespaceName = namespaceName;
            Classes = classes;
        }

        public void ToFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("Directory does not exist!");// TODO
            }

            var @namespace = GetNamespaceString(this);
            foreach (var @class in Classes)
            {
                @class.Namespace = @namespace;
                @class.ToFile(path);
            }
        }

        private static string GetNamespaceString(Namespace @namespace) 
            => @namespace.Parent is null ? @namespace.NamespaceName : GetNamespaceString(@namespace.Parent).Concat($".{@namespace.NamespaceName}").ToString();
    }
}
