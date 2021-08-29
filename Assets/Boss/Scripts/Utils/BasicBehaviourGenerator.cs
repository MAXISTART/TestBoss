using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// ����ο��� https://www.cnblogs.com/chenxizhang/archive/2008/07/09/1238572.html
//            https://docs.microsoft.com/zh-cn/dotnet/framework/reflection-and-codedom/how-to-create-a-class-using-codedom
public class BasicBehaviourGenerator : MonoBehaviour
{


    public GameObject source;                       // ����Ŀ��ű���gameObject
    public string targetComponentName;              // Ŀ��ű�������
         
    private MonoBehaviour targetComponent;
    private List<MethodInfo> actionMethods;
    private List<MethodInfo> conditionMethods;


    public void Init() {
        // Ѱ��Ŀ��Component
        targetComponent = source.GetComponent(targetComponentName) as MonoBehaviour;
        if (targetComponent == null)
        {
            Debug.LogError($"Component <{targetComponentName}> is not at source gameObject!!");
            return;
        }
        if (actionMethods == null) actionMethods = new List<MethodInfo>();
        if (conditionMethods == null) conditionMethods = new List<MethodInfo>();
        if (actionMethods.Count > 0) actionMethods.Clear();
        if (conditionMethods.Count > 0) conditionMethods.Clear();
        FindTasks();
    }

    public void GenActions() {
        foreach (var methodInfo in actionMethods) {
            Gen("Action", methodInfo);
        }
    }

    public void GenConditions()
    {
        Init();
        foreach (var methodInfo in conditionMethods)
        {
            Gen("Conditional", methodInfo);
        }
    }


    public void FindTasks() {
        // Ѱ�����з���TaskStatus�ķ���
        MethodInfo[] methods = targetComponent.GetType().GetMethods();
        foreach (var m in methods) {
            if (m.ReturnType == typeof(TaskStatus)) {
                if (m.Name.StartsWith("Condition_"))
                {
                    conditionMethods.Add(m);
                }
                else {
                    actionMethods.Add(m);
                }
            }
        }
    }


    public void Gen(string baseType, MethodInfo methodInfo)
    {
        //׼��һ�������������Ԫ
        CodeCompileUnit unit = new CodeCompileUnit();
        //׼����Ҫ�������ռ䣨�����ָҪ���ɵ���Ŀռ䣩
        CodeNamespace sampleNamespace = new CodeNamespace(targetComponentName);
        //�����Ҫ�������ռ�
        sampleNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime.Tasks"));
        //׼��Ҫ���ɵ���Ķ���
        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(methodInfo.Name);
        //ָ������һ��Class
        Customerclass.IsClass = true;
        Customerclass.TypeAttributes = TypeAttributes.Public;
        Customerclass.BaseTypes.Add(baseType);
        //������������������ռ���
        sampleNamespace.Types.Add(Customerclass);
        //�Ѹ������ռ���뵽��������Ԫ�������ռ伯����
        unit.Namespaces.Add(sampleNamespace);
        //��������ļ�
        string outputFile = $"GenCodes/{targetComponentName}/{baseType}/{methodInfo.Name}.cs";
        //����ֶ�
        // ����һ�����еĻ�������
        CodeMemberField field = new CodeMemberField("SharedTransform", $"{targetComponentName.ToLower()}TF");
        field.Attributes = MemberAttributes.Public;
        Customerclass.Members.Add(field);
        // Ȼ���ǴӲ��������ȡ������
        foreach (ParameterInfo param in methodInfo.GetParameters()) {
            CodeMemberField f = new CodeMemberField(param.ParameterType, param.Name.ToLower());
            f.Attributes = MemberAttributes.Public;
            Customerclass.Members.Add(f);
        }

        //��ӷ�����ʹ��CodeMemberMethod) 
        CodeMemberMethod method1 = new CodeMemberMethod();
        method1.Name = "OnAwake";
        method1.ReturnType = new CodeTypeReference();
        method1.Attributes = MemberAttributes.Override | MemberAttributes.Public;
        Customerclass.Members.Add(method1);


        CodeMemberMethod method2 = new CodeMemberMethod();
        method2.Name = "OnUpdate";
        method2.ReturnType = new CodeTypeReference("TaskStatus");
        method2.Attributes = MemberAttributes.Override | MemberAttributes.Public;
        CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement();
        var mps = methodInfo.GetParameters();
        CodeFieldReferenceExpression[] cfps = new CodeFieldReferenceExpression[mps.Length];
        for (int i= 0;i<cfps.Length;i++) {
            cfps[i] = new CodeFieldReferenceExpression();
            cfps[i].FieldName = mps[i].Name;
        }
        returnStatement.Expression =
            new CodeMethodInvokeExpression(
            new CodeTypeReferenceExpression($"{targetComponentName.ToLower()}TF.Value.GetComponent<{targetComponentName}> ()"), methodInfo.Name, cfps);
        method2.Statements.Add(returnStatement);
        //method2.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "text"));
        //method2.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("text")));
        Customerclass.Members.Add(method2);


        //��ӹ�����(ʹ��CodeConstructor) --�˴���
        //��ӳ�����ڵ㣨ʹ��CodeEntryPointMethod�� --�˴���
        //����¼���ʹ��CodeMemberEvent) --�˴���
        //�������(ʹ�� CodeAttributeDeclaration)
        CodeAttributeDeclaration codeAttrDecl = new CodeAttributeDeclaration(
            "TaskCategory",
            new CodeAttributeArgument(new CodePrimitiveExpression($"{targetComponentName}")));
        Customerclass.CustomAttributes.Add(codeAttrDecl);
        //���ɴ���
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        options.BlankLinesBetweenMembers = true;
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
        {
            Debug.Log($"outputFile:{outputFile}");
            provider.GenerateCodeFromCompileUnit(unit, sw, options);
        }
    }
}
