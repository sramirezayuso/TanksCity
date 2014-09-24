using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Movement
	public float movementSpeed = 20.00f;
	public float rotationSpeed = 100.00f;

	// Shooting
	public Transform shootingData;
	public float missileSpeed = 40.00f;
	public float reloadDelay = 2.0f;
	private float curReloadTime = 0.0f;
	private bool reloading = false;
	private ObjectManagementPool missilePool;

	void Awake() {
		missilePool = new ObjectManagementPool (Resources.Load ("Prefabs/missile") as GameObject);
	}

	void Update () {
		movementUpdate ();
		shootingUpdate ();
	}
	
	private void movementUpdate () {
		Vector3 movementVector = transform.forward * Input.GetAxis("Vertical");
		transform.position += movementVector * movementSpeed * Time.deltaTime;
		
		Vector3 rotationVector = new Vector3(0, Input.GetAxis("Horizontal"), 0);
		transform.Rotate (rotationVector * rotationSpeed * Time.deltaTime);
	}
	
	private void shootingUpdate () {
		if(reloading && (curReloadTime += Time.deltaTime) >= reloadDelay) {
			curReloadTime = 0.0f;
			reloading = false;
		}

		if(Input.GetKeyDown("space") && !reloading)
			shoot();

	}

	private void shoot() {
		GameObject missile = missilePool.getObject (true, shootingData.position, Quaternion.LookRotation(-shootingData.forward));
		missile.rigidbody.velocity = missileSpeed * shootingData.forward;
		missile.rigidbody.angularVelocity = new Vector3(20.0f, 0, 0);
		reloading = true;
	}
}
