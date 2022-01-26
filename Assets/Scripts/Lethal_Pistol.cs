using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lethal_Pistol : MonoBehaviour {

	[Header("Player")]
	public PlayerController PC;

	[Header("Ammo")]
	public int magazineSize = 12;
	public int Ammo = 12;
	public int AmmoReserve = 68;
	public Text currentMagazine;
	public Text ammoReserve;
	
	[Header("Bullet")]
	public GameObject bullet;
	public GameObject bulletSpawn;

	[Header("Audio")]
	public AudioSource AS;
	public AudioClip ReloadSoundEffect;
	public AudioClip gunShot;

	void Awake(){
		currentMagazine = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<Text>();
		ammoReserve = GameObject.FindGameObjectWithTag("AmmoReserveText").GetComponent<Text>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentMagazine.text = Ammo.ToString ();
		ammoReserve.text = AmmoReserve.ToString ();
		if (Level1_Controller.GS == GameState.Playing && Input.GetMouseButtonDown (0) && Ammo > 0) {
			if (PC.RC != null) {
				PC.RC.RoomAlert ();
			}
				/*Vector3 shootDirection;
				shootDirection = Input.mousePosition;
				shootDirection.z = 0.0f;
				shootDirection = Camera.main.ScreenToWorldPoint (shootDirection);
				shootDirection = shootDirection - transform.position;*/
				AS.clip = gunShot;
				AS.Play();
				GameObject bulletInstance = Instantiate (bullet, bulletSpawn.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
				bulletInstance.GetComponent<Rigidbody2D> ().AddForce (transform.right / 800, ForceMode2D.Impulse);
				Ammo--;
				Destroy (bulletInstance, 1.5f);
				//Debug.Log ("ShootDirection X/12: " + shootDirection.x/14 + "ShootDirection Y/12: " + shootDirection.y/14);
				//bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2 (shootDirection.x * 3,shootDirection.y * 3);
				//bulletInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(shootDirection.x, shootDirection.y)/12, ForceMode2D.Impulse);
				//Destroy (bulletInstance, 1.3f);
			}

		if (Input.GetKeyDown (KeyCode.R) && AmmoReserve > 0) {
			AS.clip = ReloadSoundEffect;
			AS.Play();
			Reload();
		}

	}

	public void Reload(){
		AmmoReserve -=  magazineSize - Ammo;
		Ammo = magazineSize;

	}

	public void AmmoPickup(){
		AmmoReserve += 12;
	}
}
