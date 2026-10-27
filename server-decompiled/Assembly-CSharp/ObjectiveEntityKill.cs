using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009A0 RID: 2464
[Preserve]
public class ObjectiveEntityKill : BaseObjective
{
	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06004797 RID: 18327 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06004798 RID: 18328 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x001C3290 File Offset: 0x001C1490
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

	// Token: 0x0600479A RID: 18330 RVA: 0x001C3300 File Offset: 0x001C1500
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

	// Token: 0x0600479B RID: 18331 RVA: 0x001C33A2 File Offset: 0x001C15A2
	public override void AddHooks()
	{
		QuestEventManager.Current.EntityKill += this.Current_EntityKill;
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x001C33BA File Offset: 0x001C15BA
	public override void RemoveHooks()
	{
		QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x001C33D4 File Offset: 0x001C15D4
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

	// Token: 0x0600479E RID: 18334 RVA: 0x001C348F File Offset: 0x001C168F
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

	// Token: 0x0600479F RID: 18335 RVA: 0x001C34C8 File Offset: 0x001C16C8
	public override BaseObjective Clone()
	{
		ObjectiveEntityKill objectiveEntityKill = new ObjectiveEntityKill();
		this.CopyValues(objectiveEntityKill);
		objectiveEntityKill.localizedName = this.localizedName;
		objectiveEntityKill.targetTags = this.targetTags;
		return objectiveEntityKill;
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x001C34FC File Offset: 0x001C16FC
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

	// Token: 0x060047A1 RID: 18337 RVA: 0x001C3580 File Offset: 0x001C1780
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		Dictionary<string, object> dictionary = parameters;
		string text = "target_ids";
		object obj = this.entityNames.Clone();
		dictionary.Add(text, ((obj != null) ? obj.ToString() : null) + this.ID);
		parameters.Add("count", this.neededKillCount);
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x001C35D4 File Offset: 0x001C17D4
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

	// Token: 0x04003997 RID: 14743
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003998 RID: 14744
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededKillCount;

	// Token: 0x04003999 RID: 14745
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] entityNames;

	// Token: 0x0400399A RID: 14746
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> targetTags = FastTags<TagGroup.Global>.none;

	// Token: 0x0400399B RID: 14747
	public static string PropObjectiveKey = "objective_name_key";

	// Token: 0x0400399C RID: 14748
	public static string PropNeededCount = "needed_count";

	// Token: 0x0400399D RID: 14749
	public static string PropEntityNames = "entity_names";

	// Token: 0x0400399E RID: 14750
	public static string PropTargetTags = "target_tags";
}
