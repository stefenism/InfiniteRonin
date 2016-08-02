using UnityEngine;
using System.Collections;

public class KillParticleEffect : MonoBehaviour {


	public ParticleSystem bloods;
	// Use this for initialization
	void Start () {
	
		Destroy(this.gameObject,4f);
	}
	
	// Update is called once per frame
	void Update () {
	
		/*if(!bloods.IsAlive())
		{
			Destroy(gameObject);
		}*/
	}
}
