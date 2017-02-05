using System.Collections;
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


	//===============================================[Functions]====================================================

	void GenerateLevel(){
		//GameObject s = Instantiate (StartPiece.gameObject, Vector3.zero, Quaternion.identity) as GameObject;

	}

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
