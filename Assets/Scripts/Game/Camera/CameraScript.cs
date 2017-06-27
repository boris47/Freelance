using UnityEngine;
using System.Collections;


interface ICameraScript {

	// return true if camera has a parent and is attached
	bool IsAttached();

	// set attched state if has a parent
	void SetAttached( bool b );

	// Return the camera heigth
	float GetCamHeigth();

};



public partial class CameraScript : MonoBehaviour, ICameraScript {

	// degree where mouse vertical rotation is clamped 
	[SerializeField][Range(1, 90 )] float f_Y_Clamp = 60;


	// store the actual rotation value
	float fCurrentRotation_Y = 0.0f;


	// Store mouse sensibility
	float fMouseSensibility = 1.5f;


	// Camera heigth from ground
	float fCamHeigth = 1.8f;

	// Parent of camera
	[SerializeField]GameObject pParent = null;


	// flag for entity attach active
	bool bAttached = false;


	void Start() {

		Section s = GLOBALS.pLTXSettings.GetSection ("Inputs");
		fMouseSensibility = ( s ) ? s.GetFloat( "MouseSensibility", 1.9f ) : 1.0f;

		Section CamSect = GLOBALS.pLTXReader.GetSection( "Camera" );
		fCamHeigth = ( CamSect ) ? CamSect.GetFloat( "CamHeigth" ) : 1.8f;

		if ( pParent ) bAttached = true;

		GLOBALS.pCamera = this;

	}


	public float GetCamHeigth() {

		if ( pParent ) {

				// Get parent script
			LiveEntity pLiveEntity = (LiveEntity) pParent.GetComponent ( typeof( LiveEntity ) );

			if ( pLiveEntity && pLiveEntity.IsCrouched() ) return fCamHeigth * 0.5f;

		}

		return fCamHeigth;
	}


	void FixedUpdate() {

		////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////
		//	BODY ROTATION
		////////////////////////////////////////////////////////

		// Get Actual mouse motion delta
		float fCurrentRotation_X = Input.GetAxis ( cInput.Mouse.Mouse_X ) * fMouseSensibility;

		if ( pParent ) {
			// Rotate the body horizzontally
			pParent.transform.Rotate ( new Vector3 ( 0.0f, fCurrentRotation_X, 0.0f) );
		}

		////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////
		//	CAM ROTATION
		////////////////////////////////////////////////////////
		float fMouseMotion = Input.GetAxis ( cInput.Mouse.Mouse_Y ) * fMouseSensibility;
		fCurrentRotation_Y = Mathf.Clamp (fCurrentRotation_Y + fMouseMotion, -f_Y_Clamp, f_Y_Clamp);

		this.transform.localRotation = pParent.transform.localRotation;
		this.transform.Rotate ( Vector3.left, fCurrentRotation_Y );


		////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////
		//	CAM POSITION
		////////////////////////////////////////////////////////

		if ( pParent ) {
			this.transform.position = pParent.transform.position;
		}

	}

}



public partial class CameraScript : MonoBehaviour, ICameraScript {
	
	public bool IsAttached() { return bAttached; }

	public void SetAttached( bool b ) {

		if ( pParent ) bAttached = b;

	}

};
