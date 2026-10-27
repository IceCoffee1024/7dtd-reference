using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200099B RID: 2459
[Preserve]
public class ObjectiveBuff : BaseObjective
{
	// Token: 0x06004763 RID: 18275 RVA: 0x001C26A9 File Offset: 0x001C08A9
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBuff_keyword", false, null);
		this.name = BuffManager.GetBuff(this.ID).Name;
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x001C26D3 File Offset: 0x001C08D3
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.name);
		this.StatusText = "";
	}

	// Token: 0x06004765 RID: 18277 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x001C2700 File Offset: 0x001C0900
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

	// Token: 0x06004768 RID: 18280 RVA: 0x001C2740 File Offset: 0x001C0940
	public override BaseObjective Clone()
	{
		ObjectiveBuff objectiveBuff = new ObjectiveBuff();
		this.CopyValues(objectiveBuff);
		return objectiveBuff;
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x001C275C File Offset: 0x001C095C
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

	// Token: 0x0600476A RID: 18282 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
	}

	// Token: 0x04003986 RID: 14726
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
