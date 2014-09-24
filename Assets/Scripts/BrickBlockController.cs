using UnityEngine;
using System.Collections;

public class BrickBlockController : MonoBehaviour {

	private int destroyedBricks = 0;
	public ObjectManagementPool brickPool;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroyBrick (Vector3 position) {
		brickDestructionAnimation (position);
		destroyedBricks++;
		if (destroyedBricks == 3) {
			Object.Destroy (this.gameObject);
		}
	}

	void brickDestructionAnimation (Vector3 position) {
		for(int i=0; i<10 ; i++) {
			Rigidbody body = brickPool.getObject (true, new Vector3(position.x, position.y + Random.Range(1, 3), position.z), Random.rotation).GetComponent<Rigidbody>();
		}
	}
}
