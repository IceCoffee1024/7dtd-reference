using System;
using UnityEngine.Scripting;

// Token: 0x02000E77 RID: 3703
[Preserve]
public class XUiC_WorldSelectionPopup : XUiController
{
	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x0600719C RID: 29084 RVA: 0x002BC6E0 File Offset: 0x002BA8E0
	// (set) Token: 0x0600719D RID: 29085 RVA: 0x002BC6E8 File Offset: 0x002BA8E8
	[XuiXmlBinding("msgTitle")]
	public string MsgTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = string.Empty;

	// Token: 0x0600719E RID: 29086 RVA: 0x002BC6F1 File Offset: 0x002BA8F1
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCancelButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.cancel();
	}

	// Token: 0x0600719F RID: 29087 RVA: 0x002BC6F9 File Offset: 0x002BA8F9
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnConfirmButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.confirm();
	}

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x060071A0 RID: 29088 RVA: 0x002BC701 File Offset: 0x002BA901
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

	// Token: 0x060071A1 RID: 29089 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldSelectionList.Entry> _list, XUiC_WorldSelectionList.Entry _previousEntry, XUiC_WorldSelectionList.Entry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071A2 RID: 29090 RVA: 0x002BC714 File Offset: 0x002BA914
	public override void Init()
	{
		base.Init();
		XUiC_WorldSelectionPopup.id = this.windowGroup.Id;
	}

	// Token: 0x060071A3 RID: 29091 RVA: 0x002BC72C File Offset: 0x002BA92C
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

	// Token: 0x060071A4 RID: 29092 RVA: 0x002BC7A2 File Offset: 0x002BA9A2
	public static void Open(XUi _xui, string _titleCaptionKey, string _worldName, Action _onCancel, Action<PathAbstractions.AbstractedLocation> _onConfirm)
	{
		((XUiC_WorldSelectionPopup)_xui.FindWindowGroupByName(XUiC_WorldSelectionPopup.id)).open(_titleCaptionKey, _worldName, _onCancel, _onConfirm);
	}

	// Token: 0x060071A5 RID: 29093 RVA: 0x002BC7C0 File Offset: 0x002BA9C0
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

	// Token: 0x060071A6 RID: 29094 RVA: 0x002BC855 File Offset: 0x002BAA55
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

	// Token: 0x060071A7 RID: 29095 RVA: 0x002BC884 File Offset: 0x002BAA84
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

	// Token: 0x060071A8 RID: 29096 RVA: 0x002BC8DC File Offset: 0x002BAADC
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onConfirm = null;
	}

	// Token: 0x040055D6 RID: 21974
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040055D8 RID: 21976
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040055D9 RID: 21977
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x040055DA RID: 21978
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x040055DB RID: 21979
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<PathAbstractions.AbstractedLocation> onConfirm;

	// Token: 0x040055DC RID: 21980
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldSelectionList worldList;
}
