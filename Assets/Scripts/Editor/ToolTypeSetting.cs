using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolTypeSetting", menuName = "스크립트/ToolTypeSetting", order = int.MaxValue)]
public class ToolTypeSetting : ScriptableObject
{
    [Serializable]
    public class ToolTypeSettingInfo
    {
        public string title;
        public List<string> setting = new List<string>();
    }

    public ToolTypeSettingInfo main = new ToolTypeSettingInfo();
    public List<ToolTypeSettingInfo> infos = new List<ToolTypeSettingInfo>();

    public ToolTypeSettingInfo GetInfo(string title)
    {
        return infos.Find(x=> x.title == title);
    }

    public static ToolTypeSetting GetData()
    {
        return AssetDatabase.LoadAssetAtPath("Assets/EditorData/ToolTypeSetting.asset", typeof(ToolTypeSettingInfo)) as ToolTypeSetting;
    }
}
