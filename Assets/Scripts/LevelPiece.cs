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
	public List<Transform> ConnectionPoints = new List<Transform>();
	public List<Transform> ConnectionPoint_Directions = new List<Transform>();

	public List<int> OpenPoints = new List<int> ();

	public int NumberOfSides = 0;
	int LastSide = 404;

	//[Header("Box Cast Data")]

	Bounds bounds;

	RaycastHit[] hits;
	public List<GameObject> Debug_hits;

	public LevelPiece PreviousConnection;

	//============================================[Unity Functions]=================================================

	void Awake(){
		Calc_Bounds ();
		Set_Points ();
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

	public Vector3 Get_Dir(int w){
		return ConnectionPoint_Directions [w].position - ConnectionPoints [w].position;
	}

	//--------------------------------------------------------------------------------------------------------------

	public Vector3 Get_Pos(int w){
		return ConnectionPoints [w].position;
	}

	//--------------------------------------------------------------------------------------------------------------

	public int Get_Connection(){
		return OpenPoints [Random.Range (0, OpenPoints.Count)];
	}

	//--------------------------------------------------------------------------------------------------------------

	public bool CheckIfFits(){				
		hits = Physics.BoxCastAll (transform.position,bounds.extents * 0.9f,Vector3.up,transform.rotation,bounds.size.y);
		List<RaycastHit> Hits = hits.ToList ();
		int HitsThatIsntConnected = 0;
		foreach (var i in Hits) {
			if (i.collider.transform.root.gameObject != this.transform.root.gameObject) {
				HitsThatIsntConnected++;
				Debug_hits.Add (i.collider.transform.root.gameObject);
			}
		}
		if (HitsThatIsntConnected > 0) {
			return false;
		} else {
			return true;
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	void Calc_Bounds(){
		bounds = new Bounds (transform.position,Vector3.one);
		Renderer[] kids = GetComponentsInChildren<Renderer> ();
		foreach (var i in kids) {
			bounds.Encapsulate (i.bounds);
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	void Set_Points(){	//TODO change this to numbers not directions and make it take all of them
		SpawnPoints_Items = transform.Find ("SpawnPoints_Items").Cast<Transform> ().ToList ();
		SpawnPoints_Units = transform.Find ("SpawnPoints_Units").Cast<Transform> ().ToList ();

		int j = 0;
		foreach (Transform i in transform) {
			if (i.name == "Model" || i.name == "SpawnPoints_Items" || i.name == "SpawnPoints_Units") {
				continue;
			} else {
				ConnectionPoints.Add (i);
				ConnectionPoint_Directions.Add (i.GetChild (0).transform);
				OpenPoints.Add (j++);
			}
		}

		NumberOfSides = ConnectionPoints.Count ();

	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class

