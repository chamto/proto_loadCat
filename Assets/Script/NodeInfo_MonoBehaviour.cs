using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class NodeInfo_MonoBehaviour : MonoBehaviour 
{
	//[SerializeField]
	public int _nodeNumber = -1;

	public List<int> _adjacencyEdgeList = new List<int>();
	public Dictionary<int,LineRenderer> _lineList = new Dictionary<int,LineRenderer>();

	public bool _isUpdateValue = true;

		
	void Start () 
	{
	}


	void Update () 
	{
		if (_isUpdateValue) 
		{

			this.UpdateEdgeList();
			this.UpdateNode();

			_isUpdateValue = false;
		}

	}
	public void UpdateNode()
	{
		this.name = "node (" + _nodeNumber + ")";
		TextMesh tm = this.GetComponentInChildren<TextMesh> ();
		tm.text = _nodeNumber.ToString ();

	}
	public void UpdateEdgeList()
	{
		GameObject obj = null;
		LineRenderer line = null;
		foreach (int nodeNum in _adjacencyEdgeList) 
		{
			if(false == _lineList.TryGetValue(nodeNum, out line))
			{
				obj = new GameObject();
				line = obj.AddComponent<LineRenderer>();
				_lineList.Add(nodeNum, line);

				line.transform.parent = this.transform;
				line.SetWidth (0.05f, 0.1f);
				line.useWorldSpace = false;
			}
			line.name = this._nodeNumber + "->" + nodeNum;
			line.SetPosition (0, this.transform.position); //from
			line.SetPosition (1, this.NodeToPos(nodeNum)); //to
		}

		//인접엣지리스트에 없는 엣지선들을 제거한다.
		List<int> removeList = new List<int> ();
		foreach (int key in _lineList.Keys) 
		{
			if(false == _adjacencyEdgeList.Contains(key))
			{
				removeList.Add(key);
			}
		}
		foreach (int key in removeList) 
		{
			GameObject.Destroy(_lineList[key].gameObject);
			_lineList.Remove(key);
		}
	}


	
	public Vector3 NodeToPos(int nodeNum)
	{
		GameObject obj = GameObject.Find ("node (" + nodeNum + ")");
		if (null != obj) 
		{
			return obj.transform.position;
		}
		//Debug.Log("<color=red>Failure objectFind From NodeToPos : </color>" + nodeNum); //chamto test
		return Vector3.zero;
	}

}


