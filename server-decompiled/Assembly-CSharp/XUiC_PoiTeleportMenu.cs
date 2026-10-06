using System;
using UnityEngine.Scripting;

// Token: 0x02000FBD RID: 4029
[Preserve]
public class XUiC_PoiTeleportMenu : XUiController
{
	// Token: 0x06007ED5 RID: 32469 RVA: 0x00304F8C File Offset: 0x0030318C
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

	// Token: 0x06007ED6 RID: 32470 RVA: 0x00305056 File Offset: 0x00303256
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxFilterTier_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.list.FilterTier = (int)_newValue;
	}

	// Token: 0x06007ED7 RID: 32471 RVA: 0x00305065 File Offset: 0x00303265
	[PublicizedFrom(EAccessModifier.Private)]
	public void FilterSmall_Changed(XUiC_ToggleButton _sender, bool _newvalue)
	{
		this.list.FilterSmallPois = _newvalue;
	}

	// Token: 0x06007ED8 RID: 32472 RVA: 0x00305073 File Offset: 0x00303273
	[PublicizedFrom(EAccessModifier.Private)]
	public void onListEntryClicked(XUiC_List<XUiC_PoiList.PoiListEntry> _list, XUiC_PoiList.PoiListEntry _entry)
	{
		if (_entry != null)
		{
			this.EntryPressed(_entry);
		}
	}

	// Token: 0x06007ED9 RID: 32473 RVA: 0x0030507F File Offset: 0x0030327F
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntryPressed(XUiC_PoiList.PoiListEntry _key)
	{
		this.xui.playerUI.entityPlayer.Teleport(_key.prefabInstance.boundingBoxPosition.ToVector3(), 45f);
	}

	// Token: 0x06007EDA RID: 32474 RVA: 0x003050AB File Offset: 0x003032AB
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxFilterTier.Max = (long)this.list.MaxTier;
	}

	// Token: 0x04005E3C RID: 24124
	public static string ID = "";

	// Token: 0x04005E3D RID: 24125
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PoiList list;

	// Token: 0x04005E3E RID: 24126
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxFilterTier;
}
