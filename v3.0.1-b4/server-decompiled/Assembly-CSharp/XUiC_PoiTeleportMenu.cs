using System;
using UnityEngine.Scripting;

// Token: 0x02000F95 RID: 3989
[Preserve]
public class XUiC_PoiTeleportMenu : XUiController
{
	// Token: 0x06007DD4 RID: 32212 RVA: 0x002FD7E0 File Offset: 0x002FB9E0
	public override void Init()
	{
		base.Init();
		XUiC_PoiTeleportMenu.ID = base.WindowGroup.Id;
		this.list = base.GetChildByType<XUiC_PoiList>();
		this.list.ListEntryClicked += this.onListEntryClicked;
		XUiController childById = base.GetChildById("filterSmall");
		XUiC_ToggleButton xuiC_ToggleButton = ((childById != null) ? childById.GetChildByType<XUiC_ToggleButton>() : null);
		xuiC_ToggleButton.Value = this.list.FilterSmallPois;
		xuiC_ToggleButton.OnValueChanged += this.FilterSmall_Changed;
		XUiController childById2 = base.GetChildById("cbxFilterTier");
		this.cbxFilterTier = ((childById2 != null) ? childById2.GetChildByType<XUiC_ComboBoxInt>() : null);
		this.cbxFilterTier.Value = (long)this.list.FilterTier;
		this.cbxFilterTier.OnValueChanged += this.CbxFilterTier_OnValueChanged;
	}

	// Token: 0x06007DD5 RID: 32213 RVA: 0x002FD8AA File Offset: 0x002FBAAA
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxFilterTier_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.list.FilterTier = (int)_newValue;
	}

	// Token: 0x06007DD6 RID: 32214 RVA: 0x002FD8B9 File Offset: 0x002FBAB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void FilterSmall_Changed(XUiC_ToggleButton _sender, bool _newvalue)
	{
		this.list.FilterSmallPois = _newvalue;
	}

	// Token: 0x06007DD7 RID: 32215 RVA: 0x002FD8C7 File Offset: 0x002FBAC7
	[PublicizedFrom(EAccessModifier.Private)]
	public void onListEntryClicked(XUiC_List<XUiC_PoiList.PoiListEntry> _list, XUiC_PoiList.PoiListEntry _entry)
	{
		if (_entry != null)
		{
			this.EntryPressed(_entry);
		}
	}

	// Token: 0x06007DD8 RID: 32216 RVA: 0x002FD8D3 File Offset: 0x002FBAD3
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntryPressed(XUiC_PoiList.PoiListEntry _key)
	{
		this.xui.playerUI.entityPlayer.Teleport(_key.prefabInstance.boundingBoxPosition.ToVector3(), 45f);
	}

	// Token: 0x06007DD9 RID: 32217 RVA: 0x002FD8FF File Offset: 0x002FBAFF
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxFilterTier.Max = (long)this.list.MaxTier;
	}

	// Token: 0x04005D4C RID: 23884
	public static string ID = "";

	// Token: 0x04005D4D RID: 23885
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PoiList list;

	// Token: 0x04005D4E RID: 23886
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxFilterTier;
}
