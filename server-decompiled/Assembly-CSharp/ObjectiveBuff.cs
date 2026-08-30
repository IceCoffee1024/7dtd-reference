using System;
using UnityEngine.Scripting;

// Token: 0x0200097A RID: 2426
[Preserve]
public class ObjectiveBuff : BaseObjective
{
	// Token: 0x06004711 RID: 18193 RVA: 0x001BE7BE File Offset: 0x001BC9BE
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBuff_keyword", false, null);
		this.name = BuffManager.GetBuff(this.ID).Name;
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x001BE7E8 File Offset: 0x001BC9E8
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.name);
		this.StatusText = "";
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x001BE814 File Offset: 0x001BCA14
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

	// Token: 0x06004716 RID: 18198 RVA: 0x001BE854 File Offset: 0x001BCA54
	public override BaseObjective Clone()
	{
		ObjectiveBuff objectiveBuff = new ObjectiveBuff();
		this.CopyValues(objectiveBuff);
		return objectiveBuff;
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x001BE870 File Offset: 0x001BCA70
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

	// Token: 0x040038D9 RID: 14553
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
