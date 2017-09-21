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
		return angle;
	}

	public static double GetHyp(double x, double y){
		return Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
	}
}
