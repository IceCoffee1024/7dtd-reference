using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DE RID: 1758
[Preserve]
public class MinEventActionAwardQuestStat : MinEventActionTargetedBase
{
	// Token: 0x06003612 RID: 13842 RVA: 0x00165ACC File Offset: 0x00163CCC
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

	// Token: 0x06003613 RID: 13843 RVA: 0x00165B3F File Offset: 0x00163D3F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.awardCount > 0);
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x00165B60 File Offset: 0x00163D60
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

	// Token: 0x04002B84 RID: 11140
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat;

	// Token: 0x04002B85 RID: 11141
	[PublicizedFrom(EAccessModifier.Private)]
	public int awardCount = 1;

	// Token: 0x04002B86 RID: 11142
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002B87 RID: 11143
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
