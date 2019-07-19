using Pamola.Components;
using Pamola.Solvers.UT.Components;
using System;
using System.Linq;
using System.Numerics;
using Xunit;

namespace Pamola.Solvers.UT
{
    public class AccordBaseSolverUT
    {
        [Fact]
        public void SolvesBasicCircuit()
        {
            var R = new IdealResistor(2);
            var V = new IdealDCVoltageSource(12);
            var G = new Ground();

            R.Positive.ConnectTo(V.Positive);
            R.Negative.ConnectTo(V.Negative);

            G.Terminal.ConnectTo(R.Negative.Node);

            var circuit = R.GetCircuit();

            circuit.Solve(new AccordBaseSolver(((IComponent)circuit).Variables.Select(v => v.Getter()).ToList()));

            Assert.Equal(12.0, R.Positive.Node.Voltage.Real, 4);
            Assert.Equal(0.0, R.Negative.Node.Voltage.Real, 4);

            Assert.Equal(0.0, R.Positive.Node.Voltage.Imaginary, 4);
            Assert.Equal(0.0, R.Negative.Node.Voltage.Imaginary, 4);

            Assert.Equal(6.0, R.Positive.Current.Real, 4);
            Assert.Equal(-6.0, R.Negative.Current.Real, 4);

            Assert.Equal(0.0, R.Positive.Current.Imaginary, 4);
            Assert.Equal(0.0, R.Negative.Current.Imaginary, 4);

            Assert.Equal(-6.0, V.Positive.Current.Real, 4);
            Assert.Equal(6.0, V.Negative.Current.Real, 4);

            Assert.Equal(0.0, V.Positive.Current.Imaginary, 4);
            Assert.Equal(0.0, V.Negative.Current.Imaginary, 4);

            Assert.All(((IComponent)circuit).Equations.Select(equation => equation()), result => Assert.Equal(0.0, result.Magnitude, 4));

        }
    }
}
