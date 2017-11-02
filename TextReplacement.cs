using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace PDYXS.TextReplacement
{
    public abstract class TextReplacement
    {
        protected static List<TextReplacement> replacements = new List<TextReplacement>();

        private static void AddAllReplacers() {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes()) {
                if (typeof(TextReplacement).IsAssignableFrom(type) && 
                    type != typeof(TextReplacement)) 
                {
                    var replacer = System.Activator.CreateInstance(type) as TextReplacement;
                    replacements.Add(replacer);
                }
            }
        }

        public static void Initialise()
        {
            if (replacements.Count > 0) {
                replacements.Clear();
            }

            AddAllReplacers();

            isInitialised = true;
        }

        private static bool isInitialised = false;

        protected static IReplacementContext Context;

        public static string Replace(string str, IReplacementContext context) {
            if (!isInitialised)
            {
                Initialise();
            }

            Context = context;

            str = regex.Replace(str, replaceMatch);

            return str;
        }

        private static object StringOrContext(string name) {
            name = name.Trim();
            if (name[name.Length - 1] == ',') {
                name = name.Substring(0, name.Length - 1).Trim();
            }
            if (name[0] == '\'' && name[name.Length - 1] == '\'') {
                return name.Substring(1, name.Length - 2);
            } else {
                return Context.getContext(name);
            }
        }

        protected static string replaceMatch(Match m)
        {
            var obj = StringOrContext(m.Groups["object"].Value);

            if (m.Groups["method"] != null) {
                string method = m.Groups["method"].Value.Trim();

                List<object> parameters = new List<object>();
                if (m.Groups["params"] != null) {
                    foreach (Match match in paramRegex.Matches(m.Groups["params"].Value)) {
                        parameters.Add(StringOrContext(match.Groups[match.Groups.Count - 1].Value));
                    }
                }

                foreach (var replacement in replacements) {
                    if (replacement.MethodName == method) {
                        return Replace(replacement.Run(obj, parameters.ToArray()), Context);
                    }
                }
            }

            return obj.ToString();
        }

        private static string regexStr = 
            "{{\\s*(?'object'[a-zA-Z]+)\\s*(\\|\\s*(?'method'[a-zA-Z]+)(\\s*:\\s*(?'params'[a-zA-Z', ]+?)\\s*)?)?}}";
        private static string paramRegexStr = "('.+?'|.+?)(?:,|$)";

        private static Regex regex = new Regex(regexStr);
        private static Regex paramRegex = new Regex(paramRegexStr);

        protected abstract string[] ParamNames {
            get;
        }

        protected abstract string MethodName {
            get;
        }

        private string Run(object obj, object[] parameters) {
            var parms = new Dictionary<string, object>();
            for (int i = 0; i < parameters.Length && i < ParamNames.Length; ++i) {
                parms[ParamNames[i]] = parameters[i];
            }
            return Run(obj, parms);
        }
        public abstract string Run(object obj, Dictionary<string, object> parameters);

        protected int GetIntOrCount(object obj) {
            int ret = 0;
            if (obj is int)
            {
                ret = (int)obj;
            }
            else if (obj is ICollection)
            {
                ret = ((ICollection)obj).Count;
            }
            return ret;
        }
    }
}