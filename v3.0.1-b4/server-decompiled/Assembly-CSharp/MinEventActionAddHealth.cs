using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F5 RID: 1781
[Preserve]
public class MinEventActionAddHealth : MinEventActionTargetedBase
{
	// Token: 0x0600366B RID: 13931 RVA: 0x001674C4 File Offset: 0x001656C4
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

	// Token: 0x0600366C RID: 13932 RVA: 0x00167558 File Offset: 0x00165758
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

	// Token: 0x04002BBD RID: 11197
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BBE RID: 11198
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = "";

	// Token: 0x04002BBF RID: 11199
	[PublicizedFrom(EAccessModifier.Private)]
	public int health;

	// Token: 0x04002BC0 RID: 11200
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSplatter = true;
}
