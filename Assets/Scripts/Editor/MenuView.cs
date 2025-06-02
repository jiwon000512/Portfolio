using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuView : ScriptableObject
{
    Tool parent;

    public MenuView(Tool parent)
    {
        this.parent = parent;
    }

    public void DoGUI(Rect rect)
    {
        GUILayout.BeginArea(rect);
        OnStateArea();
        GUILayout.EndArea();
    }


    public string fileName;
    void OnStateArea()
    {
        EditorGUILayout.BeginHorizontal();
        {
            fileName = EditorGUILayout.TextField("파일 이름", fileName, GUILayout.Height(20));
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("로드", GUILayout.Height(30)))
            {
                parent.Load(fileName);
            }

            if (GUILayout.Button("저장", GUILayout.Height(30)))
            {
                parent.Save(fileName);
            }

            if (GUILayout.Button("삭제", GUILayout.Height(30)))
            {
                parent.Delete();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
