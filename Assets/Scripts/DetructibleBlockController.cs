using UnityEngine;
using System.Collections;

public class DetructibleBlockController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name == "missile") {
			BrickBlockController parentBlock = transform.parent.gameObject.GetComponent<BrickBlockController> ();
			parentBlock.destroyBrick ();
			Object.Destroy (this.gameObject);
		}
	}
}
