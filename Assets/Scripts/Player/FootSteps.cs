using UnityEngine;

public class FootSteps : MonoBehaviour
{
	public AudioClip[] footstepClips;
	private AudioSource _audioSource;
	private Rigidbody _rigidbody;
	public float footstepThreshold;
	public float footstepRate;
	private float _footstepTime;

	[Range(0f, 1f)] public float volume;
	
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f) if (_rigidbody.velocity.magnitude > footstepThreshold)
			if (Time.time - footstepThreshold > footstepRate)
			{
				_footstepTime = Time.time;
				_audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
			}

		_audioSource.volume = volume;
	}
}