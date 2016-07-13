using UnityEngine;
using System.Collections;

public class Mono_SceneLoading : MonoBehaviour 
{

	bool _loadScene = true;

	// Use this for initialization
	void Start () 
	{

		//StartCoroutine (CSingleton<Table.ResourceManager>.Instance.UnityFileLoading());
		//CSingleton<Table.ResourceManager>.Instance.Load();
		Single.resource.Load_ASync ();
	}


	
	// Update is called once per frame
	void Update () 
	{
		//if (true == CSingleton<Table.ResourceManager>.Instance.IsCompleteLoad()) 
		if (true == Single.resource.IsCompleteLoad()) 
		{
			if(true == this._loadScene)
			{
				this._loadScene = false;
				Debug.Log("------------------- Loading Complete -------------------");
				Application.LoadLevel ("Game");
			}
		}
	}
}
