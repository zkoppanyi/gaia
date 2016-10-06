using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing.Optimzers
{
    public class LevenberMarquardtOptimzer
    {
        public double TolX = 1e-4;
        public double TolY = 1e-7;
        public int MaximumIterationNumber = 100;

        public LevenberMarquardtOptimzer()
        {

        }

        public double[] Run(Func<double[], double[]> fn, double[] x)
        {
            // Residual at starting point
            double[] r = fn(x);
            double S = r.Dot(r);

            int lr = r.Length;
            int lx = x.Length;

            double jepsx = TolX;

            double[,] J = JacobianApproximator(fn, x, jepsx);
            //Debug.WriteLine("Jacobian: " + J);

            int nfJ = 2;
            double[,] A = J.Transpose().Dot(J);
            double[] v = J.Transpose().Dot(r);

            // Automatic scaling
            double[,] D = Matrix.Diagonal(A.Diagonal());
            int Ddim = Math.Min(D.GetLength(0), D.GetLength(1));
            for (int i = 0; i < Ddim; i++)
            {
                if (D[i, i] == 0) D[i, i] = 1.0;
            }
            //Debug.WriteLine("Scaling: " + D);

            double Rlo = 0.25;
            double Rhi = 0.75;
            double l = 1;
            double lc = 0.75;
            int cnt = 0;

            double[] epsx = Vector.Create(lx, TolX);
            double[] epsy = Vector.Create(lr, TolY);

            double[] d = Vector.Ones(lx).Multiply(TolX);
            //Debug.WriteLine(d);

            while ((cnt < MaximumIterationNumber) && AnyGreaterThanAbsoluteOf(d, epsx) && (AnyGreaterThanAbsoluteOf(r, epsy)))
            {
                // negative solution increment
                d = SolveLinearEquationSystem(A.Add((l.Multiply(D))), v);
                double[] xd = x.Subtract(d);
                double[] rd = fn(xd);

                nfJ = nfJ + 1;
                double Sd = rd.Dot(rd);
                double dS = d.Dot((v.Multiply(2).Subtract(A.Dot(d)))); // predicted reduction

                double R = (S - Sd) / dS;
                if (R > Rhi)
                {
                    l = l / 2; // halve lambda if R too high
                    if (l < lc) l = 0;
                }
                else if (R < Rlo)  // find new nu if R too low
                {
                    double nu = (Sd - S) / (d.Dot(v)) + 2;
                    if (nu < 2)
                        nu = 2;
                    else if (nu > 10)
                    {
                        nu = 10;
                    }

                    if (l == 0)
                    {
                        double[] diag = A.Inverse().Diagonal();
                        double max_pos = diag.Max();
                        double max_neg = Math.Abs(diag.Min());
                        double abs_max = max_pos > max_neg ? max_pos : max_neg;
                        lc = 1 / abs_max;
                        l = lc;
                        nu = nu / 2;
                    }
                    l = nu * l;
                }

                cnt++;

                if (Sd < S)
                {
                    S = Sd;

                    x = xd;
                    r = rd;
                    J = JacobianApproximator(fn, x, jepsx);

                    nfJ = nfJ + 1;
                    A = J.Transpose().Dot(J);
                    v = J.Transpose().Dot(r);
                }
            }

            return x;

        }

        private double[] SolveLinearEquationSystem(double[,] A, double[] l)
        {
            return A.Solve(l);
        }

        private bool AnyGreaterThanAbsoluteOf(double[] v1, double[] v2)
        {
            for (int ik = 0; ik < v1.Length; ik++)
            {
                if (Math.Abs(v1[ik]) >= v2[ik])
                {
                    return true;
                }
            }

            return false;
        }

        public double[,] JacobianApproximator(Func<double[], double[]> fn, double[] x, double jepsx)
        {
            int lx = x.Length;
            double[] y = fn(x);
            int ly = y.Length;

            double[,] J = Matrix.Create<double>(ly, lx, 0);
            for (int k = 0; k < lx; k++)
            {
                double dx = 0.25 * jepsx;
                double[] xd = x;
                xd[k] = xd[k] + dx;
                double[] yd = fn(xd);

                J.SetColumn(k, (yd.Subtract(y)).Divide(dx));
            }

            return J;
        }
    }
}
