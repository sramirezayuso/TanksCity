using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Sound
	public AudioSource movementSource;
	public AudioSource shootingSource;

	// Animation
	public Material tracksMat;
	private float realMovForw;
	private GameObject explosion;

	// Movement
	public float movementSpeed = 20.00f;
	public float rotationSpeed = 100.00f;

	// Shooting
	public Transform shootingData;
	private float missileSpeed = 120.00f;
	public float reloadDelay = 2.0f;
	private float curReloadTime = 0.0f;
	private bool reloading = false;
	private bool hasFired;
	public ObjectManagementPool missilePool;

	void Awake() {
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

	private void animationUpdate () {
		tracksMat.mainTextureOffset = new Vector2(tracksMat.mainTextureOffset.x, tracksMat.mainTextureOffset.y + realMovForw * 0.02f );
		
		if (tracksMat.mainTextureOffset.y > 1.0f)
			tracksMat.mainTextureOffset = new Vector2(tracksMat.mainTextureOffset.x, tracksMat.mainTextureOffset.y - 1.0f);
		
	}

	private void movementUpdate () {
		rigidbody.angularVelocity = new Vector3 (0, 0, 0);
		rigidbody.velocity = new Vector3 (0, 0, 0);

		Vector3 movementVector = transform.forward * Input.GetAxis("Vertical");
		transform.position += movementVector * movementSpeed * Time.deltaTime;
		
		Vector3 rotationVector = new Vector3(0, Input.GetAxis("Horizontal"), 0);
		transform.Rotate (rotationVector * rotationSpeed * Time.deltaTime);

		realMovForw = Vector3.Project (movementVector * movementSpeed * Time.deltaTime, transform.forward).magnitude * Mathf.Sign(Input.GetAxis("Vertical"));
	}
	
	private void shootingUpdate () {
		hasFired = false;
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
