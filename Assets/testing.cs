using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour {

	// Liste des lignes dessinées
	public List<List<Vector2>> listStoredLinesPoints;

	public Vector3 positionPlayer;
	public Vector3 positionFlag;
	public List<Vector3> positionsMonster;
	public List<Vector3> positionsRuby;

	public GameObject player;
	public GameObject endFlag;
	public GameObject monster;
	public GameObject ruby;

	// Use this for initialization
	void Start () {
		listStoredLinesPoints = GameManager.Instance.listStoredLinesPoints;
		positionPlayer = GameManager.Instance.positionPlayer;
		positionFlag = GameManager.Instance.positionFlag;
		positionsMonster = GameManager.Instance.positionsMonster;
		positionsRuby = GameManager.Instance.positionsRuby;

		PlaceRubies ();
		PlaceMonsters ();
		DrawLines ();
		PlacePlayer ();
		PlaceFlag ();
	}

	// Update is called once per frame
	void Update () {

	}

	void DrawLines() {
		foreach (List<Vector2> listPoints in listStoredLinesPoints) {
			GameObject line = new GameObject ("LineInstance");
			LineRenderer tmpLineRenderer = line.AddComponent<LineRenderer> ();
			EdgeCollider2D edgeCollider = line.AddComponent<EdgeCollider2D> ();

			tmpLineRenderer.SetVertexCount(listPoints.Count);

			for(int i = 0; i < listPoints.Count; i++) {
				tmpLineRenderer.SetPosition(i, listPoints[i]);
				tmpLineRenderer.SetWidth (0.1f, 0.1f);
				Material blackDiffuseMat = new Material (Shader.Find ("Transparent/Diffuse"));
				blackDiffuseMat.color = Color.black;
				tmpLineRenderer.material = blackDiffuseMat;
			}

			edgeCollider.points = listPoints.ToArray();
		}
	}

	void PlacePlayer() {
		player = Instantiate (player, positionPlayer, Quaternion.identity);
	}

	void PlaceFlag() {
		endFlag = Instantiate (endFlag, positionFlag, Quaternion.identity);
	}

	void PlaceMonsters() {
		for (int i = 0; i < positionsMonster.Count; i++) {
			monster = Instantiate (monster, positionsMonster [i], Quaternion.identity);
		}
	}

	void PlaceRubies() {
		for (int i = 0; i < positionsRuby.Count; i++) {
			ruby = Instantiate (ruby, positionsRuby [i], Quaternion.identity);
		}
	}
}
