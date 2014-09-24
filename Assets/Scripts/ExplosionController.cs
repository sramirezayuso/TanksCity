using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {
	private float timer = 0.0f;

	void Update () {
		if (timer > 4.0f)
			Destroy(gameObject);
		else
			timer += Time.deltaTime;
	}
}
