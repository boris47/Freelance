
using System.Collections.Generic;

public class cMultiValue {

	private List< cValue > vValues;

	public cMultiValue() { }
	public cMultiValue( List < cValue > vValues ) {
		this.vValues = vValues;
	}

	public void Destroy() { vValues.Clear(); }


	public void		Add( cValue pValue )			{ vValues.Add( pValue ); }

	public void		Set( List< cValue > vValues )	{
		this.vValues = vValues;
	}

	public int		Size() { return vValues.Count; }
	public cValue	At( int index ) {
		if ( index < vValues.Count )
			return vValues[ index ];
		return null;
	}
	
}
