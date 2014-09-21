using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public Transform player;
	public Transform statue;
	private Transform currentTarget;
	private NavMeshAgent agent;


	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update () {
		float distToPlayer = Vector3.Distance (transform.position, player.position);
		float distToStatue = Vector3.Distance (transform.position, statue.position);
		currentTarget = (distToPlayer < distToStatue)? player : statue;

		agent.SetDestination (currentTarget.position);
	}
}
