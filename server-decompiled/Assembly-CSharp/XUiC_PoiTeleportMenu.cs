using System;
using UnityEngine.Scripting;

// Token: 0x02000FCD RID: 4045
[Preserve]
public class XUiC_PoiTeleportMenu : XUiController
{
	// Token: 0x06007F24 RID: 32548 RVA: 0x00306AA8 File Offset: 0x00304CA8
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

	// Token: 0x06007F25 RID: 32549 RVA: 0x00306B72 File Offset: 0x00304D72
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxFilterTier_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.list.FilterTier = (int)_newValue;
	}

	// Token: 0x06007F26 RID: 32550 RVA: 0x00306B81 File Offset: 0x00304D81
	[PublicizedFrom(EAccessModifier.Private)]
	public void FilterSmall_Changed(XUiC_ToggleButton _sender, bool _newvalue)
	{
		this.list.FilterSmallPois = _newvalue;
	}

	// Token: 0x06007F27 RID: 32551 RVA: 0x00306B8F File Offset: 0x00304D8F
	[PublicizedFrom(EAccessModifier.Private)]
	public void onListEntryClicked(XUiC_List<XUiC_PoiList.PoiListEntry> _list, XUiC_PoiList.PoiListEntry _entry)
	{
		if (_entry != null)
		{
			this.EntryPressed(_entry);
		}
	}

	// Token: 0x06007F28 RID: 32552 RVA: 0x00306B9B File Offset: 0x00304D9B
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntryPressed(XUiC_PoiList.PoiListEntry _key)
	{
		this.xui.playerUI.entityPlayer.Teleport(_key.prefabInstance.boundingBoxPosition.ToVector3(), 45f);
	}

	// Token: 0x06007F29 RID: 32553 RVA: 0x00306BC7 File Offset: 0x00304DC7
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxFilterTier.Max = (long)this.list.MaxTier;
	}

	// Token: 0x04005E77 RID: 24183
	public static string ID = "";

	// Token: 0x04005E78 RID: 24184
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PoiList list;

	// Token: 0x04005E79 RID: 24185
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxFilterTier;
}
