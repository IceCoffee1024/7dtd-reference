using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001027 RID: 4135
[Preserve]
public class XUiC_RadialEntry : XUiController
{
	// Token: 0x060081E6 RID: 33254 RVA: 0x0031760C File Offset: 0x0031580C
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

	// Token: 0x060081E7 RID: 33255 RVA: 0x00317692 File Offset: 0x00315892
	public void SetHighlighted(bool _highlighted)
	{
		this.background.Color = (_highlighted ? this.highlightColor : this.backgroundColor);
	}

	// Token: 0x060081E8 RID: 33256 RVA: 0x003176B0 File Offset: 0x003158B0
	public void SetSprite(string _sprite, Color _color)
	{
		this.icon.SpriteName = _sprite;
		this.icon.Color = _color;
	}

	// Token: 0x060081E9 RID: 33257 RVA: 0x003176CA File Offset: 0x003158CA
	public void SetText(string _text)
	{
		if (this.text != null)
		{
			this.text.Text = _text;
			this.text.IsVisible = _text != "";
		}
	}

	// Token: 0x060081EA RID: 33258 RVA: 0x003176F6 File Offset: 0x003158F6
	public void SetAtlas(string _atlas)
	{
		this.icon.UIAtlas = _atlas;
	}

	// Token: 0x060081EB RID: 33259 RVA: 0x00317704 File Offset: 0x00315904
	public void ResetScale()
	{
		this.SetScale(1f, true);
	}

	// Token: 0x060081EC RID: 33260 RVA: 0x00317714 File Offset: 0x00315914
	public void SetScale(float _scale, bool _instant = false)
	{
		float num = (_instant ? 0f : 0.15f);
		TweenScale.Begin(this.viewComponent.UiTransform.gameObject, num, Vector3.one * _scale);
	}

	// Token: 0x060081ED RID: 33261 RVA: 0x00317754 File Offset: 0x00315954
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

	// Token: 0x04006068 RID: 24680
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icon;

	// Token: 0x04006069 RID: 24681
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400606A RID: 24682
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label text;

	// Token: 0x0400606B RID: 24683
	[PublicizedFrom(EAccessModifier.Private)]
	public Color backgroundColor;

	// Token: 0x0400606C RID: 24684
	[PublicizedFrom(EAccessModifier.Private)]
	public Color highlightColor;

	// Token: 0x0400606D RID: 24685
	public string SelectionText;

	// Token: 0x0400606E RID: 24686
	public int MenuItemIndex;

	// Token: 0x0400606F RID: 24687
	public int CommandIndex;
}
