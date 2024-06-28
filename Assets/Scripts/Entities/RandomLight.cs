using UnityEngine;

public class RandomLight : MonoBehaviour 
{
	[SerializeField] private float _timer = 0.7f;
	[SerializeField] private Light _light;

	private float _nextChangeTime;
	
	private void Update()
	{
		if(_nextChangeTime < Time.time)
		{
			_light.color = new Color(
				Random.Range(0, 256),
				Random.Range(0, 256),
				Random.Range(0, 256)
			);
			_nextChangeTime = Time.time + _timer;
		}
	}
}
