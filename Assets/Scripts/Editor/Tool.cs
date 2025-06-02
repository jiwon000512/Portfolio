using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public partial class Tool : EditorWindow
{
    [MenuItem("메뉴/툴")]
    public static void OpenEditorWindow()
    {
        if(EditorSceneManager.GetActiveScene().name != "DummyPlayScene")
        {
            EditorSceneManager.OpenScene("Assets/Scenes/DummyPlayScene.unity", OpenSceneMode.Single);
        }
        Open();
    }
}

public partial class Tool : EditorWindow
{
    MenuView menuView;
    NodeView nodeView;

    private void Awake()
    {
        menuView = new MenuView(this);
        nodeView = new NodeView(this);
    }

    static void Open()
    {
        var window = GetWindow<Tool>();
        window.titleContent = new GUIContent("툴");
    }
}