using System;
using UnityEngine.Scripting;

// Token: 0x02000A09 RID: 2569
[Preserve]
public class RewardShowMessageWindow : BaseReward
{
	// Token: 0x06004C0B RID: 19467 RVA: 0x001DA205 File Offset: 0x001D8405
	public RewardShowMessageWindow()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004C0C RID: 19468 RVA: 0x001DA22A File Offset: 0x001D842A
	public override void SetupReward()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004C0D RID: 19469 RVA: 0x001DA233 File Offset: 0x001D8433
	public override void GiveReward(EntityPlayer player)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x001DA268 File Offset: 0x001D8468
	public override BaseReward Clone()
	{
		RewardShowMessageWindow rewardShowMessageWindow = new RewardShowMessageWindow();
		base.CopyValues(rewardShowMessageWindow);
		rewardShowMessageWindow.title = this.title;
		rewardShowMessageWindow.message = this.message;
		return rewardShowMessageWindow;
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001DA29B File Offset: 0x001D849B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(RewardShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(RewardShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x04003BFB RID: 15355
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x04003BFC RID: 15356
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x04003BFD RID: 15357
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x04003BFE RID: 15358
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
