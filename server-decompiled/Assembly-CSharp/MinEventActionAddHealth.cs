using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000701 RID: 1793
[Preserve]
public class MinEventActionAddHealth : MinEventActionTargetedBase
{
	// Token: 0x06003704 RID: 14084 RVA: 0x0016A000 File Offset: 0x00168200
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			int num = ((!this.cvarRef) ? this.health : ((int)this.targets[i].Buffs.GetCustomVar(this.refCvarName)));
			this.targets[i].AddHealth(num);
			if (this.showSplatter && num < 0)
			{
				EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
				if (entityPlayerLocal != null)
				{
					entityPlayerLocal.ForceBloodSplatter();
				}
			}
		}
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x0016A094 File Offset: 0x00168294
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "health"))
			{
				if (localName == "show_splatter")
				{
					this.showSplatter = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				}
			}
			else if (_attribute.Value.StartsWith("@"))
			{
				this.cvarRef = true;
				this.refCvarName = _attribute.Value.Substring(1);
			}
			else
			{
				this.health = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
			}
		}
		return flag;
	}

	// Token: 0x04002C2A RID: 11306
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C2B RID: 11307
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = "";

	// Token: 0x04002C2C RID: 11308
	[PublicizedFrom(EAccessModifier.Private)]
	public int health;

	// Token: 0x04002C2D RID: 11309
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSplatter = true;
}
