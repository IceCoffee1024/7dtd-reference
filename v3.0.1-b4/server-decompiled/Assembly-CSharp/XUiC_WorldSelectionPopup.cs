using System;
using UnityEngine.Scripting;

// Token: 0x02000E53 RID: 3667
[Preserve]
public class XUiC_WorldSelectionPopup : XUiController
{
	// Token: 0x17000CD4 RID: 3284
	// (get) Token: 0x06007104 RID: 28932 RVA: 0x002B5E0C File Offset: 0x002B400C
	// (set) Token: 0x06007105 RID: 28933 RVA: 0x002B5E14 File Offset: 0x002B4014
	[XuiXmlBinding("msgTitle")]
	public string MsgTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = string.Empty;

	// Token: 0x06007106 RID: 28934 RVA: 0x002B5E1D File Offset: 0x002B401D
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCancelButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.cancel();
	}

	// Token: 0x06007107 RID: 28935 RVA: 0x002B5E25 File Offset: 0x002B4025
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnConfirmButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.confirm();
	}

	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x06007108 RID: 28936 RVA: 0x002B5E2D File Offset: 0x002B402D
	[XuiXmlBinding("isWorldSelected")]
	public bool IsWorldSelected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_WorldSelectionList xuiC_WorldSelectionList = this.worldList;
			return xuiC_WorldSelectionList != null && xuiC_WorldSelectionList.HasSelection;
		}
	}

	// Token: 0x06007109 RID: 28937 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldSelectionList.Entry> _list, XUiC_WorldSelectionList.Entry _previousEntry, XUiC_WorldSelectionList.Entry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600710A RID: 28938 RVA: 0x002B5E40 File Offset: 0x002B4040
	public override void Init()
	{
		base.Init();
		XUiC_WorldSelectionPopup.id = this.windowGroup.Id;
	}

	// Token: 0x0600710B RID: 28939 RVA: 0x002B5E58 File Offset: 0x002B4058
	[PublicizedFrom(EAccessModifier.Private)]
	public void open(string _titleCaptionKey, string _worldName, Action _onCancel, Action<PathAbstractions.AbstractedLocation> _onConfirm)
	{
		this.MsgTitle = _titleCaptionKey;
		this.onCancel = _onCancel;
		this.onConfirm = _onConfirm;
		this.worldList.SetFilter(_worldName);
		if (this.worldList.EntryCount == 1)
		{
			if (_onConfirm != null)
			{
				_onConfirm(this.worldList.GetEntry(0).Location);
			}
			return;
		}
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
	}

	// Token: 0x0600710C RID: 28940 RVA: 0x002B5ECE File Offset: 0x002B40CE
	public static void Open(XUi _xui, string _titleCaptionKey, string _worldName, Action _onCancel, Action<PathAbstractions.AbstractedLocation> _onConfirm)
	{
		((XUiC_WorldSelectionPopup)_xui.FindWindowGroupByName(XUiC_WorldSelectionPopup.id)).open(_titleCaptionKey, _worldName, _onCancel, _onConfirm);
	}

	// Token: 0x0600710D RID: 28941 RVA: 0x002B5EEC File Offset: 0x002B40EC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false))
		{
			if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasReleased)
			{
				this.cancel();
				return;
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasPressed)
			{
				this.confirm();
				return;
			}
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
		}
	}

	// Token: 0x0600710E RID: 28942 RVA: 0x002B5F81 File Offset: 0x002B4181
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancel()
	{
		Action action = this.onCancel;
		if (action != null)
		{
			action();
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600710F RID: 28943 RVA: 0x002B5FB0 File Offset: 0x002B41B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void confirm()
	{
		if (!this.worldList.HasSelection)
		{
			return;
		}
		Action<PathAbstractions.AbstractedLocation> action = this.onConfirm;
		if (action != null)
		{
			action(this.worldList.SelectedEntryData.Location);
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007110 RID: 28944 RVA: 0x002B6008 File Offset: 0x002B4208
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onConfirm = null;
	}

	// Token: 0x040054FB RID: 21755
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040054FD RID: 21757
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040054FE RID: 21758
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x040054FF RID: 21759
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005500 RID: 21760
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<PathAbstractions.AbstractedLocation> onConfirm;

	// Token: 0x04005501 RID: 21761
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldSelectionList worldList;
}
