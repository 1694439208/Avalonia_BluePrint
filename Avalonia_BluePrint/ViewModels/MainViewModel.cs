using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;

namespace Avalonia_BluePrint.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
    }
    public static class Parser
    {
        public static Dictionary<string, Type> NodeTypes = new Dictionary<string, Type>();
        public static void Init()
        {
            //Debug.Print("注册接头对象");
            //var lays = File.ReadAllText("layers.txt").Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //foreach (var item in lays)
            //{
            //    string[] ts = item.Split(',');
            //    if (ts.Length == 2)
            //    {
            //        switch (ts[1])
            //        {
            //            case "input":
            //                NodeTypes.Add(ts[0], typeof(Node_input));
            //                break;
            //            case "output":
            //                NodeTypes.Add(ts[0], typeof(Node_output));
            //                break;
            //            case "all":
            //                NodeTypes.Add(ts[0], typeof(Node_quan));
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            //NodeTypes.Add("[net]", typeof(Node_output));
            //NodeTypes.Add("[convolutional]", typeof(Node_quan));
            //NodeTypes.Add("[maxpool]", typeof(Node_quan));
            //NodeTypes.Add("[yolo]", typeof(Node_input));
            //NodeTypes.Add("[route]", typeof(Node_quan));
            //NodeTypes.Add("[upsample]", typeof(Node_quan));
        }
        //public static List<Type> CreateLayer(List<KeyValuePair<string, Dictionary<string, string>>> keyValues)
        //{
        //    List<Type> list = new List<Type>();
        //    int index = 0;
        //    foreach (var item in keyValues)
        //    {
        //        switch (item.Key)
        //        {
        //            case "[net]":
        //                list.Add(typeof(Node_output));
        //                break;
        //            case "[yolo]":
        //                list.Add(typeof(Node_input));
        //                break;
        //            default:
        //                list.Add(typeof(Node_quan));
        //                break;
        //        }
        //        index++;
        //    }
        //    return list;
        //}
        public static List<KeyValuePair<string, Dictionary<string, string>>> parse_network_cfg(string data)
        {
            var dat = data;// File.ReadAllText();
            string[] lines = dat.Split(new string[] { "\n" }, StringSplitOptions.None);
            int nu = 0;
            List<KeyValuePair<string, Dictionary<string, string>>> list = new List<KeyValuePair<string, Dictionary<string, string>>>();
            string name = string.Empty;
            Dictionary<string, string> ky1 = new Dictionary<string, string>();// new KeyValuePair<string, Dictionary<string, string>>();
            foreach (var item in lines)
            {
                ++nu;
                var line = item.Trim();
                var kky = new Dictionary<string, string>();
                if (line.Length > 0)
                {
                    switch (line[0])
                    {
                        case '[':
                            ky1 = new Dictionary<string, string>();
                            list.Add(new KeyValuePair<string, Dictionary<string, string>>(line, ky1));
                            break;
                        case '\0':
                        case '#':
                        case ';':
                            break;
                        default:
                            var a = line.Trim().Split('=');
                            ky1.Add(a[0].Trim(), a[1].Trim());

                            //if (!read_option(line, current->options))
                            //{
                            //    //fprintf(stderr, "Config file error line %d, could parse: %s\n", nu, line);
                            //    //free(line);
                            //}
                            break;
                    }
                }
            }
            return list;
        }
    }
}