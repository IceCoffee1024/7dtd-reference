using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Backtrace.Unity.Model;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DDF RID: 3551
[Preserve]
public class XUiC_BugReportWindow : XUiController
{
	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x06006CAB RID: 27819 RVA: 0x002A42B1 File Offset: 0x002A24B1
	[XuiXmlBinding("intimeout")]
	public bool InTimeout
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_BugReportWindow.lastSubmissionTime >= 0f && Time.time - XUiC_BugReportWindow.lastSubmissionTime < 600f;
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x06006CAC RID: 27820 RVA: 0x002A42D3 File Offset: 0x002A24D3
	// (set) Token: 0x06006CAD RID: 27821 RVA: 0x002A42F5 File Offset: 0x002A24F5
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

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x06006CAE RID: 27822 RVA: 0x002A4313 File Offset: 0x002A2513
	[XuiXmlBinding("attach_saves_enabled")]
	public bool AttachSavesEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportAttachSaveFeature;
		}
	}

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x06006CAF RID: 27823 RVA: 0x002A431A File Offset: 0x002A251A
	[XuiXmlBinding("game_running")]
	public bool GameRunningBinding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_BugReportWindow.GameRunning;
		}
	}

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x06006CB0 RID: 27824 RVA: 0x002A4321 File Offset: 0x002A2521
	public static bool GameRunning
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x06006CB1 RID: 27825 RVA: 0x002A4332 File Offset: 0x002A2532
	public override void Init()
	{
		base.Init();
		this.windowGroup.isEscClosable = false;
		this.saveSelectWindow.GetChildByType<XUiC_BugReportSavesList>().SelectionChanged += this.List_SelectionChanged;
	}

	// Token: 0x06006CB2 RID: 27826 RVA: 0x002A4362 File Offset: 0x002A2562
	[PublicizedFrom(EAccessModifier.Private)]
	public void List_SelectionChanged(XUiC_List<XUiC_BugReportSavesList.ListEntry> _list, XUiC_BugReportSavesList.ListEntry _previousEntry, XUiC_BugReportSavesList.ListEntry _newEntry)
	{
		this.selectedSaveInfo = ((_newEntry != null) ? _newEntry.SaveEntryInfo : null);
		this.IsDirty = true;
	}

	// Token: 0x06006CB3 RID: 27827 RVA: 0x002A4380 File Offset: 0x002A2580
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

	// Token: 0x06006CB4 RID: 27828 RVA: 0x002A43CC File Offset: 0x002A25CC
	public static void Open()
	{
		LocalPlayerUI primaryUI = LocalPlayerUI.primaryUI;
		XUiC_BugReportWindow childByType = primaryUI.xui.GetChildByType<XUiC_BugReportWindow>();
		primaryUI.windowManager.Open(childByType.windowGroup, XUiC_BugReportWindow.GameRunning);
	}

	// Token: 0x06006CB5 RID: 27829 RVA: 0x002A4400 File Offset: 0x002A2600
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

	// Token: 0x06006CB6 RID: 27830 RVA: 0x002A445D File Offset: 0x002A265D
	public override void OnClose()
	{
		base.OnClose();
		this.uploading = false;
		if (XUiC_BugReportWindow.GameRunning)
		{
			GameManager.Instance.Pause(false);
		}
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x002A4480 File Offset: 0x002A2680
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

	// Token: 0x06006CB8 RID: 27832 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("OnChangeHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtDescriptionOnChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006CB9 RID: 27833 RVA: 0x002A457C File Offset: 0x002A277C
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

	// Token: 0x06006CBA RID: 27834 RVA: 0x002A45A7 File Offset: 0x002A27A7
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

	// Token: 0x06006CBB RID: 27835 RVA: 0x002A45B8 File Offset: 0x002A27B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePostSubmissionClose()
	{
		XUiC_ProgressWindow.Close(this.xui.playerUI);
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiBugReportHeader", false, null), Localization.Get("xuiBugReportSubmitted", false, null), "", null, false, XUiC_BugReportWindow.GameRunning, false);
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x002A4621 File Offset: 0x002A2821
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelOnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06006CBF RID: 27839 RVA: 0x002A464B File Offset: 0x002A284B
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <submitRoutine>g__BugReportCallBack|29_0(BacktraceResult _result)
	{
		Log.Out("Bug Report Send callback: {0}", new object[] { (_result == null) ? "null" : _result.message });
		this.handlePostSubmissionClose();
	}

	// Token: 0x040052AB RID: 21163
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastSubmissionTime = -1f;

	// Token: 0x040052AC RID: 21164
	[XuiBindComponent("txtDescription", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x040052AD RID: 21165
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x040052AE RID: 21166
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040052AF RID: 21167
	[XuiBindComponent("comboAttachScreenshot", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboAttachScreenshot;

	// Token: 0x040052B0 RID: 21168
	[XuiBindComponent("comboAttachSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboAttachSave;

	// Token: 0x040052B1 RID: 21169
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_BugReportSaveSelect saveSelectWindow;

	// Token: 0x040052B2 RID: 21170
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.SaveEntryInfo selectedSaveInfo;

	// Token: 0x040052B3 RID: 21171
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uploading;
}
