using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuContoller : MonoBehaviour {
	[Header("GameManager")]
	public GameManager gameManager;
	public Text PressStartTXT;

	void Awake(){
		//Test_List.Add ("Awake Add");
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
			PressStartTXT.text = "Press 'Enter' to Start";
			if (Input.GetKeyDown (KeyCode.Return)) {
				Debug.Log("I hit Enter");
				StartCoroutine (SceneChange ());
			}
		
	}

	public IEnumerator SceneChange(){
		yield return new WaitForSeconds (0.5f);
		SceneManager.LoadScene (1);
	}
}
