using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gm;

	//Controller input
	/*public bool playerIndexSet = false;
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;*/

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
		if (gm != null) {
			Destroy (this.gameObject);
		} else {
			gm = this;
		}
	}

	// Use this for initialization
	void Start () {
		/*if (gm == null) {
			gm = this;
		}

		DontDestroyOnLoad (gm);*/
	}

	// Update is called once per frame
	void Update () {/*
		if (!playerIndexSet || !prevState.IsConnected)
		{
			//print ("index set or is connected are false");
			for (int i = 0; i < 4; ++i)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected)
				{
					//Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}

		prevState = state;
		//print ("Prev state is " + prevState);
		state = GamePad.GetState(playerIndex);
		//print ("state is " + state);

		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
		{
			//Debug.Log ("A button pressed in manager");
		}

		if (!prevState.IsConnected) {
			//print ("Prevstate isnt is connected");
		}

		if (prevState.IsConnected) {
			//print ("Prevstate is connected");
		}*/
	}
}