using AutoTest.CodeInterpreter.Models.Wrappers;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public abstract class StatementAnalyzerBase
    {
        protected Task<T2> NewRequest<T1, T2>(SyntaxNode syntaxNode, ExecutionPath executionPath, ISender mediator)
        {
            var instance = (StatementAnalysisRequest<T1, T2>)Activator.CreateInstance(typeof(StatementAnalysisRequest<T1, T2>), new object[] { });

            instance.Statement = syntaxNode;
            instance.ExecutionPath = executionPath;
            //instance.ReferredType = (T1?)Activator.CreateInstance(typeof(T1), null);
            //instance.ReturnedType = (T2?)Activator.CreateInstance(typeof(T2), null);

            return mediator.Send(instance);
        }
    }
}
