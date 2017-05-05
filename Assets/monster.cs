using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour {

	public int moveSpeed;
	private Rigidbody2D monsterRigidbody;
	private Collider2D monsterCollider;
	private Transform HoleRaycast;

	private bool gameOver = false;
	private bool moveLeft = true;
	private bool moveRight = false;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame



	void FixedUpdate () {

		HoleRaycast = transform.FindChild ("HoleRaycast");
		RaycastHit2D noPlatform = Physics2D.Raycast (HoleRaycast.position, Vector2.down);
		Debug.DrawRay (HoleRaycast.position, Vector2.down);
		transform.Translate (new Vector2 (-moveSpeed, 0) * Time.deltaTime);

		if (noPlatform == false && moveLeft) {
			transform.rotation = Quaternion.Euler(0,180,0);
			moveLeft = false;
			moveRight = true;
		} else if (noPlatform == false && moveRight) {
			transform.rotation = Quaternion.Euler(0,0,0);
			moveRight = false;
			moveLeft = true;
		}

		// if (gameOver == false && moveRight) {
		// 	transform.Translate (new Vector2 (moveSpeed, 0) * Time.deltaTime);

		// 	if (right) {
		// 		moveRight = false;
		// 		moveLeft = true;
		// 	}
		// }



	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Wall") {
			if (moveLeft) {
				transform.rotation = Quaternion.Euler(0,180,0);
				moveLeft = false;
				moveRight = true;
			} else if (moveRight) {
				transform.rotation = Quaternion.Euler(0,0,0);
				moveRight = false;
				moveLeft = true;
			}
		}
	}
}
