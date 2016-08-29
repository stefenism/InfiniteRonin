using UnityEngine;
using System.Collections;

public class Arrowrotation : MonoBehaviour {

	Rigidbody arrowRB;
	BoxCollider boxCollider;
	float angle;
	private bool inAir;
	// Use this for initialization
	void Start () {

		inAir = true;
		arrowRB = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
	}

	// Update is called once per frame
	void Update () {


		if(inAir)
		{
			angle = (Mathf.Atan2(arrowRB.velocity.y, arrowRB.velocity.x) * Mathf.Rad2Deg) - 180f;
			transform.eulerAngles = new Vector3(0f,0f,angle);
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "enemy")
		{
			inAir = false;
			boxCollider.enabled = false;
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Sword")
		{
			Destroy(this.gameObject);
		}
	}
}
