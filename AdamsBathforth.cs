public abstract class AdamsBathforth  {

	int nEquations;
	double [] store;
	double [] k1;
	double [] k2;
	double [] k3;
	double [] k4;
	double [] ym1;
	double [] ym2;
	double [] ym3;
	double [] P;
	double [] dm1;
	double [] dm2;
	double [] dm3;
	double [] dp1;
	int abmSteps =0;
	double abmRms2;

	public AdamsBathforth() {
		Init (1);
	}

	public double [] getK3() {
		return k3;
	}
	
	public void Init (int nEquations) {
		// set up temp arrays
		this.nEquations = nEquations;
		store = new double[nEquations];
		k1 = new double[nEquations];
		k2 = new double[nEquations];
		k3 = new double[nEquations];
		k4 = new double[nEquations];
		ym1 = new double[nEquations];
		ym2 = new double[nEquations];
		ym3 = new double[nEquations];
		P = new double[nEquations];
		dm1 = new double[nEquations];
		dm2 = new double[nEquations];
		dm3 = new double[nEquations];
		dp1 = new double[nEquations];
		abmSteps = 0;
	}

	public abstract void RatesOfChange(double[] x, double[] xdot, double t);
	
	public void EulerStep(double [] x, double t, double h) {
		RatesOfChange(x,k1,t);
		for(int i=0;i<nEquations;i++) {
			x[i] += k1[i]*h;
		}
	}
	
	public double RK4Step(double [] x, double t, double h) {
		RatesOfChange (x,k1,t);
		for (int i = 0; i < nEquations; i++) {
			store [i] = x [i] + k1 [i] * h / 2.0;
		}
		RatesOfChange (store,k2,t);
		for (int i = 0; i < nEquations; i++) {
			store [i] = x [i] + k2 [i] * h / 2.0;
		}
		RatesOfChange (store,k3,t);
		for (int i = 0; i < nEquations; i++) {
			store [i] = x [i] + k3 [i] * h;
		}
		RatesOfChange (store, k4, t);
		for (int i = 0; i < nEquations; i++) {
			x [i] = x [i] + (k1[i] +2.0*k2[i]+ 2.0*k3 [i]+k4[i]) * h/6.0;
		}
		return t + h;
	}
	
	public double abmStep(double [] x, double t, double h) {
		abmRms2 = 0.0;
		if(abmSteps==0) {
			for(int i=0;i<x.Length;i++) {
				ym3[i] = x[i];
				ym2[i] = x[i];
			}
			RatesOfChange(dm3,ym3,t);
			t = RK4Step(ym2,t,h);
			RatesOfChange(dm2,ym2,t);
			for(int i=0;i<x.Length;i++) {
				x[i] = ym2[i];
			}
			abmSteps+=1;
			return 1.0;
		} else if(abmSteps==1) {
			for(int i=0;i<x.Length;i++) {
				ym1[i] = ym2[i];
			}
			t = RK4Step(ym1,t,h);
			RatesOfChange(dm1,ym1,t);
			for(int i=0;i<x.Length;i++) {
				x[i] = ym1[i];
			}
			abmSteps +=1;
			return 1.0;
		} else {
			RatesOfChange(k1,x,t);
			for(int i=0;i<x.Length;i++) {
				P[i] = x[i] + (h/24.0)*
					(55.0*k1[i]-59.0*dm1[i]+37.0*dm2[i]-9.0*dm3[i]);
			}
			RatesOfChange(dp1,P,t+h);
			abmRms2 = 0.0;
			for(int i=0;i<x.Length;i++) {
				store[i] = x[i];
				x[i] += (h/24.0)*(9*dp1[i]+19.0*k1[i]-5.0*dm1[i]+dm2[i]);
				dm3[i] = dm2[i];
				dm2[i] = dm1[i];
				dm1[i] = k1[i];
				ym3[i] = ym2[i];
				ym2[i] = ym1[i];
				ym1[i] = store[i];
				abmRms2 += (x[i]-P[i])*(x[i]-P[i])/(x[i]+P[i])/(x[i]+P[i]);
			}
			abmRms2 /= x.Length;
			if(abmSteps<5) abmSteps += 1;
			return t+h;
		}
	}

	public double abmError() {
		return abmRms2;
	}

}