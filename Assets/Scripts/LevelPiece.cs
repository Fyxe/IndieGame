using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose: Creating the structure that will allow different pieces to fit together.
*/
public class LevelPiece : MonoBehaviour {

	//===============================================[Variables]====================================================

	public List<Transform> SpawnPoints_Items = new List<Transform>();
	public List<Transform> SpawnPoints_Units = new List<Transform>();
	public Transform North, South, East, West;
	public Transform NorthDir, SouthDir, EastDir, WestDir;

	public int NumberOfSides = 0;
	int LastSide = 404;

	[Header("Box Cast Data")]
	public Vector3 Center;
	public Vector3 HalfExtents;
	public Vector3 Direction;

	//============================================[Unity Functions]=================================================

	void Awake(){
		SpawnPoints_Items = transform.Find ("SpawnPoints_Items").Cast<Transform> ().ToList ();
		SpawnPoints_Units = transform.Find ("SpawnPoints_Units").Cast<Transform> ().ToList ();

		North 		= transform.Find ("North");
		South 		= transform.Find ("South");
		East 		= transform.Find ("East");
		West 		= transform.Find ("West");

		if (North != null) {
			NumberOfSides++;
			NorthDir = North.transform.Find ("Dir");
			LastSide = 0;
		}
		if (South != null) {
			NumberOfSides++;
			SouthDir = South.transform.Find ("Dir");
			LastSide = 1;
		}
		if (East != null) {
			NumberOfSides++;
			EastDir = East.transform.Find ("Dir");
			LastSide = 2;
		}
		if (West != null) {
			NumberOfSides++;
			WestDir = West.transform.Find ("Dir");
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

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_NorthDir(){
		return NorthDir.transform.position - North.transform.position; 
	}

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_SouthDir(){
		return SouthDir.transform.position - South.transform.position; 
	}

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_EastDir(){
		return EastDir.transform.position - East.transform.position; 
	}

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_WestDir(){
		return WestDir.transform.position - West.transform.position; 
	}

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_Dir(int Where){
		
		switch (Where) {
		case 0:
			return NorthDir.transform.position - North.transform.position; 
		case 1:
			return SouthDir.transform.position - South.transform.position; 
		case 2:
			return EastDir.transform.position - East.transform.position; 
		case 3:
			return WestDir.transform.position - West.transform.position; 			
		}
		return Vector3.zero;
	}

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_localPos(int Where){
		switch (Where) {
		case 0:
			return North.transform.localPosition; 
		case 1:
			return South.transform.localPosition; 
		case 2:
			return East.transform.localPosition; 
		case 3:
			return West.transform.localPosition; 			
		}
		return Vector3.zero;
	}

	public Vector3 Get_Pos(int Where){
		switch (Where) {
		case 0:
			return North.transform.position; 
		case 1:
			return South.transform.position; 
		case 2:
			return East.transform.position; 
		case 3:
			return West.transform.position; 			
		}
		return Vector3.zero;
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class

