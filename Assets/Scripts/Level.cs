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

	//--------------------------------------------------------------------------------------------------------------

	void Start(){
		GenerateLevel();
	}

	//===============================================[Functions]====================================================

	void GenerateLevel(){

		int Random_Piece = 0;			// Used to get a random piece from Dangerous_Pieces

		int Last_Connection = 0;		// The last piece connection used
		int Current_Connection = 0;		// The current place that will connect to the last levelpiece's place
		int Next_Conntection = 1;		// The next piece 

		List<int> Remaining_Connections = new List<int>();	// Allows to pick a next piece which isnt the current piece

		Vector3 Last_ConnectionPosition = Vector3.zero;			// The position of the last connection
		Vector3 Last_DirectionFromConnection = Vector3.zero;	// The direction from the last connection inwards
		List<int> Last_Remaining_Connections = new List<int>();	// Allows to keep track of last positions

		GameObject p;					// Used as the piece currently being positioned
		LevelPiece lp;					// Used as the levelpiece of p
		LevelPiece Last_lp = null;		// Used as the levelpiece of last p

		float angle = 0;				// angle between last position inwards, and current piece's connection point outwards

		for (int i = 0; i < 15; i++) {	// Currently uses a set number
			Random_Piece = Random.Range (0,Dangerous_Pieces.Count);		// Picks a piece from the given list
			Remaining_Connections = new List<int> ();					// Clears remaining connections

			p = Instantiate (Dangerous_Pieces [Random_Piece].gameObject, Vector3.zero, Quaternion.identity) as GameObject;	
			p.name = "Piece: " + i;
			lp = p.GetComponent<LevelPiece> ();							// These 3 statements generate the piece and name it

			for (int j = 0; j < lp.NumberOfSides; j++) {				// This adds all available sides that can be used to connect
				Remaining_Connections.Add(j);
			}



			Current_Connection = Random.Range (0,lp.NumberOfSides);		// Picks a random side to connect
			Remaining_Connections.Remove(Current_Connection);			// Removes that side from being chosen again
			Next_Conntection = Remaining_Connections[Random.Range (0,Remaining_Connections.Count)];	// Picks a random place for the next piece to connect

			p.transform.position = Last_ConnectionPosition + (p.transform.position - lp.Get_Pos (Current_Connection));	// Moves the piece into place

			if (Last_lp != null) {	// Checks to prevent null, only for first piece
				angle = MathT.AngleSigned (lp.Get_Dir(Current_Connection),Last_lp.Get_Dir(Last_Connection) * (-1),Vector3.up); // calculates angle needed to rotate
			}

			p.transform.RotateAround(lp.Get_Pos(Current_Connection),Vector3.up,angle);	// Rotates piece into place

			//Debug.Log ("LastCon: " +Last_Connection+ " Cur Con: " + Current_Connection + " Next_con: " + Next_Conntection + " Piece: " + i + " angle: " + angle);

			Last_Connection = Next_Conntection;							// Sets the last connection to be the next one chosen
			Last_ConnectionPosition = lp.Get_Pos (Next_Conntection);	// Gets it's position
			Last_lp = lp;												// Saves the levelpiece for further use
			Last_Remaining_Connections = Remaining_Connections;			// Saves the levelpieces remaining connections (should change to be in level piece)
		}

	}	// End of function




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
			/*
			if (Physics.BoxCast (Vector3.zero, Dangerous_Pieces [r].HalfExtents, Dangerous_Pieces [r].Direction)) {
				r = Iterate (r, Dangerous_Pieces.Count - 1);
			} else {
				Fits = true;
			}
			*/
		}

		if (r == r_start) {
			return;
		} else {
			// Place Piece
		}

	}

	//--------------------------------------------------------------------------------------------------------------

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class
