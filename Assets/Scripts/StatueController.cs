using UnityEngine;
using System.Collections;

public class StatueController : MonoBehaviour {

	private GameObject explosion;

	void Awake() {
		explosion = Resources.Load ("Prefabs/Explosion") as GameObject;
	}

	void OnCollisionEnter (Collision collider) {
		if (collider.gameObject.name == "missile") {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
