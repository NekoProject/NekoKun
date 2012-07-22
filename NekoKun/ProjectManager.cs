using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NekoKun
{
    public static class ProjectManager
    {
        private static bool isProjectOpened;
        private static string projectDir;
        private static string projectFile;
        private static ProjectDocumentFile projectDocument;
        private static Dictionary<string, object> components;

        public static string ProjectDir
        {
            get { return projectDir; }
        }

        public static Dictionary<string, object> Components
        {
            get { return components; }
        }

        public static void OpenProject(string file)
        {
            if (isProjectOpened)
                throw new InvalidOperationException("A project has already been opened. ");

            try
            {
                projectFile = file;
                projectDir = System.IO.Path.GetDirectoryName(file);
                projectDocument = new ProjectDocumentFile(file);
                components = new Dictionary<string, object>();

                foreach (var item in projectDocument.Components)
                {
                    if (item.Attributes["ID"] != null)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (XmlNode property in item.ChildNodes)
                        {
                            if (property.HasChildNodes && property.ChildNodes.Count == 1 && (property.FirstChild is XmlText))
                                dict.Add(
                                    property.Attributes["Name"].Value,
                                    property.FirstChild.Value
                                );
                            else
                                dict.Add(
                                    property.Attributes["Name"].Value,
                                    property.ChildNodes
                                );

                        }

                        try
                        {
                            components.Add(
                                item.Attributes["ID"].Value,
                                System.AppDomain.CurrentDomain.CreateInstance(
                                    System.Reflection.Assembly.GetExecutingAssembly().FullName,
                                    item.Attributes["Name"].Value,
                                    false,
                                    System.Reflection.BindingFlags.CreateInstance,
                                    null,
                                    new object[] { dict },
                                    null,
                                    null,
                                    null
                                ).Unwrap()
                            );
                        }
                        catch (TypeLoadException)
                        {
                        }

                    }
                }

                isProjectOpened = true;
            }
            catch (Exception e)
            {
                Clean();
                throw e;
            }
        }

        private static void Clean()
        {
            isProjectOpened = false;
            projectDocument = null;
            projectDir = null;
            projectFile = null;
            components = null;
        }
    }
}
