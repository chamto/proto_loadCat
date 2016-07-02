using UnityEngine;
using System;
using System.Collections;

public class CatEvent_MonoBehaviour : MonoBehaviour 
{

	private Sprite_Mono _heartBar = null;

	// Use this for initialization
	void Start () 
	{
		UInt32 _HKEY_HEART_BAR = Single.hierarchy.PathToKey (this.transform, "/hp/heart_bar");
		Transform tfo = Single.hierarchy.GetData (_HKEY_HEART_BAR);
		_heartBar = tfo.GetComponent<Sprite_Mono> ();
		//

		//Single.dayAndNight._elapsedDay
	}
	


	private float _elapsedTime = 0;
	void Update () 
	{

		if (Single.dayAndNight._isNextDay) 
		{
			_elapsedTime += Time.deltaTime * 6f;
			if (_elapsedTime >= 1f)
				_elapsedTime = 0;
			
			_heartBar.topBottomCutting.y = _heartBar._spriteSize.y * _elapsedTime;

			_heartBar._Update_perform = true;
		}
	}
}
