using UnityEngine;
using System.Collections;

public class SingleBrickController : MonoBehaviour, PoolableObject {

	ObjectManagementPool brickPool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (waitAndDestroy());
	}

	public void setObjectPool(ObjectManagementPool pool) {
		this.brickPool = pool;
	}


	IEnumerator waitAndDestroy() {
		yield return new WaitForSeconds (4);
		brickPool.poolObject (this.gameObject);
	}
}
