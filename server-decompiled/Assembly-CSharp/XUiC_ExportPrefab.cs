using System;
using UnityEngine.Scripting;

// Token: 0x02000F1D RID: 3869
[Preserve]
public class XUiC_ExportPrefab : XUiController
{
	// Token: 0x06007982 RID: 31106 RVA: 0x002DD578 File Offset: 0x002DB778
	public override void Init()
	{
		base.Init();
		XUiC_ExportPrefab.ID = base.WindowGroup.Id;
		this.btnSave = (XUiC_SimpleButton)base.GetChildById("btnSave");
		this.btnSave.OnPressed += this.BtnSave_OnPressed;
		this.btnSaveLocal = (XUiC_SimpleButton)base.GetChildById("btnSaveLocal");
		this.btnSaveLocal.OnPressed += this.BtnSaveLocal_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		this.txtSaveName = (XUiC_TextInput)base.GetChildById("txtSaveName");
		this.txtSaveName.OnChangeHandler += this.TxtSaveNameOnOnChangeHandler;
		this.lblPrefabExists = base.GetChildById("lblPrefabExists").ViewComponent as XUiV_Label;
		this.lblInvalidName = base.GetChildById("lblInvalidName").ViewComponent as XUiV_Label;
		this.toggleAsPart = base.GetChildByType<XUiC_ToggleButton>();
	}

	// Token: 0x06007983 RID: 31107 RVA: 0x002DD688 File Offset: 0x002DB888
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtSaveNameOnOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		bool flag = _text.Length > 0 && !_text.Contains(" ") && GameUtils.ValidateGameName(_text);
		bool flag2 = flag && false;
		this.lblPrefabExists.IsVisible = flag2;
		this.lblInvalidName.IsVisible = !flag;
		this.btnSave.Enabled = flag && SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient && !flag2;
		this.btnSaveLocal.Enabled = flag && !flag2;
	}

	// Token: 0x06007984 RID: 31108 RVA: 0x002DD70E File Offset: 0x002DB90E
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SaveAndClose(false);
	}

	// Token: 0x06007985 RID: 31109 RVA: 0x002DD717 File Offset: 0x002DB917
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSaveLocal_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SaveAndClose(true);
	}

	// Token: 0x06007986 RID: 31110 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007987 RID: 31111 RVA: 0x002DD720 File Offset: 0x002DB920
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveAndClose(bool _local)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		string text = ConsoleCmdExportPrefab.BuildCommandString(this.txtSaveName.Text, BlockToolSelection.Instance.SelectionStart, BlockToolSelection.Instance.SelectionEnd, this.toggleAsPart.Value);
		if (_local || SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GUIWindowConsole.AddLines(SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(text, null));
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(text), false);
		}
		GUIWindowConsole.Open();
	}

	// Token: 0x06007988 RID: 31112 RVA: 0x002DD7B6 File Offset: 0x002DB9B6
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtSaveName.Text = "";
		this.TxtSaveNameOnOnChangeHandler(this, "", true);
		this.IsDirty = true;
	}

	// Token: 0x06007989 RID: 31113 RVA: 0x002DD7E2 File Offset: 0x002DB9E2
	public static void Open(XUi _xui)
	{
		_xui.playerUI.windowManager.Open(XUiC_ExportPrefab.ID, true);
	}

	// Token: 0x040059BA RID: 22970
	public static string ID = "";

	// Token: 0x040059BB RID: 22971
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSaveName;

	// Token: 0x040059BC RID: 22972
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x040059BD RID: 22973
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSaveLocal;

	// Token: 0x040059BE RID: 22974
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblPrefabExists;

	// Token: 0x040059BF RID: 22975
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblInvalidName;

	// Token: 0x040059C0 RID: 22976
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleAsPart;
}
