using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Weapon : MonoBehaviour {
	public Basic_Enemy BE;
	public GameObject bullet;
	public GameObject bulletSpawn;
	private float fireDelta = 0.85f;
	private float nextFire = 0.85f;
	private float myTime = 0.0f;
	private int Ammo = 15;

	[Header("Audio")]
	public AudioSource AS;
	public AudioClip ReloadSoundEffect;
	public AudioClip gunShot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		myTime = myTime + Time.deltaTime;
		if (BE.canShoot == true && Ammo > 0 && myTime > nextFire) {
			Debug.Log ("Shooting my gun");
			nextFire = myTime + fireDelta;
			AS.clip = gunShot;
			AS.Play();
			Fire ();
			nextFire = nextFire - myTime;
			myTime = 0.0f;
		}

		if (Ammo == 0) {
			AS.clip = ReloadSoundEffect;
			AS.Play();
			StartCoroutine(Reload());
		}
	}

	private void Fire ()
	{
		GameObject bulletInstance = Instantiate (bullet, bulletSpawn.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
		bulletInstance.GetComponent<Rigidbody2D> ().AddForce (-transform.right / 800, ForceMode2D.Impulse);
		Ammo--;
		Destroy (bulletInstance, 1.5f);
	}

	private IEnumerator Reload ()
	{
		yield return new WaitForSeconds(3.0f);
		Ammo = 15;
	}
}
