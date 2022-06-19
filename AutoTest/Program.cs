using AutoTest.CodeGenerator.Generators;
using AutoTest.TestGenerator.Generators.XUnit;

var method = @"
public int Return1()
{
    return 1;
}
".Trim();

var ss = ClassGenerator.NewClass()
    .WithClassName("UnitTestClass")
    .WithAnnotations(Enumerable.Empty<string>())
    .WithMethod(method)
    .Generate();

Console.WriteLine(ss);

var @class = new XUnitClassGenerator("SampleTest");
var str = @class.Generate();
Console.WriteLine(str);