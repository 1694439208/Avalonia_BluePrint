using BluePrint.Core.IJoin;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.Runtime
{
    public class CodeGenerator
    {
        public static string Generator(NodeAst nodeAst) {
            return Calculated(nodeAst);
        }
        private static string Calculated(NodeAst nodeAst)
        {
            var ret = "";
            if (nodeAst == null)
            {
                return "";
            }
            switch (nodeAst.NodeToken)
            {
                case Token.NodeToken.ExpressionValue:
                case Token.NodeToken.Expression:
                    List<string> PrevNodes1 = new List<string>();
                    foreach (var item in nodeAst.PrevNodes)
                    {
                        PrevNodes1.Add(Calculated(item));
                    }
                    ret = string.Join("", PrevNodes1);
                    break;
                case Token.NodeToken.Call:
                    List<string> NextNodesCode = new List<string>();
                    foreach (var item in nodeAst.NextNodes)
                    {
                        NextNodesCode.Add(Calculated(item));
                    }
                    List<ParameterAST> Arguments = new List<ParameterAST>();
                    foreach (var item in nodeAst.Arguments)
                    {
                        Arguments.Add(new ParameterAST {
                            ID = item.NodeJoinId, 
                            Join = item.Join,
                            IsThis = item.Isthis,
                        });
                    }
                    List<ParameterAST> Results = new List<ParameterAST>();
                    foreach (var item in nodeAst.Results)
                    {
                        Results.Add(new ParameterAST
                        {
                            ID = item.NodeJoinId,
                            Join = item.Join,
                            IsThis = item.Isthis,
                        });
                    }

                    List<string> PrevNodes = new List<string>();
                    for (int i = 0; i < nodeAst.Arguments.Count; i++)
                    {
                        var item = nodeAst.Arguments[i];
                        var Value = Calculated(item);
                        if (item.NodeToken == Token.NodeToken.ExpressionValue)
                        {
                            Arguments[i].CodeTemplate = Value;
                            Arguments[i].IsExpression = true;
                        }
                        else
                        {
                            PrevNodes.Add(Value);
                            Arguments[i].IsExpression = false;
                        }
                    }
                    ret = nodeAst.NodeBase.CodeTemplate(NextNodesCode, PrevNodes, Arguments, Results);
                    break;
                case Token.NodeToken.Value:
                    break;
                case Token.NodeToken.None:
                    break;
                default:
                    break;
            }
            
            
            return ret;
        }
    }
}
