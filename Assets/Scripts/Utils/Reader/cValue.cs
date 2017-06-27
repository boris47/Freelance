
public class cValue {

	private object	Value;
	private byte	iType = ( byte ) ValueTypes.NONE;

	public	cValue()		{}

	public	cValue( bool Val )		{ Value = Val; iType = ( byte ) ValueTypes.BOOLEAN; }
	public	cValue( int Val )		{ Value = Val; iType = ( byte ) ValueTypes.INTEGER; }
	public	cValue( float Val )		{ Value = Val; iType = ( byte ) ValueTypes.FLOAT; }
	public	cValue( string Val )	{ Value = Val; iType = ( byte ) ValueTypes.STRING; }

	public byte	Type() { return iType; }

	public bool ToBool() {
		if ( iType == ( byte ) ValueTypes.BOOLEAN ) return ( bool ) Value;
		return false;
	}

	public int ToInteger() {
		if ( iType == ( byte ) ValueTypes.INTEGER ) return ( int ) Value;
		return -1;
	}

	public float ToFloat() {
		if ( iType == ( byte ) ValueTypes.FLOAT ) return ( float ) Value;
		return 0.0f;
	}

	public override string ToString() {
		if ( iType == ( byte ) ValueTypes.STRING ) return ( string ) Value;
		return "";
	}

	public static implicit operator bool( cValue v ) {
		return v.ToBool();
	}

	public static implicit operator int( cValue v ) {
		return v.ToInteger();
	}

	public static implicit operator float( cValue v ) {
		return v.ToFloat();
	}

	public static implicit operator string( cValue v ) {
		return v.ToString();
	}

}
