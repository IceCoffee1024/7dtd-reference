using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000709 RID: 1801
[Preserve]
public class MinEventActionAddHealth : MinEventActionTargetedBase
{
	// Token: 0x0600371B RID: 14107 RVA: 0x0016AF98 File Offset: 0x00169198
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

	// Token: 0x0600371C RID: 14108 RVA: 0x0016B02C File Offset: 0x0016922C
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

	// Token: 0x04002C46 RID: 11334
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C47 RID: 11335
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = "";

	// Token: 0x04002C48 RID: 11336
	[PublicizedFrom(EAccessModifier.Private)]
	public int health;

	// Token: 0x04002C49 RID: 11337
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSplatter = true;
}
