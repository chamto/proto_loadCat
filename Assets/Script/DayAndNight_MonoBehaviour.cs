using UnityEngine;
using System.Collections;

public class DayAndNight_MonoBehaviour : MonoBehaviour 
{

	public Color _outColor; 
	public Color _startColor = Color.white;
	public Color _middleColor = Color.blue;
	public Color _endColor = Color.black;

	public float _dayTime = 20;
	public float _nightTime = 20;


	// Use this for initialization
	void Start () 
	{
		_outColor = _startColor;

	}

	private float _Update_elapsedTime = 0;
	private float _Update_timeOrientation = 1;
	private Color _Update_tempColor;
	void Update () 
	{
		_Update_elapsedTime += Time.deltaTime * _Update_timeOrientation;

		//day -> night
		if (_Update_elapsedTime <= 0)
		{
			_Update_timeOrientation = 1;
			_Update_elapsedTime = 0;
		}

		//day <- night
		if (_Update_elapsedTime > _dayTime + _nightTime) 
		{
			_Update_timeOrientation = -1;
		}

		if (1 == _Update_timeOrientation) 
		{
			//day -> night
			if (0 <= _Update_elapsedTime && _Update_elapsedTime <= _dayTime) 
			{
				_outColor = Color.Lerp (_startColor, _middleColor, _Update_elapsedTime / _dayTime);
				_Update_tempColor = _outColor;
			} else 
			{
				_outColor = Color.Lerp (_Update_tempColor, _endColor, (_Update_elapsedTime - _dayTime) / _nightTime);
			}
		} else 
		{
			//day <- night
			if (0 <= _Update_elapsedTime && _Update_elapsedTime <= _dayTime) {
				_outColor = Color.Lerp (_startColor, _Update_tempColor, _Update_elapsedTime / _dayTime);
			} else 
			{
				_outColor = Color.Lerp (_middleColor, _endColor, (_Update_elapsedTime - _dayTime) / _nightTime);
				_Update_tempColor = _outColor;
			}
		}


		Camera.main.backgroundColor = _outColor;
	}
}
