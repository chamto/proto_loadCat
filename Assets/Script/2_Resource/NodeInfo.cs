using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

namespace Table
{
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
	
	public class File_NodeInfo //: ILoadXML
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
