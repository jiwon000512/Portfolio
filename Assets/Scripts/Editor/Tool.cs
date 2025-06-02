using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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
        nodeView = new NodeView(this);
        menuView = new MenuView(this);

        IStyle style = nodeView.style;
        style.position = Position.Absolute;
        style.left = 0f; style.top = 0f; style.right = 300f; style.bottom = 0f;

        rootVisualElement.Add(nodeView);
    }

    void OnGUI()
    {
        menuView.DoGUI(new Rect(nodeView.layout.width + 10, nodeView.layout.y + 30, 290, nodeView.layout.height - 30));
    }

    public static void Open()
    {
        var window = GetWindow<Tool>();
        window.titleContent = new GUIContent("툴");
    }

    public void Load(string fileName)
    {

    }

    public void Save(string fileName)
    {

    }

    public void Delete()
    {

    }
}