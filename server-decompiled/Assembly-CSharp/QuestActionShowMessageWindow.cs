using System;
using UnityEngine.Scripting;

// Token: 0x0200096A RID: 2410
[Preserve]
public class QuestActionShowMessageWindow : BaseQuestAction
{
	// Token: 0x0600469A RID: 18074 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600469B RID: 18075 RVA: 0x001BE311 File Offset: 0x001BC511
	public override void PerformAction(Quest ownerQuest)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x001BE348 File Offset: 0x001BC548
	public override BaseQuestAction Clone()
	{
		QuestActionShowMessageWindow questActionShowMessageWindow = new QuestActionShowMessageWindow();
		base.CopyValues(questActionShowMessageWindow);
		questActionShowMessageWindow.message = this.message;
		questActionShowMessageWindow.title = this.title;
		return questActionShowMessageWindow;
	}

	// Token: 0x0600469D RID: 18077 RVA: 0x001BE37B File Offset: 0x001BC57B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(QuestActionShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x040038D0 RID: 14544
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x040038D1 RID: 14545
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x040038D2 RID: 14546
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x040038D3 RID: 14547
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
