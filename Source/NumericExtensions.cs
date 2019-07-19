using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Pamola.Solvers
{
    public static class NumericExtensions
    {
        public static Complex Derivative(
            this Func<Complex, Complex> func, 
            Complex value, 
            Complex tolerance)
        {
            var f1 = func(value + tolerance/2.0);
            var f2 = func(value - tolerance/2.0);

            return (f1 - f2) / tolerance;
        }

        public static Complex Derivative(this Func<Complex, Complex> func, Complex value) => 
            func.Derivative(value, new Complex(1e-8, 1e-8));

        public static IReadOnlyList<Complex> Gradient(
            this Func<IReadOnlyList<Complex>, Complex> func,
            IReadOnlyList<Complex> values,
            Complex tolerance) =>
            values.
                Select<Complex, Func<Complex, Complex>>((value, i) => (Complex x) => func(values.Select((currentX, j) => j == i ? x : currentX).ToList())).
                Select((f, i) => f.Derivative(values[i], tolerance)).ToList();
        

        public static IReadOnlyList<Complex> Gradient(this Func<IReadOnlyList<Complex>, Complex> func, IReadOnlyList<Complex> values) => 
            func.Gradient(values, new Complex(1e-8, 1e-8));

        public static IReadOnlyList<IReadOnlyList<Complex>> Jacobian(
            this IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> funcs, 
            IReadOnlyList<Complex> values, 
            Complex tolerance) => 
            funcs.
                Select(f => f.Gradient(values, tolerance)).ToList();
        

        public static IReadOnlyList<IReadOnlyList<Complex>> Jacobian(this IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> funcs, IReadOnlyList<Complex> values) => 
            funcs.Jacobian(values, new Complex(1e-8, 1e-8));
    }
}
