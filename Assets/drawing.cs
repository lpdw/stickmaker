using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class drawing : MonoBehaviour {

	// Liste des points du trait courant
	public List<Vector2> storedLinePoints;

	public List<List<Vector2>> listStoredLinesPoints;

	// Liste des lignes dessinées
	public List<LineRenderer> storedLines;

	private LineRenderer currentLineRenderer;

	private GameObject currentLine;

	private int limiterPoints;

	private Vector3 limitDrawing;

	private bool penActivated;

	private bool placingObject;
	private bool placingPlayer;
	private bool placingFlag;
	private bool placingRuby;
	private bool placingMonster;

	public Vector3 positionPlayer;
	public Vector3 positionFlag;
	public List<Vector3> positionsRuby;
	public List<Vector3> positionsMonster;

	private Texture2D cursorScroll;
	private Texture2D cursorPlayer;
	private Texture2D cursorFlag;
	private Texture2D cursorRuby;
	private Texture2D cursorMonster;

	// Use this for initialization
	void Start () {
		cursorScroll = (Texture2D)Resources.Load ("cursor-scroll");
		cursorPlayer = (Texture2D)Resources.Load ("stickman");
		cursorFlag = (Texture2D)Resources.Load ("end-flag");
		cursorRuby = (Texture2D)Resources.Load ("rubis");
		cursorMonster = (Texture2D)Resources.Load ("monster");
		penActivated = false;
		placingFlag = false;
		placingPlayer = false;
		limiterPoints = 0;
		// On initialise la liste des lignes avec une liste vide
		storedLines = new List<LineRenderer> ();
		listStoredLinesPoints = new List<List<Vector2>> ();

		GameObject menu = GameObject.FindGameObjectWithTag ("menu");
		limitDrawing = Camera.main.WorldToViewportPoint (new Vector2 (menu.transform.position.x, menu.transform.position.y));
	}

	// Update is called once per frame
	void Update () {
		if (!placingObject) {
			if (penActivated) {
				if (Input.GetMouseButtonDown (0) && Camera.main.ScreenToViewportPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y)).x <= limitDrawing.x) {			
					//On ajoute un nouveau GameObject à notre GameObject LinesList
					currentLine = new GameObject ("LineInstance");
					currentLine.tag = "line";
					// On ajoute à ce nouveau GameObject un nouveau LineRenderer qui va correspondre au trait courant
					currentLineRenderer = currentLine.AddComponent<LineRenderer> ();
					currentLineRenderer.SetWidth (0.1f, 0.1f);
					Material blackDiffuseMat = new Material (Shader.Find ("Transparent/Diffuse"));
					blackDiffuseMat.color = Color.black;
					currentLineRenderer.material = blackDiffuseMat;


					// On initialise la liste de points du trait courant avec une liste vide
					storedLinePoints = new List<Vector2> ();
				}

				// Tant que le clic gauche est pressé
				if (Input.GetMouseButton (0) && Camera.main.ScreenToViewportPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y)).x <= limitDrawing.x) {
					Vector3 point = Camera.main.ScreenToWorldPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));

					limiterPoints++;
					// On enregistre la nouvelle position de la souris et on lie cette position au lineRenderer courant
					RecordPosition (point);
				}

				if (Input.GetMouseButtonUp (0) && Camera.main.ScreenToViewportPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y)).x <= limitDrawing.x) {
					// On met à jour la liste de lignes une fois le trait terminé
					storedLines.Add (currentLineRenderer);
					listStoredLinesPoints.Add (storedLinePoints);

					// On ajoute le collider au trait qui vient d'être dessiné
					AddCollider ();
				}
			}

			if (Input.GetMouseButton (1)) {
				Cursor.SetCursor (cursorScroll, Vector2.zero, CursorMode.Auto);
				GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
				float x = Input.GetAxis ("Mouse X");
				if (camera.transform.position.x >= 0 && camera.transform.position.x <= 60) {
					camera.transform.Translate (-x, 0, 0);
					if (camera.transform.position.x < 0) {
						camera.transform.position = new Vector3 (0, 0, -10);
					} else if (camera.transform.position.x > 60) {
						camera.transform.position = new Vector3 (60, 0, -10);
					}
				}
			}

			if (Input.GetMouseButtonUp (1)) {
				Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				Vector3 placePosition;
				if (placingFlag) {
					Destroy (GameObject.Find ("end-flag"));
					GameObject flag = new GameObject ("end-flag");
					SpriteRenderer spriteRenderer = flag.AddComponent<SpriteRenderer> ();
					spriteRenderer.sprite = Resources.Load<Sprite> ("end-flag");
					placePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
					placePosition.z = 0;
					flag.transform.position = placePosition;
					positionFlag = placePosition;
					placingFlag = false;
				} else if (placingPlayer) {
					Destroy (GameObject.Find ("player"));
					GameObject player = new GameObject ("player");
					SpriteRenderer spriteRenderer = player.AddComponent<SpriteRenderer> ();
					spriteRenderer.sprite = Resources.Load<Sprite> ("stickman");
					placePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
					placePosition.z = 0;
					player.transform.position = placePosition;
					positionPlayer = placePosition;
					placingPlayer = false;
				} else if (placingRuby) {
					GameObject ruby = new GameObject ("rubis");
					SpriteRenderer spriteRenderer = ruby.AddComponent<SpriteRenderer> ();
					spriteRenderer.sprite = Resources.Load<Sprite> ("rubis");
					placePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
					placePosition.z = 0;
					ruby.transform.position = placePosition;
					positionsRuby.Add(placePosition);
					placingRuby = false;
				} else if (placingMonster) {
					GameObject monster = new GameObject ("monster");
					SpriteRenderer spriteRenderer = monster.AddComponent<SpriteRenderer> ();
					spriteRenderer.sprite = Resources.Load<Sprite> ("monster");
					placePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
					placePosition.z = 0;
					monster.transform.position = placePosition;
					positionsMonster.Add(placePosition);
					placingMonster = false;
				}
				placingObject = false;
				Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			}
		}
	}

	// Enregistre la position de la souris dans la liste de points courante, puis ajoute ce point au lineRenderer courant
	void RecordPosition(Vector2 newPoint) {
		if(limiterPoints % 2 != 0) {
			storedLinePoints.Add(newPoint);
			currentLineRenderer.SetVertexCount(storedLinePoints.Count);
			currentLineRenderer.SetPosition(storedLinePoints.Count-1, newPoint); // add newPoint as the last point on the line (count -1 because the SetPosition is 0-based and Count is 1-based)
		}
	}

	// Ajoute un edge collider à la ligne courante (qui vient d'être créée)
	void AddCollider() {
		EdgeCollider2D edgeCollider = currentLine.AddComponent<EdgeCollider2D> ();
		edgeCollider.points = storedLinePoints.ToArray ();
	}

	public void RemoveLastLine() {
		GameObject[] allLines = GameObject.FindGameObjectsWithTag ("line");
		storedLines.Remove (storedLines[storedLines.Count - 1]);
		Destroy(allLines[allLines.Length - 1]);
	}

	public void SwitchPen() {
		UnityEngine.UI.Button button = GameObject.Find("DrawButton").GetComponent<UnityEngine.UI.Button> ();
		penActivated = !penActivated;

		if (penActivated)
			button.image.color = Color.green;
		else
			button.image.color = Color.white;
	}

	public void placePlayer() {
		if (penActivated)
			SwitchPen ();
		Cursor.SetCursor (cursorPlayer, Vector2.zero, CursorMode.Auto);
		placingPlayer = true;
		placingObject = true;
	}

	public void placeFlag() {
		if (penActivated)
			SwitchPen ();
		Cursor.SetCursor (cursorFlag, Vector2.zero, CursorMode.Auto);
		placingFlag = true;
		placingObject = true;
	}

	public void placeRuby() {
		if (penActivated)
			SwitchPen ();
		Cursor.SetCursor (cursorRuby, Vector2.zero, CursorMode.Auto);
		placingRuby = true;
		placingObject = true;
	}

	public void placeMonster() {
		if (penActivated)
			SwitchPen ();
		Cursor.SetCursor (cursorMonster, Vector2.zero, CursorMode.Auto);
		placingMonster = true;
		placingObject = true;
	}

	public void loadTestingScene() {
		if (positionFlag != Vector3.zero && positionPlayer != Vector3.zero) {
			GameManager.Instance.listStoredLinesPoints = listStoredLinesPoints;
			GameManager.Instance.positionFlag = positionFlag;
			GameManager.Instance.positionPlayer = positionPlayer;
			GameManager.Instance.positionsMonster = positionsMonster;
			GameManager.Instance.positionsRuby = positionsRuby;

			SceneManager.LoadScene ("testing_scene");
		}
	}

}
