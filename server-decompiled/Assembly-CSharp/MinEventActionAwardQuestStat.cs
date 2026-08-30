using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E8 RID: 1768
[Preserve]
public class MinEventActionAwardQuestStat : MinEventActionTargetedBase
{
	// Token: 0x06003668 RID: 13928 RVA: 0x00167CA4 File Offset: 0x00165EA4
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

	// Token: 0x06003669 RID: 13929 RVA: 0x00167D17 File Offset: 0x00165F17
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.awardCount > 0);
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00167D38 File Offset: 0x00165F38
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

	// Token: 0x04002BF8 RID: 11256
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat;

	// Token: 0x04002BF9 RID: 11257
	[PublicizedFrom(EAccessModifier.Private)]
	public int awardCount = 1;

	// Token: 0x04002BFA RID: 11258
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BFB RID: 11259
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
