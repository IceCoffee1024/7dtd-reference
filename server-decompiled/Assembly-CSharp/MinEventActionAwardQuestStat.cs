using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EA RID: 1770
[Preserve]
public class MinEventActionAwardQuestStat : MinEventActionTargetedBase
{
	// Token: 0x060036AB RID: 13995 RVA: 0x00168608 File Offset: 0x00166808
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

	// Token: 0x060036AC RID: 13996 RVA: 0x0016867B File Offset: 0x0016687B
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.awardCount > 0);
	}

	// Token: 0x060036AD RID: 13997 RVA: 0x0016869C File Offset: 0x0016689C
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

	// Token: 0x04002BF1 RID: 11249
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat;

	// Token: 0x04002BF2 RID: 11250
	[PublicizedFrom(EAccessModifier.Private)]
	public int awardCount = 1;

	// Token: 0x04002BF3 RID: 11251
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BF4 RID: 11252
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
