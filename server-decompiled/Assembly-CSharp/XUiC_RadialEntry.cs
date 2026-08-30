using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001008 RID: 4104
[Preserve]
public class XUiC_RadialEntry : XUiController
{
	// Token: 0x06008185 RID: 33157 RVA: 0x003128D8 File Offset: 0x00310AD8
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

	// Token: 0x06008186 RID: 33158 RVA: 0x0031295E File Offset: 0x00310B5E
	public void SetHighlighted(bool _highlighted)
	{
		this.background.Color = (_highlighted ? this.highlightColor : this.backgroundColor);
	}

	// Token: 0x06008187 RID: 33159 RVA: 0x0031297C File Offset: 0x00310B7C
	public void SetSprite(string _sprite, Color _color)
	{
		this.icon.SpriteName = _sprite;
		this.icon.Color = _color;
	}

	// Token: 0x06008188 RID: 33160 RVA: 0x00312996 File Offset: 0x00310B96
	public void SetText(string _text)
	{
		if (this.text != null)
		{
			this.text.Text = _text;
			this.text.IsVisible = _text != "";
		}
	}

	// Token: 0x06008189 RID: 33161 RVA: 0x003129C2 File Offset: 0x00310BC2
	public void SetAtlas(string _atlas)
	{
		this.icon.UIAtlas = _atlas;
	}

	// Token: 0x0600818A RID: 33162 RVA: 0x003129D0 File Offset: 0x00310BD0
	public void ResetScale()
	{
		this.SetScale(1f, true);
	}

	// Token: 0x0600818B RID: 33163 RVA: 0x003129E0 File Offset: 0x00310BE0
	public void SetScale(float _scale, bool _instant = false)
	{
		float num = (_instant ? 0f : 0.15f);
		TweenScale.Begin(this.viewComponent.UiTransform.gameObject, num, Vector3.one * _scale);
	}

	// Token: 0x0600818C RID: 33164 RVA: 0x00312A20 File Offset: 0x00310C20
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

	// Token: 0x04006001 RID: 24577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icon;

	// Token: 0x04006002 RID: 24578
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006003 RID: 24579
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label text;

	// Token: 0x04006004 RID: 24580
	[PublicizedFrom(EAccessModifier.Private)]
	public Color backgroundColor;

	// Token: 0x04006005 RID: 24581
	[PublicizedFrom(EAccessModifier.Private)]
	public Color highlightColor;

	// Token: 0x04006006 RID: 24582
	public string SelectionText;

	// Token: 0x04006007 RID: 24583
	public int MenuItemIndex;

	// Token: 0x04006008 RID: 24584
	public int CommandIndex;
}
