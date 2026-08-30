using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FEF RID: 4079
[Preserve]
public class XUiC_RadialEntry : XUiController
{
	// Token: 0x06008093 RID: 32915 RVA: 0x0030E0AC File Offset: 0x0030C2AC
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("icon");
		XUiController childById2 = base.GetChildById("background");
		XUiController childById3 = base.GetChildById("text");
		this.icon = childById.ViewComponent as XUiV_Sprite;
		this.background = childById2.ViewComponent as XUiV_Sprite;
		if (childById3 != null)
		{
			this.text = childById3.ViewComponent as XUiV_Label;
		}
		if (this.background != null)
		{
			this.backgroundColor = this.background.Color;
		}
	}

	// Token: 0x06008094 RID: 32916 RVA: 0x0030E132 File Offset: 0x0030C332
	public void SetHighlighted(bool _highlighted)
	{
		this.background.Color = (_highlighted ? this.highlightColor : this.backgroundColor);
	}

	// Token: 0x06008095 RID: 32917 RVA: 0x0030E150 File Offset: 0x0030C350
	public void SetSprite(string _sprite, Color _color)
	{
		this.icon.SpriteName = _sprite;
		this.icon.Color = _color;
	}

	// Token: 0x06008096 RID: 32918 RVA: 0x0030E16A File Offset: 0x0030C36A
	public void SetText(string _text)
	{
		if (this.text != null)
		{
			this.text.Text = _text;
			this.text.IsVisible = _text != "";
		}
	}

	// Token: 0x06008097 RID: 32919 RVA: 0x0030E196 File Offset: 0x0030C396
	public void SetAtlas(string _atlas)
	{
		this.icon.UIAtlas = _atlas;
	}

	// Token: 0x06008098 RID: 32920 RVA: 0x0030E1A4 File Offset: 0x0030C3A4
	public void ResetScale()
	{
		this.SetScale(1f, true);
	}

	// Token: 0x06008099 RID: 32921 RVA: 0x0030E1B4 File Offset: 0x0030C3B4
	public void SetScale(float _scale, bool _instant = false)
	{
		float num = (_instant ? 0f : 0.15f);
		TweenScale.Begin(this.viewComponent.UiTransform.gameObject, num, Vector3.one * _scale);
	}

	// Token: 0x0600809A RID: 32922 RVA: 0x0030E1F4 File Offset: 0x0030C3F4
	public override bool ParseAttribute(string _name, string _value)
	{
		bool flag = base.ParseAttribute(_name, _value);
		if (!flag && _name.EqualsCaseInsensitive("highlight_color"))
		{
			this.highlightColor = StringParsers.ParseColor32(_value);
			return true;
		}
		return flag;
	}

	// Token: 0x04005F3A RID: 24378
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icon;

	// Token: 0x04005F3B RID: 24379
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005F3C RID: 24380
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label text;

	// Token: 0x04005F3D RID: 24381
	[PublicizedFrom(EAccessModifier.Private)]
	public Color backgroundColor;

	// Token: 0x04005F3E RID: 24382
	[PublicizedFrom(EAccessModifier.Private)]
	public Color highlightColor;

	// Token: 0x04005F3F RID: 24383
	public string SelectionText;

	// Token: 0x04005F40 RID: 24384
	public int MenuItemIndex;

	// Token: 0x04005F41 RID: 24385
	public int CommandIndex;
}
