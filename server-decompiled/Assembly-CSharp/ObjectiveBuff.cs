using System;
using UnityEngine.Scripting;

// Token: 0x0200098B RID: 2443
[Preserve]
public class ObjectiveBuff : BaseObjective
{
	// Token: 0x06004797 RID: 18327 RVA: 0x001C0FB2 File Offset: 0x001BF1B2
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBuff_keyword", false, null);
		this.name = BuffManager.GetBuff(this.ID).Name;
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x001C0FDC File Offset: 0x001BF1DC
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.name);
		this.StatusText = "";
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x0600479A RID: 18330 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x001C1008 File Offset: 0x001BF208
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x001C1048 File Offset: 0x001BF248
	public override BaseObjective Clone()
	{
		ObjectiveBuff objectiveBuff = new ObjectiveBuff();
		this.CopyValues(objectiveBuff);
		return objectiveBuff;
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x001C1064 File Offset: 0x001BF264
	public override string ParseBinding(string bindingName)
	{
		string id = this.ID;
		string value = this.Value;
		if (bindingName == "name")
		{
			return BuffManager.GetBuff(id).Name;
		}
		return "";
	}

	// Token: 0x0400396A RID: 14698
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
