using UnityEngine;
using System.Collections;

public class HitboxHandler: MonoBehaviour

{


	//The term "hitbox" in this case refers to colliders on certain moves that will cause
	//damage to enemy players, e.g we will place hitboxes on things like punches and kicks.


	//We have to draw hitboxes on each frame individually, this will be messy but
	//need to set the frames we are handling in question in the editor.

	//Most of this script is just declaring variables for the various hitboxes; the actual code for this script
	//can be found about 3/4ths the way down.



	//Frames 5-7 of the palm strike animation are the ones which will hold the hitboxes.

	public PolygonCollider2D palmHit1;
	public PolygonCollider2D palmHit2;
	public PolygonCollider2D palmHit3;
	

	//Frame 3 of the dive kick animation will hold the hitbox.

	public PolygonCollider2D diveHit1;


	//Frames 5-6 of the punch animation will hold the hitboxes.

	public PolygonCollider2D punchHit1;
	public PolygonCollider2D punchHit2;

	//Frames 2-4 of the kick animation will hold the hitboxes.

	public PolygonCollider2D kickHit1;
	public PolygonCollider2D kickHit2;
	public PolygonCollider2D kickHit3;


	//Frame 3 of the knee animation will hold the hitbox.

	public PolygonCollider2D kneeHit1;


	//Array to store hit/hurtboxes

	private PolygonCollider2D[] boxes;

	//Player's current collider

	public PolygonCollider2D currentHitbox;

	//Clear hitboxes
	public PolygonCollider2D clearHit;


	public enum listHitboxes
	{

		palmHit1,
		palmHit2,
		palmHit3,
		diveHit1,
		punchHit1,
		punchHit2,
		kickHit1,
		kickHit2,
		kickHit3,
		kneeHit1,
		clearHit


	}

	void Start()
	{

		//initalize array

		boxes = new PolygonCollider2D[] {

		
			palmHit1,
			palmHit2,
			palmHit3,
			diveHit1,
			punchHit1,
			punchHit2,
			kickHit1,
			kickHit2,
			kickHit3,
			kneeHit1,
			clearHit
		};

	

		//Create the colliders used by the player.

		currentHitbox = gameObject.AddComponent<PolygonCollider2D>();


		currentHitbox.isTrigger = true;

	
		currentHitbox.pathCount = 0;

	}


	void OnTriggerEnter2D(Collider2D col)
	{

		//TODO: add cases for specifc tags e.g "hitbox" and "hurtbox" tag, maybe even
		//specific tag for specific attacks, would allow us to do multihit/multiproperty moves

		Debug.Log("its not broken");

	}

	//The animation being played will call changeHitboxes with appropriate paramets, will set this manually.
	//If there is no hitbox for a respective animation, will send the clearHit value.

	public void changeHitboxes(listHitboxes hitbox)
	{


		if(hitbox != listHitboxes.clearHit)
		{

			currentHitbox.SetPath(0,boxes[(int)hitbox].GetPath(0));

		}
		else
		{
			currentHitbox.pathCount = 0;
		}

	

	}

}