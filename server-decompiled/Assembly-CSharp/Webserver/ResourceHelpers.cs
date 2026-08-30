using System;
using System.IO;
using System.Reflection;

namespace Webserver
{
	// Token: 0x02001AC0 RID: 6848
	public static class ResourceHelpers
	{
		// Token: 0x0600CE67 RID: 52839 RVA: 0x004B2668 File Offset: 0x004B0868
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

		// Token: 0x0600CE68 RID: 52840 RVA: 0x004B26A8 File Offset: 0x004B08A8
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
