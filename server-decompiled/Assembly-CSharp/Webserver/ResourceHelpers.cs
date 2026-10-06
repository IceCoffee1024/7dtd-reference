using System;
using System.IO;
using System.Reflection;

namespace Webserver
{
	// Token: 0x02001AEC RID: 6892
	public static class ResourceHelpers
	{
		// Token: 0x0600CFBF RID: 53183 RVA: 0x004B6B34 File Offset: 0x004B4D34
		public static Stream OpenManifestResource(Assembly _assembly, string _name, bool _ignoreCase = false)
		{
			foreach (string text in _assembly.GetManifestResourceNames())
			{
				if (text.EndsWith(_name, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
				{
					return _assembly.GetManifestResourceStream(text);
				}
			}
			return null;
		}

		// Token: 0x0600CFC0 RID: 53184 RVA: 0x004B6B74 File Offset: 0x004B4D74
		public static string GetManifestResourceText(Assembly _assembly, string _name, bool _ignoreCase = false)
		{
			string text;
			using (Stream stream = ResourceHelpers.OpenManifestResource(_assembly, _name, _ignoreCase))
			{
				if (stream == null)
				{
					text = null;
				}
				else
				{
					using (TextReader textReader = new StreamReader(stream))
					{
						text = textReader.ReadToEnd();
					}
				}
			}
			return text;
		}
	}
}
