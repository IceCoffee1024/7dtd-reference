using System;
using UnityEngine.Scripting;

// Token: 0x02000EBE RID: 3774
[Preserve]
public class XUiC_CategoryEntry : XUiController
{
	// Token: 0x17000DE2 RID: 3554
	// (get) Token: 0x06007643 RID: 30275 RVA: 0x002C6C04 File Offset: 0x002C4E04
	// (set) Token: 0x06007644 RID: 30276 RVA: 0x002C6C0C File Offset: 0x002C4E0C
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000DE3 RID: 3555
	// (get) Token: 0x06007645 RID: 30277 RVA: 0x002C6C15 File Offset: 0x002C4E15
	// (set) Token: 0x06007646 RID: 30278 RVA: 0x002C6C1D File Offset: 0x002C4E1D
	public string CategoryName
	{
		get
		{
			return this.categoryName;
		}
		set
		{
			this.categoryName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DE4 RID: 3556
	// (get) Token: 0x06007647 RID: 30279 RVA: 0x002C6C2D File Offset: 0x002C4E2D
	// (set) Token: 0x06007648 RID: 30280 RVA: 0x002C6C35 File Offset: 0x002C4E35
	public string CategoryDisplayName
	{
		get
		{
			return this.categoryDisplayName;
		}
		set
		{
			this.categoryDisplayName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DE5 RID: 3557
	// (get) Token: 0x06007649 RID: 30281 RVA: 0x002C6C45 File Offset: 0x002C4E45
	// (set) Token: 0x0600764A RID: 30282 RVA: 0x002C6C4D File Offset: 0x002C4E4D
	public string SpriteName
	{
		get
		{
			return this.spriteName;
		}
		set
		{
			this.spriteName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DE6 RID: 3558
	// (get) Token: 0x0600764B RID: 30283 RVA: 0x002C6C5D File Offset: 0x002C4E5D
	// (set) Token: 0x0600764C RID: 30284 RVA: 0x002C6C65 File Offset: 0x002C4E65
	public new bool Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			this.selected = value;
			this.button.Selected = this.selected;
		}
	}

	// Token: 0x0600764D RID: 30285 RVA: 0x002C6C7F File Offset: 0x002C4E7F
	public override void Init()
	{
		base.Init();
		this.button = (XUiV_Button)base.ViewComponent;
		base.OnPress += this.XUiC_CategoryEntry_OnPress;
		this.IsDirty = true;
	}

	// Token: 0x0600764E RID: 30286 RVA: 0x002C6CB4 File Offset: 0x002C4EB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_CategoryEntry_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.spriteName != string.Empty)
		{
			if (this.CategoryList.CurrentCategory == this && this.CategoryList.AllowUnselect)
			{
				this.CategoryList.CurrentCategory = null;
			}
			else
			{
				this.CategoryList.CurrentCategory = this;
			}
			this.CategoryList.HandleCategoryChanged();
		}
	}

	// Token: 0x0600764F RID: 30287 RVA: 0x002C6D13 File Offset: 0x002C4F13
	public void PlayButtonClickSound()
	{
		this.button.PlayClickSound();
	}

	// Token: 0x06007650 RID: 30288 RVA: 0x002C6D20 File Offset: 0x002C4F20
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			base.ViewComponent.IsNavigatable = !string.IsNullOrEmpty(this.SpriteName);
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06007651 RID: 30289 RVA: 0x002C6D57 File Offset: 0x002C4F57
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "categoryicon")
		{
			value = this.spriteName;
			return true;
		}
		if (!(bindingName == "categorydisplayname"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = this.categoryDisplayName;
		return true;
	}

	// Token: 0x06007652 RID: 30290 RVA: 0x002C6D94 File Offset: 0x002C4F94
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "categoryname")
		{
			if (!string.IsNullOrEmpty(_value))
			{
				this.CategoryName = _value;
			}
			return true;
		}
		if (_name == "spritename")
		{
			if (!string.IsNullOrEmpty(_value))
			{
				this.SpriteName = _value;
			}
			return true;
		}
		if (_name == "displayname")
		{
			if (!string.IsNullOrEmpty(_value))
			{
				this.CategoryDisplayName = _value;
			}
			return true;
		}
		if (!(_name == "displayname_key"))
		{
			return base.ParseAttribute(_name, _value);
		}
		if (!string.IsNullOrEmpty(_value))
		{
			this.CategoryDisplayName = Localization.Get(_value, false, null);
		}
		return true;
	}

	// Token: 0x04005731 RID: 22321
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "";

	// Token: 0x04005732 RID: 22322
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplayName = "";

	// Token: 0x04005733 RID: 22323
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "";

	// Token: 0x04005734 RID: 22324
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005735 RID: 22325
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;
}
