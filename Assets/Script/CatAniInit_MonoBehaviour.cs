using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HierarchyLoader_MonoBehaviour : MonoBehaviour
{
	protected Dictionary<string, Transform> _hierarchy = new Dictionary<string, Transform>(); 


	protected string getFullName(Transform inParent, Transform inChild, byte maxFindCount)
	{
		Transform front = inChild;
		string fullName = "";
		//bool success = false;
		for (byte i=0; i<maxFindCount; i++) 
		{
			if(inParent == front) 
			{
				//success = true;
				fullName = "~" + fullName; //~ == root
				break;
			}

			fullName = "/" + front.name + fullName;

			front = front.parent;

		}

		//if (false == success)
		//	return "";

		return fullName;
	}

	protected void init()
	{
		const byte MAX_FIND_COUNT = 5;
		foreach(Transform child  in this.GetComponentsInChildren<Transform>(true))
		{
			_hierarchy.Add(this.getFullName(this.transform, child, MAX_FIND_COUNT), child);
		}
	}

	protected void setActiveInChildren(Transform current, bool bFlag, bool bSelfInclude)
	{
		foreach(Transform child  in current.GetComponentsInChildren<Transform>(true))
		{
			if(current == child && true != bSelfInclude) continue;
			child.gameObject.SetActive(bFlag);
		}
	}
	protected GameObject getGameObject(string fullName)
	{
		Transform tfo = null;
		if (true == _hierarchy.TryGetValue (fullName, out tfo)) 
		{
			return tfo.gameObject;
		}

		return null;
	}


	public void TestPrint()
	{
		Debug.Log ("---------- HierarchyLoader_MonoBehaviour : TestPrint ----------");
		foreach(KeyValuePair<string, Transform> keyValue in _hierarchy)
		{
			Debug.Log(keyValue.Key + " : " + keyValue.Value.name);
		}
	}
}


namespace LoadCat
{
	public enum eCatAniState
	{
		None = -1,
		Hide = 0,
		Rush,
		Eat,
		Max,
	}
}

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
