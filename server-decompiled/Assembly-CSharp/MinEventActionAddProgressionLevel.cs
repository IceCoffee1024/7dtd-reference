using System;
using System.Globalization;
using System.Xml.Linq;
using BhvrAnalyticsServices.Interfaces;
using SandboxOptions;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

// Token: 0x020006F6 RID: 1782
[Preserve]
public class MinEventActionAddProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x060036D9 RID: 14041 RVA: 0x00169BAF File Offset: 0x00167DAF
	// (set) Token: 0x060036DA RID: 14042 RVA: 0x00169BB7 File Offset: 0x00167DB7
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x060036DB RID: 14043 RVA: 0x00169BC0 File Offset: 0x00167DC0
	// (set) Token: 0x060036DC RID: 14044 RVA: 0x00169BC8 File Offset: 0x00167DC8
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x00169BD4 File Offset: 0x00167DD4
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
					int level = progressionValue.Level;
					int num = this.level;
					if (progressionValue.ProgressionClass.Type == ProgressionType.Crafting && this.level == 1)
					{
						num = SandboxOptionManager.GetInt(global::SandboxOptions.SandboxOptions.PointsPerMagazine);
					}
					progressionValue.Level += num;
					if (progressionValue.Level > progressionValue.ProgressionClass.MaxLevel)
					{
						progressionValue.Level = progressionValue.ProgressionClass.MaxLevel;
					}
					if (progressionValue.Level < 0)
					{
						progressionValue.Level = 0;
					}
					if (level != progressionValue.Level && progressionValue.ProgressionClass.IsCrafting && this.targets[i] is EntityPlayerLocal)
					{
						EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
						if (XUiM_Recipes.CraftingProgression)
						{
							entityPlayerLocal.PlayerUI.xui.CollectedItemList.AddCraftingSkillNotification(progressionValue, false);
							if (this.showMessage)
							{
								progressionValue.ProgressionClass.HandleCheckCrafting(entityPlayerLocal, level, progressionValue.Level);
							}
						}
						if (!Helper.IsEditingOrPlaytesting())
						{
							PlayerCraftingSkillEventData playerCraftingSkillEventData = new PlayerCraftingSkillEventData
							{
								ServerId = Helper.GetServerId(),
								SaveId = Helper.GetSaveId(),
								TransactionTimestamp = DateTime.UtcNow.ToString("O"),
								ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
								PlayerLevel = entityPlayerLocal.Progression.Level,
								InGameDays = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime),
								CraftingSkill = progressionValue.Name,
								SourceType = "Magazine",
								SkillLevelBefore = level,
								SkillLevelAfter = progressionValue.Level
							};
							ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerCraftingSkillEventData);
						}
					}
					this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
					this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
				}
			}
		}
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x00169E3F File Offset: 0x0016803F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x00169E64 File Offset: 0x00168064
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "progression_name"))
			{
				if (!(localName == "level"))
				{
					if (localName == "show_message")
					{
						this.showMessage = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
					}
				}
				else
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

	// Token: 0x04002C1F RID: 11295
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showMessage = true;
}
