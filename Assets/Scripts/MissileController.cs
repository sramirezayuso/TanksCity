using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour, PoolableObject {

	ObjectManagementPool missilePool;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setObjectPool(ObjectManagementPool pool) {
		this.missilePool = pool;
	}

	void OnCollisionEnter(Collision collision) {
		missilePool.poolObject (this.gameObject);
	}
}
