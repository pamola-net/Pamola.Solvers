using System;
using System.Collections.Generic;
using System.Numerics;
using Accord.Math;
using System.Linq;

namespace Pamola.Solvers
{
    public class AccordBaseSolver : ISolver
    {
        public AccordBaseSolver(IReadOnlyList<Complex> initialGuess) 
        {
            InitialGuess = initialGuess;
            Tolerance = new Complex(1e-8, 1e-8);
            StopCriteria = (Y, i) => i >= 100 || Y.All(y => y.Magnitude < Tolerance.Magnitude);
        }

        public Complex Tolerance { get; set; }

        public Func<IReadOnlyList<Complex>, int, bool> StopCriteria { get; set; }

        public IReadOnlyList<Complex> InitialGuess { get; set; }


        public IReadOnlyList<Complex> Solve(IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> equations)
        {
            return IterativeSolve(equations).
                Select((Xk, k) => (Xk, k)).
                First(itk => StopCriteria(equations.Select(equation => equation(itk.Xk)).ToList(), itk.k)).
                Xk;
        }

        private IEnumerable<IReadOnlyList<Complex>> IterativeSolve(IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> funcs)
        {
            var Xk = InitialGuess.ToArray();

            while (true)
            {
                yield return Xk;

                var J = funcs.Jacobian(Xk, Tolerance).Select(j => j.ToArray()).ToArray();
                var F = funcs.Select(func => func(Xk)).ToArray();

                var Jreal = J.Re();
                var Jimag = J.Im();

                var Freal = F.Re();
                var Fimag = F.Im();

                var deltaXreal = Matrix.Solve(Jreal, Freal, true);
                var deltaXimag = Matrix.Solve(Jimag, Fimag, true);

                var XK1real = Xk.Re().Subtract(deltaXreal);
                var XK1imag = Xk.Im().Subtract(deltaXimag);

                Xk = XK1real.Zip(XK1imag, (real, imag) => new Complex(real, imag)).ToArray();
            }
        }
    }
}
