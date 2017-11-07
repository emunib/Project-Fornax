using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Trig {
	public static double GetAngle(Vector2 vec){
		double hyp = (double)vec.magnitude; 
		if (hyp == 0) {
			Debug.Log ("Error GetAngle hyp=0");
			return 0;
		}
		double angle = Math.Acos (vec.x / hyp);
		if (vec.y < 0)
			angle = (2*Math.PI) - angle;
		return angle;
	}

	public static double GetHyp(double x, double y){
		return Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
	}
}




public class Loop {
	public delegate bool rbool();
	public delegate void rvoid();
	public static void WhileOrFor(rbool condition, rvoid body, int count){
		while (condition ()) {
			if (count-- < 0) {
				throw new Exception ("Max iteration reached");
			}
			body ();
		}
	}
}
