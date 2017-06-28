using System;
using System.Collections .Generic;
using System.Linq;
using System.Text;
using UnityEngine;

enum ValueTypes : byte { NONE, BOOLEAN, INTEGER, FLOAT, STRING };

enum LineValueType : byte { SINGLE, MULTI };

struct KeyValue {
	public	string	Key;
	public	string	Value;
	public	bool	IsOK;
};

static class Utils {

	static public void MSGDBG( string format, params object[] args ) {

		String sOutput = String.Format( format, args );
		Console.WriteLine( "DBG: " + (string) sOutput );

	}

	static public void MSGCRT( string format, params object[] args ) {

		String sOutput = String.Format( format, args );
		Console.WriteLine( "CRT:     " + (string) sOutput );

	}

	static public bool FileExists( string FilePath ) {
		return System.IO.File.Exists( FilePath );
	}

	static public bool DirExists( string DirPath ) {
		return System.IO.Directory.Exists( DirPath );
	}

	static public string GetPathFromFilePath( string FilePath, bool bSlash = true ) {
		return FilePath.Substring( 0, FilePath.LastIndexOf( "\\" ) + ( ( bSlash ) ? 1 : 0 ) );
	}

	static public string StringToDotStr( string FilePath ) {
		return FilePath.Replace( '\\', '.' ).Replace( '/', '.' );
	}

	static public void String_CleanComments( ref string str ) {

		if ( str.Length < 1 ) return;
		for ( int i = 0; i < str.Length; i++ ) {
			if ( str[ i ] == ';' ) {
				str = str.Remove( i );
				return;
			}
		}

	}

	static public bool String_ContainsAlpha( string str ) {

		for ( int i = 0; i < str.Length; i++ ) {
			if ( char.IsLetter( str[ i ] ) ) {
				return true;
			}
		}
		return false;

	}

	static public bool String_ContainsDigit( string str ) {

		for ( int i = 0; i < str.Length; i++ ) {
			if ( char.IsDigit( str[ i ] ) ) {
				return true;
			}
		}
		return false;

	}

	static public bool String_IsValid( ref string str ) {

		String_CleanComments( ref str );
		if ( ( str.Length < 1 )  || ( !String_ContainsAlpha( str ) && !String_ContainsDigit( str ) ) ) return false;

		return true;
	}

	// Only contains letters and ':'
	static private bool String_IsValidChar( char Char ) {

		if ( ( Char > 64 && Char < 91  ) || // A - Z
			 ( Char > 96 && Char < 123 ) || // a - z
			 ( Char == 58 ) 				// : ( Double dot )
			 )
			 return true;
		return false;
	}

	// Return the type of value reading the string
	static private byte String_ReturnValueType( string sLine ) {
		bool b_IsString = false, b_IsNumber = false, b_DotFound = false;
		for ( int i = 0; i < sLine.Length ; i++ ) {

			char Char = sLine[ i ];
			if ( Char == 32 ) continue;								// skip parsing spaces
			if ( Char == 46 ) b_DotFound = true;					// (Dot)Useful for number determination
			if ( Char > 47 && Char < 58 && !b_IsString ) {			// is number and not a str
				b_IsNumber = true;									// ok, now is a number
			}
			if ( String_IsValidChar( Char ) ) {						// is char [ A-Z ] or [ a-z ] or :
				b_IsString = true; b_IsNumber = false;				// if was a number now is a string, never more a number
				break;
			}
		}

		if ( b_IsNumber ) {										// try understand if is a int or float type
			if ( b_DotFound ) return ( byte )ValueTypes.FLOAT;	// because of dot is probably a float value
			else return ( byte )ValueTypes.INTEGER;             // No dot found so is probably an integer
		}

		if ( b_IsString ) {										// try understand if is a string or boolean type
			if ( ( sLine.ToLower() == "true" ) || ( sLine.ToLower() == "false" ) ) {
				return ( byte )ValueTypes.BOOLEAN;
			} else return ( byte )ValueTypes.STRING;
		}

		return ( byte )ValueTypes.NONE;
	}

	// Return a cValue object if value is identified, otherwise null
	static public cValue String_RecognizeValue( string sLine ) {	
		
		switch( String_ReturnValueType( sLine ) ) {
			case ( byte ) ValueTypes.NONE:	break;
			case ( byte ) ValueTypes.INTEGER:	{
					int i = -1;
					Int32.TryParse( sLine, out i );
					return new cValue( i );
			}
			case ( byte ) ValueTypes.BOOLEAN:	{ 
				return ( sLine.ToLower() == "true" ) ? ( new cValue( true ) ) : ( new cValue( false ) );
			}
			case ( byte ) ValueTypes.FLOAT:{
					float f = 0.0f;
					float.TryParse( sLine, out f );
					return new cValue( f );
			}
			
			case ( byte ) ValueTypes.STRING:	{
					return ( new cValue( sLine ) );
			}
		}
		return null;
	}


	// parse a string and return a list of values
	static public List < cValue > String_RecognizeValues( string _Line ) {
		List < cValue > Values = new List < cValue > ();
		string Line = _Line;
		int Start = 0;
		for ( int i = 0; i < Line.Length; i++ ) {
			if ( Line[ i ] == ',' ) {
				string Result = ( Line.Substring( Start, i - Start ) ).Trim();
				Values.Add( String_RecognizeValue( Result ) );
				Start = i + 1;
			}
		}
		cValue Value = String_RecognizeValue( Line.Substring( Start ) );	// last value is not followed by a colon
		Values.Add( Value );					// So we save the last part of string entirely
		return Values;
	}


	static public KeyValue GetKeyValue( string Line ) {

		KeyValue Result;

		Result.IsOK = false;
		Result.Key = Result.Value = "";

		if ( !String_IsValid( ref Line ) ) return Result;

		int iEqualSign = 0;

		for ( int i = 0; i < Line.Length; i++ )
			if ( Line[ i ]  == '=' ) { iEqualSign = i; break; }

		if ( iEqualSign > 0 ) { // Key Value Pair
			string sKey = Line.Substring( 0, iEqualSign ).Trim();
			string sValue = Line.Substring( iEqualSign + 1 );
			if ( sValue.Length > 0 ) sValue = sValue.Trim();
			if ( sKey.Length > 0 ) {
				Result.Key = sKey;
				Result.Value = sValue;
				Result.IsOK = true;
				return Result;
			}
		}


		return Result;
	}


	public static T Math_Clamp<T>( this T Val, T Min, T Max ) where T : IComparable<T> {

		if ( Val.CompareTo( Min ) < 0 ) return Min;
		if ( Val.CompareTo( Max ) > 0 ) return Max;
		return Val;

	}

	public static float Math_ClampAngle( float Angle, float Min, float Max ) {

		if ( Angle > 360 ) Angle =-360;

		Angle = Mathf.Max (Mathf.Min (Angle, Max), Min);

		if (Angle < 0)  	Angle += 360;

		return Angle;

	}

	public static Vector3 Math_Vec3Lerp( Vector3 V1, Vector3 V2, float fInterpolant ) {

		return new Vector3(
			Mathf.Lerp( V1.x, V2.x, fInterpolant ),
			Mathf.Lerp( V1.y, V2.y, fInterpolant ),
			Mathf.Lerp( V1.z, V2.z, fInterpolant )
			);

	}



}
