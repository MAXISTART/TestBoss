using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// 代码参考自 https://www.cnblogs.com/chenxizhang/archive/2008/07/09/1238572.html
//            https://docs.microsoft.com/zh-cn/dotnet/framework/reflection-and-codedom/how-to-create-a-class-using-codedom
public class BasicBehaviourGenerator : MonoBehaviour
{


    public GameObject source;                       // 挂载目标脚本的gameObject
    public string targetComponentName;              // 目标脚本的名称
         
    private MonoBehaviour targetComponent;
    private List<MethodInfo> actionMethods;
    private List<MethodInfo> conditionMethods;


    public void Init() {
        // 寻找目标Component
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
        // 寻找所有返回TaskStatus的方法
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
        //准备一个代码编译器单元
        CodeCompileUnit unit = new CodeCompileUnit();
        //准备必要的命名空间（这个是指要生成的类的空间）
        CodeNamespace sampleNamespace = new CodeNamespace(targetComponentName);
        //导入必要的命名空间
        sampleNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime.Tasks"));
        //准备要生成的类的定义
        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(methodInfo.Name);
        //指定这是一个Class
        Customerclass.IsClass = true;
        Customerclass.TypeAttributes = TypeAttributes.Public;
        Customerclass.BaseTypes.Add(baseType);
        //把这个类放在这个命名空间下
        sampleNamespace.Types.Add(Customerclass);
        //把该命名空间加入到编译器单元的命名空间集合中
        unit.Namespaces.Add(sampleNamespace);
        //这是输出文件
        string outputFile = $"GenCodes/{targetComponentName}/{baseType}/{methodInfo.Name}.cs";
        //添加字段
        // 先是一定会有的基础属性
        CodeMemberField field = new CodeMemberField("SharedTransform", $"{targetComponentName.ToLower()}TF");
        field.Attributes = MemberAttributes.Public;
        Customerclass.Members.Add(field);
        // 然后是从参数那里获取的属性
        foreach (ParameterInfo param in methodInfo.GetParameters()) {
            CodeMemberField f = new CodeMemberField(param.ParameterType, param.Name.ToLower());
            f.Attributes = MemberAttributes.Public;
            Customerclass.Members.Add(f);
        }

        //添加方法（使用CodeMemberMethod) 
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


        //添加构造器(使用CodeConstructor) --此处略
        //添加程序入口点（使用CodeEntryPointMethod） --此处略
        //添加事件（使用CodeMemberEvent) --此处略
        //添加特征(使用 CodeAttributeDeclaration)
        CodeAttributeDeclaration codeAttrDecl = new CodeAttributeDeclaration(
            "TaskCategory",
            new CodeAttributeArgument(new CodePrimitiveExpression($"{targetComponentName}")));
        Customerclass.CustomAttributes.Add(codeAttrDecl);
        //生成代码
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
