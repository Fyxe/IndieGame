using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class Purpose: Handle all camera functions.
*/

public class CameraController : MonoBehaviour {
	//===============================================[Variables]====================================================

	[Header("Controls")]
	public bool Follow_Obj 		= true;
	public bool LookAt_Obj 		= true;

	[Header("Following")]
	public Vector3 Offset 		= new Vector3(0,5,-2);
	public float LerpSpeed 		= 0.07f;
	public GameObject Followee 	= null;



	//============================================[Unity Functions]=================================================

	void FixedUpdate(){
		if (Follow_Obj) {
			Follow ();
		}
		if (LookAt_Obj) {
			LookAt_Obj_Helper ();
		}
	}

	//===============================================[Functions]====================================================

	void Follow(){
		if (Followee != null) {			
			transform.position = Vector3.Slerp (transform.position,Followee.transform.position + Offset,LerpSpeed);
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	void LookAt_Obj_Helper(){
		Vector3 Position 	= Followee.transform.position - transform.position;
		Quaternion Rotation = Quaternion.LookRotation (Position);
		transform.rotation 	= Quaternion.Slerp (transform.rotation,Rotation,0.02f);
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------

}	// End of class
