using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {
	public controller player;
	Slider slide;
	// Use this for initialization
	void Start () {
		slide = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		slide.value = player.health / 100.0f;
	}
}
