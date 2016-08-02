using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour {

	private Text Distance;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		Distance = GetComponent<Text>();
		Distance.text = player.distance.ToString("F0");


	}

	// Update is called once per frame
	void Update () {
		UpdateDistance();

	}

	void UpdateDistance()
	{
		Distance.text = player.distance.ToString("F0");
	}
}
