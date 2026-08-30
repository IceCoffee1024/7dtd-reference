using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001007 RID: 4103
[Preserve]
public class XUiC_ServiceInfoWindow : XUiC_InfoWindow
{
	// Token: 0x0600818D RID: 33165 RVA: 0x00313B64 File Offset: 0x00311D64
	public override void Init()
	{
		base.Init();
		this.servicePreview = base.GetChildById("servicePreview");
		this.windowName = base.GetChildById("windowName");
		this.windowIcon = base.GetChildById("windowIcon");
		this.description = base.GetChildById("descriptionText");
		this.stats = base.GetChildById("statText");
		this.mainActionItemList = (XUiC_ItemActionList)base.GetChildById("itemActions");
		base.OnVisiblity += base.OnVisibilityChanged;
	}

	// Token: 0x0600818E RID: 33166 RVA: 0x00313BF4 File Offset: 0x00311DF4
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= base.OnVisibilityChanged;
	}

	// Token: 0x0600818F RID: 33167 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Deselect()
	{
	}

	// Token: 0x06008190 RID: 33168 RVA: 0x00313C10 File Offset: 0x00311E10
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			if (this.emptyInfoWindow == null)
			{
				this.emptyInfoWindow = (XUiC_InfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("emptyInfoPanel");
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x06008191 RID: 33169 RVA: 0x00313C70 File Offset: 0x00311E70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2353034265U)
		{
			if (num <= 1380912366U)
			{
				if (num != 165025526U)
				{
					if (num == 1380912366U)
					{
						if (bindingName == "servicedescription")
						{
							value = ((this.service != null) ? this.service.Description : "");
							return true;
						}
					}
				}
				else if (bindingName == "servicegroupicon")
				{
					value = ((this.service != null) ? this.service.Icon : "");
					return true;
				}
			}
			else if (num != 2341642767U)
			{
				if (num == 2353034265U)
				{
					if (bindingName == "servicecost")
					{
						value = ((this.service != null) ? this.servicecostFormatter.Format(this.service.Price) : "");
						return true;
					}
				}
			}
			else if (bindingName == "servicestats")
			{
				value = ((this.service != null) ? this.stat1 : "");
				return true;
			}
		}
		else if (num <= 2418997840U)
		{
			if (num != 2390918988U)
			{
				if (num == 2418997840U)
				{
					if (bindingName == "pricelabel")
					{
						value = Localization.Get("xuiCost", false, null);
						return true;
					}
				}
			}
			else if (bindingName == "serviceicontint")
			{
				Color32 color = Color.white;
				value = this.serviceicontintcolorFormatter.Format(color);
				return true;
			}
		}
		else if (num != 3116710815U)
		{
			if (num == 3397569669U)
			{
				if (bindingName == "serviceicon")
				{
					value = ((this.service != null) ? this.service.Icon : "");
					return true;
				}
			}
		}
		else if (bindingName == "servicename")
		{
			value = ((this.service != null) ? this.service.Name : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008192 RID: 33170 RVA: 0x00313E90 File Offset: 0x00312090
	public void SetInfo(InGameService _service, XUiController controller)
	{
		this.service = _service;
		if (this.service == null)
		{
			if (this.emptyInfoWindow == null)
			{
				this.emptyInfoWindow = (XUiC_InfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("emptyInfoPanel");
			}
			this.emptyInfoWindow.ViewComponent.IsVisible = true;
			return;
		}
		base.ViewComponent.IsVisible = true;
		if (this.servicePreview == null)
		{
			return;
		}
		string text = Utils.ColorToHex(this.valueColor);
		this.stat1 = XUiM_InGameService.GetServiceStats(this.xui, this.service).Replace("REPLACE_COLOR", text);
		this.mainActionItemList.SetServiceActionList(this.service, controller);
		base.RefreshBindings();
	}

	// Token: 0x06008193 RID: 33171 RVA: 0x00313F46 File Offset: 0x00312146
	public void OnVisibilityChanged(XUiController _sender, bool _isVisible)
	{
		if (this.service != null)
		{
			this.service.VisibleChangedHandler(_isVisible);
		}
	}

	// Token: 0x06008194 RID: 33172 RVA: 0x00313F64 File Offset: 0x00312164
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.service == null)
		{
			if (this.emptyInfoWindow == null)
			{
				this.emptyInfoWindow = (XUiC_InfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("emptyInfoPanel");
			}
			this.emptyInfoWindow.ViewComponent.IsVisible = true;
			return;
		}
	}

	// Token: 0x06008195 RID: 33173 RVA: 0x00313FBE File Offset: 0x003121BE
	public override void OnClose()
	{
		base.OnClose();
		this.service = null;
	}

	// Token: 0x04005FD8 RID: 24536
	[PublicizedFrom(EAccessModifier.Private)]
	public InGameService service;

	// Token: 0x04005FD9 RID: 24537
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController servicePreview;

	// Token: 0x04005FDA RID: 24538
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x04005FDB RID: 24539
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005FDC RID: 24540
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x04005FDD RID: 24541
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x04005FDE RID: 24542
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList mainActionItemList;

	// Token: 0x04005FDF RID: 24543
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005FE0 RID: 24544
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x04005FE1 RID: 24545
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat1 = "";

	// Token: 0x04005FE2 RID: 24546
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt servicecostFormatter = new CachedStringFormatterInt();

	// Token: 0x04005FE3 RID: 24547
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor serviceicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
