using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolNode : Node
{
    public ToolData data;
    public int id => data.id;

    public enum DropDownType
    {
        Main = 0,
        Sub1,
        Sub2,
        Sub3,
        Sub4,
        Max,
    }

    Dictionary<DropDownType, ToolNodeElement> dropdownDatas = new Dictionary<DropDownType, ToolNodeElement>();
    ToolTypeSetting settingInfo;

    public Dictionary<string, string> GetGroupName()
    {
        Dictionary<string, string> list = new Dictionary<string, string>();
        foreach (var v in mainContainer.Children())
        {
            if (v is ToolNodeElement)
            {
                var group = v as ToolNodeElement;
                list.Add(group.title, group.currentSelect);
            }
        }

        return list;
    }

    void SetGroupName(Dictionary<string, string> datas)
    {
        foreach (var v in datas)
        {
            var data = dropdownDatas.Values.ToList().Find(x => x.title == v.Key);

            if (data != null)
            {
                data.SelectDropDown(v.Value);
            }
        }
    }

    public Port CreatePort(Direction direction, string portName, Port.Capacity capacity = Port.Capacity.Multi)
    {
        var port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));
        port.portName = portName;

        return port;
    }

    void CreateInput()
    {
        var inputPort = CreatePort(Direction.Input, "Input");
        inputContainer.Add(inputPort);

        var createOutputButton = new Button(() => { CreateOutput(); }) { text = "포트추가" };
        titleButtonContainer.Add(createOutputButton);
    }

    public void CreateOutput()
    {
        var outputPort = CreatePort(Direction.Output, "Output");
        outputContainer.Add(outputPort);
        RefreshExpandedState();
        RefreshPorts();
    }

    public void Load(ToolData data, ToolSaveData.NodeInfo nodeInfo)
    {
        this.data = data;
        Init(nodeInfo.viewPosition);
        if (nodeInfo.settingName != null)
            SetGroupName(nodeInfo.settingName);
    }

    public void Create(Vector2 pos, int id)
    {
        data = new ToolData(id);
        Init(pos);
        CreateOutput();
    }

    public void Init(Vector2 pos)
    {
        SetPosition(new Rect(pos, NodeView.defaultNodeSize));
        this.title = data.id.ToString();
        settingInfo = ToolTypeSetting.GetData();

        for (DropDownType i = DropDownType.Main; i < DropDownType.Max; i++)
            dropdownDatas.Add(i, new ToolNodeElement(this));

        CreateInput();

        CreateMainDropDown();

        RefreshExpandedState();
        RefreshPorts();
    }

    void CreateMainDropDown()
    {
        dropdownDatas[DropDownType.Main].Apply(DropDownType.Main, settingInfo.main.title, settingInfo.main, Dropdown);
        mainContainer.Add(dropdownDatas[DropDownType.Main]);
    }

    void Dropdown(string str, DropDownType type)
    {
        DropDownType next = ++type;
        if (dropdownDatas.ContainsKey(next) == false)
            return;

        if (dropdownDatas[next].Apply(next, str, settingInfo.GetInfo(str), Dropdown))
        {
            mainContainer.Add(dropdownDatas[next]);
        }
    }
}