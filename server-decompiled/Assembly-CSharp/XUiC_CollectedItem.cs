using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EDD RID: 3805
[Preserve]
public class XUiC_CollectedItem : XUiController
{
	// Token: 0x17000DF9 RID: 3577
	// (get) Token: 0x0600775F RID: 30559 RVA: 0x002CF39A File Offset: 0x002CD59A
	// (set) Token: 0x06007760 RID: 30560 RVA: 0x002CF3A2 File Offset: 0x002CD5A2
	public ItemStack ItemStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			this.itemStack = value;
			this.itemClass = this.itemStack.itemValue.ItemClass;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007761 RID: 30561 RVA: 0x002CF3C8 File Offset: 0x002CD5C8
	public override void Init()
	{
		base.Init();
		TweenColor tweenColor = base.ViewComponent.UiTransform.gameObject.AddComponent<TweenColor>();
		tweenColor.enabled = false;
		tweenColor.from = Color.white;
		tweenColor.to = new Color(1f, 1f, 1f, 0f);
		tweenColor.duration = 0.8f;
		base.ViewComponent.IsVisible = false;
	}

	// Token: 0x06007762 RID: 30562 RVA: 0x002CF437 File Offset: 0x002CD637
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x06007763 RID: 30563 RVA: 0x002CF440 File Offset: 0x002CD640
	public void ShowItem()
	{
		TweenColor component = base.ViewComponent.UiTransform.gameObject.GetComponent<TweenColor>();
		component.from = Color.white;
		component.to = Color.white;
		component.duration = 0.1f;
		component.enabled = true;
	}

	// Token: 0x06007764 RID: 30564 RVA: 0x002CF480 File Offset: 0x002CD680
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "itemicon")
		{
			value = ((this.itemClass != null) ? this.itemClass.GetIconName() : "");
			return true;
		}
		if (bindingName == "itemiconcolor")
		{
			Color32 color = Color.white;
			if (this.itemStack != null && this.itemStack.itemValue.type != 0)
			{
				color = this.itemClass.GetIconTint(this.itemStack.itemValue);
			}
			value = this.itemiconcolorFormatter.Format(color);
			return true;
		}
		if (bindingName == "itemcount")
		{
			value = "";
			if (this.ItemStack != null && this.itemStack.itemValue.type != 0)
			{
				value = ((this.ItemStack.count > 0) ? this.itemcountFormatter.Format(this.ItemStack.count) : "0");
			}
			return true;
		}
		if (bindingName == "itembackground")
		{
			value = "menu_empty";
			if (this.itemClass != null && this.itemStack.itemValue.type != 0)
			{
				value = "ui_game_popup";
			}
			return true;
		}
		if (!(bindingName == "itembackgroundcolor"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = "255, 255, 255, 0";
		if (this.itemClass != null && this.itemStack.itemValue.type != 0)
		{
			value = "255, 255, 255, 255";
		}
		return true;
	}

	// Token: 0x04005816 RID: 22550
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005817 RID: 22551
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x04005818 RID: 22552
	public GameObject Item;

	// Token: 0x04005819 RID: 22553
	public float TimeAdded;

	// Token: 0x0400581A RID: 22554
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400581B RID: 22555
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> itemcountFormatter = new CachedStringFormatter<int>((int _i) => "+" + _i.ToString());
}
