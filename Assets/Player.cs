using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerMovState { Free, Ground, Swinging };
enum PlayerInputState { Ground, Free, Swinging };
enum GrapplingState { Attached, Dettached };
public class Player : C_WorldObject {
	KeyCode XPos, XNeg, YPos, YNeg;
	PlayerMovState PendulumState;

}
