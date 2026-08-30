using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DC RID: 1756
[Preserve]
public class MinEventActionGiveExp : MinEventActionTargetedBase
{
	// Token: 0x0600360A RID: 13834 RVA: 0x00165840 File Offset: 0x00163A40
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

	// Token: 0x0600360B RID: 13835 RVA: 0x001658E6 File Offset: 0x00163AE6
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.exp > 0);
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x00165908 File Offset: 0x00163B08
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

	// Token: 0x04002B7D RID: 11133
	[PublicizedFrom(EAccessModifier.Private)]
	public int exp = -1;

	// Token: 0x04002B7E RID: 11134
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002B7F RID: 11135
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
