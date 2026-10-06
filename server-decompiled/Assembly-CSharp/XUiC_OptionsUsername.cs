using System;
using UnityEngine.Scripting;

// Token: 0x02000DC6 RID: 3526
[Preserve]
public class XUiC_OptionsUsername : XUiC_OptionsDialogBase
{
	// Token: 0x06006B6C RID: 27500 RVA: 0x002A3923 File Offset: 0x002A1B23
	public override void Init()
	{
		base.Init();
		XUiC_OptionsUsername.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006B6D RID: 27501 RVA: 0x002A393B File Offset: 0x002A1B3B
	[XuiBindEvent("OnSubmitHandler", "txtUsername")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtUsername_OnSubmitHandler(XUiController _sender, string _text)
	{
		base.Event_ApplyOnPress(_sender, -1);
	}

	// Token: 0x06006B6E RID: 27502 RVA: 0x002A3945 File Offset: 0x002A1B45
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

	// Token: 0x06006B6F RID: 27503 RVA: 0x002A3951 File Offset: 0x002A1B51
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		GamePrefs.Set(EnumGamePrefs.PlayerName, this.txtUsername.Text);
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x002A396B File Offset: 0x002A1B6B
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtUsername.Text = GamePrefs.GetString(EnumGamePrefs.PlayerName);
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06006B71 RID: 27505 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x0400523D RID: 21053
	public static string ID = "";

	// Token: 0x0400523E RID: 21054
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtUsername;
}
