using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HierarchyLoader_MonoBehaviour : MonoBehaviour
{


	protected Dictionary<string, Transform> _hierarchy = new Dictionary<string, Transform>(); 

	protected void init()
	{
		this.AddHierarchy (this.transform);
	}


	protected string createFullName(Transform inParent, Transform toChild, byte maxFindCount)
	{
		Transform front = toChild;
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
		
		if (inParent != front) 
		{
			DebugWide.LogWarning("unreachable  parent: "+ fullName);
		}
		//if (false == success)
		//	return "";
		
		
		return fullName;
	}



	private void AddHierarchy(Transform tfo)
	{
		const byte MAX_FIND_COUNT = 20;
		foreach(Transform child  in this.GetComponentsInChildren<Transform>(true))
		{
			_hierarchy.Add(this.createFullName(tfo, child, MAX_FIND_COUNT), child);
		}
	}

	public void SetActiveInChildren(Transform current, bool bFlag, bool bSelfExcept)
	{
		foreach(Transform child  in current.GetComponentsInChildren<Transform>(true))
		{
			if(current == child && true == bSelfExcept) continue;
			child.gameObject.SetActive(bFlag);
		}
	}

	public GameObject GetGameObject(string fullPath_Name)
	{
		Transform tfo = null;
		if (true == _hierarchy.TryGetValue (fullPath_Name, out tfo)) 
		{
			return tfo.gameObject;
		}

		return null;
	}

	public GameObject GetGameObject(Transform start, string relativePath_Name)
	{
		Transform tfo = null;
		if (true == _hierarchy.TryGetValue (relativePath_Name, out tfo)) 
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
