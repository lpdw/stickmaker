using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class killPlayer : MonoBehaviour {

	public bool endGame = false;
	private CanvasGroup popup;


	// Use this for initialization
	void Start () {
		popup = GameObject.Find("Popup_Defeat").GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			endGame = true;
			print (endGame);
			Destroy (coll.gameObject);
			popup.alpha = 1;
			popup.interactable = true;
			EventSystem.current.SetSelectedGameObject (GameObject.Find ("ButtonPopup"));
				}

	}

}