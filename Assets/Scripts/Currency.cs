using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose : Creating a class to store a tiered currency
*/
public class Currency {

	//===============================================[Variables]====================================================

	public int Pocket;

	//============================================[Unity Functions]=================================================

	//===============================================[Functions]====================================================

	public void Add(int amount){
		Pocket += amount;
	}

	public void Subtract(int amount){
		Pocket -= amount;
	}

	public List<int> GetCoinage(){
		List<int> temp = new List<int> ();
		for (int i = 1; i < 5; i++) {
			temp.Add (Pocket / (10^(i*2)) % 100);
		}
		foreach (var item in temp) {
			Debug.Log (item);
		}
		return temp;
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------

}
