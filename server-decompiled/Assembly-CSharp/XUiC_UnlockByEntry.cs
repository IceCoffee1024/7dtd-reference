using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010BF RID: 4287
[Preserve]
public class XUiC_UnlockByEntry : XUiController
{
	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x0600887D RID: 34941 RVA: 0x003443BB File Offset: 0x003425BB
	// (set) Token: 0x0600887E RID: 34942 RVA: 0x003443C3 File Offset: 0x003425C3
	public Recipe Recipe { get; set; }

	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x0600887F RID: 34943 RVA: 0x003443CC File Offset: 0x003425CC
	// (set) Token: 0x06008880 RID: 34944 RVA: 0x003443D4 File Offset: 0x003425D4
	public RecipeUnlockData UnlockData
	{
		get
		{
			return this.unlockData;
		}
		set
		{
			this.unlockData = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06008881 RID: 34945 RVA: 0x003443E4 File Offset: 0x003425E4
	public override void Init()
	{
		base.Init();
		this.IsDirty = false;
	}

	// Token: 0x06008882 RID: 34946 RVA: 0x003443F4 File Offset: 0x003425F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.UnlockData != null;
		if (bindingName == "name")
		{
			if (flag)
			{
				value = this.UnlockData.GetName();
			}
			else
			{
				value = "";
			}
			return true;
		}
		if (bindingName == "itemicon")
		{
			if (flag)
			{
				value = this.UnlockData.GetIcon();
			}
			else
			{
				value = "";
			}
			return true;
		}
		if (bindingName == "itemiconatlas")
		{
			if (flag)
			{
				value = this.UnlockData.GetIconAtlas();
			}
			else
			{
				value = "UIAtlas";
			}
			return true;
		}
		if (bindingName == "itemicontint")
		{
			Color32 color = Color.white;
			if (flag)
			{
				color = this.UnlockData.GetItemTint();
			}
			value = this.itemicontintcolorFormatter.Format(color);
			return true;
		}
		if (!(bindingName == "level"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		if (flag)
		{
			value = this.UnlockData.GetLevel(this.xui.playerUI.entityPlayer, this.Recipe.GetOutputItemClass().Name);
		}
		else
		{
			value = "";
		}
		return true;
	}

	// Token: 0x06008883 RID: 34947 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008884 RID: 34948 RVA: 0x002E9366 File Offset: 0x002E7566
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			base.RefreshBindings();
			base.ViewComponent.IsVisible = true;
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x040065BA RID: 26042
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData unlockData;

	// Token: 0x040065BB RID: 26043
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040065BC RID: 26044
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x040065BD RID: 26045
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
