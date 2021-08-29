using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicBehaviourGenerator))]
public class BasicBehaviourGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // target ��Editor����Դ���
        BasicBehaviourGenerator gen = (BasicBehaviourGenerator)target;
        if (GUILayout.Button("Gen Init"))
        {
            gen.Init();
        }
        if (GUILayout.Button("Gen Actions")) {
            gen.GenActions();
        }
        if (GUILayout.Button("Gen Conditions"))
        {
            gen.GenConditions();
        }
    }
}
