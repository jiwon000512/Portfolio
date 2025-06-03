using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ToolNode : Node
{
    public NodeData data;
    public int id => data.id;

    public ToolNode(NodeData nodeData)
    {
        this.data = nodeData;
        this.title = nodeData.id.ToString();
        SetPosition(new Rect(nodeData.position, NodeView.defaultNodeSize));
    }
}
