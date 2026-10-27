using System;
using UnityEngine.Scripting;

// Token: 0x02000E82 RID: 3714
[Preserve]
public class XUiC_WorldSelectionPopup : XUiController
{
	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x060071C1 RID: 29121 RVA: 0x002BDC4C File Offset: 0x002BBE4C
	// (set) Token: 0x060071C2 RID: 29122 RVA: 0x002BDC54 File Offset: 0x002BBE54
	[XuiXmlBinding("msgTitle")]
	public string MsgTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = string.Empty;

	// Token: 0x060071C3 RID: 29123 RVA: 0x002BDC5D File Offset: 0x002BBE5D
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCancelButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.cancel();
	}

	// Token: 0x060071C4 RID: 29124 RVA: 0x002BDC65 File Offset: 0x002BBE65
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnConfirmButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.confirm();
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x060071C5 RID: 29125 RVA: 0x002BDC6D File Offset: 0x002BBE6D
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

	// Token: 0x060071C6 RID: 29126 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldSelectionList.Entry> _list, XUiC_WorldSelectionList.Entry _previousEntry, XUiC_WorldSelectionList.Entry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071C7 RID: 29127 RVA: 0x002BDC80 File Offset: 0x002BBE80
	public override void Init()
	{
		base.Init();
		XUiC_WorldSelectionPopup.id = this.windowGroup.Id;
	}

	// Token: 0x060071C8 RID: 29128 RVA: 0x002BDC98 File Offset: 0x002BBE98
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

	// Token: 0x060071C9 RID: 29129 RVA: 0x002BDD0E File Offset: 0x002BBF0E
	public static void Open(XUi _xui, string _titleCaptionKey, string _worldName, Action _onCancel, Action<PathAbstractions.AbstractedLocation> _onConfirm)
	{
		((XUiC_WorldSelectionPopup)_xui.FindWindowGroupByName(XUiC_WorldSelectionPopup.id)).open(_titleCaptionKey, _worldName, _onCancel, _onConfirm);
	}

	// Token: 0x060071CA RID: 29130 RVA: 0x002BDD2C File Offset: 0x002BBF2C
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

	// Token: 0x060071CB RID: 29131 RVA: 0x002BDDC1 File Offset: 0x002BBFC1
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

	// Token: 0x060071CC RID: 29132 RVA: 0x002BDDF0 File Offset: 0x002BBFF0
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

	// Token: 0x060071CD RID: 29133 RVA: 0x002BDE48 File Offset: 0x002BC048
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onConfirm = null;
	}

	// Token: 0x040055FB RID: 22011
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040055FD RID: 22013
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040055FE RID: 22014
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x040055FF RID: 22015
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005600 RID: 22016
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<PathAbstractions.AbstractedLocation> onConfirm;

	// Token: 0x04005601 RID: 22017
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldSelectionList worldList;
}
