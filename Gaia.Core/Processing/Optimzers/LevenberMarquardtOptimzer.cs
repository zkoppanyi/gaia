using MathNet.Numerics.LinearAlgebra;
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

        public Vector<double> Run(Func<Vector<double>, Vector<double>> fn, Vector<double> x)
        {
            // Residual at starting point
            Vector<double> r = fn(x);
            double S = r * r;

            int lr = r.Count;
            int lx = x.Count;

            Matrix<double> J = JacobianApproximator(fn, x);
            //Debug.WriteLine("Jacobian: " + J);

            int nfJ = 2;
            Matrix<double> A = J.Transpose() * J;
            Vector<double> v = J.Transpose() * r;

            // Automatic scaling
            Matrix<double> D = Matrix<double>.Build.DiagonalOfDiagonalVector(A.Diagonal());
            int Ddim = Math.Min(D.ColumnCount, D.RowCount);
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

            Vector<double> epsx = Vector<double>.Build.Dense(lx, TolX);
            Vector<double> epsy = Vector<double>.Build.Dense(lr, TolY);

            Vector<double> d = Vector<double>.Build.Dense(lx, TolX);
            //Debug.WriteLine(d);

            while ((cnt < MaximumIterationNumber) && AnyGreaterThanAbsoluteOf(d, epsx) && (AnyGreaterThanAbsoluteOf(r, epsy)))
            {
                // negative solution increment
                d = SolveLinearEquationSystem((A + (l * D)), v);
                Vector<double> xd = x - d;
                Vector<double> rd = fn(xd);

                nfJ = nfJ + 1;
                double Sd = rd * rd;
                double dS = d * (2 * v - A * d); // predicted reduction

                double R = (S - Sd) / dS;
                if (R > Rhi)
                {
                    l = l / 2; // halve lambda if R too high
                    if (l < lc) l = 0;
                }
                else if (R < Rlo)  // find new nu if R too low
                {
                    double nu = (Sd - S) / (d * v) + 2;
                    if (nu < 2)
                        nu = 2;
                    else if (nu > 10)
                    {
                        nu = 10;
                    }

                    if (l == 0)
                    {
                        lc = 1 / A.Inverse().Diagonal().AbsoluteMaximum();
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
                    J = JacobianApproximator(fn, x);

                    nfJ = nfJ + 1;
                    A = J.Transpose() * J;
                    v = J.Transpose() * r;
                }
            }

            return x;

        }

        private Vector<double> SolveLinearEquationSystem(Matrix<double> A, Vector<double> l)
        {
            return A.Solve(l);
        }

        private bool AnyGreaterThanAbsoluteOf(Vector<double> v1, Vector<double> v2)
        {
            for (int ik = 0; ik < v1.Count; ik++)
            {
                if (Math.Abs(v1[ik]) >= v2[ik])
                {
                    return true;
                }
            }

            return false;
        }

        double jepsx = 1e-7; // TODO: should be a vector for each parameter
        public Matrix<double> JacobianApproximator(Func<Vector<double>, Vector<double>> fn, Vector<double> x)
        {
            int lx = x.Count;
            Vector<double> y = fn(x);
            int ly = y.Count;

            Matrix<double> J = Matrix<double>.Build.Dense(ly, lx);
            for (int k = 0; k < lx; k++)
            {
                double dx = 0.25 * jepsx;
                Vector<double> xd = x;
                xd[k] = xd[k] + dx;
                Vector<double> yd = fn(xd);

                J.SetColumn(k, (yd - y) / dx);
            }

            return J;
        }
    }
}
