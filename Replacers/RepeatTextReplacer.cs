using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDYXS.TextReplacement
{
    public class RepeatTextReplacer : TextReplacement
    {
        protected override string MethodName
        {
            get
            {
                return "repeat";
            }
        }

        protected override string[] ParamNames {
            get {
                return new string[] {
                    "string"
                };
            }
        }

        public override string Run(object obj, Dictionary<string, object> parameters)
        {
            int repeats = GetIntOrCount(obj);

            var ret = "";
            if (parameters.ContainsKey("string") && parameters["string"] is string) {
                var str = parameters["string"] as string;
                for (int i = 0; i < repeats; ++i) {
                    ret += str;
                }
            }
            return ret;
        }
    }
}