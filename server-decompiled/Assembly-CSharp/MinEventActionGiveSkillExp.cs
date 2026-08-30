using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DB RID: 1755
[Preserve]
public class MinEventActionGiveSkillExp : MinEventActionTargetedBase
{
	// Token: 0x06003606 RID: 13830 RVA: 0x00165620 File Offset: 0x00163820
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.exp != -1)
			{
				this.targets[i].Progression.AddLevelExp(this.exp, "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
				this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
				this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
			}
			else if (this.level_percent != -1f)
			{
				this.targets[i].Progression.AddLevelExp(this.exp, "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
				this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
				this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
			}
		}
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x0016576A File Offset: 0x0016396A
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.skill != null && (this.exp != -1 || this.level_percent != -1f);
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x0016579C File Offset: 0x0016399C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "skill"))
			{
				if (!(localName == "experience"))
				{
					if (localName == "level_percentage")
					{
						this.level_percent = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
					}
				}
				else
				{
					this.exp = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				}
			}
			else
			{
				this.skill = _attribute.Value;
			}
		}
		return flag;
	}

	// Token: 0x04002B7A RID: 11130
	[PublicizedFrom(EAccessModifier.Private)]
	public string skill;

	// Token: 0x04002B7B RID: 11131
	[PublicizedFrom(EAccessModifier.Private)]
	public int exp = -1;

	// Token: 0x04002B7C RID: 11132
	[PublicizedFrom(EAccessModifier.Private)]
	public float level_percent = -1f;
}
