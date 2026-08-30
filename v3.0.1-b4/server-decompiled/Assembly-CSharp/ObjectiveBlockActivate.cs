using System;
using UnityEngine.Scripting;

// Token: 0x02000976 RID: 2422
[Preserve]
public class ObjectiveBlockActivate : BaseObjective
{
	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x060046ED RID: 18157 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x001BE008 File Offset: 0x001BC208
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockActivate_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x001BE05B File Offset: 0x001BC25B
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x001BE074 File Offset: 0x001BC274
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

	// Token: 0x060046F3 RID: 18163 RVA: 0x001BE0CC File Offset: 0x001BC2CC
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x001BE104 File Offset: 0x001BC304
	public override BaseObjective Clone()
	{
		ObjectiveBlockActivate objectiveBlockActivate = new ObjectiveBlockActivate();
		this.CopyValues(objectiveBlockActivate);
		return objectiveBlockActivate;
	}

	// Token: 0x040038D2 RID: 14546
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";
}
