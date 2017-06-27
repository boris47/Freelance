using UnityEngine;
using System .Collections;

static class Defaults {
	public const int	ZERO_INT	= 0;
	public const float	ZERO_FLOAT	= 0.0f;
};


static class cInput {

	public static class Axis {

		public const string Horizzontal		= "Horizontal";
		public const string Vertical		= "Vertical";
	}

	public static class Mouse {
		public const string Mouse_X			= "Mouse X";
		public const string Mouse_Y			= "Mouse Y";
	}

	public static class Keyboard {

	}

};


static class GLOBALS {
	public static Reader 		pLTXReader 		= null;
	public static Reader 		pLTXSettings 	= null;

	public static CameraScript 	pCamera			= null;
}



public class Game : MonoBehaviour {

	uint iID = 0;

	// Use this for initialization
	void Awake() {

		Reader r = new Reader ();

		r.LoadFile ("Settings.ltx");

		if (!r.IsOK ())
			Debug.LogError ("Cannot load Settings file");
		else {
			GLOBALS.pLTXSettings = r;
		}

		Reader r1 = new Reader ();

		r1.LoadFile ("Gamedata\\All.ltx");

		if (!r1.IsOK ())
			Debug.LogError ("Cannot load All.ltx file");
		else {
			GLOBALS.pLTXReader = r1;
		}

//		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() as GameObject[];
//		foreach(object go in allObjects) {

//	}


	}

	uint NewID()  { iID++; return iID; }

	// Update is called once per frame
	void Update() {

	}


	int BadlyIndentedCalculateAverage( int total, int numSamples ) {
		if ( numSamples > 0 ) {
			return total / numSamples;
		}
		return -1;
	}

}
