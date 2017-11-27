using UnityEngine;
using System.Collections;

public class HitboxHandler: MonoBehaviour{


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

	//Used for determining properties of attack in collisison detection
	public string lastHitboxUsed;

	//current direction player is facing, required for adding forces to attacks
	public int direction;


	//Variables for attack properties

	public float palmHit1ForceX;
	public float palmHit2ForceX;
	public float palmHit3ForceX;
	public float diveHitForceX;
	public float punchHit1ForceX;
	public float punchHit2ForceX;
	public float kickHit1ForceX;
	public float kickHit2ForceX;
	public float kickHit3ForceX;
	public float kneeHit1ForceX;

	public float palmHit1ForceY;
	public float palmHit2ForceY;
	public float palmHit3ForceY;
	public float diveHitForceY;
	public float punchHit1ForceY;
	public float punchHit2ForceY;
	public float kickHit1ForceY;
	public float kickHit2ForceY;
	public float kickHit3ForceY;
	public float kneeHit1ForceY;

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

	

		var caseSwitch = lastHitboxUsed;

        //var hitbox = col.GetComponent<PolygonCollider2D> ();


        //This is where we set hitstun and other attack propertie

		if (col.GetComponent<Rigidbody2D> () != null) {


			switch (caseSwitch) {

			case "NinjaSprite_Sprite_35":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * palmHit1ForceX, palmHit1ForceY));
				break;

			case "NinjaSprite_Sprite_36":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * palmHit2ForceX, palmHit2ForceY));
				break;


			case "NinjaSprite_Sprite_37":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * palmHit3ForceX, palmHit3ForceY));
				break;

			case "NinjaSprite_Sprite_42":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * diveHitForceX, diveHitForceY));
				break;

			case "NinjaSprite_Sprite_59":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * punchHit1ForceX, punchHit1ForceY));
				break;

			case "NinjaSprite_Sprite_60":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * punchHit2ForceX, punchHit2ForceY));
				break;

			case "NinjaSprite_Sprite_62":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * kickHit1ForceX, kickHit1ForceY));
				break;

			case "NinjaSprite_Sprite_63":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * kickHit2ForceX, kickHit2ForceY));
				break;

			case "NinjaSprite_Sprite_64":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * kickHit3ForceX, kickHit3ForceY));
				break;

			case "NinjaSprite_Sprite_67":

				col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * kneeHit1ForceX, kneeHit1ForceY));
				break;
			default: 
			
				break;


			}

		}


	}

	//The animation being played will call changeHitboxes with appropriate paramets, will set this manually.
	//If there is no hitbox for a respective animation, will send the clearHit value.

	public void changeHitboxes(listHitboxes hitbox)
	{
		var facing = gameObject.GetComponent<SpriteRenderer> ();

		if (Input.GetAxis ("Horizontal") > 0)
		{
			gameObject.transform.localScale = new Vector3 (-9, 9, 9);
			direction = 1;

		} 
		else if (Input.GetAxis ("Horizontal") < 0) 
		{
			gameObject.transform.localScale = new Vector3 (9, 9, 9);
			direction = -1;
		}


		if(hitbox != listHitboxes.clearHit)
		{
			
			currentHitbox.SetPath(0,boxes[(int)hitbox].GetPath(0));
			lastHitboxUsed = boxes[(int)hitbox].name;
			return;

		}

		else
		{
			currentHitbox.pathCount = 0;
		}

	

	}

}



