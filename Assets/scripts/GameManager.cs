using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	private GameObject player;

	private TimeManager timeManager;
	private bool gameStarted = false;

	private GameObject floor;
	private Spawner spawner;

	// Use this for initialization
	void Awake () {
		floor = GameObject.Find ("Foreground");
		spawner = GameObject.Find ("Spawner").GetComponent<Spawner>();
		timeManager = GetComponent<TimeManager> ();
	}
	
	// Update is called once per frame
	void Start () {

		var floorHeight = floor.transform.localScale.y;

		var pos = floor.transform.position;
		pos.x = 0;
		pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) /2) + floorHeight/2;
		floor.transform.position = pos;

		spawner.active = false;

		Time.timeScale = 0;
	}

	void Update(){
		if (!gameStarted && Time.timeScale == 0) {
			if(Input.anyKeyDown){

				timeManager.ManipulateTime(1,1);
				ResetGame();
			}
		}
	}

	void OnPlayerKilled(){
		spawner.active = false;

		var playerDestroyScript = player.GetComponent<DestroyOffscreen> ();
		playerDestroyScript.DestroyCallback -= OnPlayerKilled;

		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		timeManager.ManipulateTime (0,5.5f);

		gameStarted = false;
	}

	void ResetGame(){
		spawner.active = true;

		player = GameObjectUtility.Instantiate (playerPrefab, new Vector3(0, (Screen.height/PixelPerfectCamera.pixelsToUnits)/2, 0));

		var playerDestroyScript = player.GetComponent<DestroyOffscreen> ();
		playerDestroyScript.DestroyCallback += OnPlayerKilled;

		gameStarted = true;
	}
}
