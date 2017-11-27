using UnityEngine;

public class SpringController : C_WorldObjectController
{
	void Start () {
		Manager.ObjectLog.Add (gameObject, this);
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			var force = 30f;
			other.rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		}
	}
}
