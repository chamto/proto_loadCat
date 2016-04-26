using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class SparseGraph_2
{



}



public class GraphNode : System.ICloneable
{  
	//every node has an index. A valid index is >= 0
	protected 	int        m_iIndex;

	public const int INVALID_NODE_INDEX = -1;
		
	public GraphNode(){ m_iIndex = GraphNode.INVALID_NODE_INDEX;}
	public GraphNode(int idx){ m_iIndex = idx;}


	public int  Index() {return m_iIndex;}
	public void SetIndex(int NewIndex){m_iIndex = NewIndex;}

	public override string ToString()
	{
		return "" + m_iIndex;
	}

	public virtual object Clone()
	{
		GraphNode node = new GraphNode ();
		node.m_iIndex = this.m_iIndex;

		return (object)node;
	}

}



//-----------------------------------------------------------------------------
//
//  Graph node for use in creating a navigation graph.This node contains
//  the position of the node and a pointer to a BaseGameEntity... useful
//  if you want your nodes to represent health packs, gold mines and the like
//-----------------------------------------------------------------------------

public	class NavGraphNode : GraphNode
{
		
	//the node's position
	protected	Vector2     m_vPosition;
	
	//often you will require a navgraph node to contain additional information.
	//For example a node might represent a pickup such as armor in which
	//case m_ExtraInfo could be an enumerated value denoting the pickup type,
	//thereby enabling a search algorithm to search a graph for specific items.
	//Going one step further, m_ExtraInfo could be a pointer to the instance of
	//the item type the node is twinned with. This would allow a search algorithm
	//to test the status of the pickup during the search. 
	//protected object  m_ExtraInfo;


	public NavGraphNode(){}
	
	public NavGraphNode(int  idx, Vector2 pos):base(idx)
	{
		m_vPosition = pos;
	}

	public override string ToString ()
	{
		return "" + base.ToString () + " " + m_vPosition;
	}


	
	public Vector2   Pos() {return m_vPosition;}
	public void       SetPos(Vector2 NewPosition){m_vPosition = NewPosition;}
	
	//public object ExtraInfo() {return m_ExtraInfo;}
	//public void       SetExtraInfo(object info){m_ExtraInfo = info;}

	public  override object Clone()
	{
		NavGraphNode node = base.Clone () as NavGraphNode;
		node.m_vPosition = this.m_vPosition;
		
		return (object)node;
	}

}



public class GraphEdge : System.ICloneable
{
	//An edge connects two nodes. Valid node indices are always positive.
	protected	int     m_iFrom;
	protected	int     m_iTo;
	
	//the cost of traversing the edge
	protected 	double  m_dCost;

		
		//ctors
	public	GraphEdge(int from, int to, double cost)
	{
		m_dCost = cost;
		m_iFrom = from;
		m_iTo = to;
	}
	
	public GraphEdge(int from, int  to)
	{
		m_dCost = 1.0;
		m_iFrom = from;
		m_iTo = to;
	}
	
	public GraphEdge()
	{
		m_dCost = 1.0;
		m_iFrom = GraphNode.INVALID_NODE_INDEX;
		m_iTo = GraphNode.INVALID_NODE_INDEX;
	}
	
	public override string ToString()
	{
		return "from:" + m_iFrom + " to:" + m_iTo + " cost:" + m_dCost;
	}
	

	public int   From() {return m_iFrom;}
	public void  SetFrom(int NewIndex){m_iFrom = NewIndex;}
	
	public int   To() {return m_iTo;}
	public void  SetTo(int NewIndex){m_iTo = NewIndex;}
	
	public double Cost() {return m_dCost;}
	public void  SetCost(double NewCost){m_dCost = NewCost;}
	
	
	//these two operators are required
	public static bool operator ==(GraphEdge rhsA, GraphEdge rhsB)
	{
		return rhsA.m_iFrom == rhsB.m_iFrom &&
			rhsA.m_iTo   == rhsB.m_iTo   &&
				rhsA.m_dCost == rhsB.m_dCost;
	}
	
	public static bool operator !=(GraphEdge rhsA, GraphEdge rhsB)
	{
		return !(rhsA == rhsB);
	}

	

	
	public override bool Equals(System.Object obj)
	{
		// If parameter is null return false.
		if (obj == null)
		{
			return false;
		}
		
		// If parameter cannot be cast to Point return false.
		GraphEdge p = obj as GraphEdge;
		if ((System.Object)p == null)
		{
			return false;
		}
		
		// Return true if the fields match:
		return (m_iFrom == p.m_iFrom) && (m_iTo == p.m_iTo);
	}
	
	public bool Equals(GraphEdge p)
	{
		// If parameter is null return false:
		if ((object)p == null)
		{
			return false;
		}
		
		// Return true if the fields match:
		return (m_iFrom == p.m_iFrom) && (m_iTo == p.m_iTo);
	}
	
	public override int GetHashCode()
	{
		return m_iFrom ^ m_iTo;
	}

	public object Clone()
	{
		GraphEdge edge = this.MemberwiseClone () as GraphEdge;
		return (object)edge;
	}

}




//성긴그래프
public class SparseGraph
{
	//a couple more typedefs to save my fingers and to help with the formatting
	//of the code on the printed page
	public class NodeVector : List<GraphNode> {}
	public class EdgeList : LinkedList<GraphEdge> {}
	public class EdgeListVector : List<EdgeList> {}

	//public LinkedList<string> a = new LinkedList<string> ();
		
	//the nodes that comprise this graph
	private NodeVector      m_Nodes = new NodeVector();

	//벡터의 인접엣지리스트
	//a vector of adjacency edge lists. (each node index keys into the 
	//list of edges associated with that node)
	private EdgeListVector  m_Edges = new EdgeListVector();

	//다이그래프로 설정할 것인가 지정
	//=> 다이그래프란? 엣지의 방향성이 있는 그래프이다.
	//is this a directed graph?
	private bool            m_bDigraph;
	
	//the index of the next node to be added
	private int             m_iNextNodeIndex;

	//returns true if an edge is not already present in the graph. Used
	//when adding edges to make sure no duplicates are created.
	private bool  UniqueEdge(int from, int to)
	{

		foreach(GraphEdge iter in m_Edges[from])
		{
			if(iter.To() == to)
			{
				return false;
			}
		}

		return true;
	}
	
	//iterates through all the edges in the graph and removes any that point
	//to an invalidated node
	private void  CullInvalidEdges()
	{
		foreach (EdgeList list in m_Edges) 
		{
			foreach (GraphEdge curEdge in list) 
			{
				if (m_Nodes[curEdge.To()].Index() == GraphNode.INVALID_NODE_INDEX || 
				    m_Nodes[curEdge.From()].Index() == GraphNode.INVALID_NODE_INDEX)
				{
					//chamto watching me : 리스트 순회중 리스트요소를 지우는게 가능한지 확인하기
					list.Remove(curEdge);
				}
			}
		}

	}
	
	
	public SparseGraph(bool digraph)
	{
		m_iNextNodeIndex = 0; 
		m_bDigraph = digraph;
	}
	
	//returns the node at the given index
	public  GraphNode  GetNode(int idx)
	{
		Assert.IsTrue( (idx < m_Nodes.Count) &&
		       (idx >=0),             
		       "<SparseGraph::GetNode>: invalid index");
		
		return m_Nodes[idx];
	}
	
	//const method for obtaining a reference to an edge
	public  GraphEdge GetEdge(int from, int to)
	{
		Assert.IsTrue( (from < m_Nodes.Count) &&
		       (from >=0)              &&
		       m_Nodes[from].Index() != GraphNode.INVALID_NODE_INDEX ,
		       "<SparseGraph::GetEdge>: invalid 'from' index");
		
		Assert.IsTrue( (to < m_Nodes.Count) &&
		       (to >=0)              &&
		       m_Nodes[to].Index() != GraphNode.INVALID_NODE_INDEX ,
		       "<SparseGraph::GetEdge>: invalid 'to' index");

		foreach(GraphEdge curEdge in m_Edges[from])
		{
			if (curEdge.To() == to) return curEdge;
		}

		Assert.IsTrue (false , "<SparseGraph::GetEdge>: edge does not exist");

		return null;
	}
	
	public EdgeList GetEdges(int node)
	{
		Assert.IsTrue( (node < m_Nodes.Count) &&
		              (node >=0)              &&
		              m_Nodes[node].Index() != GraphNode.INVALID_NODE_INDEX ,
		              "<SparseGraph::GetEdges>: invalid 'node' index");

		return m_Edges [node];
	}
	
	
	//retrieves the next free node index
	public int   GetNextFreeNodeIndex() {return m_iNextNodeIndex;}
	
	//adds a node to the graph and returns its index
	public int   AddNode(GraphNode node)
	{
		if (node.Index() < (int)m_Nodes.Count)
		{
			//make sure the client is not trying to add a node with the same ID as
			//a currently active node
			Assert.IsTrue (m_Nodes[node.Index()].Index() == GraphNode.INVALID_NODE_INDEX ,
			        "<SparseGraph::AddNode>: Attempting to add a node with a duplicate ID");
			
			m_Nodes[node.Index()] = node;
			
			return m_iNextNodeIndex;
		}
		
		else
		{
			//make sure the new node has been indexed correctly
			Assert.IsTrue (node.Index() == m_iNextNodeIndex , "<SparseGraph::AddNode>:invalid index");

			m_Nodes.Add(node);
			m_Edges.Add(new EdgeList());
			
			return m_iNextNodeIndex++;
		}
	}
	
	//removes a node by setting its index to invalid_node_index
	public void  RemoveNode(int node)
	{
		Assert.IsTrue(node < (int)m_Nodes.Count , "<SparseGraph::RemoveNode>: invalid node index");
		
		//set this node's index to invalid_node_index
		m_Nodes[node].SetIndex(GraphNode.INVALID_NODE_INDEX);
		
		//if the graph is not directed remove all edges leading to this node and then
		//clear the edges leading from the node
		if (!m_bDigraph)
		{    
			//visit each neighbour and erase any edges leading to this node
			foreach (GraphEdge curEdge in m_Edges[node])
			{
				foreach (GraphEdge curE in m_Edges[curEdge.To()])
				{
					if (curE.To() == node)
					{
						m_Edges[curEdge.To()].Remove(curE);

						break;
					}
				}
			}
			
			//finally, clear this node's edges
			m_Edges[node].Clear();
		}
		
		//if a digraph remove the edges the slow way
		else
		{
			CullInvalidEdges();
		}
	}
	
	//Use this to add an edge to the graph. The method will ensure that the
	//edge passed as a parameter is valid before adding it to the graph. If the
	//graph is a digraph then a similar edge connecting the nodes in the opposite
	//direction will be automatically added.
	public void  AddEdge(GraphEdge edge)
	{
		//first make sure the from and to nodes exist within the graph 
		Assert.IsTrue( (edge.From() < m_iNextNodeIndex) && (edge.To() < m_iNextNodeIndex) ,
		       "<SparseGraph::AddEdge>: invalid node index");
		
		//make sure both nodes are active before adding the edge
		if ( (m_Nodes[edge.To()].Index() != GraphNode.INVALID_NODE_INDEX) && 
		    (m_Nodes[edge.From()].Index() != GraphNode.INVALID_NODE_INDEX))
		{
			//add the edge, first making sure it is unique
			if (UniqueEdge(edge.From(), edge.To()))
			{
				m_Edges[edge.From()].AddLast(edge);
			}
			
			//if the graph is undirected we must add another connection in the opposite
			//direction
			if (!m_bDigraph)
			{
				//check to make sure the edge is unique before adding
				if (UniqueEdge(edge.To(), edge.From()))
				{
					GraphEdge NewEdge = edge.Clone() as GraphEdge;
					
					NewEdge.SetTo(edge.From());
					NewEdge.SetFrom(edge.To());
					
					m_Edges[edge.To()].AddLast(NewEdge);
				}
			}
		}
	}
	
	//removes the edge connecting from and to from the graph (if present). If
	//a digraph then the edge connecting the nodes in the opposite direction 
	//will also be removed.
	public void  RemoveEdge(int from, int to)
	{
		Assert.IsTrue ( (from < (int)m_Nodes.Count) && (to < (int)m_Nodes.Count) ,
		        "<SparseGraph::RemoveEdge>:invalid node index");

		if (!m_bDigraph)
		{
			foreach(GraphEdge curEdge in m_Edges[to])
			{
				if (curEdge.To() == from){m_Edges[to].Remove(curEdge);break;}
			}
		}
		
		foreach(GraphEdge curEdge in m_Edges[from])
		{
			if (curEdge.To() == to){m_Edges[from].Remove(curEdge);break;}
		}
	}
	
	//sets the cost of an edge
	public void  SetEdgeCost(int from, int to, double cost)
	{
		//make sure the nodes given are valid
		Assert.IsTrue( (from < m_Nodes.Count) && (to < m_Nodes.Count) ,
		       "<SparseGraph::SetEdgeCost>: invalid index");
		
		//visit each neighbour and erase any edges leading to this node
		foreach(GraphEdge curEdge in m_Edges[from])
		{
			if (curEdge.To() == to)
			{
				curEdge.SetCost(cost);
				break;
			}
		}
	}
	
	//returns the number of active + inactive nodes present in the graph
	public int   NumNodes() {return m_Nodes.Count;}
	
	//returns the number of active nodes present in the graph (this method's
	//performance can be improved greatly by caching the value)
	public int   NumActiveNodes()
	{
		int count = 0;
		
		for (int n=0; n<m_Nodes.Count; ++n) if (m_Nodes[n].Index() != GraphNode.INVALID_NODE_INDEX) ++count;
		
		return count;
	}
	
	//returns the total number of edges present in the graph
	public int   NumEdges()
	{
		int tot = 0;

		foreach(EdgeList edgeList in m_Edges)
		{
			tot += edgeList.Count;
		}
		
		return tot;
	}
	
	//returns true if the graph is directed
	public bool  isDigraph(){return m_bDigraph;}
	
	//returns true if the graph contains no nodes
	public bool	isEmpty()
	{
		return (0 == m_Nodes.Count);
	}
	
	//returns true if a node with the given index is present in the graph
	public bool isNodePresent(int nd)
	{
		if ((nd >= (int)m_Nodes.Count || (m_Nodes[nd].Index() == GraphNode.INVALID_NODE_INDEX)))
		{
			return false;
		}
		else return true;
	}
	
	//returns true if an edge connecting the nodes 'to' and 'from'
	//is present in the graph
	public bool isEdgePresent(int from, int to)
	{
		if (isNodePresent(from) && isNodePresent(from))
		{
			foreach(GraphEdge curEdge in m_Edges[from])
			{
				if (curEdge.To() == to) return true;
			}
			
			return false;
		}
		else return false;
	}
	

	//clears the graph ready for new node insertions
	public void Clear(){m_iNextNodeIndex = 0; m_Nodes.Clear(); this.RemoveEdges (); m_Edges.Clear();}
	
	public void RemoveEdges()
	{
		foreach(EdgeList edgeList in m_Edges)
		{
			edgeList.Clear();
		}
	}

}



//----------------------------- Graph_SearchDFS -------------------------------
//
//  class to implement a depth first search. 
//-----------------------------------------------------------------------------

public class Graph_SearchDFS
{
		
		//to aid legibility
	private enum Aid : int
	{
		visited = 0, 
		unvisited = 1, 
		no_parent_assigned = 2
	}

		
		//a reference to the graph to be searched
	private	 SparseGraph m_Graph = null;
	
	//this records the indexes of all the nodes that are visited as the
	//search progresses
	private List<int>  m_Visited = null;
	
	//this holds the route taken to the target. Given a node index, the value
	//at that index is the node's parent. ie if the path to the target is
	//3-8-27, then m_Route[8] will hold 3 and m_Route[27] will hold 8.
	private List<int>  m_Route = null;
	
	//As the search progresses, this will hold all the edges the algorithm has
	//examined. THIS IS NOT NECESSARY FOR THE SEARCH, IT IS HERE PURELY
	//TO PROVIDE THE USER WITH SOME VISUAL FEEDBACK
	private List<GraphEdge>  m_SpanningTree = null;
	
	//the source and target node indices
	private int               m_iSource, m_iTarget;
	
	//true if a path to the target has been found
	private bool              m_bFound;
	
	
	//this method performs the DFS search
	private bool Search()
	{
		//create a std stack of edges
		Stack<GraphEdge> stack = new Stack<GraphEdge>();
		
		//create a dummy edge and put on the stack
		GraphEdge Dummy = new GraphEdge(m_iSource, m_iSource, 0);
		stack.Push(Dummy);
		
		//while there are edges in the stack keep searching
		while (0 != stack.Count)
		{
			//grab the next edge
			GraphEdge Next = stack.Pop();
			
			//remove the edge from the stack
			stack.Pop();
			
			//make a note of the parent of the node this edge points to
			m_Route[Next.To()] = Next.From();
			
			//put it on the tree. (making sure the dummy edge is not placed on the tree)
			if (Next != Dummy)
			{
				m_SpanningTree.Add(Next);
			}
			
			//and mark it visited
			m_Visited[Next.To()] = (int)Aid.visited;
			
			//if the target has been found the method can return success
			if (Next.To() == m_iTarget)
			{
				return true;
			}
			
			//push the edges leading from the node this edge points to onto
			//the stack (provided the edge does not point to a previously 
			//visited node)
			foreach(GraphEdge pE in m_Graph.GetEdges(Next.To()))
			{
				if (m_Visited[pE.To()] == (int)Aid.unvisited)
				{
					stack.Push(pE);
				}
			}
		}
		
		//no path to target
		return false;
	}

	public
		Graph_SearchDFS( SparseGraph  graph,
		                int          source,
		                int          target )
	{       
		m_Graph = graph;
		m_iSource = source;
		m_iTarget = target;
		m_bFound = false;
		m_Visited = new List<int>(m_Graph.NumNodes()){(int)Aid.unvisited,};  
		m_Route = new List<int>(m_Graph.NumNodes()){(int)Aid.no_parent_assigned,};
		m_SpanningTree = new List<GraphEdge>();

		m_bFound = Search(); 
	}
	
	
	//returns a vector containing pointers to all the edges the search has examined
	public List<GraphEdge> GetSearchTree() {return m_SpanningTree;}
	
	//returns true if the target node has been located
	public bool   Found() {return m_bFound;}
	
	//returns a vector of node indexes that comprise the shortest path
	//from the source to the target
	public List<int> GetPathToTarget()
	{
		List<int> path = new List<int>();
		
		//just return an empty path if no path to target found or if
		//no target has been specified
		if (!m_bFound || m_iTarget<0) return path;
		
		int nd = m_iTarget;

		path.Add(nd);
		
		while (nd != m_iSource)
		{
			nd = m_Route[nd];
			
			path.Add(nd);
		}
		
		return path;
	}
}

