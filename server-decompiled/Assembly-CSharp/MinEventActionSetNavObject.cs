using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F2 RID: 1778
[Preserve]
public class MinEventActionSetNavObject : MinEventActionTargetedBase
{
	// Token: 0x0600369B RID: 13979 RVA: 0x00168A1C File Offset: 0x00166C1C
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

	// Token: 0x0600369C RID: 13980 RVA: 0x00168AB8 File Offset: 0x00166CB8
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

	// Token: 0x04002C16 RID: 11286
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectName = "";

	// Token: 0x04002C17 RID: 11287
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideSprite = "";

	// Token: 0x04002C18 RID: 11288
	[PublicizedFrom(EAccessModifier.Private)]
	public string overrideText = "";

	// Token: 0x04002C19 RID: 11289
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarToText = "";

	// Token: 0x04002C1A RID: 11290
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd = true;
}
