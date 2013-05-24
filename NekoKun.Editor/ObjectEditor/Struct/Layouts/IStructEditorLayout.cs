using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public interface IStructEditorLayout
    {
        
    }

    public delegate System.Windows.Forms.Control CreateControlDelegate(
        System.Xml.XmlNode node
    );
}
