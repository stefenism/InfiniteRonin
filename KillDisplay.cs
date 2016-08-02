using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KillDisplay : MonoBehaviour {


	private Text Kills;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		Kills = GetComponent<Text>();
		Kills.text = player.kills.ToString();

	}

	// Update is called once per frame
	void Update () {
		UpdateKills();
	}

	void UpdateKills()
	{
		Kills.text = player.kills.ToString();
	}
}
