using System;
using UnityEngine.Scripting;

// Token: 0x020009B0 RID: 2480
[Preserve]
public class ObjectiveStatAwarded : BaseObjective
{
	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x06004924 RID: 18724 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x001C8F3F File Offset: 0x001C713F
	public override void SetupObjective()
	{
		this.keyword = this.statText;
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x001C8F5E File Offset: 0x001C715E
	public override void SetupDisplay()
	{
		base.Description = this.statText;
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x001C8F92 File Offset: 0x001C7192
	public override void AddHooks()
	{
		QuestEventManager.Current.QuestAwardCredit += this.Current_QuestAwardCredit;
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x001C8FAA File Offset: 0x001C71AA
	public override void RemoveHooks()
	{
		QuestEventManager.Current.QuestAwardCredit -= this.Current_QuestAwardCredit;
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x001C8FC2 File Offset: 0x001C71C2
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

	// Token: 0x0600492A RID: 18730 RVA: 0x001C9000 File Offset: 0x001C7200
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

	// Token: 0x0600492B RID: 18731 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x001C9056 File Offset: 0x001C7256
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStatAwarded objectiveStatAwarded = (ObjectiveStatAwarded)objective;
		objectiveStatAwarded.statName = this.statName;
		objectiveStatAwarded.statText = this.statText;
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x001C907C File Offset: 0x001C727C
	public override BaseObjective Clone()
	{
		ObjectiveStatAwarded objectiveStatAwarded = new ObjectiveStatAwarded();
		this.CopyValues(objectiveStatAwarded);
		return objectiveStatAwarded;
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x001C9098 File Offset: 0x001C7298
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

	// Token: 0x04003A24 RID: 14884
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName = "";

	// Token: 0x04003A25 RID: 14885
	[PublicizedFrom(EAccessModifier.Private)]
	public string statText = "";

	// Token: 0x04003A26 RID: 14886
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;

	// Token: 0x04003A27 RID: 14887
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatName = "stat_name";

	// Token: 0x04003A28 RID: 14888
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatText = "stat_text";

	// Token: 0x04003A29 RID: 14889
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStatTextKey = "stat_text_key";

	// Token: 0x04003A2A RID: 14890
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropNeededCount = "count";
}
