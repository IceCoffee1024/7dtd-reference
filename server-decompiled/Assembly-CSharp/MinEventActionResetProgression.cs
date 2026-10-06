using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EF RID: 1775
[Preserve]
public class MinEventActionResetProgression : MinEventActionTargetedBase
{
	// Token: 0x060036CA RID: 14026 RVA: 0x00168F50 File Offset: 0x00167150
	public override void Execute(MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = this.targets[0] as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.Progression.ResetProgression(this.resetLevels || this.resetSkills, this.removeBooks, this.removeCrafting);
			if (this.resetLevels)
			{
				entityPlayerLocal.Progression.Level = 1;
				entityPlayerLocal.Progression.ExpToNextLevel = entityPlayerLocal.Progression.GetExpForNextLevel();
				entityPlayerLocal.Progression.SkillPoints = entityPlayerLocal.QuestJournal.GetRewardedSkillPoints();
				entityPlayerLocal.Progression.ExpDeficit = 0;
			}
			if (this.removeCrafting)
			{
				List<Recipe> recipes = CraftingManager.GetRecipes();
				for (int i = 0; i < recipes.Count; i++)
				{
					if (recipes[i].IsLearnable)
					{
						entityPlayerLocal.Buffs.RemoveCustomVar(recipes[i].GetName());
					}
				}
			}
			entityPlayerLocal.Progression.ResetProgression(this.removeBooks, false, false);
			entityPlayerLocal.Progression.bProgressionStatsChanged = true;
			entityPlayerLocal.bPlayerStatsChanged = true;
		}
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x0016905C File Offset: 0x0016725C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "reset_books"))
			{
				if (!(localName == "reset_levels"))
				{
					if (!(localName == "reset_skills"))
					{
						if (localName == "reset_crafting")
						{
							this.removeCrafting = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
						}
					}
					else
					{
						this.resetSkills = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
					}
				}
				else
				{
					this.resetLevels = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				}
			}
			else
			{
				this.removeBooks = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
			}
		}
		return flag;
	}

	// Token: 0x04002C04 RID: 11268
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resetLevels;

	// Token: 0x04002C05 RID: 11269
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resetSkills;

	// Token: 0x04002C06 RID: 11270
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeBooks;

	// Token: 0x04002C07 RID: 11271
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeCrafting;
}
