using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200103F RID: 4159
[Preserve]
public class XUiC_ServiceInfoWindow : XUiC_InfoWindow
{
	// Token: 0x060082E0 RID: 33504 RVA: 0x0031D0D4 File Offset: 0x0031B2D4
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

	// Token: 0x060082E1 RID: 33505 RVA: 0x0031D164 File Offset: 0x0031B364
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= base.OnVisibilityChanged;
	}

	// Token: 0x060082E2 RID: 33506 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Deselect()
	{
	}

	// Token: 0x060082E3 RID: 33507 RVA: 0x0031D180 File Offset: 0x0031B380
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

	// Token: 0x060082E4 RID: 33508 RVA: 0x0031D1E0 File Offset: 0x0031B3E0
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

	// Token: 0x060082E5 RID: 33509 RVA: 0x0031D400 File Offset: 0x0031B600
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

	// Token: 0x060082E6 RID: 33510 RVA: 0x0031D4B6 File Offset: 0x0031B6B6
	public void OnVisibilityChanged(XUiController _sender, bool _isVisible)
	{
		if (this.service != null)
		{
			this.service.VisibleChangedHandler(_isVisible);
		}
	}

	// Token: 0x060082E7 RID: 33511 RVA: 0x0031D4D4 File Offset: 0x0031B6D4
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

	// Token: 0x060082E8 RID: 33512 RVA: 0x0031D52E File Offset: 0x0031B72E
	public override void OnClose()
	{
		base.OnClose();
		this.service = null;
	}

	// Token: 0x04006106 RID: 24838
	[PublicizedFrom(EAccessModifier.Private)]
	public InGameService service;

	// Token: 0x04006107 RID: 24839
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController servicePreview;

	// Token: 0x04006108 RID: 24840
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x04006109 RID: 24841
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x0400610A RID: 24842
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x0400610B RID: 24843
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x0400610C RID: 24844
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList mainActionItemList;

	// Token: 0x0400610D RID: 24845
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400610E RID: 24846
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x0400610F RID: 24847
	[PublicizedFrom(EAccessModifier.Private)]
	public string stat1 = "";

	// Token: 0x04006110 RID: 24848
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt servicecostFormatter = new CachedStringFormatterInt();

	// Token: 0x04006111 RID: 24849
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor serviceicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
