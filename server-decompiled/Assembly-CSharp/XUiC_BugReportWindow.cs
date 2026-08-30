using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Backtrace.Unity.Model;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DF4 RID: 3572
[Preserve]
public class XUiC_BugReportWindow : XUiController
{
	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06006D4A RID: 27978 RVA: 0x002A7745 File Offset: 0x002A5945
	[XuiXmlBinding("intimeout")]
	public bool InTimeout
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_BugReportWindow.lastSubmissionTime >= 0f && Time.time - XUiC_BugReportWindow.lastSubmissionTime < 600f;
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06006D4B RID: 27979 RVA: 0x002A7767 File Offset: 0x002A5967
	// (set) Token: 0x06006D4C RID: 27980 RVA: 0x002A7789 File Offset: 0x002A5989
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

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x06006D4D RID: 27981 RVA: 0x002A77A7 File Offset: 0x002A59A7
	[XuiXmlBinding("attach_saves_enabled")]
	public bool AttachSavesEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportAttachSaveFeature;
		}
	}

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x06006D4E RID: 27982 RVA: 0x002A77AE File Offset: 0x002A59AE
	[XuiXmlBinding("game_running")]
	public bool GameRunningBinding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_BugReportWindow.GameRunning;
		}
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06006D4F RID: 27983 RVA: 0x002A77B5 File Offset: 0x002A59B5
	public static bool GameRunning
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x06006D50 RID: 27984 RVA: 0x002A77C6 File Offset: 0x002A59C6
	public override void Init()
	{
		base.Init();
		this.windowGroup.isEscClosable = false;
		this.saveSelectWindow.GetChildByType<XUiC_BugReportSavesList>().SelectionChanged += this.List_SelectionChanged;
	}

	// Token: 0x06006D51 RID: 27985 RVA: 0x002A77F6 File Offset: 0x002A59F6
	[PublicizedFrom(EAccessModifier.Private)]
	public void List_SelectionChanged(XUiC_List<XUiC_BugReportSavesList.ListEntry> _list, XUiC_BugReportSavesList.ListEntry _previousEntry, XUiC_BugReportSavesList.ListEntry _newEntry)
	{
		this.selectedSaveInfo = ((_newEntry != null) ? _newEntry.SaveEntryInfo : null);
		this.IsDirty = true;
	}

	// Token: 0x06006D52 RID: 27986 RVA: 0x002A7814 File Offset: 0x002A5A14
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

	// Token: 0x06006D53 RID: 27987 RVA: 0x002A7860 File Offset: 0x002A5A60
	public static void Open()
	{
		LocalPlayerUI primaryUI = LocalPlayerUI.primaryUI;
		XUiC_BugReportWindow childByType = primaryUI.xui.GetChildByType<XUiC_BugReportWindow>();
		primaryUI.windowManager.Open(childByType.windowGroup, XUiC_BugReportWindow.GameRunning);
	}

	// Token: 0x06006D54 RID: 27988 RVA: 0x002A7894 File Offset: 0x002A5A94
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

	// Token: 0x06006D55 RID: 27989 RVA: 0x002A78F1 File Offset: 0x002A5AF1
	public override void OnClose()
	{
		base.OnClose();
		this.uploading = false;
		if (XUiC_BugReportWindow.GameRunning)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x06006D56 RID: 27990 RVA: 0x002A7914 File Offset: 0x002A5B14
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

	// Token: 0x06006D57 RID: 27991 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("OnChangeHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtDescriptionOnChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006D58 RID: 27992 RVA: 0x002A7A10 File Offset: 0x002A5C10
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

	// Token: 0x06006D59 RID: 27993 RVA: 0x002A7A3B File Offset: 0x002A5C3B
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

	// Token: 0x06006D5A RID: 27994 RVA: 0x002A7A4C File Offset: 0x002A5C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePostSubmissionClose()
	{
		XUiC_ProgressWindow.Close(this.xui.playerUI);
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiBugReportHeader", false, null), Localization.Get("xuiBugReportSubmitted", false, null), "", null, false, XUiC_BugReportWindow.GameRunning, false);
	}

	// Token: 0x06006D5B RID: 27995 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelOnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06006D5E RID: 27998 RVA: 0x002A7ADF File Offset: 0x002A5CDF
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <submitRoutine>g__BugReportCallBack|29_0(BacktraceResult _result)
	{
		Log.Out("Bug Report Send callback: {0}", new object[] { (_result == null) ? "null" : _result.message });
		this.handlePostSubmissionClose();
	}

	// Token: 0x0400534E RID: 21326
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastSubmissionTime = -1f;

	// Token: 0x0400534F RID: 21327
	[XuiBindComponent("txtDescription", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x04005350 RID: 21328
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x04005351 RID: 21329
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005352 RID: 21330
	[XuiBindComponent("comboAttachScreenshot", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboAttachScreenshot;

	// Token: 0x04005353 RID: 21331
	[XuiBindComponent("comboAttachSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboAttachSave;

	// Token: 0x04005354 RID: 21332
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_BugReportSaveSelect saveSelectWindow;

	// Token: 0x04005355 RID: 21333
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.SaveEntryInfo selectedSaveInfo;

	// Token: 0x04005356 RID: 21334
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uploading;
}
