using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ToolSaveData;

public class ToolData
{
    public int id = 0;
    public ToolSaveData.ToolInfo toolInfo = null;
    public List<int> outNodes = new List<int>();
    public List<int> inNodes = new List<int>();

    public ToolData(int id)
    {
        this.id = id;
    }

    public void CheckToolInfoExists(ToolInfo info)
    {
        if(toolInfo == null || toolInfo.id == 0)
        {
            AddTool(info);
        }
    }

    public ToolInfo AddTool(ToolInfo info)
    {
        this.toolInfo = info;
        this.toolInfo.id = id;

        return toolInfo;
    }

    public void ClearTool()
    {
        toolInfo = null;
    }

    void ConvertToolInfo(System.Type type, string stringToolInfo)
    {
        //if (ToolSaveData.ConvertMethod == null)
        //{
        //    ToolSaveData.ConvertMethod = typeof(Newtonsoft.Json.JsonConvert).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
        //    m.Name == nameof(Newtonsoft.Json.JsonConvert.DeserializeObject) &&
        //    m.IsGenericMethodDefinition &&
        //    m.GetParameters().Length == 1 &&
        //    m.GetParameters()[0].ParameterType == typeof(string)
        //    );
        //}

        var result = Newtonsoft.Json.JsonConvert.DeserializeObject(stringToolInfo);
        toolInfo = result as ToolInfo;
    }

    public void AddData(NodeInfo node)
    {
        ConvertToolInfo(node.toolInfoType, node.toolInfo);
    }
}


public class ToolSaveData : ScriptableObject
{
    //Json <-> Class 변환용 메소드
    public static MethodInfo ConvertMethod;

    public List<NodeInfo> nodes = new List<NodeInfo>();
    public List<LinkData> nodeLinks = new List<LinkData>();

    [Serializable]
    public class ToolInfo
    {
        public int id;
        public virtual System.Type toolType => GetType();
    }

    [Serializable]
    public class NodeInfo : ToolInfo
    {
        public string toolInfo;
        public System.Type toolInfoType;
        public List<int> outNodes = new List<int>();
        public List<int> inNodes = new List<int>();
        public Vector2 viewPosition;
        public Dictionary<string, string> settingName;
    }

    [Serializable]
    public class LinkData
    {
        public string portName;
        public int targetId;
        public int baseId;
    }

    [Serializable]
    public class Create : ToolInfo
    {
        public string path;
        public Vector3Int position;
        public Vector3Int angle;
        public Vector3Int scale;
    }

    [Serializable]
    public class CreateUnit : Create
    {
        public enum UnitType
        {
            Monster,
            Npc,
            Player,
        }
        public UnitType unitType;
        
    }

    [Serializable]
    public class CreateObject : Create
    {
    }

    [Serializable]
    public class Manage : ToolInfo
    {
        public int linkKey;
    }

    [Serializable]
    public class ManageUnit : Manage
    {
        public enum ManageType
        {
            Move,
            Attack,
            Die,
        }
        public ManageType manageType;
    }

    [Serializable]
    public class ManageObject : Manage
    {
        public enum ManageType
        {
            MovePosition,
            Delete,
        }
        public ManageType manageType;
    }
}
