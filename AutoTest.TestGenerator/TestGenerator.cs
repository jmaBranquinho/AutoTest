using AutoTest.CodeInterpreter.Models.Wrappers;

namespace AutoTest.TestGenerator
{
    public class TestGenerator
            : INameSelectionStage,
            ITestingFrameworkSelectionStage,
            IMockingFrameworkSelectionStage,
            IGenerationStage
    {
        private ClassWrapper _classWrapper;

        private string _classname;
        private bool _isUsingDefaultName;

        private TestingFrameworks _testingFramework;
        private MockingFrameworks _mockingFramework;

        private List<string> _methods;

        private TestGenerator(ClassWrapper classWrapper)
        {
            _classWrapper = classWrapper;
            _methods = new List<string>();
        }

        public static TestGenerator NewTestClass(ClassWrapper classWrapper) => new TestGenerator(classWrapper);

        public string GenerateTo(string path)
        {
            // TODO handle the path and write to file
            throw new NotImplementedException();
        }

        public ITestingFrameworkSelectionStage WithClassName(string name)
        {
            _classname = name;
            return this;
        }

        public IMockingFrameworkSelectionStage UsingMsTesting()
        {
            _testingFramework = TestingFrameworks.MsTesting;
            return this;
        }

        public IMockingFrameworkSelectionStage UsingNUnit()
        {
            _testingFramework = TestingFrameworks.NUnit;
            return this;
        }

        public IMockingFrameworkSelectionStage UsingXUnit()
        {
            _testingFramework = TestingFrameworks.XUnit;
            return this;
        }

        public IGenerationStage UsingMoq()
        {
            _mockingFramework = MockingFrameworks.Moq;
            return this;
        }

        public IGenerationStage UsingNSubstitute()
        {
            _mockingFramework = MockingFrameworks.NSubstitute;
            return this;
        }

    }

    public interface INameSelectionStage
    {
        public ITestingFrameworkSelectionStage WithClassName(string name);
    }

    public enum TestingFrameworks { MsTesting, NUnit, XUnit }

    public interface ITestingFrameworkSelectionStage
    {
        public IMockingFrameworkSelectionStage UsingMsTesting();

        public IMockingFrameworkSelectionStage UsingNUnit();

        public IMockingFrameworkSelectionStage UsingXUnit();
    }

    public enum MockingFrameworks { Moq, NSubstitute }

    public interface IMockingFrameworkSelectionStage
    {
        public IGenerationStage UsingMoq();

        public IGenerationStage UsingNSubstitute();
    }

    public interface IGenerationStage
    {
        public string GenerateTo(string path);
    }
}
