using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class Purpose: Follow a given gameobject and watch it from a given offset
*/
public class FollowScript : MonoBehaviour {

	//===============================================[Variables]====================================================

	public Vector3 Offset = new Vector3(0,8,-3);

	public float LerpSpeed = 0.07f;

	public GameObject Followee = null;

	//============================================[Unity Functions]=================================================

	void FixedUpdate(){
		if (Followee != null) {
			transform.LookAt (Followee.transform);
			transform.position = Vector3.Slerp (transform.position,Followee.transform.position + Offset,LerpSpeed);
		}
	}

	//===============================================[Functions]====================================================

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------

}
