using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
	[Range(0.0f, 1.0f)]
	public float time;
	public float fullDayLength;
	public float startTime = 0.4f;
	private float timeRate;
	public Vector3 noon;

	[Header("Sun")]
	public Light sun;
	public Gradient sunColor;
	public AnimationCurve sunIntensity;
	
	[Header("Moon")]
	public Light moon;
	public Gradient moonColor;
	public AnimationCurve moonIntensity;
	
	[Header("Other Lighting")]
	public AnimationCurve lightingIntensityMultiplier;
	public AnimationCurve reflectionIntensityMultiplier;

	void Start()
	{
		timeRate = 1.0f / fullDayLength;
		time = startTime;
	}

	void Update()
	{
		time = (time + timeRate * Time.deltaTime) % 1.0f;
		
		UpdateLighting(sun, sunColor, sunIntensity);
		UpdateLighting(moon, moonColor, moonIntensity);
		
		// A
		RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
		RenderSettings.ambientIntensity = reflectionIntensityMultiplier.Evaluate(time);
	}
	
	void UpdateLighting(Light lightSource, Gradient colorGradient, AnimationCurve intensityCurve)
	// Light : 광원, Gradient : 빛 색상 그라데이션, AnimationCurve : 그래프
	{
		float intensity = intensityCurve.Evaluate(time);
		
		lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
		lightSource.color = colorGradient.Evaluate(time);
		lightSource.intensity = intensity;
		
		GameObject go = lightSource.gameObject;
		if (lightSource.intensity == 0 && go.activeInHierarchy)
			go.SetActive(false);
		else if (lightSource.intensity > 0 && !go.activeInHierarchy)
			go.SetActive(true);
	}
}