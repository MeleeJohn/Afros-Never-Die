using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchPaper : MonoBehaviour {

	public Level1_Controller LC;
	public GameObject Player;
	public GameObject topObject;
	public GameObject minimapObject;
	[Header("Research Objective")]
	//public bool researchPickedUp;
	public GameObject researchPaper;
	public GameObject researchPaperInterface;
	public AudioSource AS;
	// Use this for initialization
	void Start () {
		//AS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		Checking();
	}

	private void Checking ()
	{
		float distance = Vector2.Distance (Player.transform.position, researchPaper.transform.position);
		if (distance < 4) {
			researchPaperInterface.SetActive (true);
		}

		if (distance < 4 && Input.GetKeyDown (KeyCode.E)) {
			AS.Play();
			researchPaper.SetActive (false);
			researchPaperInterface.SetActive (false);
			//LC.foundResearch ++;
			LC.ResearchPaperCheck();
			minimapObject.SetActive(false);
			//Destroy(topObject);
		}
	}
}
