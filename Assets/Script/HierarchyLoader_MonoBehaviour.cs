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
	
}
