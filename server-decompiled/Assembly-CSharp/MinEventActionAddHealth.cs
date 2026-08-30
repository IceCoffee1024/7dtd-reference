using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FF RID: 1791
[Preserve]
public class MinEventActionAddHealth : MinEventActionTargetedBase
{
	// Token: 0x060036C1 RID: 14017 RVA: 0x0016969C File Offset: 0x0016789C
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

	// Token: 0x060036C2 RID: 14018 RVA: 0x00169730 File Offset: 0x00167930
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

	// Token: 0x04002C31 RID: 11313
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C32 RID: 11314
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = "";

	// Token: 0x04002C33 RID: 11315
	[PublicizedFrom(EAccessModifier.Private)]
	public int health;

	// Token: 0x04002C34 RID: 11316
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSplatter = true;
}
