using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Colliding with something");
		if (other.tag == "Player") {
			if (other.GetComponent<PlayerController> ().ActiveWeapon.GetComponent<Lethal_Pistol> ().AmmoReserve <= 82) {
				other.GetComponent<PlayerController> ().ActiveWeapon.GetComponent<Lethal_Pistol> ().AmmoPickup ();
				Destroy(this.gameObject);
			}
		}
	}
}
