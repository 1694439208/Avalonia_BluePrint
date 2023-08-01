using BluePrint.Core.INode;
using BluePrint.Core.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrint.Node.Common
{
    public static class NodeRegister
    {
        public static Views.BluePrint RegisterCommonNode(this Views.BluePrint bluePrint)
        {
            bluePrint.RegisterNode(new List<Type>()
            {
                typeof(_StartNode),
                typeof(Branch),
                typeof(ImageShow),
                typeof(ImageSplit),
                typeof(sequence),
                typeof(ThanGreater),
                typeof(ScriptType),
                typeof(CreateVar),
                typeof(GetVar),
                typeof(Annotation),
                typeof(ThanEqual),
                typeof(ThanLess),
                typeof(ThanStrEqual),

                //-----------
                typeof(Sharp_StartNode),
                //Quicker
                typeof(Sharp_GetVar),
                typeof(Sharp_SetVar),
                //列表功能
                typeof(Sharp_List_Length),
                typeof(Sharp_Str_ListJoin),
                typeof(Sharp_where),
                typeof(Sharp_select),
                typeof(Sharp_Any),
                typeof(Sharp_GetStarValue),
                typeof(Sharp_FindIndex),
                typeof(Sharp_GetEnd),
                typeof(Sharp_OrderBy),
                typeof(Sharp_OrderByDescending),
                //流程
                typeof(Sharp_sequence),
                //其他
                typeof(Print),
                //逻辑运算
                typeof(Sharp_and),
                typeof(Sharp_or),
                //数据转换
                typeof(Sharp_ToEval),
                typeof(Sharp_ToJson),
                //表达式(条件)
                typeof(Sharp_TextExpression),
                typeof(Sharp_Str_Equal),
                typeof(Sharp_Str_IndexOf),
                typeof(Sharp_Str_StartsWith),
                typeof(Sharp_Str_EndsWith),
                //字符串处理
                typeof(Sharp_StrAppend),
                typeof(Sharp_Str_Replace),
                typeof(Sharp_Str_Split),
                typeof(Sharp_Str_Length),
            });
            return bluePrint;
        }
    }
}
