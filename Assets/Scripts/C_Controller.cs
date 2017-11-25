using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C_Controller {
	public Dictionary<string, string> AxisLookUp = new Dictionary<string, string>();
	static string[] Axes = {

		//Inputs for movement with left stick of controller

		"Vertical",
		"Horizontal",

		//Inputs for firing/releasing grappling hook

		"Fire1",
		"Fire2",

		//Inputs for aiming with right stick of controller

		"Horizontal_r",
		"Vertical_r",

		//Inputs for Attacks, note: in air you will only have access to strong attacks

		"LightAttack1",
		"StrongAttack1",
		"LightAttack2",
		"StrongAttack2",

		//Input for block

		"Block"
	};
	public C_Controller(int id) {
		foreach (string str in Axes) {
			AxisLookUp.Add (str, str + "_" + id);
		}
	}

	public float GetAxis(string name){
		return Input.GetAxis (AxisLookUp [name]);
	}

	public bool GetButtonDown(string name){
		return Input.GetButtonDown (AxisLookUp [name]);
	}
};
