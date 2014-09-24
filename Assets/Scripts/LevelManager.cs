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

	private int spawning;
	public float spawnRange = 50f;

	void Awake() {
		spawning = 0;
		remainingEnemies = 3;
		enemyPrefab = Resources.Load("Prefabs/enemy3d") as GameObject;
		missilePool = new ObjectManagementPool (Resources.Load ("Prefabs/missile") as GameObject, 50);
		brickPool = new ObjectManagementPool (Resources.Load ("Prefabs/single_brick") as GameObject, 200);
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<EnemyController>().missilePool = missilePool;
		}

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
		}
		if(lost) {
			GUI.Label(new Rect(Screen.width-200,0,200,95), "You Lose!");
		} else if (remainingEnemies <= 0) {
			GUI.Label(new Rect(Screen.width-200,0,200,95), "You Win!");
		}
	}

	void spawnEnemy() {
		int spawnPointIndex = Random.Range(0, 4);
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
}
