using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E6 RID: 1766
[Preserve]
public class MinEventActionGiveExp : MinEventActionTargetedBase
{
	// Token: 0x06003660 RID: 13920 RVA: 0x00167A18 File Offset: 0x00165C18
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

	// Token: 0x06003661 RID: 13921 RVA: 0x00167ABE File Offset: 0x00165CBE
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.exp > 0);
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00167AE0 File Offset: 0x00165CE0
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

	// Token: 0x04002BF1 RID: 11249
	[PublicizedFrom(EAccessModifier.Private)]
	public int exp = -1;

	// Token: 0x04002BF2 RID: 11250
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BF3 RID: 11251
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
