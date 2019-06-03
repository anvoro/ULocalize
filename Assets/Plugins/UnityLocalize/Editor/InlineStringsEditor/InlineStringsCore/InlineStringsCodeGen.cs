using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal static class InlineStringsCodeGen
    {
        public static void Generate(InlineStringsSet inlineStringsSet)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("UnityLocalize");
            CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration("InlineStrings")
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Public
            };

            codeNamespace.Types.Add(typeDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            foreach (StoredString inlineString in inlineStringsSet.InlineStrings)
            {
                CodeMemberField field = new CodeMemberField
                {
                    Name = "_" + inlineString.Id,
                    Type = new CodeTypeReference(typeof(LocalizableString)),
                    Attributes = MemberAttributes.Private | MemberAttributes.Static,
                    InitExpression = new CodeObjectCreateExpression(typeof(LocalizableString))
                    {
                        Parameters =
                        {
                            new CodePrimitiveExpression(inlineString.Id),
                            new CodeArrayCreateExpression(typeof(string[]), inlineString.Strings.Select(s => new CodePrimitiveExpression(s)).ToArray())
                        }
                    }
                };

                CodeMemberProperty property = new CodeMemberProperty
                {
                    Name = inlineString.Id,
                    Type = new CodeTypeReference(typeof(LocalizableString)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Static
                };

                property.Comments.Add(new CodeCommentStatement("<summary>", true));
                property.Comments.Add(new CodeCommentStatement(inlineString.Strings[0], true));
                property.Comments.Add(new CodeCommentStatement("</summary>", true));
                property.GetStatements.Add(new CodeMethodReturnStatement(
                                                new CodeFieldReferenceExpression(
                                                    new CodeTypeReferenceExpression(typeof(InlineStrings)), field.Name)));

                typeDeclaration.Members.Add(field);
                typeDeclaration.Members.Add(property);
            }

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions { BracingStyle = "C" };
            using (StreamWriter sourceWriter =
                new StreamWriter(Application.dataPath + "/Plugins/UnityLocalize/InlineStrings/InlineStrings.generated.cs"))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, options);
            }

            EditorUtility.DisplayDialog("Genaration complete", "Strings constants file generated!", "OK");
        }
    }
}
