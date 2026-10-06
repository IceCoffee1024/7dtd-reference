using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001017 RID: 4119
[Preserve]
public class XUiC_RadialEntry : XUiController
{
	// Token: 0x06008194 RID: 33172 RVA: 0x00315908 File Offset: 0x00313B08
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

	// Token: 0x06008195 RID: 33173 RVA: 0x0031598E File Offset: 0x00313B8E
	public void SetHighlighted(bool _highlighted)
	{
		this.background.Color = (_highlighted ? this.highlightColor : this.backgroundColor);
	}

	// Token: 0x06008196 RID: 33174 RVA: 0x003159AC File Offset: 0x00313BAC
	public void SetSprite(string _sprite, Color _color)
	{
		this.icon.SpriteName = _sprite;
		this.icon.Color = _color;
	}

	// Token: 0x06008197 RID: 33175 RVA: 0x003159C6 File Offset: 0x00313BC6
	public void SetText(string _text)
	{
		if (this.text != null)
		{
			this.text.Text = _text;
			this.text.IsVisible = _text != "";
		}
	}

	// Token: 0x06008198 RID: 33176 RVA: 0x003159F2 File Offset: 0x00313BF2
	public void SetAtlas(string _atlas)
	{
		this.icon.UIAtlas = _atlas;
	}

	// Token: 0x06008199 RID: 33177 RVA: 0x00315A00 File Offset: 0x00313C00
	public void ResetScale()
	{
		this.SetScale(1f, true);
	}

	// Token: 0x0600819A RID: 33178 RVA: 0x00315A10 File Offset: 0x00313C10
	public void SetScale(float _scale, bool _instant = false)
	{
		float num = (_instant ? 0f : 0.15f);
		TweenScale.Begin(this.viewComponent.UiTransform.gameObject, num, Vector3.one * _scale);
	}

	// Token: 0x0600819B RID: 33179 RVA: 0x00315A50 File Offset: 0x00313C50
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

	// Token: 0x0400602A RID: 24618
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icon;

	// Token: 0x0400602B RID: 24619
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400602C RID: 24620
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label text;

	// Token: 0x0400602D RID: 24621
	[PublicizedFrom(EAccessModifier.Private)]
	public Color backgroundColor;

	// Token: 0x0400602E RID: 24622
	[PublicizedFrom(EAccessModifier.Private)]
	public Color highlightColor;

	// Token: 0x0400602F RID: 24623
	public string SelectionText;

	// Token: 0x04006030 RID: 24624
	public int MenuItemIndex;

	// Token: 0x04006031 RID: 24625
	public int CommandIndex;
}
