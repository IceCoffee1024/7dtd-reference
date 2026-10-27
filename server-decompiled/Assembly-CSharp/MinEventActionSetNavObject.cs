using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FC RID: 1788
[Preserve]
public class MinEventActionSetNavObject : MinEventActionTargetedBase
{
	// Token: 0x060036F5 RID: 14069 RVA: 0x0016A318 File Offset: 0x00168518
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

	// Token: 0x060036F6 RID: 14070 RVA: 0x0016A3B4 File Offset: 0x001685B4
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

	// Token: 0x04002C2B RID: 11307
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectName = "";

	// Token: 0x04002C2C RID: 11308
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideSprite = "";

	// Token: 0x04002C2D RID: 11309
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideText = "";

	// Token: 0x04002C2E RID: 11310
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarToText = "";

	// Token: 0x04002C2F RID: 11311
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd = true;
}
