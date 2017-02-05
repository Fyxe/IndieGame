using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class Purpose: Store and transfer variables and handle scene changes
*/

public class GameManager : MonoBehaviour {		

	//===============================================[Variables]====================================================

	[Tooltip("This is the player script that holds all of the player's data")]
	public Player player = null;

	public string CurrentScene = "Testing";

	public Level CurrentLevel;

	[Header("Options")]
	[Range(0,3)]
	public int Difficulty = 0;


	[Header("Unity Usefuls")]
	bool Cor_SlowUpdate = false;							
	float SlowUpdateTime = 0.1f;							
	Coroutine Alpha;										

	//============================================[Unity Functions]=================================================
	#region UnityFunctions

	void Awake(){
		if (player == null) {
			Debug.Log ("Player object not manually set, adding...");
			player = FindObjectOfType<Player> ();
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	//void Start(){}																					

	//--------------------------------------------------------------------------------------------------------------

	//void Update(){}

	#endregion
	//===============================================[Functions]====================================================
	#region Functions

	void SlowUpdate(){
		
	}

	//--------------------------------------------------------------------------------------------------------------

	void Save(){
		
	}

	//--------------------------------------------------------------------------------------------------------------

	void Load(){
			
	}

	//--------------------------------------------------------------------------------------------------------------
	#region IEnumerator		

	IEnumerator SlowUpdate_Helper(){						
		Cor_SlowUpdate = true;								

		SlowUpdate ();

		yield return new WaitForSeconds (SlowUpdateTime);

		Cor_SlowUpdate = false;		
	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	#endregion
	//==============================================================================================================

}	// End of class

