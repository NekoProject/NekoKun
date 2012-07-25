using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace NekoKun
{
    public class CLRCodeCompiler : IAssemblyProvider
    {
        protected string lang;
        protected string code;
        protected bool debugMode = false;
        protected CompilerParameters compilerParameters;
        protected CompilerResults compilerResults;
        protected Assembly compiledAssembly;

        public CLRCodeCompiler(Dictionary<string, object> node)
        {
            this.lang = (node["Language"] as string).ToLower();
            this.code = (node["Code"] as System.Xml.XmlNodeList)[0].Value;

            if (node["DebugMode"] == null)
                this.debugMode = Boolean.Parse(node["DebugMode"] as string);

            CreateCompiler();
            Compile();
        }

        protected void Compile()
        {
            this.compilerResults = CodeDomProvider.CreateProvider(lang).CompileAssemblyFromSource(this.compilerParameters, this.code);

            if (this.compilerResults.NativeCompilerReturnValue == 0)
            {
                this.compiledAssembly = this.compilerResults.CompiledAssembly;

            }
            else
            {
                var sb = new StringBuilder();
                foreach (var item in this.compilerResults.Errors)
                {
                    sb.AppendLine(item.ToString());
                }
                
                System.Windows.Forms.MessageBox.Show(sb.ToString(), "NekoKun.CLRCodeCompiler", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                throw new ArgumentException(sb.ToString());
            }
        }

        protected void CreateCompiler()
        {
            this.compilerParameters = new CompilerParameters();
            this.compilerParameters.IncludeDebugInformation = false;
            this.compilerParameters.GenerateExecutable = false;
            this.compilerParameters.GenerateInMemory = true;
            this.compilerParameters.IncludeDebugInformation = debugMode;

            //this.compilerParameters.ReferencedAssemblies.Add("System.dll");
            //this.compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            //this.compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var file in item.GetFiles(false))
                {
                    this.compilerParameters.ReferencedAssemblies.Add(file.Name);
                }
            }

            if ("visualbasic" == lang || "vb" == lang)
            {
                lang = "VisualBasic";
                //this.compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
                if (debugMode)
                    compilerParameters.CompilerOptions += "/debug:full /optimize- /optionexplicit+ /optionstrict+ /optioncompare:text /imports:Microsoft.VisualBasic,System,System.Collections,System.Diagnostics ";
                else
                    compilerParameters.CompilerOptions += "/optimize /optionexplicit+ /optionstrict+ /optioncompare:text /imports:Microsoft.VisualBasic,System,System.Collections,System.Diagnostics ";
            }
            else if ("csharp" == lang || "cs" == lang || "c#" == lang)
            {
                lang = "CSharp";
                if (!debugMode)
                    compilerParameters.CompilerOptions += "/optimize ";
            }
        }

        public Assembly GetAssembly()
        {
            return this.compiledAssembly;
        }
    }
}
