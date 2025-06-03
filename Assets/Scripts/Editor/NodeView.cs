using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : GraphView
{
    public static readonly Vector2 defaultNodeSize = new Vector2(200, 150);

    List<ToolNode> toolNodes = new List<ToolNode>();
    Tool parent;

    int nextId = 1;

    public NodeView(Tool parent)
    {
        this.parent = parent;

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContextualMenuManipulator(OnContextMenu));
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

    }

    void OnContextMenu(ContextualMenuPopulateEvent evt)
    {
        Vector2 mousePosition = evt.localMousePosition;

        evt.menu.AppendAction("노드 생성", (e) => CreateNodeAt(mousePosition));
    }

    void RecalculateNextId()
    {
        int maxId = 0;
        foreach (var node in toolNodes)
        {
            if (node.id > maxId)
                maxId = node.id;
        }
        nextId = maxId + 1;
    }

    void CreateNodeAt(Vector2 position)
    {
        RecalculateNextId(); // 현재 노드들 기반으로 ID 재계산

        var data = new NodeData
        {
            id = nextId,
            position = position
        };
        nextId++;

        var node = new ToolNode(data);
        AddElement(node);
        toolNodes.Add(node);
    }


    public void LoadNodes(List<NodeData> datas)
    {
        toolNodes.Clear();
        ClearGraph();

        foreach (var nodeData in datas)
        {
            ToolNode node = new ToolNode(nodeData);
            AddElement(node);
            toolNodes.Add(node);

            if (nodeData.id >= nextId)
            {
                nextId = nodeData.id + 1;
            }
        }
    }

    void ClearGraph()
    {
        foreach (var node in toolNodes)
        {
            RemoveElement(node);
        }
        toolNodes.Clear();
    }

    public List<ToolNode> GetNodes()
    {
        return toolNodes;
    }
}
