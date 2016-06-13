using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class controller : MonoBehaviour {
	public int keyCenter = 30;
	public int health = 100;
	public int incr = 1;
	public bool debug = false;
	bool inAttack = false;
	bool isJumping = false;
	public controller opponent;
	bool isUnderAttack = false;
	bool isWalking = false;
	Rigidbody2D rg;
	SpriteRenderer sr;
	Animator anim;
	public AudioSource punch;
	public AudioSource jumpAudio;
	public AudioSource miss;

	private delegate void playNote(int note);
	private playNote notes;

	// Use this for initialization
	void Start () 
	{
		rg = GetComponent<Rigidbody2D> ();	
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		notes += Camera.main.GetComponent<notes> ().playNote;
	}

	IEnumerator walk(float value)
	{
		for (int i = 0; i < 20; i++) {
			transform.Translate (new Vector3 (value / 20.0f, 0.0f, 0.0f));
			yield return null;
		}
		anim.SetBool ("isWalking", false);
		isWalking = false;
	}

	void move(float value)
	{
		isWalking = true;
		anim.SetBool ("isWalking", true);
		StartCoroutine (walk(value));
//		transform.Translate(new Vector3(value, 0.0f, 0.0f)); 
	}

	IEnumerator hitting()
	{
		yield return new WaitForSeconds(0.25f);
		inAttack = false;
		anim.SetBool ("inAttack", false);
		sr.color = Color.white;
	}

//	IEnumerator up(float direction)
//	{
//		Debug.Log ("jumping");
//		for (int i = 0; i < 50; i++) {
//			Debug.Log (direction);
//			transform.Translate (new Vector3 (0.0f, direction, 0.0f));
//			yield return new WaitForSeconds (0.01f);
//		}
//	}

	IEnumerator beaten()
	{
		yield return new WaitForSeconds(0.75f);
		sr.color = Color.white;
		isUnderAttack = false;
	}

	public void beat(float x)
	{
		health -= 10;
		sr.color = Color.red;
		Debug.Log ("currentX = " + transform.position.x);
		if (transform.position.x >= -7.0f && transform.position.x <= 7.0f) {
			isUnderAttack = true;
			if (x > transform.position.x)
				transform.Translate (new Vector3 (-0.65f, 0.0f, 0.0f));
			else
				transform.Translate (new Vector3 (0.65f, 0.0f, 0.0f));
		}
		StartCoroutine ("beaten");
	}

	void jump()
	{
		if (!inAttack && rg.velocity.y == 0.0f) {
			isJumping = true;
			jumpAudio.Play();
			anim.SetBool ("isJumping", true);
			rg.AddForce (new Vector2 (0.0f, 7.5f), ForceMode2D.Impulse);
		}
	}

	void hit()
	{
		if (!inAttack) {
			inAttack = true;
			anim.SetBool ("inAttack", true);
			Debug.Log ("Distance = " + Vector2.Distance (transform.position, opponent.transform.position) + ", distanceX = " + Mathf.Abs (Mathf.Abs (transform.position.x) - Mathf.Abs (opponent.transform.position.x)));
			if (Vector2.Distance (transform.position, opponent.transform.position) <= 1.75f && Mathf.Abs (Mathf.Abs (transform.position.x) - Mathf.Abs (opponent.transform.position.x)) <= 2.0f) {
				punch.Play ();
				opponent.beat (transform.position.x);
			} else
				miss.Play ();
			StartCoroutine ("hitting");
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (health > 0 && opponent.health > 0) {
			if ((MidiMaster.GetKeyDown (keyCenter + 2) || (Input.GetKeyDown (KeyCode.RightArrow) && debug)) && rg.velocity.y <= 0.005f && keyCenter < 74 && !isUnderAttack) {
				notes (keyCenter + 2);
				keyCenter += 1;
				if (keyCenter == opponent.keyCenter) {
					keyCenter++;
					move (0.65f);
					opponent.transform.localScale = new Vector3 (opponent.transform.localScale.x == 3.0f ? -3.0f : 3.0f, 3.0f, 3.0f);
					transform.localScale = new Vector3 (transform.localScale.x == 3.0f ? -3.0f : 3.0f, 3.0f, 3.0f);
				}
				Debug.Log ("KeyCenter = " + keyCenter);
				move (0.65f);
			} else if ((MidiMaster.GetKeyDown (keyCenter - 1) || (Input.GetKeyDown (KeyCode.LeftArrow) && debug)) && rg.velocity.y <= 0.005f && keyCenter > 47 && !isUnderAttack) {
				keyCenter -= 1;
				notes (keyCenter - 1);
				if (keyCenter == opponent.keyCenter) {
					keyCenter--;
					move (-0.65f);
					opponent.transform.localScale = new Vector3 (opponent.transform.localScale.x == 3.0f ? -3.0f : 3.0f, 3.0f, 3.0f);
					transform.localScale = new Vector3 (transform.localScale.x == 3.0f ? -3.0f : 3.0f, 3.0f, 3.0f);
				}
				Debug.Log ("KeyCenter = " + keyCenter);
				move (-0.65f);
			} else if (MidiMaster.GetKeyDown (keyCenter) || (Input.GetKeyDown (KeyCode.UpArrow) && debug)) {
				notes (keyCenter);
				jump ();
			} else if (MidiMaster.GetKeyDown (keyCenter + 1) || (Input.GetKeyDown (KeyCode.DownArrow) && debug)) {
				notes (keyCenter + 1);
				hit ();
			}
			if (rg.velocity.y <= 0.005f) {
				isJumping = false;
				anim.SetBool ("isJumping", false);
			}
		} else
			Debug.Log ("Fuck Off!!!!!!!!!!!");
	}
}
