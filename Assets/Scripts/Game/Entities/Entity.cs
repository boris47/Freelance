using UnityEngine;
using System .Collections;


	enum ENTITY_TYPE{
		NONE,
		ACTOR,
		HUMAN,
		ANIMAL,
		OBJECT
	};


// "Fore ward declaration"
partial class Entity {};

interface IEntity {

	// Common
	string			Name();
	string			Section();

	// Physics
	void			SetMass( float f );
	float			GetMass ();
	void			SetParent ( Entity e );
	Entity			GetParent ();

	// Type Conversion
	bool			IsLiveEntity();
	LiveEntity		GetAsLiveEntity ();

	bool			IsHuman ();
	Human			GetAsHuman();

	// Internal type
	void			SetType( byte Type  );
	byte			GetType();

	// Conditions
	void			SetInWater (bool b);
	bool			IsInWater ();

	void			SetUnderWater (bool b);
	bool			IsUnderWater ();

	// Climbing Motion
	void			SetRotationP1ToP2 (Vector3 v);
	Vector3			GetRotationP1ToP2 ();

}

public partial class Entity : MonoBehaviour, IEntity {

						// 0 to 4,294,967,295
						protected 	uint			iID								= 0;

						protected	Section			pSection						= null;
						protected	Rigidbody		pRigidBody						= null;

						protected 	string			sName							= "Unknown";
	[SerializeField]	protected 	string			sSection						= "None";

						protected	Entity			pParent							= null;

						protected 	byte			iEntityType						= ( byte ) ENTITY_TYPE.NONE;

						protected 	bool			bIsInWater						= false;
						protected 	bool			bIsUnderWater					= false;

						protected 	Vector3			vRotationP1ToP2					= new Vector3();

						protected	bool 			bIsOK							= false;

						public		string			Name() 							{ return ( string ) sName.Clone(); }
						public		string			Section()						{ return ( string ) sSection.Clone(); }

						public		void			SetMass( float v )				{ GetComponent<Rigidbody>().mass = v; }
						public		float			GetMass()						{ return GetComponent<Rigidbody>().mass; }

						public		void			SetParent( Entity e )			{ pParent = e; }
						public		Entity			GetParent()						{ return pParent; }

						public		void			SetType( byte Type  )			{ iEntityType = Type; }
						public new	byte			GetType()						{ return iEntityType; }

						public		bool			IsLiveEntity() {
							return this is LiveEntity;
						}

						public		LiveEntity		GetAsLiveEntity() {
							if ( !IsLiveEntity() ) return null;
							return this as LiveEntity;
						}

						public		bool			IsHuman() {
							return this is LiveEntity;
						}

						public		Human			GetAsHuman() {
							if ( !IsHuman() ) return null;
							return this as Human;
						}

						public		void			SetInWater( bool b )			{ bIsInWater = b; }
						public		bool			IsInWater()						{ return bIsInWater; }

						public		void			SetUnderWater( bool b )			{ bIsUnderWater = b; }
						public		bool			IsUnderWater()					{ return bIsUnderWater; }
		
						public		void			SetRotationP1ToP2( Vector3 v )	{ vRotationP1ToP2 = v; }
						public		Vector3			GetRotationP1ToP2()				{ return vRotationP1ToP2; }

}

