using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200109B RID: 4251
[Preserve]
public class XUiC_ToolTip : XUiController
{
	// Token: 0x17000FE0 RID: 4064
	// (get) Token: 0x060086D8 RID: 34520 RVA: 0x00338009 File Offset: 0x00336209
	// (set) Token: 0x060086D9 RID: 34521 RVA: 0x00338014 File Offset: 0x00336214
	public string ToolTip
	{
		get
		{
			return this.tooltip;
		}
		set
		{
			if (this.tooltip == value)
			{
				return;
			}
			this.tooltipOld = this.tooltip;
			if (value == null)
			{
				this.tooltip = "";
			}
			else if (value.Length > 0 && value[value.Length - 1] == '\n')
			{
				this.tooltip = value.Substring(0, value.Length - 1);
			}
			else
			{
				this.tooltip = value;
			}
			if (this.tooltip == "")
			{
				this.show = false;
				return;
			}
			base.ViewComponent.Position = this.xui.GetMouseXUiPosition() + this.PositionOffset;
			this.showDelay = Time.unscaledTime + this.ShowDelaySec;
		}
	}

	// Token: 0x060086DA RID: 34522 RVA: 0x003380D2 File Offset: 0x003362D2
	public override void Init()
	{
		base.Init();
		this.xui.ToolTipWindow = this;
		this.border = (XUiV_Sprite)base.GetChildById("sprBackgroundBorder").ViewComponent;
	}

	// Token: 0x060086DB RID: 34523 RVA: 0x00338101 File Offset: 0x00336301
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.ToolTipWindow = null;
	}

	// Token: 0x060086DC RID: 34524 RVA: 0x00338118 File Offset: 0x00336318
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		if (this.tooltip != "")
		{
			if (Time.unscaledTime > this.showDelay)
			{
				this.show = true;
			}
			Vector2i vector2i = this.xui.GetXUiScreenSize() / 2;
			if ((base.ViewComponent.Position + this.border.Size).x > vector2i.x)
			{
				base.ViewComponent.Position -= new Vector2i(this.border.Size.x, 0);
			}
			if ((base.ViewComponent.Position - this.border.Size).y < -vector2i.y)
			{
				base.ViewComponent.Position += new Vector2i(20, 20 + this.border.Size.y);
			}
		}
		base.Update(_dt);
	}

	// Token: 0x060086DD RID: 34525 RVA: 0x0033821C File Offset: 0x0033641C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "screen_width")
		{
			_value = this.xui.GetXUiScreenSize().x.ToString();
			return true;
		}
		if (_bindingName == "text")
		{
			_value = ((this.tooltip.Length > 0) ? this.tooltip : this.tooltipOld);
			return true;
		}
		if (!(_bindingName == "show"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = (this.show && GameManager.Instance.isAnyCursorWindowOpen(null)).ToString();
		return true;
	}

	// Token: 0x060086DE RID: 34526 RVA: 0x003382B8 File Offset: 0x003364B8
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "show_delay")
		{
			this.ShowDelaySec = (float)StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		if (!(_name == "position_offset"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.PositionOffset = StringParsers.ParseVector2i(_value, ',');
		return true;
	}

	// Token: 0x04006437 RID: 25655
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x04006438 RID: 25656
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip = "";

	// Token: 0x04006439 RID: 25657
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipOld = "";

	// Token: 0x0400643A RID: 25658
	public float ShowDelaySec = 0.3f;

	// Token: 0x0400643B RID: 25659
	public Vector2i PositionOffset = new Vector2i(0, -36);

	// Token: 0x0400643C RID: 25660
	[PublicizedFrom(EAccessModifier.Private)]
	public float showDelay;

	// Token: 0x0400643D RID: 25661
	[PublicizedFrom(EAccessModifier.Private)]
	public bool show;
}
