using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolNodeElement : VisualElement
{
    public string title;
    public string currentSelect;

    ToolNode parent;

    public ToolNodeElement(ToolNode parent) { this.parent = parent; }
}
