using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;

namespace 蓝图重制版.BluePrint.Runtime
{
    public class Evaluate
    {
        public static void Eval(NodeAst nodeAst,object GlobalContext) {
            Context.Clear();
            Calculated(nodeAst, GlobalContext);
        }
        public static Dictionary<int, object> Context = new Dictionary<int, object>();
        private static void Calculated(NodeAst nodeAst, object GlobalContext) {
            switch (nodeAst.NodeToken)
            {
                case Token.NodeToken.Expression:
                    //表达式就执行
                    foreach (var item in nodeAst.PrevNodes)
                    {
                        Calculated(item, GlobalContext);
                    }
                    //Calculated();

                    break;
                case Token.NodeToken.Call:
                    List<object> args = new List<object>();
                    foreach (var item in nodeAst.Arguments)
                    {
                        Calculated(item, GlobalContext);
                        if (Context.TryGetValue(item.NodeJoinId,out var value))
                        {
                            args.Add(value);
                        }
                    }
                    Result result = new Result(nodeAst.NextNodes.Count,nodeAst.Results);
                    nodeAst.NodeBase.Execute(GlobalContext,args, result);
                    //执行完毕把返回数据放在数据上下文
                    for (int i = 0; i < nodeAst.Results.Count; i++)
                    {
                        var uid = nodeAst.Results[i].NodeJoinId;
                        if (Context.ContainsKey(uid))
                        {
                            Context[uid] = result.GetReturnValue(i) ;
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
                                Calculated(nodeAst.NextNodes[item], GlobalContext);
                            }
                        }
                        
                    }
                    break;
                case Token.NodeToken.Value:
                    if (Context.ContainsKey(nodeAst.NodeJoinId))
                    {
                        Context[nodeAst.NodeJoinId] = nodeAst.Join.Get().Value;
                    }
                    else {
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
        public static void test(List<object> arguments,in Result result) {
            result.SetReturnValue(0, 666);
            result.SetReturnValue(1, new string[] {"1","2","3"});
            if (arguments.Get<bool>(0) == true)
            {
                result.SetExecute(0);
            }
            else {
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
        public class Result {
            public Result(int NextNodes_size,List<NodeAst> Result) {
                _NextNode_Size = NextNodes_size;
                //System.Diagnostics.Debug.WriteLine(ResultSize);
                for (int i = 0; i < Result.Count; i++)
                {
                    Results.Add(i, Result[i].Join.Get().Value);
                }
            }
            public int _NextNode_Size = 0;
            public Dictionary<int, object> Results = new Dictionary<int, object>();
            public List<int> Nexts = new List<int>();
            /// <summary>
            /// 设置生成代码模板
            /// </summary>
            /// <param name="code"></param>
            public void SetCodeTemplate(string code) { 
            
            }
            /// <summary>
            /// 设置指定层参数的返回值
            /// </summary>
            /// <param name="index">索引</param>
            /// <param name="value">返回值</param>
            public void SetReturnValue(int index,object value)
            {
                if (Results.ContainsKey(index))
                {
                    Results[index] = value;
                }
                else {
                    Results.Add(index, value);
                }
                
            }
            /// <summary>
            /// 读取指定层参数的返回值
            /// </summary>
            /// <param name="index">索引</param>
            /// <param name="value">返回值</param>
            public object GetReturnValue(int index)
            {
                return Results[index];
            }
            /// <summary>
            /// 读取所有返回值对象
            /// </summary>
            /// <returns></returns>
            public Dictionary<int, object> GetReturns() {
                return Results;
            }
            /// <summary>
            /// 获取下一个执行数量
            /// </summary>
            /// <returns></returns>
            public int GetNextNodeSize()
            {
                return _NextNode_Size;
            }
            
            /// <summary>
            /// 执行流程索引
            /// </summary>
            /// <param name="index"></param>
            public void SetExecute(int index) {
                Nexts.Add(index);
            }
            /// <summary>
            /// 读取执行流程索引
            /// </summary>
            /// <param name="index"></param>
            public List<int> GetExecute()
            {
                return Nexts;
            }
        }
    }
}
