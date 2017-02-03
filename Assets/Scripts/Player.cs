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
		
	[Header("Combat")]
	public bool IsInCombat 			= false;
	bool Cor_IsInCombat 			= false;
	Coroutine Beta;
	public float CombatDelay		= 5f;


	[Header("UnityVariables")]
	bool Cor_SlowUpdate_Helper 		= false;							
	float SlowUpdateTime 			= 0.1f;							
	Coroutine Alpha;										

	//============================================[Unity Functions]=================================================
	#region UnityFunctions

	//void Awake(){}

	//--------------------------------------------------------------------------------------------------------------

	void Start(){
		if (!Cor_SlowUpdate_Helper) {
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
		Debug.Log ("Player was healed by a ghost for: " + amount + " hp.");
	}

	//--------------------------------------------------------------------------------------------------------------

	void Hurt(float amount){
		HP_Current = Mathf.Clamp (HP_Current - amount,0f,HP_Max);

		Debug.Log ("Player was hit by a ghost for: " + amount + " dmg.");

		if (!Cor_IsInCombat) {
			Beta = StartCoroutine (IsInCombat_Helper ());
		} else {
			StopCoroutine (Beta);
			Beta = StartCoroutine (IsInCombat_Helper ());
		}

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
		Cor_SlowUpdate_Helper = true;								

		SlowUpdate ();

		yield return new WaitForSeconds (SlowUpdateTime);

		Cor_SlowUpdate_Helper = false;		
	}

	//--------------------------------------------------------------------------------------------------------------

	IEnumerator IsInCombat_Helper(){
		Cor_IsInCombat = true;

		IsInCombat = true;

		yield return new WaitForSeconds (CombatDelay);

		IsInCombat = false;

		Cor_IsInCombat = false;
	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	#endregion
	//==============================================================================================================

}	// End of class
