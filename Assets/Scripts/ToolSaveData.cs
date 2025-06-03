using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeData
{
    public int id;
    public Vector2 position;
    public string data;
}


public class ToolSaveData : ScriptableObject
{
    public List<NodeData> nodes = new List<NodeData>();
}
