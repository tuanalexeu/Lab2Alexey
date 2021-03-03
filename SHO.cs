namespace Lab2Alexey {
    public class SHO : AdamsBathforth {
        double k = 1;
        double m = 1;
        public double [] x;

        public void SetIC(double x0, double v0) {
            x = new double[2];
            Init (2);
            x [0] = x0;
            x [1] = v0;
        }

        public override void RatesOfChange (double[] x, double[] xdot, double t)
        {
            xdot [0] = x [1];
            xdot [1] = -(k / m) * x [0];
        }
    }
}