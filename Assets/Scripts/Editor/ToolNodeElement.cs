using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using static ToolNode;

public class ToolNodeElement : VisualElement
{
    public string title = "";
    public string currentSelect = "";

    Action<string, DropDownType> dropdownCallBack;
    DropDownType dropdownType;

    ToolNode parent;
    ToolTypeSetting.ToolTypeSettingInfo settingInfo;

    IToolEditorCommand command;

    public ToolNodeElement(ToolNode parent) { this.parent = parent; }

    public bool Apply(DropDownType type, string title, ToolTypeSetting.ToolTypeSettingInfo info, Action<string, DropDownType> callBack)
    {
        Clear();

        currentSelect = "";

        if(command != null)
        {
            command.Exit(parent);
            command = null;
        }

        dropdownType = type;
        settingInfo = info;
        dropdownCallBack = callBack;
        this.title = title;

        Play(title);

        if (command != null)
            command.Execute(this, parent);

        return true;
    }

    void Play(string str)
    {
        switch(str)
        {
            case "분류":
                {
                    CreateDropdown();
                }
                break;
        }

        switch(str)
        {
            case "생성":
                {
                    CreateDropdown();
                }
                break;
        }
    }

    void CreateDropdown()
    {
        var dropdown = new DropdownField();
        dropdown.formatSelectedValueCallback = (str) =>
        {
            currentSelect = str;
            dropdownCallBack(str, dropdownType);
            return str;
        };

        dropdown.label = title;
        Add(dropdown);
        dropdown.choices.Clear();
        foreach (var v in settingInfo.setting)
        {
            dropdown.choices.Add(v);
        }
    }

    public void SelectDropDown(string value)
    {

    }
}
