using UnityEngine;
using System.Collections;

public class GroundDetect : MonoBehaviour {


	public PlayerController player;
	public float groundDistance;
	//public LayerMask Source;
	// Use this for initialization
	void Start () {

		//player = GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void LateUpdate () {

		//print(player.Grounded + " grounded");


			GroundDetection();

	}

	void GroundDetection()
	{
		RaycastHit hit;

		Ray landingRay = new Ray(transform.position, Vector3.down);

		Debug.DrawRay(transform.position, Vector3.down * groundDistance);

		//if(!player.Grounded)
		//{
			if(Physics.Raycast(landingRay, out hit, groundDistance))
			{
				//print(hit.collider.tag + " collided tag");
				if(hit.collider.tag == "Ground")
				{
					player.Grounded = true;
					player.anim.SetBool("jumping", false);
				}
			}
			else
			{
				player.Grounded = false;
				player.anim.SetBool("jumping", true);
			}
		//}
	}

	/*
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			player.Grounded = false;
			GroundDetection();
      Debug.Log(collision);
		}
  }

	/*
	void OnTriggerExit(Collider collision)
  {
    if (collision.gameObject.tag == "Ground")
    {
      player.Grounded = false;
    }
  }
	*/

}
