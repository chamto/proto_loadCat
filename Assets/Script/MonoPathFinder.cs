using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
//using System.String;


public class MonoPathFinder : MonoBehaviour 
{

	public SparseGraph 	_graph = new SparseGraph (true);
	public Graph_SearchDFS _searchDFS = new Graph_SearchDFS();



	public Transform _town = null;

	public bool _saveXML = false;
	public bool _loadXML = false;
	public bool _updateNode = false;

	// Use this for initialization
	void Start () 
	{
		Table.CTableNodeInfo table = CSingleton<Table.ResourceManager>.Instance._nodeInfo;
		foreach (Table.NodeInfo nodeFrom in table._data) 
		{
			_graph.AddNode (new NavGraphNode (nodeFrom.nodeNum, nodeFrom.nodePos));
		}

		foreach (Table.NodeInfo nodeFrom in table._data) 
		{
			foreach(int edgeTo in nodeFrom.edgeList)
			{
				_graph.AddEdge (new GraphEdge (nodeFrom.nodeNum, edgeTo));
			}
		}

//		_searchDFS.Init (_graph, 0, 17);
//		List<int> pathList = _searchDFS.GetPathToTarget ();
//		string nodeChaine = "nodeChange : ";
//		foreach (int node in pathList) 
//		{
//			nodeChaine += node + "->";
//		}
//		Debug.Log (nodeChaine); //chamto test




		//Debug.DrawLine(Vector3.zero, new Vector3(1, 1, 0), Color.red);
		//Debug.Assert (false, "sdfsdfsdfsdf assert");

	}
	

	void Update () 
	{

		if (true == _saveXML) 
		{
			Table.CTableNodeInfo table = CSingleton<Table.ResourceManager>.Instance._nodeInfo;
			List<Table.NodeInfo> saveList = new List<Table.NodeInfo>();
			NodeInfo_MonoBehaviour[] monoList =  _town.GetComponentsInChildren <NodeInfo_MonoBehaviour>(false);
			foreach (NodeInfo_MonoBehaviour mono in monoList) 
			{
				saveList.Add(new Table.NodeInfo(mono._nodeNumber, mono.transform.position, mono._adjacencyEdgeList));
			}
			table._data = saveList;
			table.SaveXML ("Assets/StreamingAssets/"+"townNode.xml", table._data);

			//---------------
			_saveXML = false;
		}

		if (true == _loadXML) 
		{
			Table.CTableNodeInfo table = CSingleton<Table.ResourceManager>.Instance._nodeInfo;
			NodeInfo_MonoBehaviour[] monoList =  _town.GetComponentsInChildren <NodeInfo_MonoBehaviour>(false);
			foreach(NodeInfo_MonoBehaviour mono in monoList)
			{
				GameObject.Destroy(mono.gameObject);
			}
			foreach(Table.NodeInfo info in table._data)
			{
				this.AddNodePrefab(info);
			}


			//---------------
			_loadXML = false;

		}

		if (true == _updateNode) 
		{

			NodeInfo_MonoBehaviour[] monoList =  _town.GetComponentsInChildren <NodeInfo_MonoBehaviour>(false);
			foreach(NodeInfo_MonoBehaviour mono in monoList)
			{
				//mono.UpdateEdgeList();
				mono._isUpdateValue = true;
			}
			_updateNode = false;
		}
	}

//	public void fix_LoadGraphNode()
//	{
//		NodeInfo_MonoBehaviour[] monoList =  _town.GetComponentsInChildren <NodeInfo_MonoBehaviour>(true);
//		foreach (NodeInfo_MonoBehaviour mono in monoList) 
//		{
//			Debug.Log("<color=red>Load Number:</color>" + mono._nodeNumber);
//		}
//
//	}

//	public int FindNearNode_From(Vector3 current)
//	{
//		Table.CTableNodeInfo table = CSingleton<Table.ResourceManager>.Instance._nodeInfo;
//
//
//	}


	public void AddNodePrefab(Table.NodeInfo info)
	{
		GameObject obj = this.CreatePrefab ("node (-1)");
		obj.transform.parent = _town;
		obj.transform.position = info.nodePos;

		NodeInfo_MonoBehaviour mono = obj.GetComponent<NodeInfo_MonoBehaviour> ();
		mono._nodeNumber = info.nodeNum;
		mono._adjacencyEdgeList = info.edgeList;

		mono._isUpdateValue = true;

	}

	public GameObject CreatePrefab(string path)
	{
		const string root = "Prefab/";
		return MonoBehaviour.Instantiate(Resources.Load(root + path)) as GameObject;
	}
}

namespace Table
{
	public class ResourceManager
	{

		public CTableNodeInfo _nodeInfo = new CTableNodeInfo();



		//ex) XML_Manager.AsyncFileLoading(CDefine.ASSET_PATH + m_strFileName, value => stream = value)
		public static IEnumerator AsyncFileLoading(string strFilePath, System.Action<MemoryStream> result = null)
		{
			MemoryStream memStream = null;
			#if SERVER || TOOL 
			{
				//CDefine.CommonLog("1__" + strFilePath); //chamto test
				memStream = new MemoryStream(File.ReadAllBytes(strFilePath));
			}
			
			#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
			{

				UnityEngine.WWW wwwUrl = new UnityEngine.WWW(strFilePath);


				while (!wwwUrl.isDone)
				{   
					if (wwwUrl.error != null)
					{
						CDefine.DebugLog("error : " + wwwUrl.error.ToString());
						yield break;
					}
					CDefine.DebugLog("wwwUrl.progress---" + wwwUrl.progress);
					yield return null;
				}
				
				if (wwwUrl.isDone)
				{   
					CDefine.DebugLog("wwwUrl.isDone---size : "+wwwUrl.size);
					CDefine.DebugLog("wwwUrl.isDone---bytesLength : "+wwwUrl.bytes.Length);
					memStream = new MemoryStream(wwwUrl.bytes);
				}
			}
			#endif

			
			if (null != result)
			{   
				result(memStream);
			}
			CDefine.DebugLog("AsyncLoading complete");
			yield return memStream;
		}


		public IEnumerator UnityFileLoading()
		{
			string strFilePath = CDefine.ASSET_PATH + "townNode.xml";
			CDefine.DebugLog ("-------------" + strFilePath + "-------------");


			MemoryStream memStream = null;

			{
				
				UnityEngine.WWW wwwUrl = new UnityEngine.WWW(strFilePath);
				
				
				while (!wwwUrl.isDone)
				{   
					if (wwwUrl.error != null)
					{
						CDefine.DebugLog("error : " + wwwUrl.error.ToString());
						yield break;
					}
					CDefine.DebugLog("wwwUrl.progress---" + wwwUrl.progress);
					yield return null;
				}
				
				if (wwwUrl.isDone)
				{   
					CDefine.DebugLog("wwwUrl.isDone---size : "+wwwUrl.size);
					CDefine.DebugLog("wwwUrl.isDone---bytesLength : "+wwwUrl.bytes.Length);
					memStream = new MemoryStream(wwwUrl.bytes);
				}
			}

			_nodeInfo.LoadXMLFromMemory (memStream); //chamto test
			

			CDefine.DebugLog("AsyncLoading complete");
			yield return memStream;
		}



		public void Load()
		{
			//fileLoding
			//CSingleton<WideUseCoroutine>.Instance.Start_Sync (_nodeInfo.LoadXML (), null, "CTableNodeInfo");
			CSingleton<WideUseCoroutine>.Instance.Start_Async (_nodeInfo.LoadXML (), null, "CTableNodeInfo");
		}

		public bool IsCompleteLoad()
		{
			return _nodeInfo.bCompleteLoad;
		}
		


	}

	public class NodeInfo 
	{
		public int nodeNum = -1;
		public Vector3 nodePos = Vector3.zero;
		public List<int> edgeList = new List<int>();

		public NodeInfo() {}

		public NodeInfo(int nodeNum, Vector3 nodePos, List<int> edgeList)
		{
			this.nodeNum = nodeNum; this.nodePos = nodePos; this.edgeList = edgeList;
		}

		public override string ToString ()
		{
			string temp = "";
			foreach (int edge in edgeList) 
			{
				temp += nodeNum + "->" + edge + " , ";
			}
			return temp;
		}
	}

//	public interface ILoadXML
//	{
//		void ILoadXMLFromMemory(MemoryStream stream);
//	}

	public class CTableNodeInfo //: ILoadXML
	{



		private string m_strFileName = "townNode.xml";

		private bool _bCompleteLoad = false;
		public bool bCompleteLoad
		{
			get { return _bCompleteLoad; }
		}
		

		public List<NodeInfo> _data = new List<NodeInfo>();
		
		public void PrintValue ()
		{
			Debug.Log ("-------CTableNodeInfo-------");
			Debug.Log (m_strFileName);
			foreach (NodeInfo node in _data) 
			{
				Debug.Log(node.ToString());
			}
		}


		public  Vector3 Vector3FromString(string s)
		{
			char[] delimiterChars = { ' ', ',' , '(' , ')' };
			string[] parts = s.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);


//			Debug.Log ("Vector3FromString_________"); //chamto test
//			foreach(string p in parts)
//			{
//				Debug.Log("_"+p+"_");
//			}

			return new Vector3(
				float.Parse(parts[0]),
				float.Parse(parts[1]),
				float.Parse(parts[2]));
		}

		//WidUseCoroutine 으로 사용해야 동작함. 유니티코루틴으로는 동작안함
		//ex) CSingleton<WideUseCoroutine>.Instance.StartCoroutine (t.LoadXML (), null, false, "CTableNodeInfo");
		public IEnumerator LoadXML()
		{
			//내부 코루틴 부분
			//------------------------------------------------------------------------
			CDefine.DebugLog(CDefine.ASSET_PATH + m_strFileName); //chamto test
			MemoryStream stream = null;
			yield return ResourceManager.AsyncFileLoading(CDefine.ASSET_PATH + m_strFileName, value => stream = value);
			
			if (null == stream)
			{
				CDefine.DebugLog("error : failed LoadFromFile : " + CDefine.ASSET_PATH + m_strFileName);
				yield break;
			}
			this.LoadXMLFromMemory (stream);
			
			//chamto test
			//PrintValue ();
			//SaveXML ("Assets/StreamingAssets/"+"abc.xml", _data);
			
		}
		
		public void LoadXMLFromMemory(MemoryStream stream)
		{
			_bCompleteLoad = false;
			_data.Clear();
		
			//------------------------------------------------------------------------

//			<root>
//				<NodeInfo nodeNum="0" nodePos="(1.0 , 1.0 , 1.0)">
//					<n0 edgeNum="1"  />
//					<n1 edgeNum="2"  />
//					<n2 edgeNum="3"  />
//				</NodeInfo>
//			</root>

			XmlDocument Xmldoc = new XmlDocument();
			Xmldoc.Load(stream);
			
			XmlElement root_element = Xmldoc.DocumentElement; 	//<root>		
			XmlNodeList secondList = root_element.ChildNodes;	//	<NodeInfo>
			XmlNodeList thirdList = null;;
			XmlAttributeCollection attrs = null;
			XmlNode xmlNode = null;
			//Debug.Log ("loadXML : " + secondList.Count); //chamto test
			NodeInfo item = null;
			for (int i = 0; i < secondList.Count; ++i) 
			{
				item = new NodeInfo();
				_data.Add(item);
				xmlNode = secondList[i].Attributes.GetNamedItem("nodeNum");
				item.nodeNum = int.Parse(xmlNode.Value);
				xmlNode = secondList[i].Attributes.GetNamedItem("nodePos");
				item.nodePos = this.Vector3FromString(xmlNode.Value);
				//Debug.Log ("parse : " + xmlNode.Name + " : "+ xmlNode.Value); //chamto test

				thirdList = secondList[i].ChildNodes;
				for (int j = 0; j < thirdList.Count; ++j) 
				{
					attrs = thirdList[j].Attributes;
					foreach(XmlNode n in attrs)
					{
						switch(n.Name)
						{
						case "edgeNum":
							item.edgeList.Add(int.Parse(n.Value)); break;
						}
					}
				}
			}

			_bCompleteLoad = true;
		}//func end

		public void SaveXML(string directory, List<NodeInfo> list)
		{     
			XmlDocument Xmldoc = new XmlDocument();
			XmlDeclaration decl = Xmldoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
			Xmldoc.AppendChild(decl);
			XmlElement root_element = Xmldoc.CreateElement("root");
			XmlElement second_element = null;
			XmlElement third_element = null;
			

			//-----------------------------------
			foreach (NodeInfo n in list) 
			{
				second_element = Xmldoc.CreateElement("NodeInfo");
				second_element.SetAttribute("nodeNum",n.nodeNum.ToString());
				second_element.SetAttribute("nodePos",n.nodePos.ToString());


				int count = 0;
				foreach (int edgeNum in n.edgeList) 
				{
					third_element = Xmldoc.CreateElement("n" + count.ToString());
					third_element.SetAttribute("edgeNum", edgeNum.ToString());
					
					count++;
					second_element.AppendChild(third_element);
				}
				root_element.AppendChild(second_element);
			}

			Xmldoc.AppendChild(root_element);
			Xmldoc.Save(directory);

		}


	}//class end
}//namespace end
