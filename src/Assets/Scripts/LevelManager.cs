using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private GameObject enemyPrefab;

	// Game Termination
	public int remainingEnemies;
	public bool lost;

	// Pools
	public ObjectManagementPool missilePool;
	public ObjectManagementPool brickPool;

	public GameObject[] enemies;
	public GameObject[] spawnPoints;
	public GameObject statue;
	public GameObject player;

	public GameObject lostLabel;
	public GameObject winLabel;

	public GameObject loseCam;
	
	private bool loseGUI = false;
	public float spawnRange = 50f;
	private Vector3 lastPlayerPosition;
	private Quaternion lastPlayerRotation;

	void Awake() {
		remainingEnemies = 10;
		enemyPrefab = Resources.Load("Prefabs/enemy3d") as GameObject;
		missilePool = new ObjectManagementPool (Resources.Load ("Prefabs/missile") as GameObject, 50);
		brickPool = new ObjectManagementPool (Resources.Load ("Prefabs/single_brick") as GameObject, 200);
		foreach (GameObject enemy in enemies)
			if(enemy != null)
				enemy.GetComponent<EnemyController>().missilePool = missilePool;


		player.GetComponent<PlayerController> ().missilePool = missilePool;

		BrickBlockController[] brickBlocks = GameObject.FindObjectsOfType(typeof(BrickBlockController)) as BrickBlockController[];
		foreach (BrickBlockController brickBlock in brickBlocks) {
			brickBlock.brickPool = brickPool;
		}
	}

	void Start () {
		
	}
	
	void Update () {
		int destroyedEnemies = 0;
		foreach(GameObject enemy in enemies) {
			if(enemy == null) {
				destroyedEnemies++;
			}
		}

		for(int i = 0; i < destroyedEnemies; i++) {
			spawnEnemy();
		}

		if (player == null || statue == null)
		{
			lost = true;
		} else {
			lastPlayerPosition = player.transform.position;
			lastPlayerRotation = player.transform.rotation;
		}

		if(lost && !loseGUI) {
			loseGUI = true;
			StartCoroutine(loseGame());
		} else if (remainingEnemies <= 0) {
			StartCoroutine(winGame());
		}
	}

	void spawnEnemy() {
		int spawnPointIndex = Random.Range(0, 2);
		Transform spawnPointLocation = spawnPoints[spawnPointIndex].transform;
		bool canSpawn = true;
		if(Vector3.Distance(spawnPointLocation.position, player.transform.position) < spawnRange) {
			canSpawn = false;
		}
		foreach(GameObject enemy in enemies) {
			if(enemy != null && Vector3.Distance(spawnPointLocation.position, enemy.transform.position) < spawnRange) {
				canSpawn = false;
			}
		}
		if (canSpawn) {
			GameObject newEnemy = Instantiate(enemyPrefab, spawnPointLocation.position, spawnPointLocation.root.rotation) as GameObject;
			EnemyController newEnemyController = newEnemy.GetComponent<EnemyController>();
			newEnemyController.statue = statue.transform;
			newEnemyController.player = player.transform;
			newEnemyController.missilePool = missilePool;

			for(int i = 0; i < enemies.Length; i++) {
				if(enemies[i] == null) {
					enemies[i] = newEnemy;
				}
			}
			remainingEnemies--;
		}
	}

	private IEnumerator loseGame() {
		loseCam.GetComponent<Camera> ().depth = 0;
		Rigidbody camBody = loseCam.GetComponent<Rigidbody> ();
		camBody.position = lastPlayerPosition + new Vector3(0f, 10f, 0f);
		camBody.rotation = lastPlayerRotation;
		camBody.velocity = new Vector3 (Random.Range(-5f, 5f), 20f, Random.Range(-5f, 5f));
		camBody.AddTorque (new Vector3 (Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
		GameObject label = Instantiate (lostLabel) as GameObject;
		yield return new WaitForSeconds(5.0f);
		Destroy(label);
		Application.LoadLevel("MainMenu");
	}

	private IEnumerator winGame() {
		GameObject label = Instantiate (winLabel) as GameObject;
		yield return new WaitForSeconds(5.0f);
		Destroy(label);
		Application.LoadLevel("MainMenu");
	}
}
