using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadquartersMovement : MonoBehaviour {
	[Header("GameManager")]
	public GameManager gameManager;
	public Rigidbody2D RB;
	[Header("Animations")]
	public Animator Player_AnimCon;
	public SpriteRenderer player_sprite;
	[SerializeField]
	private float speed = 7;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void FixedUpdate(){
		#region KeyboardInput
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

			if (Input.GetButtonDown ("L_Shift")) {
				//player_state = Player_States.Crouching;
				Player_AnimCon.SetBool("Crouch_to_Idle",false);
				Player_AnimCon.SetBool("Idle_to_Crouch",true);
				//transform.localScale = new Vector3 (0.8164563f, 0.6164563f, 0.8164563f);
				speed = 3;
			} else if(Input.GetButton("L_Shift")){
				Player_AnimCon.SetBool("Crouch_to_Idle",false);
				Player_AnimCon.SetBool("Idle_to_Crouch",false);
			}else if (Input.GetButtonUp ("L_Shift")) {
				//player_state = Player_States.Walking;
				Player_AnimCon.SetBool("Idle_to_Crouch",false);
				Player_AnimCon.SetBool("Crouch_to_Idle",true);
				//transform.localScale = new Vector3 (0.8164563f, 0.8164563f, 0.8164563f);
				speed = 5;
			}
			#endregion
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
