using System;
using UnityEngine.Scripting;

// Token: 0x020009E4 RID: 2532
[Preserve]
public class RewardShowMessageWindow : BaseReward
{
	// Token: 0x06004B92 RID: 19346 RVA: 0x001D5685 File Offset: 0x001D3885
	public RewardShowMessageWindow()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x001D56AA File Offset: 0x001D38AA
	public override void SetupReward()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x001D56B3 File Offset: 0x001D38B3
	public override void GiveReward(EntityPlayer player)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x001D56E8 File Offset: 0x001D38E8
	public override BaseReward Clone()
	{
		RewardShowMessageWindow rewardShowMessageWindow = new RewardShowMessageWindow();
		base.CopyValues(rewardShowMessageWindow);
		rewardShowMessageWindow.title = this.title;
		rewardShowMessageWindow.message = this.message;
		return rewardShowMessageWindow;
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x001D571B File Offset: 0x001D391B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(RewardShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(RewardShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x04003B3C RID: 15164
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x04003B3D RID: 15165
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x04003B3E RID: 15166
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x04003B3F RID: 15167
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
