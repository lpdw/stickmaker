using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraRunnerScript : MonoBehaviour {

	private GameObject camera;
	private GameObject player;


	// Use this for initialization
	void Start () {
		

	}

	void FixedUpdate() {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (camera.transform.position.x >= 0 && camera.transform.position.x <= 60) {
			camera.transform.position = new Vector3 (player.transform.position.x, 0, -10);
			if (camera.transform.position.x < 0) {
				camera.transform.position = new Vector3 (0, 0, -10);
			} else if (camera.transform.position.x > 60) {
				camera.transform.position = new Vector3 (60, 0, -10);
			}
		}

	}

}




