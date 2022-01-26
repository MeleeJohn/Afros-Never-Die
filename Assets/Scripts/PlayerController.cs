using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum HealthyNess {Alive, Dead};
public enum Stealth_States {Hidden, Detected};
public enum Disguised_States {UnDisguised, Disguised};
public enum Player_States{ Walking, Crouching, Running, Rolling};

public class PlayerController : MonoBehaviour {

	[Header("GameManager")]
	public GameManager gameManager;
	public Rigidbody2D RB;
	//[SerializeField]
	//private float moveX = 0;
	//[SerializeField]
	//private float moveY = 0;
	[SerializeField]
	private float speed = 7;

	[SerializeField]
	public Slider HealthSlider;
	[SerializeField]
	private float Health = 4;

	//public float LerpSpeed = 2.0f;
	[Header("Animations")]
	public Animator Player_AnimCon;
	public SpriteRenderer player_sprite;
	[Header("Player States")]
	[SerializeField]
	public Player_States player_state;
	[SerializeField]
	public Stealth_States stealth_State;
	public HealthyNess health_state;
	[Header("Enemies")]
	public float ClosestEnemy_distance;
	public GameObject ClosestEnemy;
	//public GameObject[] Enemies_Player;
	public List<float> Enemy_Distances_List = new List<float> ();
	public List<GameObject> Enemies_Player_List = new List<GameObject> ();

	[Header("Particles")]
	public GameObject QuickParticles;

	[Header("Gun")]
	public GameObject GunTop_Object;
	public GameObject ActiveWeapon;

	[Header("Level Controller")]
	public Level1_Controller LC;
	public Room_Controller RC;

	[Header("invulnerability")]
	public bool invulnerable;

	[Header("Audio Source")]
	public AudioSource AS;
	public AudioClip Hurt;
	public AudioClip dodgeRoll;
	public bool canRoll;
	//public List<string> Test_List = new List<string> ();
	//public float[] Enemy_Distances;

	void Awake(){
		//Test_List.Add ("Awake Add");
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		LC = GameObject.FindGameObjectWithTag("GameController").GetComponent<Level1_Controller>();
	}

	void Start () {
		RB = this.GetComponent<Rigidbody2D> ();
		//Test_List.Add ("Start Add");
	}

	void FixedUpdate(){
		#region KeyboardInput
		if(health_state != HealthyNess.Dead && LC.canStart == true){
			float moveX = Input.GetAxis ("Horizontal");
			float moveY = Input.GetAxis ("Vertical");
			if (Input.GetKeyDown (KeyCode.A)) {
				Player_AnimCon.SetBool("Idle_to_Walk",true);
				Player_AnimCon.SetBool("Walk_to_Roll",false);
				Player_AnimCon.SetBool("Walk_to_Idle",false);
				transform.localScale = (new Vector3(-1f,1f,1f));
			} else if(Input.GetKeyUp (KeyCode.A)){
				Player_AnimCon.SetBool("Idle_to_Walk",false);
				Player_AnimCon.SetBool("Walk_to_Idle",true);
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				Player_AnimCon.SetBool("Idle_to_Walk",true);
				Player_AnimCon.SetBool("Walk_to_Roll",false);
				Player_AnimCon.SetBool("Walk_to_Idle",false);
				transform.localScale = (new Vector3(-1f,1f,1f));
			} else if(Input.GetKeyUp (KeyCode.W)){
				Player_AnimCon.SetBool("Idle_to_Walk",false);
				Player_AnimCon.SetBool("Walk_to_Idle",true);
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				Player_AnimCon.SetBool("Idle_to_Walk",true);
				Player_AnimCon.SetBool("Walk_to_Roll",false);
				Player_AnimCon.SetBool("Walk_to_Idle",false);
				transform.localScale = (new Vector3(-1f,1f,1f));
			} else if(Input.GetKeyUp (KeyCode.S)){
				Player_AnimCon.SetBool("Idle_to_Walk",false);
				Player_AnimCon.SetBool("Walk_to_Idle",true);
			}

			if (Input.GetKeyDown (KeyCode.D)) {
				Player_AnimCon.SetBool("Idle_to_Walk",true);
				Player_AnimCon.SetBool("Walk_to_Roll",false);
				Player_AnimCon.SetBool("Walk_to_Idle",false);
				transform.localScale = (new Vector3(1f,1f,1f));
			} else if(Input.GetKeyUp (KeyCode.D)){
				Player_AnimCon.SetBool("Idle_to_Walk",false);
				Player_AnimCon.SetBool("Walk_to_Idle",true);
			}

			Vector2 movement = new Vector2 (moveX, moveY);
			RB.velocity = movement * speed;

			/*if (Input.GetButtonDown ("L_Shift") && player_state == Player_States.Walking) {
				Debug.Log("Crouching");
				StartCoroutine(Crouchbuffer(Player_States.Crouching, 0.4f));
				//player_state = Player_States.Crouching;
				Player_AnimCon.SetBool("Crouch_to_Idle",false);
				Player_AnimCon.SetBool("Idle_to_Crouch",true);
				//transform.localScale = new Vector3 (0.8164563f, 0.6164563f, 0.8164563f);
				speed = 3;
			}*/

			if(Input.GetButton("L_Shift") && player_state == Player_States.Walking) {
				player_state = Player_States.Crouching;
				speed = 3;
				Player_AnimCon.SetBool("TransitionBack", false);
				Player_AnimCon.SetBool("Idle_to_Crouch",true);
				Player_AnimCon.SetBool("Walk_to_Crouch",true);
			}

			if (Input.GetButtonUp ("L_Shift") && player_state == Player_States.Crouching) {
				Debug.Log("Walking");
				//player_state = Player_States.Walking;
				StartCoroutine(Crouchbuffer(Player_States.Walking, 0.4f));
				Player_AnimCon.SetBool("Idle_to_Crouch",false);
				Player_AnimCon.SetBool("Walk_to_Crouch",false);
				//Player_AnimCon.SetBool("Crouch_to_Idle",true);
				Player_AnimCon.SetBool("TransitionBack", true);
				//transform.localScale = new Vector3 (0.8164563f, 0.8164563f, 0.8164563f);
				speed = 5;
			}

			if (Input.GetKeyDown (KeyCode.E) && ClosestEnemy != null && ClosestEnemy_distance < 2 && stealth_State == Stealth_States.Hidden) {
				//Debug.Log("Killing enemy");
				//Enemies_Player_List.Remove (ClosestEnemy);
				RemoveFromList(Enemies_Player_List, ClosestEnemy);
				ClosestEnemy.GetComponent<Basic_Enemy> ().Dead ();
				ClosestEnemy = null;
				//Debug.Log("Killed enemy");
			}

			if(player_state != Player_States.Rolling && Input.GetButtonDown("Jump")){
				StartCoroutine(DodgeRoll());
			}

			/*Vector3 GunRotation;
			GunRotation = Input.mousePosition;
			GunRotation.z = 0.0f;
			GunRotation = Camera.main.ScreenToWorldPoint (GunRotation);
			//GunRotation = GunRotation - transform.position;
			float GunAngle = Mathf.Atan2(GunRotation.y, GunRotation.x) * Mathf.Rad2Deg;
			GunTop_Object.transform.rotation = Quaternion.Euler(new Vector3(0,0, GunAngle));
			*/

			var mouse = Input.mousePosition;
			var screentoPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
			var offset = new Vector2(mouse.x - screentoPoint.x, mouse.y - screentoPoint.y);
			var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			GunTop_Object.transform.rotation = Quaternion.Euler(new Vector3 (0,0 ,angle));

//			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			float AngleRad = Mathf.Atan2(pos.y - GunTop_Object.transform.position.y, pos.x - GunTop_Object.transform.position.x);
//			float AngleDeg = (180 / Mathf.PI) * AngleRad;
//			GunTop_Object.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
		
		#endregion

		//Debug.Log ("Input.mousePosition.x is: " + Input.mousePosition.x);
		if (Input.mousePosition.x < 500) {
			this.transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
			GunTop_Object.transform.localScale = new Vector3 (-1.59f, -1.59f, 1.59f);
		} else {
			this.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			GunTop_Object.transform.localScale = new Vector3 (1.59f, 1.59f, 1.59f);
		}
		#region Miscellanous
		if(ClosestEnemy != null){
			if(ClosestEnemy_distance<2 && stealth_State == Stealth_States.Hidden){
				Debug.Log("Displaying Keys");
				ClosestEnemy.GetComponent<Basic_Enemy> ().DisplayKey();
			} else if (ClosestEnemy == null || ClosestEnemy_distance>2 && stealth_State != Stealth_States.Hidden){
				ClosestEnemy.GetComponent<Basic_Enemy> ().RemoveKey();
				Debug.Log("Removing Keys");
			}
		}
		#endregion
		}
	}



	void Update ()
	{
		HealthSlider.value = Health;

		if (Health <= 0 && health_state != HealthyNess.Dead) {
			health_state = HealthyNess.Dead;
			StartCoroutine(LC.GameOver());
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		/*Debug.Log ("Hitting something");
		if (other.gameObject.tag == "Enemy") {
			Enemy = other.gameObject;
		}*/

		Debug.Log ("Hitting something");
		if (other.gameObject.tag == "Enemy_Bullet" && invulnerable == false) {
			invulnerable = true;
			StartCoroutine(Invunerability());
			Health-=1;
			//Hit();
			Destroy(other.gameObject);
		} else if(other.gameObject.tag == "Enemy_Bullet" && invulnerable == true){
			Destroy(other.gameObject);
		}
	}

	public void getEnemies(List<GameObject> Enemies){
		//Enemies_Player = new GameObject[Enemies.Length];
		//Enemy_Distances = new float[Enemies.Length];
		//List<GameObject> Enemies_Player_List = new List<GameObject> ();
		for (int i = 0; i < Enemies.Count; i++) {
			Enemies_Player_List.Add (Enemies [i].gameObject);
		}
		//Debug.Log ("Grabbing Enemies");
		StartCoroutine (CheckEnemyDistance ());
	}

	public void RemoveEnemies(){
		/*for (int i = Enemies_Player_List.Count; i > 0; i--) {
			//Enemies_Player [i] = null;
			Enemies_Player_List.RemoveAt(i);
		}*/
		Enemy_Distances_List.Clear ();
		Enemies_Player_List.Clear();
		//Debug.Log ("Removing Enemies");
	}

	private IEnumerator CheckEnemyDistance(){
		if (Enemies_Player_List.Count == 0) {

		} else{
		
			if (Enemies_Player_List.Count == 1) {
				ClosestEnemy = Enemies_Player_List [0];
				for (int i = 0; i < Enemies_Player_List.Count; i++) {
					if (Enemies_Player_List [i] != null) {
						Debug.Log ("Closest enemy is: " + Enemies_Player_List [i]);
						ClosestEnemy = Enemies_Player_List [i];
					}
				}
			} else {
				for (int i = 0; i < Enemies_Player_List.Count; i++) {
					float distance = Vector2.Distance (transform.position, Enemies_Player_List [i].gameObject.transform.position);
					Enemy_Distances_List.Add (distance);
				}
				if (Enemies_Player_List.Count != 1) {
					ClosestEnemy = Enemies_Player_List [SmallestDistance ()];
				}
			}
			yield return new WaitForSeconds (0.1f);

			StartCoroutine (CheckEnemyDistance ());
		}
	}

	private int SmallestDistance(){
		int indexOfClosestEnemy = 0;

		for (int i = 0; i < Enemy_Distances_List.Count; i++) {
			//Debug.Log ("Checking distance on: " + Enemies_Player_List [i]);
			//Debug.Log ("Enemy distance list count is: " + Enemy_Distances_List.Count + " and variable (i) is: " + i);
			if (i != Enemy_Distances_List.Count - 1 && Enemy_Distances_List [i] < Enemy_Distances_List [i + 1]) {
				Debug.Log ("Closest enemy is: " + Enemies_Player_List [indexOfClosestEnemy]);
				indexOfClosestEnemy = i;
			} else {
				if (Enemy_Distances_List [i] < Enemy_Distances_List [0]) {
					Debug.Log ("Closest enemy is: " + Enemies_Player_List [indexOfClosestEnemy]);
					indexOfClosestEnemy = i;
				}
			}
		}
		//Debug.Log ("Checked Distances");
		Enemy_Distances_List.Clear ();
		Debug.Log ("Index of closest enemy is " + indexOfClosestEnemy);

		if (Enemies_Player_List.Count != 0) {
			//Debug.Log("Closest Enemy Distance... thing");
			ClosestEnemy_distance = Vector2.Distance (transform.position, Enemies_Player_List [indexOfClosestEnemy].gameObject.transform.position);
		}
		return indexOfClosestEnemy;
	}

	public IEnumerator DodgeRoll(){
		player_state = Player_States.Rolling;
		AS.clip = dodgeRoll;
		AS.Play();
		speed = 9;
		//QuickParticles.Play ();
		if (Player_AnimCon.GetBool ("Idle_to_Walk") == true) {
			Player_AnimCon.SetBool ("Walk_to_Roll", true);
			Player_AnimCon.SetBool ("Roll_to_Walk", false);
			Player_AnimCon.SetBool ("Roll_to_Idle", false);
		} else {
			Player_AnimCon.SetBool("Idle_to_Roll",true);
			Player_AnimCon.SetBool("Roll_to_Walk",false);
		}
		Instantiate(QuickParticles,transform.position,Quaternion.identity);
		yield return new WaitForSeconds (0.75f);
		if (Player_AnimCon.GetBool ("Walk_to_Roll") == true) {
			Player_AnimCon.SetBool ("Roll_to_Walk", true);
			Player_AnimCon.SetBool ("Walk_to_Roll", false);
		} else {
			Player_AnimCon.SetBool ("Roll_to_Idle", true);
			Player_AnimCon.SetBool ("Idle_to_Roll", false);
		}
		speed = 5;
		yield return new WaitForSeconds (0.5f);
		player_state = Player_States.Walking;
	}

	public void RemoveFromList(List<GameObject> otherList, GameObject other){
		otherList.Remove (other);
	}

	private IEnumerator Crouchbuffer (Player_States DesiredPS, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		player_state = DesiredPS;
	}

	public void Hit(){
		Debug.Log("Been hit");
		AS.clip = Hurt;
		AS.Play();
		Health-=1;
	}

	public IEnumerator Invunerability ()
	{
		invulnerable = true;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(0.25f);
		GetComponent<SpriteRenderer>().enabled = true;
		invulnerable = false;
	}
}
