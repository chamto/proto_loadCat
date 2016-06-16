using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonoGameInit : MonoBehaviour 
{

	// Use this for initialization
	void Awake () 
	{

		Single.hierarchy.Init ();

		//Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Default"), LayerMask.NameToLayer ("Ignore Physics"), true);
		//Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("SuperCat"), LayerMask.NameToLayer ("Ignore Physics"), true);
		Physics2D.IgnoreLayerCollision (GlobalConstants.Layer.Num.superCat, GlobalConstants.Layer.Num.building, true);


		GlobalConstants.InitStatic ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}




