using System;

namespace Lab2Alexey {
    class Program {
        static void Main(string[] args) {
            
            // Runge-Kutta 4th order
            double x0 = 0, y = 1, x = 2, h = 0.2;
            Console.WriteLine("\nThe value of y"
                              + " at x is : "
                              + RungeKutta4.rungeKutta(x0, y, x, h));
            
            
            // Predictor-Corrector
            double x1 = 0, y1 = 0.5;
            double xn = 1;
            double h1 = 0.2;

            PredictorCorrector predictor = new PredictorCorrector();
            predictor.printFinalValues(x1, xn, y1, h1); 
            
            // Adams Bathforth

            AdamsBathforth adamsBathforth = new SHO();
            double[] x2 = {1, 2, 3};
            double t = 5;
            double h3 = 0.2;
            adamsBathforth.abmStep(x2, t, h3);

        }
    }
}