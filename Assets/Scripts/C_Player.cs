using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PlayerMovState { Free, Ground, Swinging };
public enum E_PlayerInputState { Ground, Free, Swinging };
public enum E_GrapplingState { Attached, Detached };
public class C_Player : C_WorldObject {
	public float Xaccel = 10;
	public KeyCode XPos = KeyCode.RightArrow, XNeg = KeyCode.LeftArrow, YPos = KeyCode.UpArrow, YNeg = KeyCode.DownArrow;
	public float MaxSpeed;
	public float AngularAccel;
	public float RadiusDelta;
    public E_PlayerMovState PlayerMovState;
	public E_PlayerInputState PlayerInputState;
	public E_GrapplingState GrapplingState;
}
