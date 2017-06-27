using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

	[SerializeField]Light pVolumetricLight = null;
	[SerializeField]Light pSun = null;
	[SerializeField]GameObject pPlayer = null;

	VolumetricLight VolLightClass = null;

	const int iLightDistance = 100;

	// Use this for initialization
	void Start () {

		pVolumetricLight.range = 450;
		pVolumetricLight.spotAngle = 2.2f;
		pVolumetricLight.intensity = 2.27f;
		pVolumetricLight.shadowStrength = 0.757f;
		pVolumetricLight.shadowBias = 0.0f;
		pVolumetricLight.shadowNormalBias = 0.0f;
		pVolumetricLight.shadowNearPlane = 0.1f;

		VolLightClass = pVolumetricLight.GetComponent<VolumetricLight>();
		VolLightClass.SampleCount 			= 7;
		VolLightClass.ScatteringCoef 		= 1.0f;
		VolLightClass.ExtinctionCoef 		= 0.0f;
		VolLightClass.SkyboxExtinctionCoef 	= 0.0f;
		VolLightClass.MieG 					= 0.187f;
		VolLightClass.MaxRayLength 			= 1000.0f;

	}



	// Update is called once per frame
	void FixedUpdate () {

		// Set the same sun roation
		pVolumetricLight.transform.rotation = pSun.transform.rotation;

		// Set the position between player and the sun at 50 units far
		pVolumetricLight.transform.position = pPlayer.transform.position - ( pSun.transform.TransformDirection (new Vector3 ( 0.0f, 0.0f, 1.0f ) ) * ( pVolumetricLight.range / 4 ) );

		if ( pVolumetricLight.transform.position.y > 0.0f )
			pVolumetricLight.range = ( iLightDistance + ( iLightDistance * ( 1.0f - ( pVolumetricLight.transform.position.y / 50.0f ) ) ) );

	}
}
