using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F7 RID: 1783
[Preserve]
public class MinEventActionResetProgression : MinEventActionTargetedBase
{
	// Token: 0x060036E1 RID: 14049 RVA: 0x00169EFC File Offset: 0x001680FC
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
			entityPlayerLocal.Progression.bProgressionStatsChanged = true;
			entityPlayerLocal.bPlayerStatsChanged = true;
		}
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x00169FF4 File Offset: 0x001681F4
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

	// Token: 0x04002C20 RID: 11296
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resetLevels;

	// Token: 0x04002C21 RID: 11297
	[PublicizedFrom(EAccessModifier.Private)]
	public bool resetSkills;

	// Token: 0x04002C22 RID: 11298
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeBooks;

	// Token: 0x04002C23 RID: 11299
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeCrafting;
}
