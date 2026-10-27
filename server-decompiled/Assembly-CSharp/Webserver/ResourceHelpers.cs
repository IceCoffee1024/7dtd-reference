using System;
using System.IO;
using System.Reflection;

namespace Webserver
{
	// Token: 0x02001B1D RID: 6941
	public static class ResourceHelpers
	{
		// Token: 0x0600D0D3 RID: 53459 RVA: 0x004BA630 File Offset: 0x004B8830
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

		// Token: 0x0600D0D4 RID: 53460 RVA: 0x004BA670 File Offset: 0x004B8870
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
