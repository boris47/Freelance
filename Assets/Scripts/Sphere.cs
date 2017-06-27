using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Timers;


static class InputAxis { 
	public static string Horizzontal	= "Horizontal";
	public static string Vertical		= "Vertical";

	public static string Mouse_X 		= "Mouse X";
	public static string Mouse_Y 		= "Mouse Y";
};


public class Sphere : MonoBehaviour {

	[SerializeField]					private float fWalkSpeed;
	[SerializeField]					private float fRunSpeed;
	[SerializeField]					private float fJumpForce;
	[SerializeField][Range(0.0f, 1.0f)]	private float fAcceleration;
	[SerializeField][Range(0.0f, 1.0f)]	private float fDecelleration;

	float fSmooth_X, fSmooth_Z;
	float fDistToGround = 0.0f;

	public float fGravity = 9.8f;

	public Camera cam;

	Rigidbody rb;
	Collider c;
	PhysicMaterial PhyMat;
	Vector3 vInputs;


	// Called before Start
	void Awake() {
		
	}

	// Use this for initialization
	void Start () {

		// Rigid body
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;

		// Physiscs Material
		c = GetComponent<Collider> ();
		PhyMat = c.material;
		c.material = null;

		// Initial distance to ground
		fDistToGround = GetComponent<Collider> ().bounds.extents.y;

		// Variables storing smooothd speed
		fSmooth_X = fSmooth_Z = 0.0f;

		// sanity check for run speed
		fRunSpeed = Mathf.Clamp (fRunSpeed, fWalkSpeed, fRunSpeed);

	}


	bool IsGrounded() {

		return Physics.Raycast ( transform.position, -Vector3.up, fDistToGround + 0.1f );
	}


	// Called at fixet time ( def every 20ms )
	void FixedUpdate()  {

		float fStrafe	= Input.GetAxis (InputAxis.Horizzontal);
		float fMove		= Input.GetAxis (InputAxis.Vertical);

		float fMouseX	= Input.GetAxis ( InputAxis.Mouse_X );
		float fMouseY	= Input.GetAxis ( InputAxis.Mouse_Y );

		if (IsGrounded ()) {

			c.material = PhyMat;

			// evaluate inputs
			float fMoveSpeed = (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) ? fRunSpeed : fWalkSpeed;
			float fJump = (Input.GetKeyDown (KeyCode.Space)) ? fJumpForce : 0.0f;

			Vector3 vTargetVelocity = transform.TransformDirection (fStrafe, 0.0f, fMove);
			vTargetVelocity *= fMoveSpeed;

			// Smooth inputs
			fSmooth_X = Mathf.MoveTowards (rb.velocity.x, fSmooth_X + (vTargetVelocity.x * fMoveSpeed), 5.0f);
			fSmooth_Z = Mathf.MoveTowards (rb.velocity.z, fSmooth_Z + (vTargetVelocity.z * fMoveSpeed), 5.0f);

			// clamp to min  and max values
			fSmooth_X = Mathf.Clamp (fSmooth_X, -fMoveSpeed, fMoveSpeed);
			fSmooth_Z = Mathf.Clamp (fSmooth_Z, -fMoveSpeed, fMoveSpeed);

			if (fMove != 0.0f || fStrafe != 0.0f) {
				rb.velocity = new Vector3 (fSmooth_X, 0.0f, fSmooth_Z);
			}

			// Add jump to forces
			rb.velocity.Set (rb.velocity.x, fJump, rb.velocity.z);

		} else {

			// when not grounded remove physicsl material
			c.material = null;

		}

		cam.transform.position = transform.position - transform.TransformDirection (new Vector3 (0.0f, -1.0f, 3.0f));

		transform.Rotate( new Vector3( -fMouseY, fMouseX, 0.0f ) * 2.0f );

		cam.transform.rotation = transform.rotation;




	}
}
