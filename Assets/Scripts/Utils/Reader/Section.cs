
using System.Collections.Generic;
using UnityEngine;


// PUBLIC INTERFACE
interface ISection {

	void					Destroy();
	bool					IsOK();
	List < cLineValue >		GetData();
	int						Lines();
	string					Name();
	void					Add( cLineValue LineValue );
	cLineValue 				GetLineValue( string Key );
	bool					HasKey( string Key );

	int						KeyType( string Key );
	string					GetRawValue ( string Key, string Default = "" );

	bool					GetBool( string Key, bool Default = false );
	int						GetInt( string Key, int Default = 0 );
	float					GetFloat( string Key, float Default = 0.0f );
	string					GetString( string Key, string Default = "" );

	cValue					GetMultiValue( string Key, int Index, int Type );

	bool					bGetBool( string Key, out bool Out, bool Default = false );
	bool					bGetInt( string Key, out int Out, int Default = 0 );
	bool					bGetFloat( string Key, out float Out, float Default = 0.0f );
	bool					bGetString( string Key, out string Out, string Default = "" );

	int						GetMultiSize( string Key );

	bool					bGetMultiValue( string Key, out cValue Out, int Index, int Type );

	bool					bGetVec2( string Key, out Vector2 Out, Vector2 Default );
	bool					bGetVec3( string Key, out Vector3 Out, Vector3 Default );
	bool					bGetVec4( string Key, out Vector4 Out, Vector4 Default );

	void					SetValue( string Key, cValue Value );
	void					SetMultiValue( string Key, List < cValue > vValues );
	void					SetInt( string Key, int Value );
	void					SetBool( string Key, bool Value );
	void					SetFloat( string Key, float Value );
	void					SetString ( string Key, string Value );

}

public class Section : ISection {


	// INTERNAL VARS
	string sName = "";
	List < cLineValue > vSection;

	bool bIsOK = false;


	public static bool operator !( Section Sec ) {
		return Sec == null;
	}

	public static bool operator false( Section Sec ) {
		return Sec == null;
	}

	public static bool operator true( Section Sec ) {
		return Sec != null;
	}



	public Section( string SecName, Section Mother = null ) {

		sName = SecName;
		if ( Mother != null ) vSection = Mother.GetData();
		else vSection = new List < cLineValue >();
		bIsOK = true;

	}

	public void Destroy() {

		foreach( cLineValue pLineValue in vSection ) pLineValue.Destroy();
		vSection.Clear();

	}

	public bool						IsOK()				{ return bIsOK; }

	public	List < cLineValue >		GetData()			{ return vSection; }
	public	int						Lines()				{ return vSection.Count; }
	public	string					Name()				{ return ( string ) sName.Clone(); }

	public	void					Add( cLineValue LineValue ) { vSection.Add( LineValue ); }


	public	cLineValue 				GetLineValue( string Key ) {

		if ( vSection.Count < 1 ) return null;

		cLineValue pLineValue = null;
		foreach ( cLineValue LineValue in vSection )  {
			if ( LineValue.IsKey( Key ) ) { pLineValue = LineValue; }
		}

		return pLineValue;

	}

	public	bool					HasKey( string Key ) {

		if ( vSection.Count < 1 ) return false;

		foreach ( cLineValue pLineValue in vSection )  {
			if ( pLineValue.IsKey( Key ) ) return true;
		}

		return false;

	}

	// Return the type of a value assigned to a key, or -1
	public	int						KeyType( string Key ) {

		if ( vSection.Count < 1 ) return -1;

		cLineValue pLineValue = null; cValue Value = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			int iType = pLineValue.Type();
			switch( iType ) {
				case ( byte ) LineValueType.SINGLE: {
					if ( ( Value = pLineValue.GetValue() ) != null ) { return Value.Type(); }
					Debug.LogError( "cSection::KeyType:WARNING! In section " + sName + " a key has no value but designed as SINGLE !!!" );
					break;
				}
				case ( byte ) LineValueType.MULTI: {
						if ( ( pLineValue.GetMultiValue() != null ) ) return iType;
						Debug.LogError( "cSection::KeyType:WARNING! In section " + sName + " a key has no value but designed as MULTI !!!" );
						break;
				}
			}
		}

		return -1;
	}

	public	string					GetRawValue ( string Key, string Default = "" ) {

		cLineValue pLineValue = null;
		return ( ( pLineValue = GetLineValue( Key ) ) != null ) ? pLineValue.RawValue() : Default;

	}


	public	bool					GetBool( string Key, bool Default = false ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				return pLineValue.GetValue().ToBool();
			}
		}
		return Default;

	}

	public	int						GetInt( string Key, int Default = 0 ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				return pLineValue.GetValue().ToInteger();
			}
		}
		return Default;

	}

	public	float					GetFloat( string Key, float Default = 0.0f ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				return pLineValue.GetValue().ToFloat();
			}
		}
		return Default;

	}

	public	string					GetString( string Key, string Default = "" ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				return pLineValue.GetValue().ToString();
			}
		}
		return Default;

	}

	public	cValue					GetMultiValue( string Key, int Index, int Type ) {

		cLineValue pLineValue = null; cMultiValue pMultiValue = null; cValue pValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( ( pMultiValue = pLineValue.GetMultiValue() ) != null ) {
				if ( ( pValue = pMultiValue.At( Index - 1 ) ) != null ) {
					return pValue;
				}
			}
		}

		return null;
	}


	public	bool					bGetBool( string Key, out bool Out, bool Default = false ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				Out = pLineValue.GetValue().ToBool();
				return true;
			}
		}

		Out = Default;
		return false;
	}

	public	bool					bGetInt( string Key, out int Out, int Default = 0 ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				Out = pLineValue.GetValue().ToInteger();
				return true;
			}
		}

		Out = Default;
		return false;
	}

	public	bool					bGetFloat( string Key, out float Out, float Default = 0.0f ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				Out = pLineValue.GetValue().ToFloat();
				return true;
			}
		}

		Out = Default;
		return false;
	}

	public	bool					bGetString( string Key, out string Out, string Default = "" ) {

		cLineValue pLineValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( pLineValue.Type() == ( byte ) LineValueType.SINGLE ) {
				Out = pLineValue.GetValue().ToString();
				return true;
			}
		}

		Out = Default;
		return false;
	}

	public	int						GetMultiSize( string Key ) {

		cLineValue pLineValue = null; cMultiValue pMultiValue = null;
		return (
			( ( pLineValue = GetLineValue( Key ) )			!= null ) && 
			( ( pMultiValue = pLineValue.GetMultiValue() )	!= null ) ) ? 
			( pMultiValue.Size() + 1 ) : 0;

	}

	public	bool					bGetMultiValue( string Key, out cValue Out, int Index, int Type ) {

		cLineValue pLineValue = null; cMultiValue pMultiValue = null; cValue pValue = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( ( pMultiValue = pLineValue.GetMultiValue() ) != null ) {
				if ( ( pValue = pMultiValue.At( Index - 1 ) ) != null ) {
					Out = pValue;
					return true;
				}
			}
		}

		Out = null;
		return false;
	}

	public	bool					bGetVec2( string Key, out Vector2 Out, Vector2 Default ) {

		cLineValue pLineValue = null; cMultiValue pMultiValue = null;
		cValue pValue1 = null; cValue pValue2 = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( ( pMultiValue = pLineValue.GetMultiValue() ) != null ) {
				if ( ( ( pValue1 = pMultiValue.At( 0 ) ) != null ) && 
					   ( pValue2 = pMultiValue.At( 1 ) ) != null ) {
					Out = new Vector2( pValue1.ToFloat(), pValue2.ToFloat() );
					return true;
				}
			}
		}

		Out = Default;
		return false;
	}


	public	bool					bGetVec3( string Key, out Vector3 Out, Vector3 Default ) {

		cLineValue pLineValue = null; cMultiValue pMultiValue = null;
		cValue pValue1 = null; cValue pValue2 = null; cValue pValue3 = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( ( pMultiValue = pLineValue.GetMultiValue() ) != null ) {
				if ( (	( pValue1 = pMultiValue.At( 0 ) ) != null ) && 
					 (	( pValue2 = pMultiValue.At( 1 ) ) != null ) &&
					 (  ( pValue3 = pMultiValue.At( 2 ) ) != null )  ){
					Out = new Vector3( pValue1.ToFloat(), pValue2.ToFloat(), pValue3.ToFloat() );
					return true;
				}
			}
		}

		Out = Default;
		return false;
	}

	public	bool					bGetVec4( string Key, out Vector4 Out, Vector4 Default ) {

		cLineValue pLineValue = null; cMultiValue pMultiValue = null;
		cValue pValue1 = null; cValue pValue2 = null; cValue pValue3 = null; cValue pValue4 = null;
		if ( ( pLineValue = GetLineValue( Key ) ) != null ) {
			if ( ( pMultiValue = pLineValue.GetMultiValue() ) != null ) {
				if ( (	( pValue1 = pMultiValue.At( 0 ) ) != null ) && 
					 (	( pValue2 = pMultiValue.At( 1 ) ) != null ) &&
					 (	( pValue3 = pMultiValue.At( 2 ) ) != null ) &&
					 (  ( pValue4 = pMultiValue.At( 3 ) ) != null )  ){
					Out = new Vector4( pValue1.ToFloat(), pValue2.ToFloat(), pValue3.ToFloat(), pValue4.ToFloat() );
					return true;
				}
			}
		}

		Out = Default;
		return false;
	}



	public	void					SetValue( string Key, cValue Value ) {

		cLineValue pLineValue = GetLineValue( Key );
		// if not exists create one
		if ( pLineValue == null ) pLineValue = new cLineValue( Key, ( byte ) LineValueType.SINGLE );
		pLineValue.Clear();
		pLineValue.Set( Value );

	}

	public	void					SetMultiValue( string Key, List < cValue > vValues ) {

		cLineValue pLineValue = GetLineValue( Key );
		// if not exists create one
		if ( pLineValue == null ) pLineValue = new cLineValue( Key, ( byte ) LineValueType.MULTI );
		pLineValue.Clear();
		pLineValue.Set( new cMultiValue( vValues ) );

	}


	public	void					SetInt( string Key, int Value ) {

		SetValue( Key, new cValue( Value ) );

	}

	public	void					SetBool( string Key, bool Value ) {

		SetValue( Key, new cValue( Value ) );

	}

	public	void					SetFloat( string Key, float Value ) {

		SetValue( Key, new cValue( Value ) );

	}
	
	public	void					SetString ( string Key, string Value ) {

		SetValue( Key, new cValue( Value ) );

	}



	public	void			SetVec2( string Key, Vector2 Vec ) {

		cLineValue pLineValue = GetLineValue( Key );
		// if not exists create one
		if ( pLineValue == null ) pLineValue = new cLineValue( Key, ( byte ) LineValueType.MULTI );
		pLineValue.Clear();

		List < cValue > vValues = new List<cValue> { new cValue( Vec.x ), new cValue( Vec.y ) };
		pLineValue.Set( new cMultiValue( vValues ) );

	}

	public	void			SetVec3( string Key, Vector3 Vec ) {

		cLineValue pLineValue = GetLineValue( Key );
		// if not exists create one
		if ( pLineValue == null ) pLineValue = new cLineValue( Key, ( byte ) LineValueType.MULTI );
		pLineValue.Clear();

		List < cValue > vValues = new List<cValue> { new cValue( Vec.x ), new cValue( Vec.y ), new cValue( Vec.z ) };
		pLineValue.Set( new cMultiValue( vValues ) );

	}

	public	void			SetVec4( string Key, Vector4 Vec ) {

		cLineValue pLineValue = GetLineValue( Key );
		// if not exists create one
		if ( pLineValue == null ) pLineValue = new cLineValue( Key, ( byte ) LineValueType.MULTI );
		pLineValue.Clear();

		List < cValue > vValues = new List<cValue> { new cValue( Vec.x ), new cValue( Vec.y ), new cValue( Vec.z ), new cValue( Vec.w ) };
		pLineValue.Set( new cMultiValue( vValues ) );

	}



};