using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDYXS.TextReplacement
{
    public class ListTextReplacer : TextReplacement
    {
        protected override string[] ParamNames
        {
            get
            {
                return new string[] {
                    "separator",
                    "finalSeparator"
                };
            }
        }

        protected override string MethodName {
            get {
                return "list";
            }
        }

        public override string Run(object obj, Dictionary<string, object> parameters)
        {
            if (obj is ICollection) {
                var ret = "";
                var list = obj as ICollection;
                int i = 0;
                foreach (var o in list) {
                    ret += o.ToString();

                    ++i;
                    if (i < list.Count && parameters.ContainsKey("separator"))
                    {
                        if (i == list.Count - 1 && parameters.ContainsKey("finalSeparator")) {
                            ret += parameters["finalSeparator"].ToString();
                        } else {
                            ret += parameters["separator"].ToString();
                        }
                    }
                }
                return ret;
            }
            return obj.ToString();
        }
    }
}