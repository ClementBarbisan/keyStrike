using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class notes : MonoBehaviour {
	List<AudioSource> sources = new List<AudioSource>();
	public List<AudioClip> clips; 
	int shift = 46;
	// Use this for initialization
	void Start () {
		foreach (AudioClip clip in clips)
		{
			sources.Add (gameObject.AddComponent<AudioSource>());
			sources [sources.Count - 1].clip = clips [sources.Count - 1];
			sources [sources.Count - 1].loop = false;
			sources [sources.Count - 1].playOnAwake = false;
		}
	}

	public void playNote(int nb)
	{
		sources [nb - shift].Play ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
