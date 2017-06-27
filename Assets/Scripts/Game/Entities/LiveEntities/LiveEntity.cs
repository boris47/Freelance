using UnityEngine;
using System.Collections;


public class LiveEntity : Entity {
	
	EntityFlags			States							= new EntityFlags();
	EntityFlags			MotionFlag						= new EntityFlags();

	// Movements
	protected	float	fHealth							= Defaults.ZERO_FLOAT;

	protected	float	fWalkSpeed						= Defaults.ZERO_FLOAT;
	protected	float	fRunSpeed						= Defaults.ZERO_FLOAT;
	protected	float	fCrouchSpeed					= Defaults.ZERO_FLOAT;
	protected	float	fClimbSpeed						= Defaults.ZERO_FLOAT;
		
	protected	float	fWalkJumpCoef					= Defaults.ZERO_FLOAT;
	protected	float	fRunJumpCoef					= Defaults.ZERO_FLOAT;
	protected	float	fCrouchJumpCoef					= Defaults.ZERO_FLOAT;
		
	protected	float	fWalkStamina					= Defaults.ZERO_FLOAT;
	protected	float	fRunStamina						= Defaults.ZERO_FLOAT;
	protected	float	fCrouchStamina					= Defaults.ZERO_FLOAT;

	protected	float	fJumpForce						= Defaults.ZERO_FLOAT;
	protected	float	fJumpStamina					= Defaults.ZERO_FLOAT;
		
	protected	float	fStaminaRestore					= Defaults.ZERO_FLOAT;
	protected	float	fStaminaRunMin					= Defaults.ZERO_FLOAT;
	protected	float	fStaminaJumpMin					= Defaults.ZERO_FLOAT;

	protected	float	fMaxItemsMass					= Defaults.ZERO_FLOAT;


	// This variable control which physic to use on entity
	protected	byte	iMotionType						= ( byte ) ENTITIES_CONSTANTS.Motion.None;
	protected	byte	iPrevMotionType					= ( byte ) ENTITIES_CONSTANTS.Motion.None;


	// Var used for smooth movements of entity
	protected	float	fMoveSmooth						= Defaults.ZERO_FLOAT;
	protected	float	fStrafeSmooth					= Defaults.ZERO_FLOAT;
	protected	float	fVerticalSmooth					= Defaults.ZERO_FLOAT;

	protected	float	fViewRange						= Defaults.ZERO_FLOAT;

	// Stamina always reach 1.0f
	protected	float	fStamina						= Defaults.ZERO_FLOAT;

	protected	bool	bIsUnderSomething				= false;
	protected	bool	bTiredness						= false;
	protected	bool	bGrounded						= true;
	protected	bool	bHeavyFall						= false;

	protected	Entity  pFlashLight						= null;

///	protected	FootstepManager 	pFootstepManager	= null;



	public		long	GetState() 						{ return this.States.GetState(); }

	public		void	ResetStates()					{ States.Reset(); }

	public		bool	IsMoving() 						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Moving ); }
	public		bool	IsIdle()						{ return !States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Moving ); }
	public		bool	IsLeaning()						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Leaning ); }
	public		bool	IsWalking()						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Walking ); }
	public		bool	IsRunning()						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Running ); }
	public		bool	IsJumping()						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Jumping ); }
	public		bool	IsHanging()						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Hanging ); }
	public		bool	IsFalling()						{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Falling ); }
	public		bool	IsCrouched()					{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Actions.Crouched ); }

	public		void	SetMoving()						{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Moving, true ); }
	public		void	SetIdle()						{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Moving, false ); }
	public		void	SetLeaning(  bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Leaning, b ); }
	public		void	SetWalking(  bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Walking, b ); }
	public		void	SetRunning(  bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Running, b ); }
	public		void	SetJumping(  bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Jumping, b ); }
	public		void	SetHanging(  bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Hanging, b ); }
	public		void	SetFalling(  bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Falling, b ); }
	public		void	SetCrouched( bool b )			{ States.SetState( ( byte )ENTITIES_CONSTANTS.Actions.Crouched, b ); }


	public		bool	Motion_IsWalking()				{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Motion.Walk ); }
	public		bool	Motion_IsSwimming()				{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Motion.Swim ); }
	public		bool	Motion_IsFlying()				{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Motion.Fly ); }
	public		bool	Motion_IsP1ToP2()				{ return States.HasState( ( byte )ENTITIES_CONSTANTS.Motion.P1ToP2 ); }

}
