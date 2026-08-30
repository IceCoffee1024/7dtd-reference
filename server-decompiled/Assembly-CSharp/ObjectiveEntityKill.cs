using System;
using UnityEngine.Scripting;

// Token: 0x02000990 RID: 2448
[Preserve]
public class ObjectiveEntityKill : BaseObjective
{
	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x060047C8 RID: 18376 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x060047C9 RID: 18377 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060047CA RID: 18378 RVA: 0x001C1B44 File Offset: 0x001BFD44
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

	// Token: 0x060047CB RID: 18379 RVA: 0x001C1BB4 File Offset: 0x001BFDB4
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

	// Token: 0x060047CC RID: 18380 RVA: 0x001C1C56 File Offset: 0x001BFE56
	public override void AddHooks()
	{
		QuestEventManager.Current.EntityKill += this.Current_EntityKill;
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x001C1C6E File Offset: 0x001BFE6E
	public override void RemoveHooks()
	{
		QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x001C1C88 File Offset: 0x001BFE88
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

	// Token: 0x060047CF RID: 18383 RVA: 0x001C1D43 File Offset: 0x001BFF43
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

	// Token: 0x060047D0 RID: 18384 RVA: 0x001C1D7C File Offset: 0x001BFF7C
	public override BaseObjective Clone()
	{
		ObjectiveEntityKill objectiveEntityKill = new ObjectiveEntityKill();
		this.CopyValues(objectiveEntityKill);
		objectiveEntityKill.localizedName = this.localizedName;
		objectiveEntityKill.targetTags = this.targetTags;
		return objectiveEntityKill;
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x001C1DB0 File Offset: 0x001BFFB0
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

	// Token: 0x060047D2 RID: 18386 RVA: 0x001C1E34 File Offset: 0x001C0034
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

	// Token: 0x0400397B RID: 14715
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x0400397C RID: 14716
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededKillCount;

	// Token: 0x0400397D RID: 14717
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] entityNames;

	// Token: 0x0400397E RID: 14718
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> targetTags = FastTags<TagGroup.Global>.none;

	// Token: 0x0400397F RID: 14719
	public static string PropObjectiveKey = "objective_name_key";

	// Token: 0x04003980 RID: 14720
	public static string PropNeededCount = "needed_count";

	// Token: 0x04003981 RID: 14721
	public static string PropEntityNames = "entity_names";

	// Token: 0x04003982 RID: 14722
	public static string PropTargetTags = "target_tags";
}
