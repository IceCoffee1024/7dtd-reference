using System;
using System.Globalization;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006E2 RID: 1762
[Preserve]
public class MinEventActionAddProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x06003629 RID: 13865 RVA: 0x0016618B File Offset: 0x0016438B
	// (set) Token: 0x0600362A RID: 13866 RVA: 0x00166193 File Offset: 0x00164393
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x0600362B RID: 13867 RVA: 0x0016619C File Offset: 0x0016439C
	// (set) Token: 0x0600362C RID: 13868 RVA: 0x001661A4 File Offset: 0x001643A4
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x001661B0 File Offset: 0x001643B0
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

	// Token: 0x0600362E RID: 13870 RVA: 0x00166357 File Offset: 0x00164557
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x0016637C File Offset: 0x0016457C
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

	// Token: 0x04002B96 RID: 11158
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showMessage = true;
}
