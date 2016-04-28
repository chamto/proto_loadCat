using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class NodeInfo_MonoBehaviour : MonoBehaviour 
{
	//[SerializeField]

	public int nodeNumber = -1;
	public List<int> adjacencyEdgeList = new List<int>();
	public Dictionary<int,LineRenderer> lineList = new Dictionary<int,LineRenderer>();

	public bool isUpdateValue = true;

		
	void Start () 
	{
	}


	void Update () 
	{
		if (isUpdateValue) 
		{

			this.UpdateEdgeList();

			isUpdateValue = false;
		}

	}

	public void UpdateEdgeList()
	{
		GameObject obj = null;
		LineRenderer line = null;
		foreach (int nodeNum in adjacencyEdgeList) 
		{
			if(false == lineList.TryGetValue(nodeNum, out line))
			{
				obj = new GameObject();
				line = obj.AddComponent<LineRenderer>();
				lineList.Add(nodeNum, line);

				line.transform.parent = this.transform;
				line.SetWidth (0.1f, 0.3f);
				line.useWorldSpace = false;
			}
			line.name = this.nodeNumber + "->" + nodeNum;
			line.SetPosition (0, this.transform.position); //from
			line.SetPosition (1, this.NodeToPos(nodeNum)); //to
		}

		//인접엣지리스트에 없는 엣지선들을 제거한다.
		List<int> removeList = new List<int> ();
		foreach (int key in lineList.Keys) 
		{
			if(false == adjacencyEdgeList.Contains(key))
			{
				removeList.Add(key);
			}
		}
		foreach (int key in removeList) 
		{
			GameObject.Destroy(lineList[key].gameObject);
			lineList.Remove(key);
		}
	}


	
	public Vector3 NodeToPos(int nodeNum)
	{
		GameObject obj = GameObject.Find ("node (" + nodeNum + ")");
		if (null != obj) 
		{
			return obj.transform.position;
		}
		
		return Vector3.zero;
	}

}


