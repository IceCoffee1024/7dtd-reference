using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F4 RID: 1780
[Preserve]
public class MinEventActionSetNavObject : MinEventActionTargetedBase
{
	// Token: 0x060036DE RID: 14046 RVA: 0x00169380 File Offset: 0x00167580
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

	// Token: 0x060036DF RID: 14047 RVA: 0x0016941C File Offset: 0x0016761C
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

	// Token: 0x04002C0F RID: 11279
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectName = "";

	// Token: 0x04002C10 RID: 11280
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideSprite = "";

	// Token: 0x04002C11 RID: 11281
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideText = "";

	// Token: 0x04002C12 RID: 11282
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarToText = "";

	// Token: 0x04002C13 RID: 11283
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd = true;
}
