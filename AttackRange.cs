using UnityEngine;
using System.Collections;

public class AttackRange : MonoBehaviour {


	public BoxCollider AttackRangeCollider;

	public EnemyHitDetection enemyHitDetection;

	// Use this for initialization
	void Start () {
		AttackRangeCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			enemyHitDetection.inAttackRange = true;
		}
	}
}
