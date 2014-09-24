using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour, PoolableObject {

	ObjectManagementPool missilePool;
	private GameObject explosion;
	
	// Use this for initialization
	void Start () {
		explosion = Resources.Load ("Prefabs/Explosion") as GameObject;
	}

	public void setObjectPool(ObjectManagementPool pool) {
		this.missilePool = pool;
	}

	void OnCollisionEnter(Collision collision) {
		Instantiate (explosion, transform.position, transform.rotation);
		missilePool.poolObject (this.gameObject);
	}
}
