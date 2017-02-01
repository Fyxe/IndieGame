using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class Purpose: Store and compute data that is used for the player
*/

public class Player : MonoBehaviour {		

	//===============================================[Variables]====================================================

	[Header("Attributes")]
	public string Player_Name = "Bob";

	[Header("Statistics")]
	public float HP_Current 		= 100f;
	public float HP_Max 			= 100f;

	public float Stamina_Current	= 100f;
	public float Stamina_Max		= 100f;

	[Header("Movement")]
	public int Move_Mode 			= 0;
	public float Move_Speed 		= 1f;
	public float Jump_Force 		= 100f;
	public bool IsGrounded  		= false;
		
	bool Cor_SlowUpdate 			= false;							
	float SlowUpdateTime 			= 0.1f;							
	Coroutine Alpha;										

	//============================================[Unity Functions]=================================================
	#region UnityFunctions

	//void Awake(){}

	//--------------------------------------------------------------------------------------------------------------

	void Start(){
		if (!Cor_SlowUpdate) {
			Alpha = StartCoroutine (SlowUpdate_Helper ());
		}
	}																					

	//--------------------------------------------------------------------------------------------------------------

	void Update(){
		Input ();
	}

	#endregion
	//===============================================[Functions]====================================================
	#region Functions

	void Input(){
		
	}

	//--------------------------------------------------------------------------------------------------------------
	#region HP_Commands

	void Heal(float amount){
		
		HP_Current = Mathf.Clamp (HP_Current + amount,0f,HP_Max);

	}

	//--------------------------------------------------------------------------------------------------------------

	void Hurt(float amount){

		HP_Current = Mathf.Clamp (HP_Current - amount,0f,HP_Max);

		if (HP_Current == 0) {

			PlayerHasDied ();

		}

	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	void PlayerHasDied(){
		
	}

	//--------------------------------------------------------------------------------------------------------------

	void SlowUpdate(){

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
