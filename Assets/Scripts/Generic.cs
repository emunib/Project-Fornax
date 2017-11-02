using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Trig {
	public static double GetAngle(double adjacent, double opposite){
		double angle = Math.Atan (opposite / adjacent);
		if (adjacent < 0) {
			angle += Math.PI;
		} else if (opposite < 0) {
			angle += 2 * Math.PI;
		}
		if ((angle > Math.PI * 2) || (angle < 0)){
			throw new Exception();
		} else if (double.IsNaN(angle)){
			throw new Exception ("adjacent: " + adjacent + " opposite: " + opposite);
		}
		return angle;
	}

	public static double GetHyp(double x, double y){
		return Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
	}
}

