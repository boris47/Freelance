
using System.Collections.Generic;
using UnityEngine;

public class cLineValue {

	string		sKey		= "";
	string		sRawValue	= "";

	byte		iType		= 0;

	cMultiValue	pMultiValue = null;
	cValue		pValue		= null;

	bool bIsOK				= false;

	// Type can be Single or Multi
	public cLineValue( string Key, byte Type ) {
		iType = Type; sKey = Key;
	}

	public cLineValue ( string Key, string sLine ) {

		sKey = Key;
		sRawValue = ( ( sLine.Length > 0 ) ? sLine : "" );

		if ( sLine.IndexOf( ',' ) > -1 ) {				// Supposing is a MultiVal string
			iType = ( byte ) LineValueType.MULTI;
			List < cValue  > vValues = Utils.String_RecognizeValues( sLine );
			if ( vValues.Count < 1 ) return;
			pMultiValue = new cMultiValue( vValues );
		
		} else { // Single value
			iType = ( byte ) LineValueType.SINGLE;
			cValue pValue = Utils.String_RecognizeValue( sLine );
			if ( pValue == null ) {
				 Debug.LogError( " cLineValue::Constructor: for key " + Key + " value type is undefined" );
				return;
			}
			this.pValue = pValue;
		}
	
		bIsOK = true;

	}

	public void Destroy() { pValue = null; pMultiValue = null; }

	public bool IsOK() { return bIsOK; }



	/////////////////////////////////////////////////////////////////////////////////


	public 	bool IsKey( string Key )					{ return ( sKey == Key ); }

	// Can be NONE, SINGLE, MULTI, KEYONLY
	public 	int Type() 									{ return iType; }
	public 	string Key()								{ return ( string ) sKey.Clone(); }
	public 	string RawValue() 							{ return ( string ) sRawValue.Clone(); }
	public 	void Clear()								{ pValue = null; pMultiValue = null; }
		
	public 	cLineValue Set( cValue _Value )				{ pValue = _Value; return this; }
	public 	cLineValue Set( cMultiValue _MultiValue )	{ pMultiValue = _MultiValue; return this;	}
		
	public 	cValue		GetValue()						{ return pValue; }
	public 	cMultiValue	GetMultiValue()					{ return pMultiValue;	}

}
