using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using BehaviorDesigner.Runtime;

// ����ο��� https://www.cnblogs.com/chenxizhang/archive/2008/07/09/1238572.html
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
        //׼��һ�������������Ԫ
        CodeCompileUnit unit = new CodeCompileUnit();
        //׼����Ҫ�������ռ䣨�����ָҪ���ɵ���Ŀռ䣩
        CodeNamespace sampleNamespace = new CodeNamespace(NameSpaceName);
        //�����Ҫ�������ռ�
        sampleNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("BehaviorDesigner.Runtime.Tasks"));
        //׼��Ҫ���ɵ���Ķ���
        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(ActionName);
        //ָ������һ��Class
        Customerclass.IsClass = true;
        Customerclass.TypeAttributes = TypeAttributes.Public;
        Customerclass.BaseTypes.Add("Action");
        //������������������ռ���
        sampleNamespace.Types.Add(Customerclass);
        //�Ѹ������ռ���뵽��������Ԫ�������ռ伯����
        unit.Namespaces.Add(sampleNamespace);
        //��������ļ�
        string outputFile = $"GenCodes/{NameSpaceName}/Actions/{ActionName}.cs";
        //����ֶ�
        CodeMemberField field = new CodeMemberField("SharedTransform", "snakeBossTF");
        field.Attributes = MemberAttributes.Public;
        Customerclass.Members.Add(field);
        //�������
        //CodeMemberProperty property = new CodeMemberProperty();
        //property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        //property.Name = "Id";
        //property.HasGet = true;
        //property.HasSet = true;
        //property.Type = new CodeTypeReference(typeof(System.String));
        //property.Comments.Add(new CodeCommentStatement("����Id����"));
        //property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id")));
        //property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id"), new CodePropertySetValueReferenceExpression()));
        //Customerclass.Members.Add(property);

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
        returnStatement.Expression =
            new CodeMethodInvokeExpression(
            new CodeTypeReferenceExpression("snakeBossTF.Value.GetComponent<SnakeBoss>()"), $"{ActionName}");
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
            new CodeAttributeArgument(new CodePrimitiveExpression("SnakeBoss")));
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
