using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F0 RID: 1776
[Preserve]
public class MinEventActionGiveExp : MinEventActionTargetedBase
{
	// Token: 0x060036BA RID: 14010 RVA: 0x00169164 File Offset: 0x00167364
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			if (entityAlive.Progression != null)
			{
				entityAlive.Progression.AddLevelExp((!this.cvarRef) ? this.exp : ((int)entityAlive.Buffs.GetCustomVar(this.refCvarName)), "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
				entityAlive.Progression.bProgressionStatsChanged = !entityAlive.isEntityRemote;
			}
			entityAlive.bPlayerStatsChanged |= !entityAlive.isEntityRemote;
		}
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x0016920A File Offset: 0x0016740A
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.exp > 0);
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x0016922C File Offset: 0x0016742C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "experience" || localName == "exp")
			{
				if (_attribute.Value.StartsWith("@"))
				{
					this.cvarRef = true;
					this.refCvarName = _attribute.Value.Substring(1);
				}
				else
				{
					this.exp = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				}
			}
		}
		return flag;
	}

	// Token: 0x04002C06 RID: 11270
	[PublicizedFrom(EAccessModifier.Private)]
	public int exp = -1;

	// Token: 0x04002C07 RID: 11271
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C08 RID: 11272
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
