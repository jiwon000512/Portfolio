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
        this.AddManipulator(new ContentZoomer());
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

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        foreach (Port port in ports.ToList())
        {
            // 자기 자신은 제외
            if (startPort == port)
                continue;

            // 방향이 반대여야 연결 가능
            if (startPort.direction == port.direction)
                continue;

            // 타입이 동일해야 연결 가능
            if (startPort.portType != port.portType)
                continue;

            compatiblePorts.Add(port);
        }

        return compatiblePorts;
    }

    public void CreateStartNode(ToolData data)
    {
        var node = new ToolNode()
        {
            title = "Start",
        };

        var generatedPort = GetPortInstance(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(100, 100, 100, 150));
        AddElement(node);

        if (data == null)
        {
            node.data = new ToolData(0);
            node.data.toolInfo = new ToolSaveData.ToolInfo();
        }
        else
        {
            node.data = data;
        }
    }

    private Port GetPortInstance(ToolNode node, Direction nodeDirection,
        Port.Capacity capacity = Port.Capacity.Multi)
    {
        return node.CreatePort(Direction.Output, "start");
    }

    public ToolNode LoadNode(ToolData data,ToolSaveData.NodeInfo nodeInfo)
    {
        var node = CreateNode("test");
        node.Load(data, nodeInfo);
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

        int max = 0;
        toolNodes.ForEach(x =>
        {
            if (x.data != null && x.id > max)
                max = x.id;
        });

        node.Create(pos, max + 1);
    }
}
