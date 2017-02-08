using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose : Basic extention of 'Mathf' by adding things that are needed.
*/
public class MathT : MonoBehaviour {

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 norm){	// Better form of Vector3.Angle()
		return Mathf.Atan2 (Vector3.Dot(norm,Vector3.Cross(v1,v2)),Vector3.Dot(v1,v2)) * Mathf.Rad2Deg;
	}
}
