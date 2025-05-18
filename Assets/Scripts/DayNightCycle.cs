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

	[Header("Moon")]

	[Header("Other Lighting")]
}