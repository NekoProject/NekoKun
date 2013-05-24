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

        public static ProjectDocumentFile ProjectDocument
        {
            get { return projectDocument; }
        }

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
                ResourceManager.SearchPaths.Add(projectDir);
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
                                NekoKun.Core.ReflectionHelper.CreateInstanceFromTypeName(item.Attributes["Name"].Value, dict)
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
                NekoKun.Core.Application.Logger.Log(NekoKun.Core.ExceptionHelper.ExceptionMessage(e));
                CannotOpenProjectException j = new CannotOpenProjectException(string.Format("Error when opening {0}", file), e);
                throw j;
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

        public static string CreateProject(string dir, ArchiveFile template)
        {
            System.IO.Directory.CreateDirectory(dir);
            template.Extract(dir);

            return System.IO.Path.Combine(dir, "Game.nkproj");
        }
    }

    [global::System.Serializable]
    public class CannotOpenProjectException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CannotOpenProjectException() { }
        public CannotOpenProjectException(string message) : base(message) { }
        public CannotOpenProjectException(string message, Exception inner) : base(message, inner) { }
        protected CannotOpenProjectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
