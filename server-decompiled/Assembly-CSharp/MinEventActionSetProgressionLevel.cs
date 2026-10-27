using System;
using System.Globalization;
using System.Xml.Linq;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

// Token: 0x020006F5 RID: 1781
[Preserve]
public class MinEventActionSetProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x060036D1 RID: 14033 RVA: 0x001698C5 File Offset: 0x00167AC5
	// (set) Token: 0x060036D2 RID: 14034 RVA: 0x001698CD File Offset: 0x00167ACD
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x060036D3 RID: 14035 RVA: 0x001698D6 File Offset: 0x00167AD6
	// (set) Token: 0x060036D4 RID: 14036 RVA: 0x001698DE File Offset: 0x00167ADE
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x001698E8 File Offset: 0x00167AE8
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
					EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
					int num = progressionValue.ProgressionClass.Parent.Children.IndexOf(progressionValue.ProgressionClass);
					string text = string.Format("Volume {0}", num + 1);
					PlayerPerkProgressionEventData playerPerkProgressionEventData = new PlayerPerkProgressionEventData
					{
						ServerId = Helper.GetServerId(),
						SaveId = Helper.GetSaveId(),
						ProgressionEventTS = DateTime.UtcNow.ToString("O"),
						ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
						PlayerLevel = entityPlayerLocal.Progression.Level,
						PerkSection = progressionValue.ProgressionClass.Parent.ParentName,
						PerkSubSection = text,
						PerkParent = progressionValue.ProgressionClass.ParentName,
						PerkName = progressionValue.ProgressionClass.Name,
						ChangeType = "Learn",
						ChangeSource = "Book"
					};
					ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerPerkProgressionEventData);
				}
			}
		}
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x00169B2A File Offset: 0x00167D2A
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x00169B4C File Offset: 0x00167D4C
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
