using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose: Creating the structure that will allow different pieces to fit together.
*/
public class LevelPiece : MonoBehaviour {

	//===============================================[Variables]====================================================

	public List<Transform> SpawnPoints = new List<Transform>();
	public Transform North, South, East, West;

	//============================================[Unity Functions]=================================================

	void Awake(){
		SpawnPoints = transform.Find("SpawnPoints").Cast<Transform> ().ToList ();
		North = transform.Find ("North");
		South = transform.Find ("South");
		East = transform.Find ("East");
		West = transform.Find ("West");

	}

	//===============================================[Functions]====================================================

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class
