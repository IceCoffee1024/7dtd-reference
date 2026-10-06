using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009B6 RID: 2486
[Preserve]
public class ObjectiveStatAwarded : BaseObjective
{
	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x060048E0 RID: 18656 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x001C9954 File Offset: 0x001C7B54
	public override void SetupObjective()
	{
		this.keyword = this.statText;
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x001C9973 File Offset: 0x001C7B73
	public override void SetupDisplay()
	{
		base.Description = this.statText;
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x001C99A7 File Offset: 0x001C7BA7
	public override void AddHooks()
	{
		QuestEventManager.Current.QuestAwardCredit += this.Current_QuestAwardCredit;
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x001C99BF File Offset: 0x001C7BBF
	public override void RemoveHooks()
	{
		QuestEventManager.Current.QuestAwardCredit -= this.Current_QuestAwardCredit;
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x001C99D7 File Offset: 0x001C7BD7
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

	// Token: 0x060048E6 RID: 18662 RVA: 0x001C9A14 File Offset: 0x001C7C14
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

	// Token: 0x060048E7 RID: 18663 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x001C9A6A File Offset: 0x001C7C6A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStatAwarded objectiveStatAwarded = (ObjectiveStatAwarded)objective;
		objectiveStatAwarded.statName = this.statName;
		objectiveStatAwarded.statText = this.statText;
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x001C9A90 File Offset: 0x001C7C90
	public override BaseObjective Clone()
	{
		ObjectiveStatAwarded objectiveStatAwarded = new ObjectiveStatAwarded();
		this.CopyValues(objectiveStatAwarded);
		return objectiveStatAwarded;
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x001C9AAC File Offset: 0x001C7CAC
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

	// Token: 0x060048EB RID: 18667 RVA: 0x001C9B39 File Offset: 0x001C7D39
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("stat_name", this.statName);
	}

	// Token: 0x04003A21 RID: 14881
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName = "";

	// Token: 0x04003A22 RID: 14882
	[PublicizedFrom(EAccessModifier.Private)]
	public string statText = "";

	// Token: 0x04003A23 RID: 14883
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;

	// Token: 0x04003A24 RID: 14884
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatName = "stat_name";

	// Token: 0x04003A25 RID: 14885
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatText = "stat_text";

	// Token: 0x04003A26 RID: 14886
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatTextKey = "stat_text_key";

	// Token: 0x04003A27 RID: 14887
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropNeededCount = "count";
}
