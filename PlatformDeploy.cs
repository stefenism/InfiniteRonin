using UnityEngine;
using System.Collections;

public class PlatformDeploy : MonoBehaviour {

	private Rigidbody rb;
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			rb.isKinematic = true;
		}
	}
}
