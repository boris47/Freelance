using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ RequireComponent ( typeof ( Rigidbody ) ) ]
[ RequireComponent ( typeof ( CapsuleCollider ) ) ]

public class Actor: Human {

	[Header("Actor cfgs")] // thx Fabio
	public 	float 	fGravity			= 10.0f;
	public 	float 	fMaxVelocityChange	= 0.5f;
	public 	bool 	bCanJump			= true;

	CapsuleCollider	pCapsColider		= null;


	void Start () {

		// Hide the cylinder mesh
		GetComponent<MeshRenderer> ().enabled = false;

		pCapsColider = GetComponent<CapsuleCollider>();

		// Retrieve rigid body
		pRigidBody = GetComponent< Rigidbody >();
	    pRigidBody.freezeRotation = true;
	    pRigidBody.useGravity = false;

		Section pActorSec = GLOBALS.pLTXReader.GetSection( "Actor" );

		////////////////////////////////////////////////////////////////////////////////////////
		// Entity Setup
		{
			if ( !pActorSec ) return;
			// Walking
			fWalkSpeed				= pActorSec.GetMultiValue( "Walk",	1, ( byte ) ValueTypes.FLOAT );
			fWalkJumpCoef			= pActorSec.GetMultiValue( "Walk",	2, ( byte ) ValueTypes.FLOAT );
			fWalkStamina			= pActorSec.GetMultiValue( "Walk",	3, ( byte ) ValueTypes.FLOAT );

			// Running
			fRunSpeed				= pActorSec.GetMultiValue( "Run",	1, ( byte ) ValueTypes.FLOAT );
			fRunJumpCoef			= pActorSec.GetMultiValue( "Run",	2, ( byte ) ValueTypes.FLOAT );
			fRunStamina				= pActorSec.GetMultiValue( "Run",	3, ( byte ) ValueTypes.FLOAT );

			// Crouched
			fCrouchSpeed			= pActorSec.GetMultiValue( "Crouch",1, ( byte ) ValueTypes.FLOAT );
			fCrouchJumpCoef			= pActorSec.GetMultiValue( "Crouch",2, ( byte ) ValueTypes.FLOAT );
			fCrouchStamina			= pActorSec.GetMultiValue( "Crouch",3, ( byte ) ValueTypes.FLOAT );

			// Climbing
			fClimbSpeed				= pActorSec.GetFloat( "Climb", 0.12f );

			// Jumping
			fJumpForce				= pActorSec.GetMultiValue( "Jump", 1, ( byte ) ValueTypes.FLOAT );
			fJumpStamina			= pActorSec.GetMultiValue( "Jump", 2, ( byte ) ValueTypes.FLOAT );

			// Stamina
			fStaminaRestore			= pActorSec.GetFloat( "StaminaRestore", 0.0f );
			fStaminaRunMin			= pActorSec.GetFloat( "StaminaRunMin", 0.3f );
			fStaminaJumpMin			= pActorSec.GetFloat( "StaminaJumpMin", 0.4f );

			// Set Entity parameters
//			SetHealth( pActorSec.GetFloat( "Health", 100.f ) );
//			SetStamina( 1.0f );
//			SetMaxItemMass( pSection->GetFloat( "MaxItemsMass", 0.0f ) );

		}

	}

	void FixedUpdate () {

		////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////
		//	BODY MOVEMENT
		////////////////////////////////////////////////////////

		// Clear all the state flags
		ResetStates();

		CheckGrounded();

		// Get keyboard inputs
		float 	fMove 			= Input.GetAxis ( cInput.Axis.Vertical );
		float 	fStrafe			= Input.GetAxis ( cInput.Axis.Horizzontal );

		bool	bSprintInput	= Input.GetButton ( "Sprint" );
		bool	bJumpInput		= Input.GetButtonDown ( "Jump" );
		bool	bCrouchInput	= Input.GetKey( KeyCode.LeftControl ) || Input.GetKey( KeyCode.RightControl );

	    if ( bGrounded ) {

			// Calculate how fast we should be moving
			Vector3 vTargetVelocity = new Vector3( fStrafe, 0.0f, fMove );
			vTargetVelocity = transform.TransformDirection( vTargetVelocity );
			Vector3 vActualVelocity = pRigidBody.velocity;

			// If is moving
			if ( vTargetVelocity.magnitude > 0.0f ) {
				SetMoving();					// Set flag moving to true

				// Speed hack: fck u
				if ( ( fMove != 0.0f ) && ( fStrafe != 0.0f  ) ) vTargetVelocity *= 0.707f;

			} else SetIdle();					// else set flag to idle

			// Set crouched state
			SetCrouched( bCrouchInput );

			// If sprinting
			if ( bSprintInput ) {

				// If is crouch, stand up and set as running
				if ( IsCrouched() ) SetCrouched( false );
				SetRunning( true );

			}

			// Jump
			if ( bCanJump && bJumpInput ) {

				float fRunBoost = IsRunning() ? 1.5f : 1.0f;
				pRigidBody.velocity = new Vector3( vActualVelocity.x, CalculateJumpVerticalSpeed() * fRunBoost, vActualVelocity.z );
				SetJumping( true );
				SetCrouched( false );
				bGrounded = false;
				return;
			}

			pCapsColider.height = GLOBALS.pCamera.GetCamHeigth() * ( bCrouchInput ? 0.5f : 1.0f ); 
			pCapsColider.center.Set( 0.0f, ( bCrouchInput ? -0.5f : 0.0f ), 0.0f );

			if ( bSprintInput )
				vTargetVelocity *= fRunSpeed;
			else if ( bCrouchInput )
				vTargetVelocity *= fCrouchSpeed;
			else 
				vTargetVelocity *= fWalkSpeed;

			// Apply a force that attempts to reach our target velocity
			Vector3 velocityChange = ( vTargetVelocity - vActualVelocity );
			velocityChange.x = Mathf.Clamp( velocityChange.x, -fMaxVelocityChange, fMaxVelocityChange );
			velocityChange.z = Mathf.Clamp( velocityChange.z, -fMaxVelocityChange, fMaxVelocityChange );
			velocityChange.y = 0.0f;
			pRigidBody.AddForce( velocityChange, ForceMode.VelocityChange );
			
	        
	    }
		
	    // We apply gravity manually for more tuning control
		pRigidBody.AddForce( new Vector3 ( 0.0f, -fGravity * pRigidBody.mass, 0.0f ) );
		
		bGrounded = false;

	}

	/* void Update() {
	 *    transform.position = new vector3( -time.deltatime * Speed * input.getaxis( "Horizzontal" ), 0.0f, 0.0f );
	 * }
	 */

	/*
	void LateUpdate() {

	}


	// collider or trigger

	void OnTriggerEnter( Collider Object ) {

	}

	void OnTriggerEnter2D( Collider2D Object ) {

	}

	void OnTriggerStay( Collider Object ) {

	}

	void OnTriggerStay2D( Collider2D Object ) {

	}

	void OnTriggerExit( Collider Object ) {

	}

	void OnTriggerExit2D( Collider2D Object ) {

	}

*/

	void OnCollisionEnter() {

		pRigidBody.velocity = new Vector3( 0.0f, 0.0f, 0.0f );

	}


	void CheckGrounded() {

		bGrounded = Physics.Raycast( transform.position, Vector3.down, 2.6f );   

	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
		return Mathf.Sqrt( 2 * fJumpForce * fGravity );
	}

}
