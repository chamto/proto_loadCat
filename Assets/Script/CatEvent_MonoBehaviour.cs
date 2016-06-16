using UnityEngine;
using System;
using System.Collections;

public class CatEvent_MonoBehaviour : MonoBehaviour 
{
	private UInt32 _HKEY_HEART_BAR = 0;
	private Sprite_Mono _heartBar = null;

	// Use this for initialization
	void Start () 
	{
		_HKEY_HEART_BAR = Single.hierarchy.PathToKey (this.transform, "/hp/heart_bar");
		Transform tfo = Single.hierarchy.GetData (_HKEY_HEART_BAR);
		_heartBar = tfo.GetComponent<Sprite_Mono> ();
		_heartBar.topBottomCutting.y = 0;
	}
	


	float _elapsedTime = 0;
	void Update () 
	{
		_elapsedTime += Time.deltaTime;
		if (_elapsedTime >= 1f)
			_elapsedTime = 0;

		_heartBar.topBottomCutting.y = 40f * _elapsedTime;
		_heartBar._Update_perform = true;
	}
}
