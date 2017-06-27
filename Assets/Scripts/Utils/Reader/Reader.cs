﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

// PUBLIC INTERFACE
interface IReader {

	bool IsOK();
	bool LoadFile( string sFilePath );
	bool IsLoaded( string sFilePath );
	bool HasFileElement( string SecName, out Section pSec );
	Section NewSection ( string Name, Section Inherited );
	Section GetSection( string SecName );
}


public class Reader : IReader {

	// READING PHASES
	public const int ReadingNothing = 0;
	public const int ReadingSection = 1;

	// CONTAINERS
	private Dictionary < string, Section > mSectionMap = new Dictionary < string, Section > ();
	private List < string > mFilePaths = new List < string >();

	// INTERNAL VARS
	private	int				iReadingPhase = ReadingNothing;
	private Section			pSection = null;

	private	bool	bIsOK	= false;

	// CREATE A NEW SECTION WHILE READING FILE
	private bool Section_Create( string sLine, string sFilePath, int iLine ) {

		if ( pSection != null )
			Section_Close();

		// Get the name of mother section, if present
		Section pInheritedSection = null;

		int iIndex = sLine.IndexOf( "]:", 0 );
		if ( iIndex > 0 ) {
			string sMotherName = sLine.Substring( iIndex + 2 );
			if ( ( pInheritedSection = GetSection( sMotherName ) ) == null ) {
				 Debug.LogError( "Reader::Section_Create:" + sFilePath + ":[" + iLine + "]: Section requested for inheritance \"" + sMotherName + "\" doesn't exist!" );
				return false;
			}
		}

		Section bump;
		string sSectionName = sLine.Substring( 1, sLine.IndexOf( "]", 0 ) - 1 );
		if ( HasFileElement( sSectionName, out bump ) ) {
			 Debug.LogError( "Reader::Section_Create:" + sFilePath + ":[" + iLine + "]: Section \"" + sSectionName + "\" already exists!" );
			return false;
		}

		iReadingPhase = ReadingSection;
		pSection = new Section( sSectionName, pInheritedSection );

		return true;
	}

	// INSERT A KEY VALUE PAIR INSIDE THE ACTUAL PARSING SECTION
	private bool Section_Add(  KeyValue Pair, string sFilePath, int iLine ) {

		cLineValue pLineValue = new cLineValue( Pair.Key , Pair.Value );

		if ( !pLineValue.IsOK() ) {
			 Debug.LogError( " Reader::Section_Add:LineValue invalid for key |" + Pair.Key + "| in Section |" + pSection.Name() + "| in file |" + sFilePath + "|" );
			return false;
		}

		pSection.Add( pLineValue );

		return true;

	}

	// DEFINITELY SAVE THE PARSING SECTION
	private void Section_Close() {

		if ( pSection != null )
			mSectionMap.Add( pSection.Name(), pSection );

		pSection = null;
		iReadingPhase = ReadingNothing;

	}

	public bool IsOK() { return bIsOK; }

	public static bool operator !( Reader r ) {
		return r.IsOK() == false;
	}

	// LOAD A FILE AND ALL INCLUDED FILES
	public bool LoadFile( string sFilePath ) {

		if ( IsLoaded( sFilePath ) ) return true;

		bIsOK = false;

		FileStream fs = null;
		try {
			fs = File.Open( sFilePath, FileMode.Open );
		}
		catch( Exception e ) {
			Debug.LogError( "Reader::LoadFile:Error opening file: " + sFilePath + " !!!\nMessage: " + e.Message );
			Application.Quit();
			return false;
		}

		if ( !( fs.Length > 0 ) ) {
			Debug.LogError( "Reader::LoadFile:File " + sFilePath + " is empty!!!" );
			Application.Quit();
			return false;
		}

		StreamReader sr = new StreamReader( fs );

		int iLine = 0;
		while( !sr.EndOfStream ) {
			iLine++;
			string sLine = sr.ReadLine();

			if ( !Utils.String_IsValid( ref sLine ) ) continue;

			// INCLUSION
			/// Able to include file in same dir of root file
			if ( ( sLine[ 0 ] == '#' ) && sLine.Contains( "#include" ) ) {
				string sPath = Utils.GetPathFromFilePath( sFilePath );
				string sFileName = sLine.Trim().Substring( "#include".Length + 1 );
				if ( !LoadFile( sPath + sFileName ) ) return false;
				continue;

			}

			// SECTION CREATION
			if ( ( sLine[ 0 ] == '[' ) ) {	// Create a new section
				Section_Create( sLine, sFilePath, iLine );
				continue;
			}

			// INSERTION
			KeyValue pKeyValue = Utils.GetKeyValue( sLine );
			if ( pKeyValue.IsOK ) {

				if ( pSection == null ) {
					 Debug.LogError( " Reader::LoadFile:No section created to insert KeyValue at line |" + iLine + "| in file |" + sFilePath + "| " );
					return false;
				}

				if ( iReadingPhase != ReadingSection ) {
					 Debug.LogError( " Reader::LoadFile:Trying to insert a KeyValue into a non section type FileElement, line \"" + sLine + "\" of file " + sFilePath + "!" );
					continue;
				}

				if ( !Section_Add( pKeyValue, sFilePath, iLine ) ) return false;
				continue;
			}

			// NO CORRECT LINE DETECTED
			 Debug.LogError( "Reader::LoadFile:Incorrect line " + iLine + " in file " + sFilePath );
			return false;
		}

		Section_Close();
		mFilePaths.Add( sFilePath );
		bIsOK = true;
		return true;
	}

	// CHECK IF A FILE IS ALREADY LOADED BY PATH
	public bool IsLoaded( string sFilePath ) {

		foreach( var Entry in  mFilePaths )
			if ( Entry == sFilePath ) return true;

		return false;

	}

	// CHECK AND RETURN IN CASE IF SECTION ALREADY EXISTS
	public bool HasFileElement( string SecName, out Section pSec ) {

		pSec = null;
		foreach( var Entry in mSectionMap ) {
			if ( Entry.Key == SecName ) {
				pSec = Entry.Value;
				return true;
			}
		}

		return false;
	}

	// CREATE AND SAVE A NEW EMPTY SECTION
	public Section NewSection ( string Name, Section Inherited ) {

		Section pSec = null;
		if ( HasFileElement( Name, out pSec  ) ) pSec.Destroy();

		pSec = new Section( Name, Inherited );
		mSectionMap.Add( Name, pSec );
		return pSec;
	}

	// RETRIEVE A SECTION, IF EXISTS, OTHERWISE RETURN NULL
	public Section GetSection( string SecName ) {

		foreach( var Entry in mSectionMap ) {
			if ( Entry.Key == SecName ) return Entry.Value;
		}

		return null;
	}



}
