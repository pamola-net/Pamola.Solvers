using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace Pamola.Solvers.UT.Components
{
    public class Ground : Element
    {
        public Ground() : base(1)
        { }

        public Terminal Terminal { get => Terminals.First(); }

        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        protected override IReadOnlyCollection<Func<Complex>> Equations => new List<Func<Complex>>() { TerminalVoltageIsZero };

        private Complex TerminalVoltageIsZero()
        {
            return Terminal.Node?.Voltage ?? new Complex();
        }
    }
}
