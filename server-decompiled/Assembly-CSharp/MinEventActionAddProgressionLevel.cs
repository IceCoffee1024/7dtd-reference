using System;
using System.Globalization;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006EC RID: 1772
[Preserve]
public class MinEventActionAddProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x0600367F RID: 13951 RVA: 0x00168363 File Offset: 0x00166563
	// (set) Token: 0x06003680 RID: 13952 RVA: 0x0016836B File Offset: 0x0016656B
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06003681 RID: 13953 RVA: 0x00168374 File Offset: 0x00166574
	// (set) Token: 0x06003682 RID: 13954 RVA: 0x0016837C File Offset: 0x0016657C
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x00168388 File Offset: 0x00166588
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

	// Token: 0x06003684 RID: 13956 RVA: 0x0016852F File Offset: 0x0016672F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x00168554 File Offset: 0x00166754
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

	// Token: 0x04002C0A RID: 11274
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showMessage = true;
}
