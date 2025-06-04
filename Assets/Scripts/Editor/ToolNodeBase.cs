using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolNodeBase : Node
{
    public ToolData data;
    public int id => data.id;

    public virtual Dictionary<string, string> GetGroupName()
    {
        return new Dictionary<string, string>();
    }
}

public class ToolNode : ToolNodeBase
{
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

    public override Dictionary<string, string> GetGroupName()
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
            //var data = DropdownDatas.(k => k.Value.Title == v.Key);

            //if (data.Value != null)
            //{
            //    data.Value.SelectDropDown(v.Value);
            //}
        }
    }

    void CreateInput()
    {
        var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);

        var createOutputButton = new Button(() => { CreateOutput(); })
        {
            text = "포트추가"
        };
        titleButtonContainer.Add(createOutputButton);
    }

    public void CreateOutput()
    {
        var generatedPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        generatedPort.portName = "Next";
        outputContainer.Add(generatedPort);
        RefreshExpandedState();
        RefreshPorts();
    }

    public void Load(ToolData data)
    {
        this.data = data;
    }

    public void Create(Vector2 pos)
    {
        CreateOutput();
        Init(pos);
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
        //DropdownDatas[DropDownType.Main].Apply(DropDownType.Main, SettingInfo.Main.Title, SettingInfo.Main, Dropdown);
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
