using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class playerMove : MonoBehaviour {

	private Rigidbody2D playerRigidbody;

	// game over
	private bool isDead = false;

	// speed variable
	public float moveSpeed;
	Vector2 playerFlip;

	// jump variable
	public float jumpForce;
	public float jumpForceBack;
	public float jumpCoroutineTime;
	private float airTime = .1f;
	// on déclare isJumping à false pour que le joueur puisse sauter dés le début
	private bool isJumping = false;

	public float distanceFloor;

	private Animator animate;

	public int score;
	private Text scoreText;

	private CanvasGroup popup;


	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody2D> ();
		animate = GetComponentInChildren<Animator> ();
		popup = GameObject.Find ("Popup").GetComponent<CanvasGroup> ();
		scoreText = GameObject.Find ("Text").GetComponent<Text> ();
		scoreText.text = "Score : 0";
	}

	void FixedUpdate() {

		if (isDead == false) {

			// move and animation
			if (Input.GetKey (KeyCode.RightArrow)) {
				animate.SetBool ("walkRight", true);
				transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
			}
			if (Input.GetKeyUp (KeyCode.RightArrow)) {
				animate.SetBool ("walkRight", false);
			}

			if (Input.GetKey (KeyCode.LeftArrow)) {
				animate.SetBool ("walkLeft", true);
				transform.Translate(new Vector2(-moveSpeed, 0) * Time.deltaTime);
			}
			if (Input.GetKeyUp (KeyCode.LeftArrow)) {
				animate.SetBool ("walkLeft", false);
			}


			// Raycast qui permet de définir à quelle distance du sol se trouve le joueur
			RaycastHit2D ray = Physics2D.Raycast(transform.position, -Vector2.up);
			if (ray.collider != null) 
			{
				// si le joueur est à une certaine distance (2f ici) du sol, isJumping passe à true et empêche le saut
				if (ray.distance > distanceFloor)
					isJumping = true;
				// sinon, il passe à false, le joueur est au sol et peut donc sauter
				else {
					StopAllCoroutines ();
					isJumping = false;
				}
			}
			// on vérifie si le personnage saute ou non avec le bool isJumping qui est déterminé au dessus
			// jump and animation	
			if (Input.GetKey (KeyCode.UpArrow) && !isJumping) {
				StartCoroutine (JumpCoroutine ());
			}

			if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) {
				animate.SetBool ("rightJump", true);
			} else if (!isJumping) {
				animate.SetBool ("rightJump", false);
			}

			if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow)) {
				animate.SetBool ("leftJump", true);
			} else if (!isJumping) {
				animate.SetBool ("leftJump", false);
			}

			if (isJumping && Input.GetKey (KeyCode.RightArrow)) {
				animate.SetBool ("leftJump", false);
				animate.SetBool ("rightJump", true);
			}

			if (isJumping && Input.GetKey (KeyCode.LeftArrow)) {
				animate.SetBool ("rightJump", false);
				animate.SetBool ("leftJump", true);
			}
		}
	}

	IEnumerator JumpCoroutine ()
	{
		float temps = jumpCoroutineTime;
		playerRigidbody.AddForce (Vector3.up * jumpForce,ForceMode2D.Impulse);
		yield return new WaitForSeconds(airTime);
		while (temps > 0f) {
			print ("ForceBack");
			temps -= Time.deltaTime;
			playerRigidbody.AddForce (-Vector3.up * jumpForceBack,ForceMode2D.Impulse);
			yield return null;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Monster") {
			isDead = true;
		}

		if (coll.gameObject.tag == "Rubis") {
			score++;
			scoreText.text = "Score : " + score.ToString();
			Destroy (coll.gameObject);
		}

		if (coll.gameObject.tag == "Finish") {
			isDead = true;
			animate.SetBool ("idle", true);
			GameObject monster = GameObject.Find ("Monster");
			Destroy (monster);
			Destroy (coll.gameObject);
			popup.alpha = 1;
			popup.interactable = true;
			EventSystem.current.SetSelectedGameObject (GameObject.Find ("ButtonPopupDefeat"));

		}

	}

}




