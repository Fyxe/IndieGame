using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class Purpose: TBA (Describe what this class will be used for)
*/

public class Example : MonoBehaviour {		// This example is set to half of a 1920x1080 resolution screen

	//===============================================[Variables]====================================================
	int stuff 			= 1;								// Keep ='s on line near others
	int NothingSpecial  = 0;								// Declare variables closer to those that are related
															// Keep comments in-line with eachother 
															// throughout the entire file if possible, if not
															// Change it for one function and bring it back
															// For example see line 34

	bool Cor_SlowUpdate  = false;							// Each coroutine should have a bool that tracks use
	float SlowUpdateTime = 0.1f;							
															

	Coroutine Alpha;										// Coroutine naming should follow greek letters 

	//============================================[Unity Functions]=================================================
	#region UnityFunctions

	void Awake(){

	}

	//--------------------------------------------------------------------------------------------------------------

	void Start(){																		// Some functions are much
		Vector3 ThisIsTheVariableForAVeryLongLine = new Vector3 (10f, 123.043f, 122f);	// longer than others and 
																						// comments should reflect
	}																					// that.

	//--------------------------------------------------------------------------------------------------------------

	//void Update(){} 										// If functions are not used but might be needed, 
															// Comment them out to prevent extra calls

	#endregion
	//===============================================[Functions]====================================================
	#region Functions

	void SlowUpdate(){
		
	}

	//--------------------------------------------------------------------------------------------------------------
	#region IEnumerator		// Internal Regions must be bounded by the --- barriers 

	IEnumerator SlowUpdate_Helper(){						// Specific coroutines for functions should be labeled 
															// as X_Helper

		Cor_SlowUpdate = true;								// and coroutine booleans as Cor_X

		SlowUpdate ();

		yield return new WaitForSeconds (SlowUpdateTime);

		Cor_SlowUpdate = false;		
	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	#endregion
	//==============================================================================================================

}	// End of class (at the end of every class)


/*

(Easy copy and paste structure)


	
	Class Purpose: TBA (Describe what this class will be used for)


	//===============================================[Variables]====================================================
	//============================================[Unity Functions]=================================================
	//===============================================[Functions]====================================================
	//==============================================================================================================	
	//--------------------------------------------------------------------------------------------------------------

*/

/*
The nomenclature that I can remember.
Cortoutines:
	- Variable names for corutines should follow greek letters
	- Booleans to keep track of coroutines should be Cor_FunctionName
	- Coroutines that are for specific functions are FunctionName_Helper

Functions that calculate variables or data structures should be labeled as Calc_FunctionName
	- Such as to create the terrain and store it in a created class could be Calc_Terrain()

Functions that return a boolean value should have a Check_X prefix
	- Check_IsGrounded


*/




