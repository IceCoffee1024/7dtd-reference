using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000997 RID: 2455
[Preserve]
public class ObjectiveBlockActivate : BaseObjective
{
	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x0600473B RID: 18235 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x001C1E48 File Offset: 0x001C0048
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockActivate_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
	}

	// Token: 0x0600473D RID: 18237 RVA: 0x001C1E9B File Offset: 0x001C009B
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
	}

	// Token: 0x0600473E RID: 18238 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x0600473F RID: 18239 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x06004740 RID: 18240 RVA: 0x001C1EB4 File Offset: 0x001C00B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockActivate(string blockName)
	{
		if (base.Complete)
		{
			return;
		}
		if ((this.ID == null || this.ID == "" || this.ID.EqualsCaseInsensitive(blockName)) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x001C1F0C File Offset: 0x001C010C
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x001C1F44 File Offset: 0x001C0144
	public override BaseObjective Clone()
	{
		ObjectiveBlockActivate objectiveBlockActivate = new ObjectiveBlockActivate();
		this.CopyValues(objectiveBlockActivate);
		return objectiveBlockActivate;
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
	}

	// Token: 0x0400397F RID: 14719
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";
}
