using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001095 RID: 4245
[Preserve]
public class XUiC_UnlockByEntry : XUiController
{
	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x0600873C RID: 34620 RVA: 0x0033BD7B File Offset: 0x00339F7B
	// (set) Token: 0x0600873D RID: 34621 RVA: 0x0033BD83 File Offset: 0x00339F83
	public Recipe Recipe { get; set; }

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x0600873E RID: 34622 RVA: 0x0033BD8C File Offset: 0x00339F8C
	// (set) Token: 0x0600873F RID: 34623 RVA: 0x0033BD94 File Offset: 0x00339F94
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

	// Token: 0x06008740 RID: 34624 RVA: 0x0033BDA4 File Offset: 0x00339FA4
	public override void Init()
	{
		base.Init();
		this.IsDirty = false;
	}

	// Token: 0x06008741 RID: 34625 RVA: 0x0033BDB4 File Offset: 0x00339FB4
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

	// Token: 0x06008742 RID: 34626 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008743 RID: 34627 RVA: 0x002E2032 File Offset: 0x002E0232
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

	// Token: 0x040064B1 RID: 25777
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData unlockData;

	// Token: 0x040064B2 RID: 25778
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040064B3 RID: 25779
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt havecountFormatter = new CachedStringFormatterInt();

	// Token: 0x040064B4 RID: 25780
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt needcountFormatter = new CachedStringFormatterInt();
}
