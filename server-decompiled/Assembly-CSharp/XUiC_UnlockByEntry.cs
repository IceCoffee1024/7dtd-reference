using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010AE RID: 4270
[Preserve]
public class XUiC_UnlockByEntry : XUiController
{
	// Token: 0x17000FE1 RID: 4065
	// (get) Token: 0x0600882E RID: 34862 RVA: 0x003405A3 File Offset: 0x0033E7A3
	// (set) Token: 0x0600882F RID: 34863 RVA: 0x003405AB File Offset: 0x0033E7AB
	public Recipe Recipe { get; set; }

	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x06008830 RID: 34864 RVA: 0x003405B4 File Offset: 0x0033E7B4
	// (set) Token: 0x06008831 RID: 34865 RVA: 0x003405BC File Offset: 0x0033E7BC
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

	// Token: 0x06008832 RID: 34866 RVA: 0x003405CC File Offset: 0x0033E7CC
	public override void Init()
	{
		base.Init();
		this.IsDirty = false;
	}

	// Token: 0x06008833 RID: 34867 RVA: 0x003405DC File Offset: 0x0033E7DC
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

	// Token: 0x06008834 RID: 34868 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008835 RID: 34869 RVA: 0x002E5FE6 File Offset: 0x002E41E6
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

	// Token: 0x04006578 RID: 25976
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData unlockData;

	// Token: 0x04006579 RID: 25977
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400657A RID: 25978
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x0400657B RID: 25979
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
