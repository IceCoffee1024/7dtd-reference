using System;
using UnityEngine.Scripting;

// Token: 0x02000F34 RID: 3892
[Preserve]
public class XUiC_ExportPrefab : XUiController
{
	// Token: 0x06007A3B RID: 31291 RVA: 0x002E152C File Offset: 0x002DF72C
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

	// Token: 0x06007A3C RID: 31292 RVA: 0x002E163C File Offset: 0x002DF83C
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

	// Token: 0x06007A3D RID: 31293 RVA: 0x002E16C2 File Offset: 0x002DF8C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SaveAndClose(false);
	}

	// Token: 0x06007A3E RID: 31294 RVA: 0x002E16CB File Offset: 0x002DF8CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSaveLocal_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SaveAndClose(true);
	}

	// Token: 0x06007A3F RID: 31295 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007A40 RID: 31296 RVA: 0x002E16D4 File Offset: 0x002DF8D4
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

	// Token: 0x06007A41 RID: 31297 RVA: 0x002E176A File Offset: 0x002DF96A
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtSaveName.Text = "";
		this.TxtSaveNameOnOnChangeHandler(this, "", true);
		this.IsDirty = true;
	}

	// Token: 0x06007A42 RID: 31298 RVA: 0x002E1796 File Offset: 0x002DF996
	public static void Open(XUi _xui)
	{
		_xui.playerUI.windowManager.Open(XUiC_ExportPrefab.ID, true);
	}

	// Token: 0x04005A6D RID: 23149
	public static string ID = "";

	// Token: 0x04005A6E RID: 23150
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSaveName;

	// Token: 0x04005A6F RID: 23151
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x04005A70 RID: 23152
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSaveLocal;

	// Token: 0x04005A71 RID: 23153
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblPrefabExists;

	// Token: 0x04005A72 RID: 23154
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblInvalidName;

	// Token: 0x04005A73 RID: 23155
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleAsPart;
}
