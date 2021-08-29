using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using BehaviorDesigner.Runtime;

// 代码参考自 https://www.cnblogs.com/chenxizhang/archive/2008/07/09/1238572.html
//            https://docs.microsoft.com/zh-cn/dotnet/framework/reflection-and-codedom/how-to-create-a-class-using-codedom
public class ActionGenerator : MonoBehaviour
{
    public string NameSpaceName = "SnakeBoss";
    public string ActionName = "ShootBullet_B";

    private void Start()
    {
        GenAction();
    }

    public void GenAction()
    {
        //准备一个代码编译器单元
        CodeCompileUnit unit = new CodeCompileUnit();
        //准备必要的命名空间（这个是指要生成的类的空间）
        CodeNamespace sampleNamespace = new CodeNamespace(NameSpaceName);
        //导入必要的命名空间
        sampleNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime.Tasks"));
        //准备要生成的类的定义
        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(ActionName);
        //指定这是一个Class
        Customerclass.IsClass = true;
        Customerclass.TypeAttributes = TypeAttributes.Public;
        Customerclass.BaseTypes.Add("Action");
        //把这个类放在这个命名空间下
        sampleNamespace.Types.Add(Customerclass);
        //把该命名空间加入到编译器单元的命名空间集合中
        unit.Namespaces.Add(sampleNamespace);
        //这是输出文件
        string outputFile = $"GenCodes/{NameSpaceName}/Actions/{ActionName}.cs";
        //添加字段
        CodeMemberField field = new CodeMemberField("SharedTransform", "snakeBossTF");
        field.Attributes = MemberAttributes.Public;
        Customerclass.Members.Add(field);
        //添加属性
        //CodeMemberProperty property = new CodeMemberProperty();
        //property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        //property.Name = "Id";
        //property.HasGet = true;
        //property.HasSet = true;
        //property.Type = new CodeTypeReference(typeof(System.String));
        //property.Comments.Add(new CodeCommentStatement("这是Id属性"));
        //property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id")));
        //property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id"), new CodePropertySetValueReferenceExpression()));
        //Customerclass.Members.Add(property);

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
        returnStatement.Expression =
            new CodeMethodInvokeExpression(
            new CodeTypeReferenceExpression("snakeBossTF.Value.GetComponent<SnakeBoss>()"), $"{ActionName}");
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
            new CodeAttributeArgument(new CodePrimitiveExpression("SnakeBoss")));
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
