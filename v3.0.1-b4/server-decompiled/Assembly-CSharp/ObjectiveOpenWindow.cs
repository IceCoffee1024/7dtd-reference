using System;
using UnityEngine.Scripting;

// Token: 0x0200098B RID: 2443
[Preserve]
public class ObjectiveOpenWindow : BaseObjective
{
	// Token: 0x060047D9 RID: 18393 RVA: 0x001C22D8 File Offset: 0x001C04D8
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveOpenWindow_keyword", false, null);
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x001C22EC File Offset: 0x001C04EC
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.ID);
		this.StatusText = "";
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x001C2317 File Offset: 0x001C0517
	public override void AddHooks()
	{
		QuestEventManager.Current.WindowChanged += this.Current_WindowOpened;
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x001C232F File Offset: 0x001C052F
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WindowChanged -= this.Current_WindowOpened;
	}

	// Token: 0x060047DD RID: 18397 RVA: 0x001C2347 File Offset: 0x001C0547
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WindowOpened(string windowName)
	{
		if (windowName.EqualsCaseInsensitive(this.ID) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x060047DE RID: 18398 RVA: 0x001C2374 File Offset: 0x001C0574
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x001C23AC File Offset: 0x001C05AC
	public override BaseObjective Clone()
	{
		ObjectiveOpenWindow objectiveOpenWindow = new ObjectiveOpenWindow();
		this.CopyValues(objectiveOpenWindow);
		return objectiveOpenWindow;
	}
}
