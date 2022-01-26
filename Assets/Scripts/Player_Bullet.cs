using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bullet_Type {Lethal, Slepper};


public class Player_Bullet : MonoBehaviour {
	public Bullet_Type bulletState;
	public PlayerController Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("hitting something");
		if (other.gameObject.tag == "Enemy") {
			Debug.Log ("hitting enemy");
			//Player.Enemies_Player_List.Remove (other.gameObject);
			Player.RemoveFromList(Player.Enemies_Player_List, other.gameObject);
			other.gameObject.GetComponent<Basic_Enemy> ().Dead ();
			Destroy (this.gameObject);
		} else if(other.tag == "Walls" || other.tag == "RoomItem"){
			Debug.Log("Hitting: " + other.gameObject);
			Destroy (this.gameObject);
		}
	}
}