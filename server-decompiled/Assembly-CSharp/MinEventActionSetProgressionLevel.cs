using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006ED RID: 1773
[Preserve]
public class MinEventActionSetProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x060036BA RID: 14010 RVA: 0x00168ADD File Offset: 0x00166CDD
	// (set) Token: 0x060036BB RID: 14011 RVA: 0x00168AE5 File Offset: 0x00166CE5
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x060036BC RID: 14012 RVA: 0x00168AEE File Offset: 0x00166CEE
	// (set) Token: 0x060036BD RID: 14013 RVA: 0x00168AF6 File Offset: 0x00166CF6
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x00168B00 File Offset: 0x00166D00
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].Progression != null)
			{
				ProgressionValue progressionValue = this.targets[i].Progression.GetProgressionValue(this.progressionName);
				if (progressionValue != null)
				{
					if (this.level != -1)
					{
						progressionValue.Level = this.level;
						this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
						this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
					}
					else
					{
						progressionValue.Level = progressionValue.ProgressionClass.MaxLevel;
						this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
						this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
					}
				}
			}
		}
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x00168C3F File Offset: 0x00166E3F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x00168C64 File Offset: 0x00166E64
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "progression_name"))
			{
				if (localName == "level")
				{
					this.level = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				}
			}
			else
			{
				this.progressionName = _attribute.Value;
			}
		}
		return flag;
	}
}
