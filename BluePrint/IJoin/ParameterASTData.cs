using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.IJoin
{
    public class ParameterAST
    {
        /// <summary>
        /// 取ast代码生成
        /// </summary>
        /// <param name="IsD">是否添加var申明</param>
        /// <returns></returns>
        public string GetUid(bool IsD = true)
        {
            if (IsThis)
            {
                return Join.Get()?.Value?.ToString() ?? "";
            }
            if (IsExpression)
            {
                return CodeTemplate??"";
            }
            else
            {
                return ID.GetID(IsD);
            }
        }
        /// <summary>
        /// 取ast代码生成批量，如果不确定他是变量还是内容 就调用这个，统一处理
        /// </summary>
        /// <param name="IsD">是否添加var申明</param>
        /// <returns></returns>
        public string GetUidALL(bool IsD = true)
        {
            if (IsThis)
            {
                return $"\"{Join.Get()?.Value?.ToString() ?? ""}\"";
            }
            if (IsExpression)
            {
                return CodeTemplate ?? "";
            }
            else
            {
                return ID.GetID(IsD);
            }
        }
        /// <summary>
        /// 接口id用来生成变量名
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 接口对象
        /// </summary>
        public Node.IJoinControl Join { set; get; }
        /// <summary>
        /// 表达式模板代码
        /// </summary>
        public string CodeTemplate { set; get; }
        /// <summary>
        /// 是否为表达式
        /// </summary>
        public bool IsExpression { set; get; }
        /// <summary>
        /// 当前接口指针是否是自己
        /// </summary>
        public bool IsThis { set; get; }
        /// <summary>
        /// 读取接口数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public string GetData<TResult>(Func<Node_Interface_Data, TResult> func) {
            if (IsThis)
            {
                return func(Join.Get()).ToString();
            }
            else {
                return ID.GetID();
            }
        }
    }
}
