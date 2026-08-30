using System;
using UnityEngine.Scripting;

// Token: 0x02000987 RID: 2439
[Preserve]
public class ObjectiveBlockActivate : BaseObjective
{
	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x06004773 RID: 18291 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x001C07FC File Offset: 0x001BE9FC
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockActivate_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x001C084F File Offset: 0x001BEA4F
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x001C0868 File Offset: 0x001BEA68
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

	// Token: 0x06004779 RID: 18297 RVA: 0x001C08C0 File Offset: 0x001BEAC0
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x001C08F8 File Offset: 0x001BEAF8
	public override BaseObjective Clone()
	{
		ObjectiveBlockActivate objectiveBlockActivate = new ObjectiveBlockActivate();
		this.CopyValues(objectiveBlockActivate);
		return objectiveBlockActivate;
	}

	// Token: 0x04003963 RID: 14691
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";
}
