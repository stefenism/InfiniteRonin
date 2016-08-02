using UnityEngine;
using System.Collections;

public class PlaySounds : MonoBehaviour {

	private AudioSource sound;
	public AudioClip[] sfx;

	// Use this for initialization
	void Awake()
	{
		sound = GetComponent<AudioSource>();
	}
	void Start () {
	
	}
	
	public void PlaySound(int clippy)
	{
		sound.clip = sfx[clippy];
		sound.Play();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
