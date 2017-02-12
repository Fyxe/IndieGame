using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose : Creating a class to store a tiered currency
*/
public class Currency {

	//===============================================[Variables]====================================================

	public List<int> Pocket = new List<int>();

	//============================================[Unity Functions]=================================================

	//===============================================[Functions]====================================================

	public void Remove(int amount, int type){
		if (type > Pocket.Count - 1) {
			// The currency holder does not have enough money
			return;
		} else {
			if (Pocket [type] < amount) {
				// Check if there is larger currency
				for (int i = type; i < Pocket.Count - 1; i++) {
					if (Pocket[i] > 0){
						Pocket [i] -= 1;
						// need to add it to the previous piece;
					} else {
						continue;
					}
				}
			} 

			Pocket [type] -= amount;
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	public void Add(int amount, int type){	
		if (Pocket.Count - 1 < type) {
			for (int i = Pocket.Count - 1; i < type; i++) {
				Pocket.Add (0);
			}
		} 
		Pocket [type] += amount;
		CheckAmounts ();
	}

	//--------------------------------------------------------------------------------------------------------------

	void CheckAmounts(){
		for (int i = 0; i < Pocket.Count; i++) {
			if (Pocket [i] > 99) {
				if (Pocket.Count - 1 < i + 1) {
					Pocket.Add (0);
				}
				int l = Mathf.RoundToInt(Mathf.Floor(Pocket[i] / 100f));
				Pocket [i + 1] += l;
				Pocket [i] = Pocket [i] - (l * 100);
			} 
		}
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------

}
