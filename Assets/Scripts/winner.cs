using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class winner : MonoBehaviour {
	public controller player1;
	public controller player2;
	AudioSource clip;
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		clip = GetComponent<AudioSource> ();
		text.text = "";
	}

	IEnumerator reload()
	{
		yield return new WaitForSeconds (2);
		SceneManager.LoadScene("test");
	}

	// Update is called once per frame
	void Update () {
		if (player1.health <= 0 || player2.health <= 0) {
			clip.Play ();
			text.text = "Win";
			StartCoroutine ("reload");
		}
	}	
}
