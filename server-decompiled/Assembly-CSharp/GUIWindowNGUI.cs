using System;

// Token: 0x0200123C RID: 4668
public class GUIWindowNGUI : GUIWindow
{
	// Token: 0x060094EC RID: 38124 RVA: 0x003857EF File Offset: 0x003839EF
	public GUIWindowNGUI(EnumNGUIWindow _nguiEnum)
		: base(_nguiEnum.ToStringCached<EnumNGUIWindow>())
	{
		this.nguiEnum = _nguiEnum;
	}

	// Token: 0x060094ED RID: 38125 RVA: 0x00385804 File Offset: 0x00383A04
	public override void OnOpen()
	{
		this.playerUI.nguiWindowManager.Show(this.nguiEnum, true);
	}

	// Token: 0x060094EE RID: 38126 RVA: 0x0038581D File Offset: 0x00383A1D
	public override void OnClose()
	{
		base.OnClose();
		this.playerUI.nguiWindowManager.Show(this.nguiEnum, false);
	}

	// Token: 0x04006F91 RID: 28561
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumNGUIWindow nguiEnum;
}
