using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
	public class FormatProvider
	{
		protected Dictionary<String, String> aliases;
		protected List<System.Reflection.MethodInfo> methods;
        protected List<Enum> enums;
        protected List<Dictionary<string, string>> enumLocaleInfo;

        public FormatProvider(Dictionary<string, object> node)
        {
        	aliases = new Dictionary<string, string>();
            enums = new List<Enum>();
            enumLocaleInfo = new List<Dictionary<string, string>>();
            methods = new List<System.Reflection.MethodInfo>();
            // 你又想起某个夏天，热闹海岸线
            //   还是无法忘记沙滩上的印记
            //     你在窗边 满是大海的旋律
            // 不。

            var aliasesa = node["Aliases"] as System.Xml.XmlNodeList;
            foreach (System.Xml.XmlNode alias in aliasesa)
            {
                if (alias.Name != "Alias") continue;

                aliases.Add(
                    alias.Attributes["ID"].Value,
                	alias.FirstChild.Value
                );
            }
        }

        public string ParseFormat(string format, List<object> parameters)
        {
            if (format.IndexOf("{") < 0)
                return format;

            if (parameters == null)
                parameters = new List<object>();

            return System.Text.RegularExpressions.Regex.Replace(
                format,
                @"\{
                    (
                        (\d*)       \|
                    )?
                    (
                        (assembly   \|  (\d+)   (\|.*?)? ) |
                        (enum       \|  (\d+)           )
                    )?
                \}
                |
                \{
                    (\d*)
                \}",
                delegate(System.Text.RegularExpressions.Match match)
                {
                    object parm = null;

                    format.ToString();
                    if (match.Groups[2].Success || match.Groups[9].Success)
                    {
                        int index = Int32.Parse(match.Groups[2].Success ? match.Groups[2].Value : match.Groups[9].Value);
                        if (index >= 0 && index < parameters.Count)
                            parm = parameters[index];
                    }

                    if (match.Groups[9].Success && parm != null)
                        return parm.ToString();

                    if (match.Groups[8].Success)
                        return ParseFormat(this.enumLocaleInfo[Int32.Parse(match.Groups[8].Value)][parm.ToString()], parameters);

                    if (match.Groups[5].Success)
                    {
                        string[] para;
                        para = match.Groups[6].Value.Substring(1).Split(Char.Parse("|"));
                        if (parm == null)
                            parm = parameters.ToArray();
                        return this.methods[Int32.Parse(match.Groups[5].Value)].Invoke(null, new object[] { parm, para }) as string;
                    }

                    return match.Value;
                },
                System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace
            );
        }

        public string PrepareFormat(string format)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                format,
                @"\{(\d*\|)?([^0-9/].*?)\}",
                delegate(System.Text.RegularExpressions.Match match){
                    if (match.Value == "{hide}")
                        return match.Value;

                    string processed = match.Groups[2].Value;
                    if (match.Groups[1].Success && processed.StartsWith("enum|"))
                    {
                        var parts = processed.Split(new string[] {"|"}, 3,  StringSplitOptions.None);
                        var enu = (ProjectManager.Components[parts[1]] as EnumProvider).Enums[parts[2]];

                        if (!enums.Contains(enu))
                        {
                            enums.Add(enu);

                            var newenu = new Dictionary<string, string>();
                            enumLocaleInfo.Add(newenu);
                            foreach (var item in enu.GetKeys())
                            {
                                newenu.Add(item, PrepareFormat(enu[item]));
                            }
                        }

                        return "{" + match.Groups[1].Value + "enum|" + enums.IndexOf(enu).ToString() + "}";
                    }
                    else
                        foreach (string item in aliases.Keys)
                        {
                            if (processed.StartsWith(item))
                            {
                                processed = aliases[item] + processed.Substring(item.Length);
                                break;
                            }
                        }

                    System.Text.RegularExpressions.Match ma = System.Text.RegularExpressions.Regex.Match(
                        processed,
                        @"^(.*?)\|(.*?)\|(.*?)\|(.*?)(\|.*)?$"
                    );
                    if (ma.Success)
                    {
                        if ((ma.Groups[1].Value) == "assembly")
                        {
                            System.Reflection.Assembly assembly = 
                                (ProjectManager.Components[
                                    ma.Groups[2].Value
                                 ] as IAssemblyProvider).GetAssembly();
                            var formatMethod = assembly
                                .GetType(ma.Groups[3].Value, true, true)
                                .GetMethod(ma.Groups[4].Value);

                            methods.Add(formatMethod);

                            processed = "assembly|" + (methods.Count - 1).ToString() + ma.Groups[5].Value;
                        }
                    }

                    return "{" + match.Groups[1].Value + processed + "}";
                }
            );
        }
	}
}
