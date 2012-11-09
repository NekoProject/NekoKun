using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NekoKun
{
    public class ProjectDocumentFile : AbstractFile
    {
        private XmlDocument doc;
        public List<XmlNode> Components = new List<XmlNode>();

        public ProjectDocumentFile(string filename)
            : base(filename)
        {
            this.doc = LoadDocument(filename);
            var projectVersion = new Version(doc["NekoKunProject"].Attributes["Version"].Value);
            if (projectVersion > new Version(((System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyFileVersionAttribute), false)[0] as System.Reflection.AssemblyFileVersionAttribute).Version)))
                throw new ArgumentException("The project file was created in a newer version of NekoKun which is not supported by this version of NekoKun.");

            foreach (XmlNode item in doc["NekoKunProject"])
            {
                if (item.NodeType == XmlNodeType.Element && item.Name == "Component")
                    Components.Add(item);
            }
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        public override AbstractEditor CreateEditor()
        {
            throw new NotImplementedException();
        }

        public virtual XmlDocument LoadDocument(string filename)
        {
            XmlDocument doc;
            doc = new XmlDocument();

            string ext = System.IO.Path.GetExtension(filename).ToLower();
            switch (ext)
            {
                case ".nkproj":
                    doc.Load(filename);
                    return doc;
                case ".rvproj2":
                    doc.Load(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        "NekoKun.Core.Projects.ProjectConversations.rvproj2.xml"
                    ));
                    return doc;
                default:
                    throw new ArgumentException("Invalid project file extension.", "filename");
            }
        }
    }
}
