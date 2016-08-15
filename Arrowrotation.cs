using UnityEngine;
using System.Collections;

public class Arrowrotation : MonoBehaviour {

	Rigidbody arrowRB;
	float angle;
	// Use this for initialization
	void Start () {
		arrowRB = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {


		angle = Mathf.Atan2(arrowRB.velocity.y, arrowRB.velocity.x) * Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0f,0f,angle);
	}
}
