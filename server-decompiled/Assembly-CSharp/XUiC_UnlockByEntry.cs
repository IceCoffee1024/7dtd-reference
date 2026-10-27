using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010CF RID: 4303
[Preserve]
public class XUiC_UnlockByEntry : XUiController
{
	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x060088D0 RID: 35024 RVA: 0x0034643F File Offset: 0x0034463F
	// (set) Token: 0x060088D1 RID: 35025 RVA: 0x00346447 File Offset: 0x00344647
	public Recipe Recipe { get; set; }

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x060088D2 RID: 35026 RVA: 0x00346450 File Offset: 0x00344650
	// (set) Token: 0x060088D3 RID: 35027 RVA: 0x00346458 File Offset: 0x00344658
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

	// Token: 0x060088D4 RID: 35028 RVA: 0x00346468 File Offset: 0x00344668
	public override void Init()
	{
		base.Init();
		this.IsDirty = false;
	}

	// Token: 0x060088D5 RID: 35029 RVA: 0x00346478 File Offset: 0x00344678
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

	// Token: 0x060088D6 RID: 35030 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060088D7 RID: 35031 RVA: 0x002EAC62 File Offset: 0x002E8E62
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

	// Token: 0x040065F9 RID: 26105
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData unlockData;

	// Token: 0x040065FA RID: 26106
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040065FB RID: 26107
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x040065FC RID: 26108
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
