using UnityEngine;
using System.Collections;

public class SpawnArcher : MonoBehaviour {

	public GameObject archer;
	public Transform[] spawnPoints;

	private GameObject clone;


	// Use this for initialization
	void Start () {

		for(int i = 0; i < spawnPoints.Length; i++)
		{
			clone = Instantiate(archer, spawnPoints[i].transform.position, Quaternion.identity) as GameObject;
		}


	}

	// Update is called once per frame
	void Update () {

	}
}
