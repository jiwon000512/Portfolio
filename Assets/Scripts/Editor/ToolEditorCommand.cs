using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static ToolSaveData;

public interface IToolEditorCommand
{
    public abstract void Execute(ToolNodeElement element, ToolNode node);
    public abstract void Exit(ToolNode node);
}

public class ToolEditorCommand<T> : IToolEditorCommand where T : ToolInfo, new()
{
    protected T tool;

    public virtual void Execute(ToolNodeElement element, ToolNode node)
    {
        node.data.CheckToolInfoExists(new T());
        tool = node.data.toolInfo as T;
    }

    public void Exit(ToolNode node)
    {
        node.data.ClearTool();
    }

    public IntegerField CreateIntField(ToolNodeElement element, string fieldName, int value, Action<int> callBack)
    {
        var field = new IntegerField(fieldName);
        field.RegisterValueChangedCallback(evt => { callBack?.Invoke(field.value); });
        field.value = value;
        element.Add(field);
        return field;
    }

    public TextField CreateTextField(ToolNodeElement element, string fieldName, string value, Action<string> callBack)
    {
        var field = new TextField(fieldName);
        field.RegisterValueChangedCallback(evt => { callBack(field.value); });
        field.value = value;
        element.Add(field);
        return field;
    }

    public DropdownField CreateDropdownField(ToolNodeElement element, string fieldName, List<string> choices, int valueIndex, Action<string> callBack)
    {
        var field = new DropdownField(fieldName, choices, 0);
        field.RegisterValueChangedCallback(evt => { callBack(field.value); });
        field.index = valueIndex;
        element.Add(field);
        return field;
    }
}

public class ToolEditorCommand_CreateUnit : ToolEditorCommand<CreateUnit>
{
    public override void Execute(ToolNodeElement element, ToolNode node)
    {
        base.Execute(element, node);


    }
}

public class ToolEditorCommand_CreateObjecct : ToolEditorCommand<CreateObject>
{
    public override void Execute(ToolNodeElement element, ToolNode node)
    {
        base.Execute(element, node);

        CreateTextField(element, "파일경로", tool.path, (v) => { tool.path = v; });
    }
}