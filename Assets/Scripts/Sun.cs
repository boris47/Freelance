using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sun : MonoBehaviour {
	
	public Vector3 vRotation;

	void FixedUpdate()  {

		transform.Rotate( vRotation );

	}
}
