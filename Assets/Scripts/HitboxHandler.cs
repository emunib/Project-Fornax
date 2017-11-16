using UnityEngine;
using System.Collections;

public class HitboxHandler: MonoBehaviour

{


	//The term "hitbox" in this case refers to colliders on certain moves that will cause
	//damage to enemy players, e.g we will place hitboxes on things like punches and kicks.

	//The term "hurtbox" refers to colliders that, when in collision with an opponents hitbox, will
	//cause damage to oneself, e.g we will place one hurtbox on the majority of the character's body
	//in every frame. 

	//We have to draw hitboxes on each frame individually, this will be messy but
	//need to set the frames we are handling in question in the editor; one for every frame, and more since
	//we are manually drawing hurtboxes as well.

	//Most of this script is just declaring variables for the various hit/hurtboxes; the actual code for this script
	//can be found about 3/4ths the way down.


	//Idle animation hurtboxes.

	public PolygonCollider2D idle1;
	public PolygonCollider2D idle2;
	public PolygonCollider2D idle3;
	public PolygonCollider2D idle4;
	public PolygonCollider2D idle5;
	public PolygonCollider2D idle6;
	public PolygonCollider2D idle7;
	public PolygonCollider2D idle8;
	public PolygonCollider2D idle9;


	//Walking animation hurtboxes.

	public PolygonCollider2D walk1;
	public PolygonCollider2D walk2;
	public PolygonCollider2D walk3;
	public PolygonCollider2D walk4;
	public PolygonCollider2D walk5;

	//Jumping animation hurtboxes.

	public PolygonCollider2D jump1;
	public PolygonCollider2D jump2;
	public PolygonCollider2D jump3;
	public PolygonCollider2D jump4;
	public PolygonCollider2D jump5;
	public PolygonCollider2D jump7;


	//Ninja run/dash animation hurtboxes.

	public PolygonCollider2D dash1;
	public PolygonCollider2D dash2;
	public PolygonCollider2D dash3;
	public PolygonCollider2D dash4;
	public PolygonCollider2D dash5;
	public PolygonCollider2D dash6;
	public PolygonCollider2D dash7;
	public PolygonCollider2D dash8;
	public PolygonCollider2D dash9;
	public PolygonCollider2D dash10;
	public PolygonCollider2D dash11;
	

	//Palm strike animation hit/hurtboxes.

	public PolygonCollider2D palmHurt1;
	public PolygonCollider2D palmHurt2;
	public PolygonCollider2D palmHurt3;
	public PolygonCollider2D palmHurt4;
	public PolygonCollider2D palmHurt5;
	public PolygonCollider2D palmHurt6;
	public PolygonCollider2D palmHurt7;
	public PolygonCollider2D palmHurt8;
	public PolygonCollider2D palmHurt9;

	//Frames 5-7 of the palm strike animation are the ones which will hold the hitboxes.

	public PolygonCollider2D palmHit1;
	public PolygonCollider2D palmHit2;
	public PolygonCollider2D palmHit3;
	

	//Divekick animation hit/hurtboxes.

	public PolygonCollider2D diveHurt1;
	public PolygonCollider2D diveHurt2;
	public PolygonCollider2D diveHurt3;

	//Frame 3 of the dive kick animation will hold the hitbox.

	public PolygonCollider2D diveHit1;

	//Grapple up animation hurtboxes.

	public PolygonCollider2D grappleUp1;
	public PolygonCollider2D grappleUp2;
	public PolygonCollider2D grappleUp3;
	public PolygonCollider2D grappleUp4;


	//Grapple forward animation hurtboxes.

	public PolygonCollider2D grappleForward1;
	public PolygonCollider2D grappleForward2;
	public PolygonCollider2D grappleForward3;
	public PolygonCollider2D grappleForward4;


	//Grapple down animation hurtboxes.

	public PolygonCollider2D grappleDown1;
	public PolygonCollider2D grappleDown2;
	public PolygonCollider2D grappleDown3;
	public PolygonCollider2D grappleDown4;

	//Punch animation hit/hurtboxes.

	public PolygonCollider2D punchHurt1;
	public PolygonCollider2D punchHurt2;
	public PolygonCollider2D punchHurt3;
	public PolygonCollider2D punchHurt4;
	public PolygonCollider2D punchHurt5;
	public PolygonCollider2D punchHurt6;

	//Frames 5-6 of the punch animation will hold the hitboxes.

	public PolygonCollider2D punchHit1;
	public PolygonCollider2D punchHit2;

	//Kick animation hit/hurtboxes.

	public PolygonCollider2D kickHurt1;
	public PolygonCollider2D kickHurt2;
	public PolygonCollider2D kickHurt3;
	public PolygonCollider2D kickHurt4;

	//Frames 2-4 of the kick animation will hold the hitboxes.

	public PolygonCollider2D kickHit1;
	public PolygonCollider2D kickHit2;
	public PolygonCollider2D kickHit3;

	//Knee animation hit/hurtboxes.

	public PolygonCollider2D kneeHurt1;
	public PolygonCollider2D kneeHurt2;
	public PolygonCollider2D kneeHurt3;

	//Frame 3 of the knee animation will hold the hitbox.

	public PolygonCollider2D kneeHit1;

	//Getting hit animation hurtboxes.

	public PolygonCollider2D hit1;
	public PolygonCollider2D hit2;
	public PolygonCollider2D hit3;

	//Blocking animation hurtbox *special case, this hurtbox can only be affected by grapple attacks.*

	public PolygonCollider2D block1;


	//Array to store hit/hurtboxes

	private PolygonCollider2D[] boxes;

	//Player's current collider, one for hit and hurt respectively.

	private PolygonCollider2D currentHurtbox;
	private PolygonCollider2D currentHitbox;


	public enum listHitboxes
	{

		idle1,
		idle2,
		idle3,
		idle4,
		idle5,
		idle6,
		idle7,
		idle8,
		idle9,
		walk1,
		walk2,
		walk3,
		walk4,
		walk5,
		jump1,
		jump2,
		jump3,
		jump4,
		jump5,
		jump6,
		jump7,
		dash1,
		dash2,
		dash3,
		dash4,
		dash5,
		dash6,
		dash7,
		dash8,
		dash9,
		dash10,
		dash11,
		palmHurt1,
		palmHurt2,
		palmHurt3,
		palmHurt4,
		palmHurt5,
		palmHurt6,
		palmHurt7,
		palmHurt8,
		palmHurt9,
		palmHit1,
		palmHit2,
		palmHit3,
		diveHurt1,
		diveHurt2,
		diveHurt3,
		diveHit1,
		grappleUp1,
		grappleUp2,
		grappleUp3,
		grappleUp4,
		grappleForward1,
		grappleForward2,
		grappleForward3,
		grappleForward4,
		grappleDown1,
		grappleDown2,
		grappleDown3,
		grappleDown4,
		punchHurt1,
		punchHurt2,
		punchHurt3,
		punchHurt4,
		punchHurt5,
		punchHurt6,
		punchHit1,
		punchHit2,
		kickHurt1,
		kickHurt2,
		kickHurt3,
		kickHurt4,
		kickHit1,
		kickHit2,
		kickHit3,
		kneeHurt1,
		kneeHurt2,
		kneeHurt3,
		kneeHit1,
		hit1,
		hit2,
		hit3,
		clearHurt,
		clearHit


	}

	void Start()
	{

		//initalize array

		boxes = new PolygonCollider2D[]{idle1,
		idle2,
		idle3,
		idle4,
		idle5,
		idle6,
		idle7,
		idle8,
		idle9,
		walk1,
		walk2,
		walk3,
		walk4,
		walk5,
		jump1,
		jump2,
		jump3,
		jump4,
		jump5,
		jump6,
		jump7,
		dash1,
		dash2,
		dash3,
		dash4,
		dash5,
		dash6,
		dash7,
		dash8,
		dash9,
		dash10,
		dash11,
		palmHurt1,
		palmHurt2,
		palmHurt3,
		palmHurt4,
		palmHurt5,
		palmHurt6,
		palmHurt7,
		palmHurt8,
		palmHurt9,
		palmHit1,
		palmHit2,
		palmHit3,
		diveHurt1,
		diveHurt2,
		diveHurt3,
		diveHit1,
		grappleUp1,
		grappleUp2,
		grappleUp3,
		grappleUp4,
		grappleForward1,
		grappleForward2,
		grappleForward3,
		grappleForward4,
		grappleDown1,
		grappleDown2,
		grappleDown3,
		grappleDown4,
		punchHurt1,
		punchHurt2,
		punchHurt3,
		punchHurt4,
		punchHurt5,
		punchHurt6,
		punchHit1,
		punchHit2,
		kickHurt1,
		kickHurt2,
		kickHurt3,
		kickHurt4,
		kickHit1,
		kickHit2,
		kickHit3,
		kneeHurt1,
		kneeHurt2,
		kneeHurt3,
		kneeHit1,
		hit1,
		hit2,
		hit3,
		clearHurt,
		clearHit
		}


		//Create the colliders used by the player.
		currentHurtbox= gameObject.AddComponenet<PolygonCollider2D();
		currentHitbox= gameObject.AddComponenet<PolygonCollider2D();

		currentHurtbox.isTrigger = true;
		currentHitbox.isTrigger = true;

		currentHurtbox.pathCount = 0;
		currentHitbox.pathCount = 0;

	}


	void OnTriggerEnter2d(Collider2d col)
	{

		//TODO: add cases for specifc tags e.g "hitbox" and "hurtbox" tag, maybe even
		//specific tag for specific attacks, would allow us to do multihit/multiproperty moves

		Debug.Log("its not broken");

	}

	//The animation being played will call changeHitboxes with appropriate paramets, will set this manually.
	//If there is no hitbox for a respective animation, will send the clearHit value.

	public void changeHitboxes(listHitboxes hurtbox, listHitboxes hitbox)
	{


		if(hitbox != listHitboxes.clearHit)
		{

			currentHitbox.SetPath(0,boxes[(int)hitbox].GetPath(0));

		}
		else
		{
			currentHitbox.pathCount = 0;
		}

		if(hurtbox != listHitboxes.clearHurt){

	//We return when changing the player's hurtbox because every animation has a hurtbox, so we need
	//to be ready to call the function again.
			currentHurtbox.SetPath(0,boxes[(int)hurtbox].GetPath(0));
			return;

		}
		else
		{
			currentHurtbox.pathCount = 0;
		}

	}

}