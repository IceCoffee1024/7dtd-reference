using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E7 RID: 1767
[Preserve]
public class MinEventActionGiveSkillExp : MinEventActionTargetedBase
{
	// Token: 0x0600369F RID: 13983 RVA: 0x0016815C File Offset: 0x0016635C
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

	// Token: 0x060036A0 RID: 13984 RVA: 0x001682A6 File Offset: 0x001664A6
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.skill != null && (this.exp != -1 || this.level_percent != -1f);
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x001682D8 File Offset: 0x001664D8
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

	// Token: 0x04002BE7 RID: 11239
	[PublicizedFrom(EAccessModifier.Private)]
	public string skill;

	// Token: 0x04002BE8 RID: 11240
	[PublicizedFrom(EAccessModifier.Private)]
	public int exp = -1;

	// Token: 0x04002BE9 RID: 11241
	[PublicizedFrom(EAccessModifier.Private)]
	public float level_percent = -1f;
}
