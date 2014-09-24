using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

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
	public ObjectManagementPool missilePool;
	

	void Awake() {
		missilePool = new ObjectManagementPool (Resources.Load ("Prefabs/missile") as GameObject);
	}

	
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update () {
		movementUpdate ();
		shootingUpdate ();
	}

	private void movementUpdate () {
		float distToPlayer = Vector3.Distance (transform.position, player.position);
		float distToStatue = Vector3.Distance (transform.position, statue.position);
		currentTarget = (distToPlayer < distToStatue)? player : statue;
		
		agent.SetDestination (currentTarget.position);
	}

	private void shootingUpdate () {
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
		missile.rigidbody.angularVelocity = new Vector3(20.0f, 0, 0);
		reloading = true;
	}


}


