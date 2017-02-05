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
	int NumberOfSides = 0;
	int LastSide = 404;

	[Header("Box Cast Data")]
	public Vector3 Center;
	public Vector3 HalfExtents;
	public Vector3 Direction;

	//============================================[Unity Functions]=================================================

	void Awake(){
		SpawnPoints = transform.Find ("SpawnPoints").Cast<Transform> ().ToList ();
		North 		= transform.Find ("North");
		South 		= transform.Find ("South");
		East 		= transform.Find ("East");
		West 		= transform.Find ("West");

		if (North != null) {
			NumberOfSides++;
			LastSide = 0;
		}
		if (South != null) {
			NumberOfSides++;
			LastSide = 1;
		}
		if (East != null) {
			NumberOfSides++;
			LastSide = 2;
		}
		if (West != null) {
			NumberOfSides++;
			LastSide = 3;
		}

		if (NumberOfSides > 1) {
			LastSide = 404;
		}
	}

	//===============================================[Functions]====================================================

	public bool IsEnd(){
		if (NumberOfSides == 1) {
			return true;
		} else {
			return false;
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	public int GetLastSide(){
		return LastSide;
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class
