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
	public Vector3 HalfExtents = new Vector3(1f,0.01f,1f);
	public float Box_Distance = 1f;

	RaycastHit[] hits;

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

	//--------------------------------------------------------------------------------------------------------------

	public bool CheckIfFits(){
		hits = Physics.BoxCastAll (transform.position,HalfExtents,Vector3.up,Quaternion.identity,Box_Distance);
		List<RaycastHit> Hits = hits.ToList ();
		int HitsThatIsntConnected = 0;
		foreach (var i in Hits) {
			if (i.collider.transform.root != this.transform.root) {
				HitsThatIsntConnected++;
			}
		}
		if (HitsThatIsntConnected > 0) {
			return false;
		} else {
			return true;
		}
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class

