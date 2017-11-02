using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDYXS.TextReplacement
{
    public class PluralizeTextReplacer : TextReplacement
    {
        protected override string[] ParamNames
        {
            get
            {
                return new string[] {
                    "singular",
                    "plural"
                };
            }
        }

        protected override string MethodName {
            get {
                return "pluralize";
            }
        }

        public override string Run(object obj, Dictionary<string, object> parameters)
        {
            int count = GetIntOrCount(obj);

            if (count < 2 && parameters.ContainsKey("singular")) {
                return parameters["singular"].ToString();
            } else if (parameters.ContainsKey("plural")) {
                return parameters["plural"].ToString();
            }
            return "";
        }
    }
}