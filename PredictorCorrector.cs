using System;

namespace Lab2Alexey {
    public class PredictorCorrector {
        static double f(double x, double y) 
        { 
            double v = y - 2 * x * x + 1; 
            return v; 
        } 

        static double predict(double x, double y, double h) 
        { 
            // value of next y(predicted) is returned 
            double y1p = y + h * f(x, y); 
            return y1p; 
        } 
        
        static double correct(double x, double y, 
            double x1, double y1, 
            double h) 
        { 
 
            double e = 0.00001; 
            double y1c = y1; 
  
            do 
            { 
                y1 = y1c; 
                y1c = y + 0.5 * h * (f(x, y) + f(x1, y1)); 
            } 
            while (Math.Abs(y1c - y1) > e); 
  
            // every iteration is correcting the value 
            // of y using average slope 
            return y1c; 
        }

        public void printFinalValues(double x, double xn, 
            double y, double h) 
        { 
  
            while (x < xn)  
            { 
                double x1 = x + h; 
                double y1p = predict(x, y, h); 
                double y1c = correct(x, y, x1, y1p, h); 
                x = x1; 
                y = y1c; 
            } 
  
            // at every iteration first the value 
            // of for next step is first predicted 
            // and then corrected. 
            Console.WriteLine("The final value of y at x = "+ 
                              x + " is : " + Math.Round(y, 5)); 
        }
    }
}