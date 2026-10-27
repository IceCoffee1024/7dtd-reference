using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using SandboxOptions;

// Token: 0x02000D45 RID: 3397
public class SandboxOverridesFromXml
{
	// Token: 0x060066CF RID: 26319 RVA: 0x0028C0D4 File Offset: 0x0028A2D4
	public static IEnumerator CreateOverrides(XmlFile xmlFile)
	{
		XElement root = xmlFile.XmlDoc.Root;
		SandboxOptionManager sandboxManager = SandboxOptionManager.Current;
		sandboxManager.RemoveOverrides();
		MicroStopwatch msw = new MicroStopwatch(true);
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "preset")
			{
				SandboxOptionManager.Current.LoadPresetFromXml(xelement, "Modded", true);
			}
			else
			{
				if (!(xelement.Name == "sandbox_override"))
				{
					string text = "Unrecognized xml element ";
					XName name = xelement.Name;
					throw new Exception(text + ((name != null) ? name.ToString() : null));
				}
				if (xelement.HasAttribute("option"))
				{
					global::SandboxOptions.SandboxOptions sandboxOptions = global::SandboxOptions.SandboxOptions.Max;
					if (Enum.TryParse<global::SandboxOptions.SandboxOptions>(xelement.GetAttribute("option"), out sandboxOptions))
					{
						sandboxManager.AddOverride(sandboxOptions);
					}
				}
			}
			if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
			{
				yield return null;
				msw.ResetAndRestart();
			}
		}
		IEnumerator<XElement> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060066D0 RID: 26320 RVA: 0x0028C0E3 File Offset: 0x0028A2E3
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(SandboxOverridesFromXml.CreateOverrides(xmlFile));
	}
}
