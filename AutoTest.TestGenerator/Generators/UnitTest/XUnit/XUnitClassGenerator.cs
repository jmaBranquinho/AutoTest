using AutoTest.TestGenerator.Generators.Abstracts;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit
{
    public class XUnitClassGenerator : UnitTestClassGenerator
    {
        protected override string ClassAnnotation => string.Empty;

        public XUnitClassGenerator(string className)
        {
            _className = className;
        }

        public XUnitClassGenerator WithMethod()
        {

            return this;
        }
    }
}
