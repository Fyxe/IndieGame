using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastTester : MonoBehaviour {

	public List<GameObject> Inside = new List<GameObject>();

	public RaycastHit[] r;
	public bool rr;

	void Start(){
		rr = Physics.BoxCast (transform.position,new Vector3(0.5f,0.5f,0.5f),Vector3.up,Quaternion.identity,0.5f);
		r = Physics.BoxCastAll (transform.position,new Vector3(0.5f,0.5f,0.5f),Vector3.up,Quaternion.identity,0.5f);
		foreach (var i in r) {
			Inside.Add (i.collider.gameObject);
		}
	}
}
