using System;
using System.Collections;
using ICSharpCode.Core;

namespace orzTech.NekoKun.ObjectEditor
{
	public interface IObjectDisplayBinding
	{
		/// <summary>
		/// Loads the file and opens a <see cref="IViewContent"/>.
		/// When this method returns <c>null</c>, the display binding cannot handle the file type.
		/// </summary>
		IObjectViewContent OpenObject(object Object);
	}
}
