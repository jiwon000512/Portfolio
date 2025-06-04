using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ToolSaveData;

public interface IToolEditorCommand
{
    public abstract void Execute(ToolNodeElement element, ToolNode node);
    public abstract void Exit(ToolNode node);
}

public class ToolEditorCommand<T> : IToolEditorCommand where T : ToolInfo, new()
{
    protected T tool;

    public void Execute(ToolNodeElement element, ToolNode node)
    {
        node.data.CheckToolInfoExists(new T());
        tool = node.data.toolInfo as T;
    }

    public void Exit(ToolNode node)
    {
        node.data.ClearTool();
    }
}
