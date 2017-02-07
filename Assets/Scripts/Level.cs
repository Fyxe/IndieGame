﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose: Hold all data for current level and deal with level generation
*/
public class Level : MonoBehaviour {

	//===============================================[Variables]====================================================

	public string Level_Name 					= "New Level";

	int Difficulty;

	Player player;

	public List<LevelPiece> Dangerous_Pieces 	= new List<LevelPiece> ();
	public List<LevelPiece> Friendly_Pieces 	= new List<LevelPiece> ();
	public List<Item> Items 					= new List<Item>();

	public List<LevelPiece> Shop_Pieces 		= new List<LevelPiece> ();

	public LevelPiece StartPiece;
	public LevelPiece EndPiece;

	public int PiecesToGen = 10;

	//============================================[Unity Functions]=================================================

	void Awake(){
		player = FindObjectOfType<GameManager> ().player;
		Difficulty = FindObjectOfType<GameManager> ().Difficulty;
	}

	void Start(){
		GenerateLevel_Linear ();
	}

	//===============================================[Functions]====================================================

	void GenerateLevel_Linear(){	// Does not check boundaries
		//GameObject s = Instantiate (StartPiece.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		int PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
		int W = 0;
		int name = 0;
		Vector3 LastDir = Vector3.back;
		Vector3 LastPos = Vector3.zero;
		for (int i = 0; i < 5; i++) {
			PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
			W = Random.Range(0,3);
			GameObject p = null;
			LevelPiece lp = null;
			float angle = 0;

			switch (W) {
			case 0:	// North
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();
				p.transform.position = LastPos + (p.transform.position - p.GetComponent<LevelPiece>().North.transform.position);

				angle = Vector3.Angle (lp.Get_NorthDir (), LastDir);
				p.transform.RotateAround (LastPos, Vector3.up,-angle);

				Debug.Log ("n");
				LastPos = lp.South.transform.position;
				break;
			case 1:	// South
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();
				p.transform.position = LastPos + (p.transform.position - p.GetComponent<LevelPiece>().South.transform.position);

				angle = Vector3.Angle (lp.Get_SouthDir(),LastDir);
				p.transform.RotateAround (LastPos,Vector3.up,angle);

				Debug.Log ("s");
				LastPos = lp.North.transform.position;
				break;
			case 2: // East
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();
				p.transform.position = LastPos + (p.transform.position - lp.East.transform.position);

				angle = Vector3.Angle (lp.Get_EastDir(),LastDir);
				p.transform.RotateAround (LastPos,Vector3.up,angle);

				Debug.Log ("e");
				LastPos = lp.West.position;
				break;
			case 3: // West
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();
				p.transform.position = LastPos + (p.transform.position - lp.West.transform.position);

				angle = Vector3.Angle (lp.Get_WestDir(),LastDir);
				//p.transform.RotateAround (LastPos,Vector3.up,-angle);

				Debug.Log ("w");
				LastPos = lp.East.transform.position;
				break;
			}
			p.name = "Piece: " + name;
			name++;
			LastDir = LastPos - p.transform.position;

		}
	}

	/*
		Level gen backup
		void GenerateLevel_Linear(){	// Does not check boundaries
		//GameObject s = Instantiate (StartPiece.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		int PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
		Vector3 LastPos = Vector3.zero;
		for (int i = 0; i < 10; i++) {
			PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
			GameObject p = Instantiate(Dangerous_Pieces[PieceNumber].gameObject,
				LastPos + (Dangerous_Pieces[PieceNumber].transform.position - Dangerous_Pieces[PieceNumber].North.transform.position),
				Quaternion.identity) as GameObject;
			LastPos = p.GetComponent<LevelPiece>().South.transform.position;
		}
	}
	*/

	//--------------------------------------------------------------------------------------------------------------

	void AddPiece(Transform LastPlace, int place){		
		/*
			DOES NOT WORK. Was trying to think through the algorihm at 2:00am and it just wasnt computing.
			Will revisit this when I've got the time.
		*/
		int r = Random.Range (0,Dangerous_Pieces.Count - 1);
		int r_start = r;
		bool Fits = false;
		while (!Fits && r != r_start) {
			if (Physics.BoxCast (Vector3.zero, Dangerous_Pieces [r].HalfExtents, Dangerous_Pieces [r].Direction)) {
				r = Iterate (r, Dangerous_Pieces.Count - 1);
			} else {
				Fits = true;
			}
		}

		if (r == r_start) {
			return;
		} else {
			// Place Piece
		}

	}

	//--------------------------------------------------------------------------------------------------------------

	int Iterate(int r, int max_r){
		r++;
		if (r > max_r) {
			return r = 0;
		} else {
			return r;
		}
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class
