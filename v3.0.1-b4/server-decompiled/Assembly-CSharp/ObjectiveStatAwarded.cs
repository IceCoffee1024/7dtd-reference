using System;
using UnityEngine.Scripting;

// Token: 0x0200099F RID: 2463
[Preserve]
public class ObjectiveStatAwarded : BaseObjective
{
	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x0600489E RID: 18590 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x001C674B File Offset: 0x001C494B
	public override void SetupObjective()
	{
		this.keyword = this.statText;
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x001C676A File Offset: 0x001C496A
	public override void SetupDisplay()
	{
		base.Description = this.statText;
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x001C679E File Offset: 0x001C499E
	public override void AddHooks()
	{
		QuestEventManager.Current.QuestAwardCredit += this.Current_QuestAwardCredit;
	}

	// Token: 0x060048A2 RID: 18594 RVA: 0x001C67B6 File Offset: 0x001C49B6
	public override void RemoveHooks()
	{
		QuestEventManager.Current.QuestAwardCredit -= this.Current_QuestAwardCredit;
	}

	// Token: 0x060048A3 RID: 18595 RVA: 0x001C67CE File Offset: 0x001C49CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_QuestAwardCredit(string stat, int awardCount)
	{
		if (base.Complete)
		{
			return;
		}
		if (this.statName.EqualsCaseInsensitive(stat))
		{
			base.CurrentValue += (byte)awardCount;
			if ((int)base.CurrentValue >= this.neededCount)
			{
				this.Refresh();
			}
		}
	}

	// Token: 0x060048A4 RID: 18596 RVA: 0x001C680C File Offset: 0x001C4A0C
	public override void Refresh()
	{
		if ((int)base.CurrentValue > this.neededCount)
		{
			base.CurrentValue = (byte)this.neededCount;
		}
		base.Complete = (int)base.CurrentValue >= this.neededCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060048A5 RID: 18597 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x060048A6 RID: 18598 RVA: 0x001C6862 File Offset: 0x001C4A62
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStatAwarded objectiveStatAwarded = (ObjectiveStatAwarded)objective;
		objectiveStatAwarded.statName = this.statName;
		objectiveStatAwarded.statText = this.statText;
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x001C6888 File Offset: 0x001C4A88
	public override BaseObjective Clone()
	{
		ObjectiveStatAwarded objectiveStatAwarded = new ObjectiveStatAwarded();
		this.CopyValues(objectiveStatAwarded);
		return objectiveStatAwarded;
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x001C68A4 File Offset: 0x001C4AA4
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		string text = "";
		properties.ParseString(ObjectiveStatAwarded.PropStatName, ref this.statName);
		properties.ParseString(ObjectiveStatAwarded.PropStatText, ref this.statText);
		if (properties.Contains(ObjectiveStatAwarded.PropNeededCount))
		{
			int num = 0;
			properties.ParseInt(ObjectiveStatAwarded.PropNeededCount, ref num);
			this.Value = num.ToString();
		}
		properties.ParseString(ObjectiveStatAwarded.PropStatTextKey, ref text);
		if (text != "")
		{
			this.statText = Localization.Get(text, false, null);
		}
	}

	// Token: 0x04003993 RID: 14739
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName = "";

	// Token: 0x04003994 RID: 14740
	[PublicizedFrom(EAccessModifier.Private)]
	public string statText = "";

	// Token: 0x04003995 RID: 14741
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;

	// Token: 0x04003996 RID: 14742
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatName = "stat_name";

	// Token: 0x04003997 RID: 14743
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatText = "stat_text";

	// Token: 0x04003998 RID: 14744
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatTextKey = "stat_text_key";

	// Token: 0x04003999 RID: 14745
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropNeededCount = "count";
}
