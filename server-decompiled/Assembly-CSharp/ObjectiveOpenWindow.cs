using System;
using UnityEngine.Scripting;

// Token: 0x0200099C RID: 2460
[Preserve]
public class ObjectiveOpenWindow : BaseObjective
{
	// Token: 0x0600485F RID: 18527 RVA: 0x001C4ACC File Offset: 0x001C2CCC
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveOpenWindow_keyword", false, null);
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x001C4AE0 File Offset: 0x001C2CE0
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.ID);
		this.StatusText = "";
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x001C4B0B File Offset: 0x001C2D0B
	public override void AddHooks()
	{
		QuestEventManager.Current.WindowChanged += this.Current_WindowOpened;
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x001C4B23 File Offset: 0x001C2D23
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WindowChanged -= this.Current_WindowOpened;
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x001C4B3B File Offset: 0x001C2D3B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WindowOpened(string windowName)
	{
		if (windowName.EqualsCaseInsensitive(this.ID) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x001C4B68 File Offset: 0x001C2D68
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004865 RID: 18533 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x06004866 RID: 18534 RVA: 0x001C4BA0 File Offset: 0x001C2DA0
	public override BaseObjective Clone()
	{
		ObjectiveOpenWindow objectiveOpenWindow = new ObjectiveOpenWindow();
		this.CopyValues(objectiveOpenWindow);
		return objectiveOpenWindow;
	}
}
