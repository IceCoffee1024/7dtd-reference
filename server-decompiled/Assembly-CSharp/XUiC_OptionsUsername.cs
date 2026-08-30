using System;
using UnityEngine.Scripting;

// Token: 0x02000DB7 RID: 3511
[Preserve]
public class XUiC_OptionsUsername : XUiC_OptionsDialogBase
{
	// Token: 0x06006B74 RID: 27508 RVA: 0x002A0593 File Offset: 0x0029E793
	public override void Init()
	{
		base.Init();
		XUiC_OptionsUsername.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006B75 RID: 27509 RVA: 0x002A05AB File Offset: 0x0029E7AB
	[XuiBindEvent("OnSubmitHandler", "txtUsername")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtUsername_OnSubmitHandler(XUiController _sender, string _text)
	{
		base.Event_ApplyOnPress(_sender, -1);
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x002A05B5 File Offset: 0x0029E7B5
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

	// Token: 0x06006B77 RID: 27511 RVA: 0x002A05C1 File Offset: 0x0029E7C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		GamePrefs.Set(EnumGamePrefs.PlayerName, this.txtUsername.Text);
	}

	// Token: 0x06006B78 RID: 27512 RVA: 0x002A05DB File Offset: 0x0029E7DB
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtUsername.Text = GamePrefs.GetString(EnumGamePrefs.PlayerName);
	}

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x06006B79 RID: 27513 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x04005206 RID: 20998
	public static string ID = "";

	// Token: 0x04005207 RID: 20999
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtUsername;
}
