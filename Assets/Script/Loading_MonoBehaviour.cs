using UnityEngine;
using System.Collections;

public class Loading_MonoBehaviour : MonoBehaviour 
{

	bool _loadScene = true;

	// Use this for initialization
	void Start () 
	{

		//StartCoroutine (CSingleton<Table.ResourceManager>.Instance.UnityFileLoading());
		CSingleton<Table.ResourceManager>.Instance.Load();
	}


	
	// Update is called once per frame
	void Update () 
	{
		CSingleton<WideUseCoroutine>.Instance.Update ();

		if (true == CSingleton<Table.ResourceManager>.Instance.IsCompleteLoad()) 
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
