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

    DropdownField currentDropdown;
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

        if (command != null)
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
        switch (str)
        {
            case "분류":
                {
                    CreateDropdown();
                }
                break;
        }

        switch (str)
        {
            case "생성":
                {
                    CreateDropdown();
                }
                break;
            case "유닛생성":
                {
                    command = new ToolEditorCommand_CreateUnit();
                }
                break;
            case "오브젝트생성":
                {
                    command = new ToolEditorCommand_CreateObjecct();
                }
                break;
        }
    }

    void CreateDropdown()
    {
        currentDropdown = new DropdownField(title);
        currentDropdown.formatSelectedValueCallback = (str) =>
        {
            currentSelect = str;
            dropdownCallBack(str, dropdownType);
            return str;
        };

        Add(currentDropdown);
        currentDropdown.choices.Clear();
        foreach (var v in settingInfo.setting)
        {
            currentDropdown.choices.Add(v);
        }
    }

    public void SelectDropDown(string value)
    {
        dropdownCallBack(value, dropdownType);
        if(currentDropdown != null)
        {
            currentDropdown.value = value;
        }
    }
}
