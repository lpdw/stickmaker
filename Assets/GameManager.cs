using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	// Liste des lignes dessinées
	public List<List<Vector2>> listStoredLinesPoints;
	public Vector3 positionPlayer;
	public Vector3 positionFlag;
	public List<Vector3> positionsMonster;
	public List<Vector3> positionsRuby;

	// Use this for initialization
	void Awake () {
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}
}
