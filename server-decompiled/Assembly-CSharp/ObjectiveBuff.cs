using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000991 RID: 2449
[Preserve]
public class ObjectiveBuff : BaseObjective
{
	// Token: 0x06004740 RID: 18240 RVA: 0x001C1625 File Offset: 0x001BF825
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBuff_keyword", false, null);
		this.name = BuffManager.GetBuff(this.ID).Name;
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x001C164F File Offset: 0x001BF84F
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.name);
		this.StatusText = "";
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x001C167C File Offset: 0x001BF87C
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

	// Token: 0x06004745 RID: 18245 RVA: 0x001C16BC File Offset: 0x001BF8BC
	public override BaseObjective Clone()
	{
		ObjectiveBuff objectiveBuff = new ObjectiveBuff();
		this.CopyValues(objectiveBuff);
		return objectiveBuff;
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x001C16D8 File Offset: 0x001BF8D8
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

	// Token: 0x06004747 RID: 18247 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
	}

	// Token: 0x04003966 RID: 14694
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
