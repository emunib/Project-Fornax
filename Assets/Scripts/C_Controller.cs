using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C_Controller {
	public Dictionary<string, string> AxisLookUp = new Dictionary<string, string>();
	static string[] Axes = {
		"Vertical",
		"Horizontal",
		"Fire1",
		"Fire2",
		"Horizontal_r",
		"Vertical_r",
		"LightAttack1",
		"StrongAttack1",
		"LightAttack2",
		"StrongAttack2",
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
