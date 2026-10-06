using System;
using System.Globalization;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006EE RID: 1774
[Preserve]
public class MinEventActionAddProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x060036C2 RID: 14018 RVA: 0x00168CC7 File Offset: 0x00166EC7
	// (set) Token: 0x060036C3 RID: 14019 RVA: 0x00168CCF File Offset: 0x00166ECF
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x060036C4 RID: 14020 RVA: 0x00168CD8 File Offset: 0x00166ED8
	// (set) Token: 0x060036C5 RID: 14021 RVA: 0x00168CE0 File Offset: 0x00166EE0
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x00168CEC File Offset: 0x00166EEC
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
					}
					this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
					this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
				}
			}
		}
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00168E93 File Offset: 0x00167093
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x00168EB8 File Offset: 0x001670B8
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

	// Token: 0x04002C03 RID: 11267
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showMessage = true;
}
