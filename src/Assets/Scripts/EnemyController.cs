using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// Sound
	public AudioSource movementSource;
	public AudioSource shootingSource;

	// Animation
	public Material tracksMat;
	private float realMovForw;
	private GameObject explosion;

	// Movement
	public Transform player;
	public Transform statue;
	private Transform currentTarget;
	private NavMeshAgent agent;

	// Shooting
	public Transform shootingData;
	public float missileSpeed = 40.00f;
	public float reloadDelay = 2.0f;
	private float curReloadTime = 0.0f;
	private bool reloading = false;
	private bool hasFired;
	public ObjectManagementPool missilePool;
	

	void Awake() {
	}

	
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		explosion = Resources.Load ("Prefabs/Explosion") as GameObject;
	}

	void Update () {
		movementUpdate ();
		animationUpdate ();
		shootingUpdate ();
		soundUpdate ();
	}

	private void soundUpdate () {
		if (hasFired)
			shootingSource.Play();

		movementSource.volume = Mathf.Abs (realMovForw) *3.0f;
	}

	private void movementUpdate () {
		if(player != null) {
			rigidbody.angularVelocity = new Vector3 (0,0,0);
			rigidbody.velocity = new Vector3 (0,0,0);

			float distToPlayer = Vector3.Distance (transform.position, player.position);
			float distToStatue = Vector3.Distance (transform.position, statue.position);
			currentTarget = (distToPlayer < distToStatue)? player : statue;
			
			agent.SetDestination (currentTarget.position);

			realMovForw = Vector3.Project(agent.velocity, transform.forward).magnitude;
		}
	}

	private void animationUpdate () {
		tracksMat.mainTextureOffset = new Vector2(tracksMat.mainTextureOffset.x, tracksMat.mainTextureOffset.y + realMovForw * 0.002f );
		
		if (tracksMat.mainTextureOffset.y > 1.0f)
			tracksMat.mainTextureOffset = new Vector2(tracksMat.mainTextureOffset.x, tracksMat.mainTextureOffset.y - 1.0f);
		
	}

	private void shootingUpdate () {
		hasFired = false;
		if(reloading && (curReloadTime += Time.deltaTime) >= reloadDelay) {
			curReloadTime = 0.0f;
			reloading = false;
		}

		bool shouldFire = true;
		RaycastHit[] hitsInfo = Physics.RaycastAll (transform.position, transform.forward);

		if (!reloading && hitsInfo != null) {
			
			foreach (RaycastHit hitInfo in hitsInfo) {
				if (hitInfo.transform.GetComponent<EnemyController>() != null)
					shouldFire = false;
				else if (hitInfo.transform.GetComponent<PlayerController>() != null || hitInfo.transform.GetComponent<StatueController>()) {
					shouldFire = true;
					break;
				}
			}

			if(shouldFire)
				shoot();
		}
	}

	private void shoot() {
		GameObject missile = missilePool.getObject (true, shootingData.position, Quaternion.LookRotation(-shootingData.forward));
		missile.rigidbody.velocity = missileSpeed * shootingData.forward;
		//missile.rigidbody.angularVelocity = shootingData.forward * 20.0f;
		reloading = true;
		hasFired = true;
	}

	void OnCollisionEnter (Collision collider) {
		if (collider.gameObject.name == "missile") {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}

}


