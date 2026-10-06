using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E8 RID: 1768
[Preserve]
public class MinEventActionGiveExp : MinEventActionTargetedBase
{
	// Token: 0x060036A3 RID: 13987 RVA: 0x0016837C File Offset: 0x0016657C
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

	// Token: 0x060036A4 RID: 13988 RVA: 0x00168422 File Offset: 0x00166622
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.exp > 0);
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x00168444 File Offset: 0x00166644
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

	// Token: 0x04002BEA RID: 11242
	[PublicizedFrom(EAccessModifier.Private)]
	public int exp = -1;

	// Token: 0x04002BEB RID: 11243
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BEC RID: 11244
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
