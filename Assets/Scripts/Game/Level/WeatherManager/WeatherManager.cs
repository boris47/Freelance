using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

	[SerializeField]Light pVolumetricLight = null;
	[SerializeField]Light pSun = null;
	[SerializeField]GameObject pPlayer = null;

	VolumetricLight VolLightClass = null;
	private float fLightRange;


	const float LIGHT_DIST_MULT = 0.6f;
	const float MAX_LIGHT_POWER = 8.0f;
	const float MAX_LIGHT_EXTINCION = 0.03f;
	const float MAX_LIGHT_MIEG = 0.2f;

	Color32 DefSunColor;
	Color32 SunColor;

	// Use this for initialization
	void Start () {

		fLightRange = pVolumetricLight.range;
		pVolumetricLight.spotAngle = 120.0f;
		pVolumetricLight.intensity = 0.1f;
		pVolumetricLight.shadowStrength = 1.0f;
		pVolumetricLight.shadowBias = 0.0f;
		pVolumetricLight.shadowNormalBias = 0.0f;
		pVolumetricLight.shadowNearPlane = 0.1f;

		VolLightClass = pVolumetricLight.GetComponent<VolumetricLight>();
		VolLightClass.SampleCount 			= 16;
		VolLightClass.SkyboxExtinctionCoef 	= 0.0f;

		DefSunColor = (Color32) pVolumetricLight.color;
		SunColor = new Color32( 255, 141, 76, 255 );

	}

	// Update is called once per frame
	void FixedUpdate () {

		// Set the same sun roation
		pVolumetricLight.transform.rotation = pSun.transform.rotation;

		// Set the position between player and the sun at distance
		pVolumetricLight.transform.position = pPlayer.transform.position - ( pSun.transform.TransformDirection ( Vector3.forward ) * ( fLightRange * LIGHT_DIST_MULT ) );

		// Light power
		// Go from 0.0 to 1.0;
		float fSunPower  = ( pVolumetricLight.transform.position.y / fLightRange );  // 0.0f - 1.0f

		// Dynamic Range
//		if ( pVolumetricLight.transform.position.y > 0.0f )
//			pVolumetricLight.range = ( fLightRange + ( ( fLightRange * ( 1.0f - fSunPower ) ) ) * 0.5f );

		if ( pVolumetricLight.transform.position.y > 0.0f )
		pVolumetricLight.range = ( fLightRange + ( fLightRange * ( fSunPower * 0.5f ) ) );

		// Power of the light
		VolLightClass.ScatteringCoef = MAX_LIGHT_POWER - ( MAX_LIGHT_POWER * fSunPower /* * ( ( fSunPower ) * 0.7f )*/ );

		// Extincion of the light, "controls shafts length"
		VolLightClass.ExtinctionCoef = MAX_LIGHT_EXTINCION - ( MAX_LIGHT_EXTINCION * ( 1.0f - fSunPower ) );
		
		// Controls mie scattering (controls how light is reflected with respect to light's direction)
		VolLightClass.MieG = 0.5f + ( 0.25f * fSunPower );

		pVolumetricLight.color = new Color( ( 1.0f - fSunPower ), ( 1.0f - fSunPower ), ( 1.0f - fSunPower ), ( 1.0f - fSunPower ) );

		VolLightClass.UserColor = Color32.Lerp( DefSunColor, SunColor, ( 1.0f - fSunPower ) );

	}
}
