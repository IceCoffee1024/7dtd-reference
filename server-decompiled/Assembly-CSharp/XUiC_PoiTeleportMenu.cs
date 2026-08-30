using System;
using UnityEngine.Scripting;

// Token: 0x02000FAE RID: 4014
[Preserve]
public class XUiC_PoiTeleportMenu : XUiController
{
	// Token: 0x06007EC6 RID: 32454 RVA: 0x00301FB4 File Offset: 0x003001B4
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

	// Token: 0x06007EC7 RID: 32455 RVA: 0x0030207E File Offset: 0x0030027E
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxFilterTier_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.list.FilterTier = (int)_newValue;
	}

	// Token: 0x06007EC8 RID: 32456 RVA: 0x0030208D File Offset: 0x0030028D
	[PublicizedFrom(EAccessModifier.Private)]
	public void FilterSmall_Changed(XUiC_ToggleButton _sender, bool _newvalue)
	{
		this.list.FilterSmallPois = _newvalue;
	}

	// Token: 0x06007EC9 RID: 32457 RVA: 0x0030209B File Offset: 0x0030029B
	[PublicizedFrom(EAccessModifier.Private)]
	public void onListEntryClicked(XUiC_List<XUiC_PoiList.PoiListEntry> _list, XUiC_PoiList.PoiListEntry _entry)
	{
		if (_entry != null)
		{
			this.EntryPressed(_entry);
		}
	}

	// Token: 0x06007ECA RID: 32458 RVA: 0x003020A7 File Offset: 0x003002A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntryPressed(XUiC_PoiList.PoiListEntry _key)
	{
		this.xui.playerUI.entityPlayer.Teleport(_key.prefabInstance.boundingBoxPosition.ToVector3(), 45f);
	}

	// Token: 0x06007ECB RID: 32459 RVA: 0x003020D3 File Offset: 0x003002D3
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxFilterTier.Max = (long)this.list.MaxTier;
	}

	// Token: 0x04005E13 RID: 24083
	public static string ID = "";

	// Token: 0x04005E14 RID: 24084
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PoiList list;

	// Token: 0x04005E15 RID: 24085
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxFilterTier;
}
