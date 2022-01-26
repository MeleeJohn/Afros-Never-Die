using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState{Paused, Playing};

public class Level1_Controller : MonoBehaviour {

	public GameObject Player;
	public GameObject ScreenFade;
	public GameObject PauseScreen;
	public static GameState GS = GameState.Playing;
	public GameObject Gameover_UI;
	//public GameObject GameoverFade;
	public GameObject GameOverTimeLine;
	public GameObject objectiveUpdatedTimeLine;
	public GameObject objectiveUpdatedItmes;
	public GameObject levelCompletedTimeLine;
	public GameObject levelCompletedItems;
	public bool canStart;

	[Header("Objective Board")]
	public GameObject Objectives;

	[Header("Sabotage Objective")]
	public bool timeBombPlanted;
	public GameObject timeBomb;
	public GameObject timeBombOutline;
	public GameObject timeBombInterface;
	public int bombTimer = 180;
	public GameObject bombObjectiveCroussout;
	public GameObject timeBombUI;
	public Text bombTimerText;

	[Header("Research Objective")]
	public bool researchPickedUp;
	public GameObject researchObjectiveCroussout;
	public int foundResearch;
	public Text researchCountText;


	[Header("Objective TextBox")]
	public GameObject masterTextBox;
	public Text masterText;
	public string[] objectiveText;

	[Header("Win Condition")]
	public bool canWin;
	public bool gameWon;
	// Use this for initialization
	void Start () {
		StartCoroutine (OpeningFade ());
		StartCoroutine(ObjectiveBoardWait());
		Player = GameObject.FindGameObjectWithTag("Player");	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GS == GameState.Playing && Input.GetKeyDown (KeyCode.Escape)) {
			Debug.Log ("Pressed Escape");
			Time.timeScale = 0.0f;	
			PauseScreen.SetActive (true);
			GS = GameState.Paused;
		} else if (GS == GameState.Paused && Input.GetKeyDown (KeyCode.Escape)) {
			Debug.Log ("Pressed Escape again");
			Resume ();
		}
		if (timeBombPlanted == false) {
			TimeBombCheck ();
		}

		/*if (researchPickedUp == false) {
			ResearchPaperCheck ();
		}*/

		if (timeBombPlanted == true) {
			canWin = true;
		}

		if (timeBombPlanted == true) {
			bombTimerText.text = bombTimer.ToString ();
		}

		if (bombTimer <= 0 && gameWon != true) {
			StartCoroutine(GameOver());
		}
	}

	private IEnumerator OpeningFade(){
		yield return new WaitForSeconds (1.0f);
		StartCoroutine(MasterText(15f,6));
		masterText.fontSize = 17;
		ScreenFade.SetActive (false);
	}

	public IEnumerator GameOver ()
	{
		Gameover_UI.SetActive(true);
		//GameoverFade.SetActive(true);
		GameOverTimeLine.SetActive(true);
		yield return new WaitForSeconds (4.0f);
		SceneManager.LoadScene(1);
	}

	public IEnumerator MissionUpdated ()
	{
		objectiveUpdatedItmes.SetActive(true);
		objectiveUpdatedTimeLine.SetActive(true);
		yield return new WaitForSeconds (6.0f);
		objectiveUpdatedTimeLine.SetActive(false);
		objectiveUpdatedItmes.SetActive(false);
	}

	public IEnumerator MissionCompleted ()
	{
		gameWon = true;
		levelCompletedItems.SetActive(true);
		levelCompletedTimeLine.SetActive(true);
		StartCoroutine(MasterText(7.0f,2));
		yield return new WaitForSeconds(7.0f);
		SceneManager.LoadScene(1);
		levelCompletedItems.SetActive(false);
		levelCompletedTimeLine.SetActive(false);
	}

	#region In-game Menu

	public void Resume(){
		Time.timeScale = 1.0f;
		GS = GameState.Playing;
		PauseScreen.SetActive (false);
	}

	public void QuitGame(){
		Time.timeScale = 1.0f;	
		SceneManager.LoadScene (1);
	}

	#endregion

	public void TimeBombCheck ()
	{
		float distance = Vector2.Distance (Player.transform.position, timeBombOutline.transform.position);
		if (distance < 4) {
			timeBombInterface.SetActive (true);
		}

		if (distance < 4 && Input.GetKeyDown (KeyCode.E)) {
			timeBomb.SetActive (true);
			timeBombPlanted = true;
			timeBombInterface.SetActive (false);
			bombObjectiveCroussout.SetActive(true);
			StartCoroutine (MissionUpdated ());
			timeBombUI.SetActive(true);
			StartCoroutine(BombTimer());
			if (researchPickedUp == true) {
				StartCoroutine (MasterText (5f, 1));
			} else {
				StartCoroutine (MasterText (10f, 0));
			}

		}

		if(timeBombPlanted == true) {
			timeBombInterface.SetActive (false);
		}
	}

	public void ResearchPaperCheck ()
	{
		if (foundResearch < 4) {
			Debug.Log("Found a research paper");
			foundResearch++;
			researchCountText.text = foundResearch.ToString();
		} else {
			Debug.Log("Found all 5 research paper");
			foundResearch++;
			researchCountText.text = foundResearch.ToString();
			researchPickedUp = true;
			researchObjectiveCroussout.SetActive(true);
			StartCoroutine (MasterText (7f, 4));
		}
	}

	public IEnumerator MasterText (float timeUp, int arrayIndex)
	{
		masterTextBox.SetActive(true);
		masterText.text = objectiveText[arrayIndex];
		yield return new WaitForSeconds(timeUp);
		masterText.fontSize = 21;
		masterTextBox.SetActive(false);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log("Welcome Cleveland");
		if (other.tag == "Player" && canWin == true) {
			StartCoroutine(MissionCompleted());
		}
	}

	public IEnumerator BombTimer ()
	{
		if (GS != GameState.Paused && bombTimer > 0) {
			bombTimer--;
		}
		yield return new WaitForSeconds(1.0f);
		StartCoroutine(BombTimer());
	}

	public IEnumerator ObjectiveBoardWait ()
	{
		Objectives.SetActive(true);
		yield return new WaitForSeconds(3.0f);
		canStart = true;
	}
}
