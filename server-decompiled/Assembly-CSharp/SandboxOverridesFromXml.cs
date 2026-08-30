using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using SandboxOptions;

// Token: 0x02000D1A RID: 3354
public class SandboxOverridesFromXml
{
	// Token: 0x0600662C RID: 26156 RVA: 0x00284920 File Offset: 0x00282B20
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

	// Token: 0x0600662D RID: 26157 RVA: 0x0028492F File Offset: 0x00282B2F
	public static void Reload(XmlFile xmlFile)
	{
		ThreadManager.RunCoroutineSync(SandboxOverridesFromXml.CreateOverrides(xmlFile));
	}
}
