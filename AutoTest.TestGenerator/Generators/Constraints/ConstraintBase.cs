using AutoTest.TestGenerator.Generators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    // TODO: split into several constrains under a base or interface
    public abstract class ConstraintBase : IConstraint
    {
        public abstract object Generate();
    }
}
