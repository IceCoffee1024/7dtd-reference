using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F2 RID: 1778
[Preserve]
public class MinEventActionAwardQuestStat : MinEventActionTargetedBase
{
	// Token: 0x060036C2 RID: 14018 RVA: 0x001693F0 File Offset: 0x001675F0
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				QuestEventManager.Current.QuestAwardCredited(this.stat, (!this.cvarRef) ? this.awardCount : ((int)entityPlayerLocal.Buffs.GetCustomVar(this.refCvarName)));
			}
		}
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00169463 File Offset: 0x00167663
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.awardCount > 0);
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x00169484 File Offset: 0x00167684
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "count"))
			{
				if (localName == "stat")
				{
					this.stat = _attribute.Value;
				}
			}
			else if (_attribute.Value.StartsWith("@"))
			{
				this.cvarRef = true;
				this.refCvarName = _attribute.Value.Substring(1);
			}
			else
			{
				this.awardCount = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
			}
		}
		return flag;
	}

	// Token: 0x04002C0D RID: 11277
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat;

	// Token: 0x04002C0E RID: 11278
	[PublicizedFrom(EAccessModifier.Private)]
	public int awardCount = 1;

	// Token: 0x04002C0F RID: 11279
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C10 RID: 11280
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
