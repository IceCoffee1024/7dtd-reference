using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E8 RID: 1768
[Preserve]
public class MinEventActionSetNavObject : MinEventActionTargetedBase
{
	// Token: 0x06003645 RID: 13893 RVA: 0x00166844 File Offset: 0x00164A44
	public override void Execute(MinEventParams _params)
	{
		if (this.navObjectName == "")
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			if (this.isAdd)
			{
				entityAlive.AddNavObject(this.navObjectName, this.overrideSprite, (this.cvarToText != "") ? entityAlive.GetCVar(this.cvarToText).ToString() : this.overrideText);
			}
			else
			{
				entityAlive.RemoveNavObject(this.navObjectName);
			}
		}
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x001668E0 File Offset: 0x00164AE0
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "nav_object")
			{
				this.navObjectName = _attribute.Value;
				return true;
			}
			if (localName == "sprite")
			{
				this.overrideSprite = _attribute.Value;
				return true;
			}
			if (localName == "text")
			{
				this.overrideText = _attribute.Value;
				return true;
			}
			if (localName == "cvar_to_text")
			{
				this.cvarToText = _attribute.Value;
				return true;
			}
			if (localName == "add")
			{
				this.isAdd = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002BA2 RID: 11170
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectName = "";

	// Token: 0x04002BA3 RID: 11171
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideSprite = "";

	// Token: 0x04002BA4 RID: 11172
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideText = "";

	// Token: 0x04002BA5 RID: 11173
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarToText = "";

	// Token: 0x04002BA6 RID: 11174
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd = true;
}
