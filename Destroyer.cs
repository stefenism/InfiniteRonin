using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {


	private Rigidbody rb;
	public PlayerController player;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		//player = GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {


		rb.transform.position = new Vector3(player.transform.position.x - 300f, player.transform.position.y, player.transform.position.z);
	}

	void OnCollisionEnter(Collision collider)
	{
		if(collider.gameObject.tag != null)
		{
			print(collider.gameObject.tag + " last destroyed");
			Destroy(collider.gameObject);
		}
		/*else if(collider.gameObject.tag == "Enemy")
		{
			Destroy(collider.gameObject);
		}*/
	}
}
