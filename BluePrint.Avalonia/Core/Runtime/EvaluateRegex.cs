using BluePrint.Core.IJoin;
using BluePrint.Core.INode;
using BluePrint.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BluePrint.Core.Runtime.Evaluate;

namespace easyData.Runtime
{
    public class EvaluateRegex
    {
        public static async Task Eval(NodeAst nodeAst, object GlobalContext)
        {
            Context.Clear();
            await Calculated(nodeAst, GlobalContext);
        }
        public static Dictionary<int, object> Context = new Dictionary<int, object>();
        private static async Task Calculated(NodeAst nodeAst, object GlobalContext)
        {
            switch (nodeAst.NodeToken)
            {
                case Token.NodeToken.ExpressionValue:
                case Token.NodeToken.Expression:
                    //表达式就执行 
                    foreach (var item in nodeAst.PrevNodes)
                    {
                        await Calculated(item, GlobalContext);
                    }
                    //Calculated();

                    break;
                case Token.NodeToken.Call:
                    List<object> args = new List<object>();
                    foreach (var item in nodeAst.Arguments)
                    {
                        await Calculated(item, GlobalContext);
                        if (Context.TryGetValue(item.NodeJoinId, out var value))
                        {
                            args.Add(value);
                        }
                    }
                    Result result = new Result(nodeAst.NextNodes.Count, nodeAst.Results);
                    try
                    {
                        nodeAst.NodeBase.SetState(NodeState.Running);
                        await nodeAst.NodeBase.Execute(GlobalContext, args, result);
                        nodeAst.NodeBase.SetState(NodeState.OK);
                    }
                    catch (Exception ex)
                    {
                        nodeAst.NodeBase.SetState(NodeState.Error, ex.Message);
                    }
                    //执行完毕把返回数据放在数据上下文
                    for (int i = 0; i < nodeAst.Results.Count; i++)
                    {
                        var uid = nodeAst.Results[i].NodeJoinId;
                        if (Context.ContainsKey(uid))
                        {
                            Context[uid] = result.GetReturnValue(i);
                        }
                        else
                        {
                            Context.Add(uid, result.GetReturnValue(i));
                        }
                    }
                    //然后再根据返回的执行
                    foreach (var item in result.GetExecute())
                    {
                        if (nodeAst.NextNodes.Count > item)
                        {
                            if (nodeAst.NextNodes[item] != null)
                            {
                                await Calculated(nodeAst.NextNodes[item], GlobalContext);
                            }
                        }

                    }
                    break;
                case Token.NodeToken.Value:
                    if (Context.ContainsKey(nodeAst.NodeJoinId))
                    {
                        Context[nodeAst.NodeJoinId] = nodeAst.Join.Get().Value;
                    }
                    else
                    {
                        Context.Add(nodeAst.NodeJoinId, nodeAst.Join.Get().Value);
                    }
                    break;
                case Token.NodeToken.ObjectValue:
                    if (Context.ContainsKey(nodeAst.NodeJoinId))
                    {
                        Context[nodeAst.NodeJoinId] = nodeAst.Value;
                    }
                    else
                    {
                        Context.Add(nodeAst.NodeJoinId, nodeAst.Value);
                    }
                    break;
                case Token.NodeToken.None:
                    break;
                default:
                    break;
            }
        }
        public static void test(List<object> arguments, in Result result)
        {
            result.SetReturnValue(0, 666);
            result.SetReturnValue(1, new string[] { "1", "2", "3" });
            if (arguments.Get<bool>(0) == true)
            {
                result.SetExecute(0);
            }
            else
            {
                result.SetExecute(0);
            }
            result.SetCodeTemplate(@"
                if({1}){
                    {2}
                }else{
                    {3}
                }
            ");
        }
        
    }
}
