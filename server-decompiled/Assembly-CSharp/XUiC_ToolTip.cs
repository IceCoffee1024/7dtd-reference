using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200108A RID: 4234
[Preserve]
public class XUiC_ToolTip : XUiController
{
	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x06008689 RID: 34441 RVA: 0x003341F2 File Offset: 0x003323F2
	// (set) Token: 0x0600868A RID: 34442 RVA: 0x003341FC File Offset: 0x003323FC
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

	// Token: 0x0600868B RID: 34443 RVA: 0x003342BA File Offset: 0x003324BA
	public override void Init()
	{
		base.Init();
		this.xui.ToolTipWindow = this;
		this.border = (XUiV_Sprite)base.GetChildById("sprBackgroundBorder").ViewComponent;
	}

	// Token: 0x0600868C RID: 34444 RVA: 0x003342E9 File Offset: 0x003324E9
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.ToolTipWindow = null;
	}

	// Token: 0x0600868D RID: 34445 RVA: 0x00334300 File Offset: 0x00332500
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

	// Token: 0x0600868E RID: 34446 RVA: 0x00334404 File Offset: 0x00332604
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

	// Token: 0x0600868F RID: 34447 RVA: 0x003344A0 File Offset: 0x003326A0
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

	// Token: 0x040063F5 RID: 25589
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x040063F6 RID: 25590
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip = "";

	// Token: 0x040063F7 RID: 25591
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipOld = "";

	// Token: 0x040063F8 RID: 25592
	public float ShowDelaySec = 0.3f;

	// Token: 0x040063F9 RID: 25593
	public Vector2i PositionOffset = new Vector2i(0, -36);

	// Token: 0x040063FA RID: 25594
	[PublicizedFrom(EAccessModifier.Private)]
	public float showDelay;

	// Token: 0x040063FB RID: 25595
	[PublicizedFrom(EAccessModifier.Private)]
	public bool show;
}
