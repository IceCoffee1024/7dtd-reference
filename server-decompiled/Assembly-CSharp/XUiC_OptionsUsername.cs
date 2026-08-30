using System;
using UnityEngine.Scripting;

// Token: 0x02000DA3 RID: 3491
[Preserve]
public class XUiC_OptionsUsername : XUiC_OptionsDialogBase
{
	// Token: 0x06006AD9 RID: 27353 RVA: 0x0029D433 File Offset: 0x0029B633
	public override void Init()
	{
		base.Init();
		XUiC_OptionsUsername.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006ADA RID: 27354 RVA: 0x0029D44B File Offset: 0x0029B64B
	[XuiBindEvent("OnSubmitHandler", "txtUsername")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtUsername_OnSubmitHandler(XUiController _sender, string _text)
	{
		base.Event_ApplyOnPress(_sender, -1);
	}

	// Token: 0x06006ADB RID: 27355 RVA: 0x0029D455 File Offset: 0x0029B655
	[XuiBindEvent("OnChangeHandler", "txtUsername")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtUsername_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_changeFromCode)
		{
			return;
		}
		base.SetChanged();
	}

	// Token: 0x06006ADC RID: 27356 RVA: 0x0029D461 File Offset: 0x0029B661
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		GamePrefs.Set(EnumGamePrefs.PlayerName, this.txtUsername.Text);
	}

	// Token: 0x06006ADD RID: 27357 RVA: 0x0029D47B File Offset: 0x0029B67B
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtUsername.Text = GamePrefs.GetString(EnumGamePrefs.PlayerName);
	}

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x06006ADE RID: 27358 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x04005164 RID: 20836
	public static string ID = "";

	// Token: 0x04005165 RID: 20837
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtUsername;
}
