using System;
using System.Collections;
using ICSharpCode.Core;

namespace orzTech.NekoKun.ObjectEditor
{
	public class ObjectDisplayBindingDoozer : IDoozer
	{
		public bool HandleConditions {
			get {
				return false;
			}
		}
		
		public object BuildItem(object caller, Codon codon, ArrayList subItems)
		{
            return new ObjectDisplayBindingDescriptor(codon.AddIn, codon.Properties["typename"], codon.Properties["class"]);
		}
	}
}
