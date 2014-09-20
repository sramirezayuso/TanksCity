using UnityEngine;
using System.Collections;

public class BrickBlockController : MonoBehaviour {

	private int destroyedBricks = 0;
	private ObjectManagementPool brickPool;

	// Use this for initialization
	void Start () {
		brickPool = new ObjectManagementPool (Resources.Load ("Prefabs/single_brick") as GameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroyBrick() {
		brickDestructionAnimation ();
		destroyedBricks++;
		if (destroyedBricks == 3) {
			Object.Destroy (this.gameObject);
		}
	}

	void brickDestructionAnimation() {
 		brickPool.getObject (true, this.transform.position, this.transform.rotation);
		brickPool.getObject (true, this.transform.position, this.transform.rotation);
		brickPool.getObject (true, this.transform.position, this.transform.rotation);
	}
}
