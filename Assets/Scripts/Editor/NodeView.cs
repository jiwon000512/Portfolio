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

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        if (evt.target is GraphView)
        {
            evt.menu.AppendAction("노드 생성", CreateNode, DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendSeparator();
        }

        if (evt.target is GraphView || evt.target is Node || evt.target is Group)
        {
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("복사", delegate
            {
                DuplicateSelectionCallback();
            }, (DropdownMenuAction a) => canDuplicateSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
            evt.menu.AppendSeparator();
        }


        if (evt.target is GraphView || evt.target is Node || evt.target is Group || evt.target is Edge)
        {
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("삭제", delegate
            {
                if (evt.target is Node)
                {

                }
                else
                {
                    DeleteSelectionCallback(AskUser.DontAskUser);
                }
            }, (DropdownMenuAction a) => canDeleteSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);


        }
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
        var node = new ToolNode()
        {
            title = nodeName,
        };

        AddElement(node);
        return node;
    }

    private void CreateNode(DropdownMenuAction a)
    { 
        var node = CreateNode("노드");

        var pos = viewTransform.matrix.inverse.MultiplyPoint(a.eventInfo.mousePosition);

        node.Create(pos);

    }
}
