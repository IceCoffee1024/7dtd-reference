using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001020 RID: 4128
[Preserve]
public class XUiC_ServiceInfoWindow : XUiC_InfoWindow
{
	// Token: 0x0600827F RID: 33407 RVA: 0x0031838C File Offset: 0x0031658C
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

	// Token: 0x06008280 RID: 33408 RVA: 0x0031841C File Offset: 0x0031661C
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= base.OnVisibilityChanged;
	}

	// Token: 0x06008281 RID: 33409 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Deselect()
	{
	}

	// Token: 0x06008282 RID: 33410 RVA: 0x00318438 File Offset: 0x00316638
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

	// Token: 0x06008283 RID: 33411 RVA: 0x00318498 File Offset: 0x00316698
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

	// Token: 0x06008284 RID: 33412 RVA: 0x003186B8 File Offset: 0x003168B8
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

	// Token: 0x06008285 RID: 33413 RVA: 0x0031876E File Offset: 0x0031696E
	public void OnVisibilityChanged(XUiController _sender, bool _isVisible)
	{
		if (this.service != null)
		{
			this.service.VisibleChangedHandler(_isVisible);
		}
	}

	// Token: 0x06008286 RID: 33414 RVA: 0x0031878C File Offset: 0x0031698C
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

	// Token: 0x06008287 RID: 33415 RVA: 0x003187E6 File Offset: 0x003169E6
	public override void OnClose()
	{
		base.OnClose();
		this.service = null;
	}

	// Token: 0x0400609F RID: 24735
	[PublicizedFrom(EAccessModifier.Private)]
	public InGameService service;

	// Token: 0x040060A0 RID: 24736
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController servicePreview;

	// Token: 0x040060A1 RID: 24737
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x040060A2 RID: 24738
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x040060A3 RID: 24739
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x040060A4 RID: 24740
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x040060A5 RID: 24741
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList mainActionItemList;

	// Token: 0x040060A6 RID: 24742
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040060A7 RID: 24743
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x040060A8 RID: 24744
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat1 = "";

	// Token: 0x040060A9 RID: 24745
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt servicecostFormatter = new CachedStringFormatterInt();

	// Token: 0x040060AA RID: 24746
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor serviceicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
