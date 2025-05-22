using System;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
	public AudioSource audioSource;
	public float fadeTime;
	public float maxVolume;
	private float _targetVolume;
	
	void Start()
	{
		_targetVolume = 0;
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = _targetVolume;
		audioSource.Play();
	}
	
	void Update()
	{
		if (!Mathf.Approximately(audioSource.volume, _targetVolume))
		{
			// Mathf.MoveTowards : 점진적으로 증가시켜주는 코드
			audioSource.volume = Mathf.MoveTowards(audioSource.volume, _targetVolume, (maxVolume / fadeTime) * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			_targetVolume = maxVolume;
		
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			_targetVolume = 0;
	}
}