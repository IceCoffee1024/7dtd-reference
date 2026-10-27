using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Backtrace.Unity.Model;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E0E RID: 3598
[Preserve]
public class XUiC_BugReportWindow : XUiController
{
	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06006D67 RID: 28007 RVA: 0x002ABFFD File Offset: 0x002AA1FD
	[XuiXmlBinding("intimeout")]
	public bool InTimeout
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_BugReportWindow.lastSubmissionTime >= 0f && Time.time - XUiC_BugReportWindow.lastSubmissionTime < 600f;
		}
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06006D68 RID: 28008 RVA: 0x002AC01F File Offset: 0x002AA21F
	// (set) Token: 0x06006D69 RID: 28009 RVA: 0x002AC041 File Offset: 0x002AA241
	[XuiXmlBinding("description")]
	public string Description
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TextInput xuiC_TextInput = this.txtDescription;
			return ((xuiC_TextInput != null) ? xuiC_TextInput.Text.Trim() : null) ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.txtDescription.Text = value ?? "";
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06006D6A RID: 28010 RVA: 0x002AC05F File Offset: 0x002AA25F
	[XuiXmlBinding("attach_saves_enabled")]
	public bool AttachSavesEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportAttachSaveFeature;
		}
	}

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06006D6B RID: 28011 RVA: 0x002AC066 File Offset: 0x002AA266
	[XuiXmlBinding("game_running")]
	public bool GameRunningBinding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_BugReportWindow.GameRunning;
		}
	}

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x06006D6C RID: 28012 RVA: 0x002AC06D File Offset: 0x002AA26D
	public static bool GameRunning
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x06006D6D RID: 28013 RVA: 0x002AC07E File Offset: 0x002AA27E
	public override void Init()
	{
		base.Init();
		this.windowGroup.isEscClosable = false;
		this.saveSelectWindow.GetChildByType<XUiC_BugReportSavesList>().SelectionChanged += this.List_SelectionChanged;
	}

	// Token: 0x06006D6E RID: 28014 RVA: 0x002AC0AE File Offset: 0x002AA2AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void List_SelectionChanged(XUiC_List<XUiC_BugReportSavesList.ListEntry> _list, XUiC_BugReportSavesList.ListEntry _previousEntry, XUiC_BugReportSavesList.ListEntry _newEntry)
	{
		this.selectedSaveInfo = ((_newEntry != null) ? _newEntry.SaveEntryInfo : null);
		this.IsDirty = true;
	}

	// Token: 0x06006D6F RID: 28015 RVA: 0x002AC0CC File Offset: 0x002AA2CC
	[XuiBindEvent("OnValueChanged", "comboAttachSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboAttachSave_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		if (!XUiC_BugReportWindow.GameRunning)
		{
			this.saveSelectWindow.ViewComponent.IsVisible = _newValue;
			if (!_newValue)
			{
				this.selectedSaveInfo = null;
				XUiC_BugReportSaveSelect xuiC_BugReportSaveSelect = this.saveSelectWindow;
				if (xuiC_BugReportSaveSelect != null)
				{
					xuiC_BugReportSaveSelect.List.ClearSelection();
				}
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x06006D70 RID: 28016 RVA: 0x002AC118 File Offset: 0x002AA318
	public static void Open()
	{
		LocalPlayerUI primaryUI = LocalPlayerUI.primaryUI;
		XUiC_BugReportWindow childByType = primaryUI.xui.GetChildByType<XUiC_BugReportWindow>();
		primaryUI.windowManager.Open(childByType.windowGroup, XUiC_BugReportWindow.GameRunning);
	}

	// Token: 0x06006D71 RID: 28017 RVA: 0x002AC14C File Offset: 0x002AA34C
	public override void OnOpen()
	{
		base.OnOpen();
		this.Description = "";
		this.selectedSaveInfo = null;
		this.uploading = false;
		this.comboAttachSave.Value = false;
		this.comboAttachScreenshot.Value = false;
		if (XUiC_BugReportWindow.GameRunning)
		{
			GameManager.Instance.Pause(true);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006D72 RID: 28018 RVA: 0x002AC1A9 File Offset: 0x002AA3A9
	public override void OnClose()
	{
		base.OnClose();
		this.uploading = false;
		if (XUiC_BugReportWindow.GameRunning)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x06006D73 RID: 28019 RVA: 0x002AC1CC File Offset: 0x002AA3CC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!XUiC_BugReportWindow.GameRunning && this.saveSelectWindow != null && this.saveSelectWindow.ViewComponent.IsVisible != this.comboAttachSave.Value)
		{
			this.saveSelectWindow.ViewComponent.IsVisible = this.comboAttachSave.Value;
		}
		else if (XUiC_BugReportWindow.GameRunning && this.saveSelectWindow != null && this.saveSelectWindow.ViewComponent.IsVisible)
		{
			this.saveSelectWindow.ViewComponent.IsVisible = false;
		}
		if (!this.uploading && (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased || this.xui.playerUI.playerInput.GUIActions.Cancel.WasReleased))
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006D74 RID: 28020 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("OnChangeHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtDescriptionOnChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006D75 RID: 28021 RVA: 0x002AC2C8 File Offset: 0x002AA4C8
	[XuiBindEvent("OnPress", "btnSubmit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSubmitOnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnSubmit.ViewComponent.Enabled)
		{
			return;
		}
		XUiC_BugReportWindow.lastSubmissionTime = Time.time;
		ThreadManager.StartCoroutine(this.submitRoutine());
	}

	// Token: 0x06006D76 RID: 28022 RVA: 0x002AC2F3 File Offset: 0x002AA4F3
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator submitRoutine()
	{
		string saveName = "";
		string saveDir = "";
		string worldName = "";
		PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.AbstractedLocation.None;
		if (this.comboAttachSave.Value)
		{
			if (XUiC_BugReportWindow.GameRunning)
			{
				saveName = GamePrefs.GetString(EnumGamePrefs.GameName);
				worldName = GamePrefs.GetString(EnumGamePrefs.GameWorld);
				if (GameManager.Instance.World.IsRemote())
				{
					saveDir = GameIO.GetSaveGameLocalDir();
				}
				else
				{
					saveDir = GameIO.GetSaveGameDir();
				}
				abstractedLocation = PathAbstractions.Contextual.FindActiveWorldLocation();
			}
			else
			{
				saveName = this.selectedSaveInfo.Name;
				saveDir = this.selectedSaveInfo.SaveDir;
				worldName = this.selectedSaveInfo.WorldEntry.Name;
				if (this.selectedSaveInfo.WorldEntry.Location.Type == PathAbstractions.EAbstractedLocationType.None && !this.selectedSaveInfo.WorldEntry.Type.Equals(SaveInfoProvider.DeletedWorldsType))
				{
					XUiC_BugReportWindow.<>c__DisplayClass29_0 CS$<>8__locals1 = new XUiC_BugReportWindow.<>c__DisplayClass29_0();
					CS$<>8__locals1.userWorldSelection = null;
					XUiC_WorldSelectionPopup.Open(this.xui, "xuiWorldConflictBugReport", worldName, delegate
					{
						CS$<>8__locals1.userWorldSelection = new PathAbstractions.AbstractedLocation?(PathAbstractions.AbstractedLocation.None);
					}, delegate(PathAbstractions.AbstractedLocation _location)
					{
						CS$<>8__locals1.userWorldSelection = new PathAbstractions.AbstractedLocation?(_location);
					});
					while (CS$<>8__locals1.userWorldSelection == null)
					{
						yield return null;
					}
					if (CS$<>8__locals1.userWorldSelection.Value.Type == PathAbstractions.EAbstractedLocationType.None)
					{
						yield break;
					}
					abstractedLocation = CS$<>8__locals1.userWorldSelection.Value;
					CS$<>8__locals1 = null;
				}
				else
				{
					abstractedLocation = this.selectedSaveInfo.WorldEntry.Location;
				}
			}
			PathAbstractions.EAbstractedLocationType type = abstractedLocation.Type;
			if (type - PathAbstractions.EAbstractedLocationType.LocalSave > 1)
			{
				abstractedLocation = PathAbstractions.AbstractedLocation.None;
			}
		}
		string worldDir = ((abstractedLocation.Type != PathAbstractions.EAbstractedLocationType.None) ? abstractedLocation.FullPath : null);
		this.uploading = true;
		string screenshotPath = null;
		if (this.comboAttachScreenshot.Value)
		{
			base.ViewComponent.IsVisible = false;
			GameManager.Instance.Pause(false);
			int num;
			for (int i = 0; i < 10; i = num + 1)
			{
				yield return null;
				num = i;
			}
			yield return ThreadManager.CoroutineWrapperWithExceptionCallback(GameUtils.TakeScreenshotEnum(GameUtils.EScreenshotMode.File, PlatformApplicationManager.Application.temporaryCachePath + "/" + Application.productName, 0f, false, 0, 0, false), new Action<Exception>(Log.Exception));
			GameManager.Instance.Pause(true);
			yield return null;
			base.ViewComponent.IsVisible = true;
			screenshotPath = GameUtils.lastSavedScreenshotFilename;
		}
		yield return null;
		XUiC_ProgressWindow.Open(this.xui.playerUI, Localization.Get("xuiBugReportUploading", false, null), null, false, true, true);
		yield return new WaitForSecondsRealtime(0.5f);
		BacktraceUtils.SendBugReport(this.Description, worldName, saveName, worldDir, saveDir, this.comboAttachSave.Value, screenshotPath, new Action<BacktraceResult>(this.<submitRoutine>g__BugReportCallBack|29_0));
		yield break;
	}

	// Token: 0x06006D77 RID: 28023 RVA: 0x002AC304 File Offset: 0x002AA504
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePostSubmissionClose()
	{
		XUiC_ProgressWindow.Close(this.xui.playerUI);
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiBugReportHeader", false, null), Localization.Get("xuiBugReportSubmitted", false, null), "", null, false, XUiC_BugReportWindow.GameRunning, false);
	}

	// Token: 0x06006D78 RID: 28024 RVA: 0x002AC36D File Offset: 0x002AA56D
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelOnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06006D7B RID: 28027 RVA: 0x002AC397 File Offset: 0x002AA597
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <submitRoutine>g__BugReportCallBack|29_0(BacktraceResult _result)
	{
		Log.Out("Bug Report Send callback: {0}", new object[] { (_result == null) ? "null" : _result.message });
		this.handlePostSubmissionClose();
	}

	// Token: 0x040053AA RID: 21418
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastSubmissionTime = -1f;

	// Token: 0x040053AB RID: 21419
	[XuiBindComponent("txtDescription", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x040053AC RID: 21420
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x040053AD RID: 21421
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040053AE RID: 21422
	[XuiBindComponent("comboAttachScreenshot", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboAttachScreenshot;

	// Token: 0x040053AF RID: 21423
	[XuiBindComponent("comboAttachSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboAttachSave;

	// Token: 0x040053B0 RID: 21424
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_BugReportSaveSelect saveSelectWindow;

	// Token: 0x040053B1 RID: 21425
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.SaveEntryInfo selectedSaveInfo;

	// Token: 0x040053B2 RID: 21426
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uploading;
}
