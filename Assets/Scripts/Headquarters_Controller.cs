using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomState{Paused, Playing};

public class Headquarters_Controller : MonoBehaviour {

	public GameObject Bartender;
	public GameObject Player;

	public GameObject MissionSelect;
	public bool InMissionSelect = false;
	public GameObject missionOneDescription;
	public Animator missionSelectAnim;

	public GameObject ScreenFade;
	public GameObject BartenderKey;

	public GameObject PauseScreen;
	public static RoomState RS = RoomState.Playing;

	public GameObject Controls;
	public bool ControlsOnScreen;

	public GameObject creditsMenu;
	public GameObject creditsObj;
	public Animator creditsAnim;
	public bool inCredits;
	public GameObject creditsKey;

	// Use this for initialization
	void Start () {
		StartCoroutine (OpeningFade ());	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float distance = Vector2.Distance (Player.transform.position, Bartender.transform.position);
		float creditsDistance = Vector2.Distance (Player.transform.position, creditsObj.transform.position);
		if (distance < 2) {
			BartenderDisplay ();
		} else {
			BartenderRetractDisplay ();
		}

		if (distance < 2 && Input.GetKeyDown (KeyCode.E) && InMissionSelect == false && creditsDistance > 2) {
			//Debug.Log ("I pressed E");
			InMissionSelect = true;
			MissionSelect.SetActive (true);

		} else if (distance < 2 && Input.GetKeyDown (KeyCode.E) && InMissionSelect == true) {
			//Debug.Log ("Mission select should close");

			missionSelectAnim.SetBool ("MenuAway", InMissionSelect);
			//MissionSelect.SetActive (false);
			InMissionSelect = false;
			StartCoroutine (MenuAway ());
		}

		if (distance > 3 && InMissionSelect == true) {
			missionSelectAnim.SetBool ("MenuAway", InMissionSelect);
			//MissionSelect.SetActive (false);
			InMissionSelect = false;
			StartCoroutine (MenuAway ());
		}

		if (creditsDistance < 2) {
			creditsKey.SetActive (true);
		} else {
			creditsKey.SetActive (false);
		}

		if (creditsDistance < 2 && Input.GetKeyDown (KeyCode.E) && inCredits == false && distance > 2) {
			//Debug.Log ("I pressed E");
			inCredits = true;
			//creditsAnim.SetBool ("CreditsDown", true);
			creditsMenu.SetActive (true);
		} else if (Input.GetKeyDown (KeyCode.E) && inCredits == true) { 
			inCredits = false;
			creditsAnim.SetBool ("CreditsAway", true);
			StartCoroutine (CreditsAway ());
			//creditsMenu.SetActive (true);
		}

		if (creditsDistance > 3 && inCredits == true) {
			inCredits = false;
			creditsAnim.SetBool ("CreditsAway", true);
			StartCoroutine (CreditsAway ());
		}

		if (RS == RoomState.Playing && Input.GetKeyDown (KeyCode.Escape)) {
			Debug.Log ("Pressed Escape");
			Time.timeScale = 0.0f;	
			PauseScreen.SetActive (true);
			RS = RoomState.Paused;
		} else if(RS == RoomState.Paused && Input.GetKeyDown (KeyCode.Escape)){
			//Debug.Log ("Pressed Escape again");
			Resume ();
		}

		if (ControlsOnScreen == false && Input.GetKeyDown (KeyCode.C)) {
			Controls.SetActive (true);
			ControlsOnScreen = true;
		} else if (ControlsOnScreen == true && Input.GetKeyDown (KeyCode.C)) {
			Controls.SetActive (false);
			ControlsOnScreen = false;	
		}
	}

	public void MissionOneHoverSelection ()
	{
		missionOneDescription.SetActive(true);
	}

	public void MissionOneHoverunSelection ()
	{
		missionOneDescription.SetActive(false);
	}

	public void LoadMission1(){
		SceneManager.LoadScene (2);
	}

	private IEnumerator OpeningFade(){
		yield return new WaitForSeconds (1.9f);
		ScreenFade.SetActive (false);
	}

	private void BartenderDisplay(){
		BartenderKey.SetActive (true);
	}

	private void BartenderRetractDisplay(){
		BartenderKey.SetActive (false);
	}

	#region In-game Menu

	public void Resume(){
		Time.timeScale = 1.0f;
		RS = RoomState.Playing;
		PauseScreen.SetActive (false);
	}

	public void StartScreen(){
		Time.timeScale = 1.0f;	
		SceneManager.LoadScene (0);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public IEnumerator MenuAway(){
		yield return new WaitForSeconds(1.5f);
		MissionSelect.SetActive(false);
	}

	public IEnumerator CreditsAway(){
		yield return new WaitForSeconds(2f);
		creditsAnim.SetBool ("CreditsAway",false);
		creditsMenu.SetActive(false);
	}

	#endregion
}
