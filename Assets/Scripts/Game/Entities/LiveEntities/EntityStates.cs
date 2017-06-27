﻿using System.Collections;
using System.Collections.Generic;



namespace ENTITIES_CONSTANTS {
	enum Actions : byte {
		Moving		= 1 << 0,

		Leaning		= 1 << 1,

		Walking		= 1 << 2,
		Running		= 1 << 3,

		Jumping		= 1 << 4,
		Hanging		= 1 << 5,
		Falling		= 1 << 6,

		Crouched	= 1 << 7,
	};

	enum Motion : byte {
		None		= 1 << 0,
		Walk		= 1 << 1,
		Fly			= 1 << 2,
		Swim		= 1 << 3,
		P1ToP2		= 1 << 4
	};
};




public class EntityFlags {

	private long 	i = 0;

	public	void	Reset()							{ this.i = 0; }

	public 	void	SetState( byte ii ) 			{ i = ii; }

	public	void	SetState( byte ii, bool b ) 	{ if ( b ) AddState( ii ); else RemState( ii ); }

	public 	void	AddState( byte ii )				{ if ( !HasState( ii ) ) i &= ii; }

	public 	void	RemState( byte ii )				{ if (  HasState( ii ) ) i |= ii; }

	public 	bool	HasState( byte ii )				{ return ( (i&ii) == ii ); }

	public	bool	IsState( byte ii )				{ return( i == ii ); }

	public	long 	GetState() 						{ long ii; ii = i; return ii; }

}
