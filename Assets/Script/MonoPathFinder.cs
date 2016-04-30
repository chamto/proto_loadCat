﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class MonoPathFinder : MonoBehaviour 
{

	private SparseGraph 	_graph = new SparseGraph (true);
	private Graph_SearchDFS _searchDFS = new Graph_SearchDFS();

	public Transform _town = null;

	// Use this for initialization
	void Start () 
	{
		_graph.AddNode (new NavGraphNode (0, Vector2.zero));
		//_graph.AddEdge (new GraphEdge (0, 1));


		//_searchDFS.Init (_graph, 0, 10);
		//List<int> pathList = _searchDFS.GetPathToTarget ();

		this.LoadGraphNode ();

		//Debug.DrawLine(Vector3.zero, new Vector3(1, 1, 0), Color.red);
		//Debug.Assert (false, "sdfsdfsdfsdf assert");


		//fileLoding
		Table.CTableNodeInfo t = new Table.CTableNodeInfo ();
		//this.StartCoroutine (t.LoadXML ());
		CSingleton<WideUseCoroutine>.Instance.StartCoroutine (t.LoadXML (), null, false, "CTableNodeInfo");

	}
	

	void Update () 
	{
	}

	public void LoadGraphNode()
	{
		NodeInfo_MonoBehaviour[] list =  _town.GetComponentsInChildren <NodeInfo_MonoBehaviour>(true);
		foreach (NodeInfo_MonoBehaviour info in list) 
		{
			Debug.Log("<color=red>Load Number:</color>" + info._nodeNumber);
		}


		this.loadAdjacencyEdgeList ();
	}

	private void loadAdjacencyEdgeList()
	{
	}
}

public class XML_Manager
{
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


	public void Load()
	{

	}
	
	public void Save()
	{
		
	}
	
}

namespace Table
{
	public class NodeInfo 
	{
		public int nodeNum = -1;
		public List<int> edgeList = new List<int>();

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
	public class CTableNodeInfo
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

		//WidUseCoroutine 으로 사용해야 동작함. 유니티코루틴으로는 동작안함
		//ex) CSingleton<WideUseCoroutine>.Instance.StartCoroutine (t.LoadXML (), null, false, "CTableNodeInfo");
		public IEnumerator LoadXML()
		{
			//내부 코루틴 부분
			//------------------------------------------------------------------------
			CDefine.DebugLog(CDefine.ASSET_PATH + m_strFileName); //chamto test
			MemoryStream stream = null;
			yield return XML_Manager.AsyncFileLoading(CDefine.ASSET_PATH + m_strFileName, value => stream = value);

			if (null == stream)
			{
				CDefine.DebugLog("error : failed LoadFromFile : " + CDefine.ASSET_PATH + m_strFileName);
				yield break;
			}
			this.loadXMLFromMemory (stream);

			//chamto test
			PrintValue ();
			SaveXML ("Assets/StreamingAssets/"+"abc.xml", _data);

		}

		private void loadXMLFromMemory(MemoryStream stream)
		{
			_bCompleteLoad = false;
			_data.Clear();
		
			//------------------------------------------------------------------------

//			<root>
//				<NodeInfo nodeNum="0" >
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
				//Debug.Log ("parse : " + xmlNode.Name + " : "+ item.nodeNum); //chamto test

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
