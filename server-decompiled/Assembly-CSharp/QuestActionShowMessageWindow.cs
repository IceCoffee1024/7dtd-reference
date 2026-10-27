using System;
using UnityEngine.Scripting;

// Token: 0x0200097A RID: 2426
[Preserve]
public class QuestActionShowMessageWindow : BaseQuestAction
{
	// Token: 0x0600465E RID: 18014 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x001BF89D File Offset: 0x001BDA9D
	public override void PerformAction(Quest ownerQuest)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x001BF8D4 File Offset: 0x001BDAD4
	public override BaseQuestAction Clone()
	{
		QuestActionShowMessageWindow questActionShowMessageWindow = new QuestActionShowMessageWindow();
		base.CopyValues(questActionShowMessageWindow);
		questActionShowMessageWindow.message = this.message;
		questActionShowMessageWindow.title = this.title;
		return questActionShowMessageWindow;
	}

	// Token: 0x06004661 RID: 18017 RVA: 0x001BF907 File Offset: 0x001BDB07
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(QuestActionShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x040038EC RID: 14572
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x040038ED RID: 14573
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x040038EE RID: 14574
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x040038EF RID: 14575
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
