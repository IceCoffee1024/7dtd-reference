using System;
using UnityEngine.Scripting;

// Token: 0x02000F52 RID: 3922
[Preserve]
public class XUiC_ExportPrefab : XUiController
{
	// Token: 0x06007A7B RID: 31355 RVA: 0x002E618C File Offset: 0x002E438C
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

	// Token: 0x06007A7C RID: 31356 RVA: 0x002E629C File Offset: 0x002E449C
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

	// Token: 0x06007A7D RID: 31357 RVA: 0x002E6322 File Offset: 0x002E4522
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SaveAndClose(false);
	}

	// Token: 0x06007A7E RID: 31358 RVA: 0x002E632B File Offset: 0x002E452B
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSaveLocal_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SaveAndClose(true);
	}

	// Token: 0x06007A7F RID: 31359 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007A80 RID: 31360 RVA: 0x002E6334 File Offset: 0x002E4534
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

	// Token: 0x06007A81 RID: 31361 RVA: 0x002E63CA File Offset: 0x002E45CA
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtSaveName.Text = "";
		this.TxtSaveNameOnOnChangeHandler(this, "", true);
		this.IsDirty = true;
	}

	// Token: 0x06007A82 RID: 31362 RVA: 0x002E63F6 File Offset: 0x002E45F6
	public static void Open(XUi _xui)
	{
		_xui.playerUI.windowManager.Open(XUiC_ExportPrefab.ID, true);
	}

	// Token: 0x04005AD8 RID: 23256
	public static string ID = "";

	// Token: 0x04005AD9 RID: 23257
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSaveName;

	// Token: 0x04005ADA RID: 23258
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x04005ADB RID: 23259
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSaveLocal;

	// Token: 0x04005ADC RID: 23260
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblPrefabExists;

	// Token: 0x04005ADD RID: 23261
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblInvalidName;

	// Token: 0x04005ADE RID: 23262
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleAsPart;
}
