using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200102F RID: 4143
[Preserve]
public class XUiC_ServiceInfoWindow : XUiC_InfoWindow
{
	// Token: 0x0600828E RID: 33422 RVA: 0x0031B3D0 File Offset: 0x003195D0
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

	// Token: 0x0600828F RID: 33423 RVA: 0x0031B460 File Offset: 0x00319660
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= base.OnVisibilityChanged;
	}

	// Token: 0x06008290 RID: 33424 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Deselect()
	{
	}

	// Token: 0x06008291 RID: 33425 RVA: 0x0031B47C File Offset: 0x0031967C
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

	// Token: 0x06008292 RID: 33426 RVA: 0x0031B4DC File Offset: 0x003196DC
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

	// Token: 0x06008293 RID: 33427 RVA: 0x0031B6FC File Offset: 0x003198FC
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

	// Token: 0x06008294 RID: 33428 RVA: 0x0031B7B2 File Offset: 0x003199B2
	public void OnVisibilityChanged(XUiController _sender, bool _isVisible)
	{
		if (this.service != null)
		{
			this.service.VisibleChangedHandler(_isVisible);
		}
	}

	// Token: 0x06008295 RID: 33429 RVA: 0x0031B7D0 File Offset: 0x003199D0
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

	// Token: 0x06008296 RID: 33430 RVA: 0x0031B82A File Offset: 0x00319A2A
	public override void OnClose()
	{
		base.OnClose();
		this.service = null;
	}

	// Token: 0x040060C8 RID: 24776
	[PublicizedFrom(EAccessModifier.Private)]
	public InGameService service;

	// Token: 0x040060C9 RID: 24777
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController servicePreview;

	// Token: 0x040060CA RID: 24778
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x040060CB RID: 24779
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x040060CC RID: 24780
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x040060CD RID: 24781
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x040060CE RID: 24782
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList mainActionItemList;

	// Token: 0x040060CF RID: 24783
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040060D0 RID: 24784
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x040060D1 RID: 24785
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat1 = "";

	// Token: 0x040060D2 RID: 24786
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt servicecostFormatter = new CachedStringFormatterInt();

	// Token: 0x040060D3 RID: 24787
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor serviceicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
