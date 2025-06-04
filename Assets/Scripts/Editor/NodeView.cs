using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : GraphView
{
    public static readonly Vector2 defaultNodeSize = new Vector2(200, 150);
   
    public List<ToolNode> toolNodes => nodes.ToList().Cast<ToolNode>().ToList(); 
    Tool parent;

    public NodeView(Tool parent)
    {
        this.parent = parent;

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
    }

    public void CreateStartNode(ToolData data)
    {

    }

    public ToolNode LoadNode(ToolData data)
    {
        var node = CreateNode("test");
        node.Load(data);
        return node;
    }

    public ToolNode CreateNode(string nodeName)
    {
        return null;
    }
}
