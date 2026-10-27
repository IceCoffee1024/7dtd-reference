using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009C0 RID: 2496
[Preserve]
public class ObjectiveStatAwarded : BaseObjective
{
	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06004903 RID: 18691 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x001CAA28 File Offset: 0x001C8C28
	public override void SetupObjective()
	{
		this.keyword = this.statText;
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x001CAA47 File Offset: 0x001C8C47
	public override void SetupDisplay()
	{
		base.Description = this.statText;
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001CAA7B File Offset: 0x001C8C7B
	public override void AddHooks()
	{
		QuestEventManager.Current.QuestAwardCredit += this.Current_QuestAwardCredit;
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x001CAA93 File Offset: 0x001C8C93
	public override void RemoveHooks()
	{
		QuestEventManager.Current.QuestAwardCredit -= this.Current_QuestAwardCredit;
	}

	// Token: 0x06004908 RID: 18696 RVA: 0x001CAAAB File Offset: 0x001C8CAB
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

	// Token: 0x06004909 RID: 18697 RVA: 0x001CAAE8 File Offset: 0x001C8CE8
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

	// Token: 0x0600490A RID: 18698 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x001CAB3E File Offset: 0x001C8D3E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStatAwarded objectiveStatAwarded = (ObjectiveStatAwarded)objective;
		objectiveStatAwarded.statName = this.statName;
		objectiveStatAwarded.statText = this.statText;
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x001CAB64 File Offset: 0x001C8D64
	public override BaseObjective Clone()
	{
		ObjectiveStatAwarded objectiveStatAwarded = new ObjectiveStatAwarded();
		this.CopyValues(objectiveStatAwarded);
		return objectiveStatAwarded;
	}

	// Token: 0x0600490D RID: 18701 RVA: 0x001CAB80 File Offset: 0x001C8D80
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

	// Token: 0x0600490E RID: 18702 RVA: 0x001CAC0D File Offset: 0x001C8E0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("stat_name", this.statName);
	}

	// Token: 0x04003A41 RID: 14913
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName = "";

	// Token: 0x04003A42 RID: 14914
	[PublicizedFrom(EAccessModifier.Private)]
	public string statText = "";

	// Token: 0x04003A43 RID: 14915
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;

	// Token: 0x04003A44 RID: 14916
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatName = "stat_name";

	// Token: 0x04003A45 RID: 14917
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatText = "stat_text";

	// Token: 0x04003A46 RID: 14918
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatTextKey = "stat_text_key";

	// Token: 0x04003A47 RID: 14919
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropNeededCount = "count";
}
