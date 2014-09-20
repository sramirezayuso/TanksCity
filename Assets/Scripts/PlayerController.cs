using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private static float movementSpeed = 20.00f;
	private static float rotationSpeed = 100.00f;

	private ObjectManagementPool missilePool;

	void Awake() {
		missilePool = new ObjectManagementPool (Resources.Load ("Prefabs/missile") as GameObject);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 movementVector = transform.forward * Input.GetAxis("Vertical");
		transform.position += movementVector * movementSpeed * Time.deltaTime;

		Vector3 rotationVector = new Vector3(0, Input.GetAxis("Horizontal"), 0);
		transform.Rotate (rotationVector * rotationSpeed * Time.deltaTime);

		if(Input.GetKeyDown("space")) {
			shoot();
		}
	}

	void shoot() {
		Vector3 cameraPosition = transform.Find ("Camera").transform.position;
		GameObject missile = missilePool.getObject (true, new Vector3(cameraPosition.x, 7.5f, cameraPosition.z) + 10 * transform.forward, Quaternion.LookRotation(-transform.forward));
		missile.rigidbody.velocity = 40f * transform.forward;
	}
}
