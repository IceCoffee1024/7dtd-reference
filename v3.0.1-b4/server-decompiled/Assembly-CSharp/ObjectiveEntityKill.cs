using System;
using UnityEngine.Scripting;

// Token: 0x0200097F RID: 2431
[Preserve]
public class ObjectiveEntityKill : BaseObjective
{
	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x06004742 RID: 18242 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x06004743 RID: 18243 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x001BF350 File Offset: 0x001BD550
	public override void SetupObjective()
	{
		this.neededKillCount = Convert.ToInt32(this.Value);
		if (this.ID != null)
		{
			string[] array = this.ID.Split(',', StringSplitOptions.None);
			if (array.Length > 1)
			{
				this.ID = array[0];
				this.entityNames = new string[array.Length - 1];
				for (int i = 1; i < array.Length; i++)
				{
					this.entityNames[i - 1] = array[i];
				}
			}
		}
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x001BF3C0 File Offset: 0x001BD5C0
	public override void SetupDisplay()
	{
		this.keyword = Localization.Get("ObjectiveZombieKill_keyword", false, null);
		if (this.localizedName == "")
		{
			this.localizedName = ((this.ID != null && this.ID != "") ? Localization.Get(this.ID, false, null) : "Any Zombie");
		}
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededKillCount);
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x001BF462 File Offset: 0x001BD662
	public override void AddHooks()
	{
		QuestEventManager.Current.EntityKill += this.Current_EntityKill;
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x001BF47A File Offset: 0x001BD67A
	public override void RemoveHooks()
	{
		QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x001BF494 File Offset: 0x001BD694
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_EntityKill(EntityAlive killedBy, EntityAlive killedEntity)
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = false;
		string entityClassName = killedEntity.EntityClass.entityClassName;
		if (this.targetTags.IsEmpty)
		{
			if (this.ID == null || entityClassName.EqualsCaseInsensitive(this.ID))
			{
				flag = true;
			}
			if (!flag && this.entityNames != null)
			{
				for (int i = 0; i < this.entityNames.Length; i++)
				{
					if (this.entityNames[i].EqualsCaseInsensitive(entityClassName))
					{
						flag = true;
						break;
					}
				}
			}
		}
		else
		{
			flag = killedEntity.EntityClass.Tags.Test_AnySet(this.targetTags);
		}
		if (flag && base.OwnerQuest.CheckRequirements())
		{
			byte currentValue = base.CurrentValue;
			base.CurrentValue = currentValue + 1;
			this.Refresh();
		}
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x001BF54F File Offset: 0x001BD74F
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.neededKillCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x001BF588 File Offset: 0x001BD788
	public override BaseObjective Clone()
	{
		ObjectiveEntityKill objectiveEntityKill = new ObjectiveEntityKill();
		this.CopyValues(objectiveEntityKill);
		objectiveEntityKill.localizedName = this.localizedName;
		objectiveEntityKill.targetTags = this.targetTags;
		return objectiveEntityKill;
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x001BF5BC File Offset: 0x001BD7BC
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveEntityKill.PropObjectiveKey))
		{
			this.localizedName = Localization.Get(properties.Values[ObjectiveEntityKill.PropObjectiveKey], false, null);
		}
		properties.ParseString(ObjectiveEntityKill.PropEntityNames, ref this.ID);
		properties.ParseString(ObjectiveEntityKill.PropNeededCount, ref this.Value);
		string text = "";
		properties.ParseString(ObjectiveEntityKill.PropTargetTags, ref text);
		this.targetTags = FastTags<TagGroup.Global>.Parse(text);
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x001BF640 File Offset: 0x001BD840
	public override string ParseBinding(string bindingName)
	{
		string id = this.ID;
		string value = this.Value;
		if (this.localizedName == "")
		{
			this.localizedName = ((id != null && id != "") ? Localization.Get(id, false, null) : "Any Zombie");
		}
		if (bindingName == "target")
		{
			return this.localizedName;
		}
		if (!(bindingName == "targetwithcount"))
		{
			return "";
		}
		return Convert.ToInt32(value).ToString() + " " + this.localizedName;
	}

	// Token: 0x040038EA RID: 14570
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040038EB RID: 14571
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededKillCount;

	// Token: 0x040038EC RID: 14572
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] entityNames;

	// Token: 0x040038ED RID: 14573
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> targetTags = FastTags<TagGroup.Global>.none;

	// Token: 0x040038EE RID: 14574
	public static string PropObjectiveKey = "objective_name_key";

	// Token: 0x040038EF RID: 14575
	public static string PropNeededCount = "needed_count";

	// Token: 0x040038F0 RID: 14576
	public static string PropEntityNames = "entity_names";

	// Token: 0x040038F1 RID: 14577
	public static string PropTargetTags = "target_tags";
}
