using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using static ToolSaveData;

public partial class Tool : EditorWindow
{
    [MenuItem("메뉴/툴")]
    public static void OpenEditorWindow()
    {
        if (EditorSceneManager.GetActiveScene().name != "DummyPlayScene")
        {
            EditorSceneManager.OpenScene("Assets/Scenes/DummyPlayScene.unity", OpenSceneMode.Single);
        }
        Open();
    }
}

public partial class Tool : EditorWindow
{
    MenuView menuView;
    NodeView nodeView;

    private void Awake()
    {
        nodeView = new NodeView(this);
        menuView = new MenuView(this);

        IStyle style = nodeView.style;
        style.position = Position.Absolute;
        style.left = 0f; style.top = 0f; style.right = 300f; style.bottom = 0f;

        rootVisualElement.Add(nodeView);
    }

    void OnGUI()
    {
        menuView.DoGUI(new Rect(nodeView.layout.width + 10, nodeView.layout.y + 30, 290, nodeView.layout.height - 30));
    }

    public static void Open()
    {
        var window = GetWindow<Tool>();
        window.titleContent = new GUIContent("툴");
    }

    const string SaveFolderPath = "Assets/EditorData/";

    public void Load(string fileName)
    {
        DeleteAll();

        var data = AssetDatabase.LoadAssetAtPath<ToolSaveData>(SaveFolderPath + fileName + ".asset");

        if (data == null || data.nodes.Count == 0)
        {
            nodeView.CreateStartNode(null);
            return;
        }

        List<ToolData> toolDatas = new List<ToolData>();
        foreach(var v in data.nodes)
        {
            ToolData toolData = new ToolData(v.id);
            toolData.AddData(v);
            toolDatas.Add(toolData);
        }

        foreach(var toolData in toolDatas)
        {
            if(toolData.id == 0)
            {
                nodeView.CreateStartNode(toolData);
            }
            else
            {
                var nodeInfo = data.nodes.Find(x => x.id == toolData.id);
                var node = nodeView.LoadNode(toolData, nodeInfo);
                nodeView.AddElement(node);

                var nodePorts = data.nodeLinks.Where(x=> x.baseId == toolData.id).ToList();
                nodePorts.ForEach(x => node.CreateOutput());
            }
        }

        var nodes = nodeView.toolNodes;

        for (int i = 0; i < nodes.Count; i++)
        {
            var k = i;
            var connections = data.nodeLinks.Where(x => x.baseId == nodes[k].data.id).ToList();
            for (var j = 0; j < connections.Count(); j++)
            {
                var id = connections[j].targetId;

                var targetNode = nodes.Find(x => x.data.id == id);

                if (targetNode == null || targetNode.data?.id == 0)
                {
                    continue;
                }

                if (nodes[i].outputContainer.childCount <= j)
                    continue;

                LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0], nodeView);

                targetNode.SetPosition(new Rect(
                    data.nodes.First(x => x.id == id).viewPosition,
                    NodeView.defaultNodeSize));
            }
        }
    }

    public void Save(string fileName)
    {
        var data = AssetDatabase.LoadAssetAtPath<ToolSaveData>(SaveFolderPath + fileName + ".asset");
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<ToolSaveData>();
            AssetDatabase.CreateAsset(data, SaveFolderPath + fileName + ".asset");
        }
        else
        {
            EditorUtility.SetDirty(data);
        }

        data.nodes.Clear();
        Dictionary<int, NodeInfo> dic = new Dictionary<int, NodeInfo>();
        List<ToolNode> nodes = nodeView.nodes.ToList().Cast<ToolNode>().ToList();

        foreach (var node in nodes)
        {
            if (node.data.toolInfo == null)
            {
                node.data.toolInfo = new ToolInfo();
            }


            NodeInfo info = new NodeInfo()
            {
                toolInfoType = node.data.toolInfo.toolType,
                viewPosition = node.GetPosition().position,
                settingName = node.GetGroupName(),
                id = node.data.id,
            };

            info.toolInfo = Newtonsoft.Json.JsonConvert.SerializeObject(node.data.toolInfo);
            data.nodes.Add(info);
            dic.Add(info.id, info);
        }

        List<Edge> edges = nodeView.edges.ToList();

        if (!edges.Any()) return;

        data.nodeLinks.Clear();

        var connectedSockets = edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedSockets.Count(); i++)
        {
            var outputNode = (connectedSockets[i].output.node as ToolNode);
            var inputNode = (connectedSockets[i].input.node as ToolNode);
            data.nodeLinks.Add(new LinkData
            {
                portName = connectedSockets[i].output.portName,
                targetId = inputNode.data.id,
                baseId = outputNode.data.id,
            });

            dic[outputNode.data.id].outNodes.Add(inputNode.data.id);
            dic[inputNode.data.id].inNodes.Add(outputNode.data.id);
        }

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        AssetDatabase.Refresh();
    }

    void DeleteAll()
    {
        if (nodeView != null)
            nodeView.DeleteAll();
    }

    public void Delete()
    {

    }

    public static void LinkNodesTogether(Port outputSocket, Port inputSocket, NodeView view)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        view.Add(tempEdge);
    }
}