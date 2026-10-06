using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000996 RID: 2454
[Preserve]
public class ObjectiveEntityKill : BaseObjective
{
	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x06004774 RID: 18292 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06004775 RID: 18293 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x001C220C File Offset: 0x001C040C
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

	// Token: 0x06004777 RID: 18295 RVA: 0x001C227C File Offset: 0x001C047C
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

	// Token: 0x06004778 RID: 18296 RVA: 0x001C231E File Offset: 0x001C051E
	public override void AddHooks()
	{
		QuestEventManager.Current.EntityKill += this.Current_EntityKill;
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x001C2336 File Offset: 0x001C0536
	public override void RemoveHooks()
	{
		QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x001C2350 File Offset: 0x001C0550
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

	// Token: 0x0600477B RID: 18299 RVA: 0x001C240B File Offset: 0x001C060B
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

	// Token: 0x0600477C RID: 18300 RVA: 0x001C2444 File Offset: 0x001C0644
	public override BaseObjective Clone()
	{
		ObjectiveEntityKill objectiveEntityKill = new ObjectiveEntityKill();
		this.CopyValues(objectiveEntityKill);
		objectiveEntityKill.localizedName = this.localizedName;
		objectiveEntityKill.targetTags = this.targetTags;
		return objectiveEntityKill;
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x001C2478 File Offset: 0x001C0678
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

	// Token: 0x0600477E RID: 18302 RVA: 0x001C24FC File Offset: 0x001C06FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		Dictionary<string, object> dictionary = parameters;
		string text = "target_ids";
		object obj = this.entityNames.Clone();
		dictionary.Add(text, ((obj != null) ? obj.ToString() : null) + this.ID);
		parameters.Add("count", this.neededKillCount);
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x001C2550 File Offset: 0x001C0750
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

	// Token: 0x04003977 RID: 14711
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003978 RID: 14712
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededKillCount;

	// Token: 0x04003979 RID: 14713
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] entityNames;

	// Token: 0x0400397A RID: 14714
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> targetTags = FastTags<TagGroup.Global>.none;

	// Token: 0x0400397B RID: 14715
	public static string PropObjectiveKey = "objective_name_key";

	// Token: 0x0400397C RID: 14716
	public static string PropNeededCount = "needed_count";

	// Token: 0x0400397D RID: 14717
	public static string PropEntityNames = "entity_names";

	// Token: 0x0400397E RID: 14718
	public static string PropTargetTags = "target_tags";
}
