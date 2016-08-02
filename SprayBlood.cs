using UnityEngine;
using System.Collections;

public class SprayBlood : MonoBehaviour {

	public ParticleSystem bloods;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void FireBloodParticles()
	{
		Vector3 position = transform.position + new Vector3(0,0,-.1f);
		ParticleSystem localBloodsObj = GameObject.Instantiate(bloods, position, bloods.transform.rotation) as ParticleSystem;
		localBloodsObj.Play();
	}
}
