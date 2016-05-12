using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CatAniInit_MonoBehaviour : HierarchyLoader_MonoBehaviour 
{

	public void AniInitHide()
	{
		this.setActiveInChildren (this.transform, false, false);

		this.getGameObject ("~/eye").SetActive (true);
		this.getGameObject ("~/eye/open").SetActive (true);

	}

	public void AniInitRush()
	{
		this.setActiveInChildren (this.transform, false, false);

		this.getGameObject ("~/eye").SetActive (true);
		this.getGameObject ("~/eye/open").SetActive (true);
		this.getGameObject ("~/head").SetActive (true);
		this.getGameObject ("~/body").SetActive (true);
	}

	public void AniInitEat()
	{
		this.setActiveInChildren (this.transform, false, false);

		this.getGameObject ("~/eat").SetActive (true);
	}

	public void AniDirection(Vector3 dir)
	{

		//    dir.x
		//left  :  right
		// -    :    +
		//   scale.x
		// +    :    -
		Vector3 aniDir = Vector3.zero; ////애니의 기준방향
		Vector3 scale = this.transform.localScale;
		if (dir.x <= 0) 
		{	//left
			scale.x = Mathf.Abs(scale.x);

			aniDir = Vector3.left;
			//Debug.Log("AniDirection left"); //chamto test
		}
		if (0 < dir.x) 
		{	//right
			scale.x = Mathf.Abs(scale.x) * -1;

			aniDir = Vector3.right;
			//Debug.Log("AniDirection right"); //chamto test
		}
		this.transform.localScale = scale;


		//transform.localRotation = Quaternion.LookRotation (Vector3.forward, dir);
		transform.localRotation = Quaternion.FromToRotation (aniDir, dir);

	}


	void Start () 
	{
		this.init ();


		//this.TestPrint (); //chamto test
	}


	public float angle = 0;
	void Update () 
	{
		//test code 
		if (Input.GetKey ("a")) 
		{
			angle += 400 * Time.deltaTime;
			transform.eulerAngles = new Vector3(0,0,angle);
		}
		if (Input.GetKey ("d")) 
		{
			angle -= 400 * Time.deltaTime;
			transform.eulerAngles = new Vector3(0,0,angle);
		}

	}
}
