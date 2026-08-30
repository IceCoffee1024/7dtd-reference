using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001037 RID: 4151
[Preserve]
public class XUiC_SizeBar : XUiController
{
	// Token: 0x060083B2 RID: 33714 RVA: 0x00322BC0 File Offset: 0x00320DC0
	public override void Init()
	{
		base.Init();
		this.border = (XUiV_Sprite)base.GetChildById("border").ViewComponent;
		this.background = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		this.bar_used = (XUiV_Sprite)base.GetChildById("bar_used").ViewComponent;
		this.bar_excess = (XUiV_Sprite)base.GetChildById("bar_excess").ViewComponent;
		this.bar_selected_fill = (XUiV_Sprite)base.GetChildById("bar_selected_fill").ViewComponent;
		this.bar_selected_outline = (XUiV_Sprite)base.GetChildById("bar_selected_outline").ViewComponent;
		this.bar_hovered_outline = (XUiV_Sprite)base.GetChildById("bar_hovered_outline").ViewComponent;
		this.normalBorderColor = this.border.Color;
		this.selectionFillColor = this.bar_selected_fill.Color;
		this.fullWidth = this.background.Size.x;
		this.fullHeight = this.background.Size.y;
	}

	// Token: 0x060083B3 RID: 33715 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060083B4 RID: 33716 RVA: 0x002C9895 File Offset: 0x002C7A95
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x060083B5 RID: 33717 RVA: 0x00322CE8 File Offset: 0x00320EE8
	public void SetSelectedRegion(XUiC_SizeBar.BarRegionFloat primaryRegion)
	{
		if (!this.selectedRegion.Equals(primaryRegion))
		{
			this.selectedRegion = primaryRegion;
			this.IsDirty = true;
		}
	}

	// Token: 0x060083B6 RID: 33718 RVA: 0x00322D06 File Offset: 0x00320F06
	public void SetHoveredRegion(XUiC_SizeBar.BarRegionFloat region)
	{
		if (!this.hoveredRegion.Equals(region))
		{
			this.hoveredRegion = region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060083B7 RID: 33719 RVA: 0x00322D24 File Offset: 0x00320F24
	public void SetUsed(float used)
	{
		if (this.used == used)
		{
			return;
		}
		this.used = used;
		this.IsDirty = true;
	}

	// Token: 0x060083B8 RID: 33720 RVA: 0x00322D3E File Offset: 0x00320F3E
	public void SetAllowance(float allowance)
	{
		if (this.allowance == allowance)
		{
			return;
		}
		this.allowance = allowance;
		this.IsDirty = true;
	}

	// Token: 0x060083B9 RID: 33721 RVA: 0x00322D58 File Offset: 0x00320F58
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x060083BA RID: 33722 RVA: 0x00322D70 File Offset: 0x00320F70
	[PublicizedFrom(EAccessModifier.Private)]
	public void Refresh()
	{
		this.bar_used.IsVisible = false;
		this.bar_excess.IsVisible = false;
		this.bar_selected_fill.IsVisible = false;
		this.bar_selected_outline.IsVisible = false;
		this.bar_hovered_outline.IsVisible = false;
		this.RefreshSelectionMode();
		base.RefreshBindings();
		this.IsDirty = false;
	}

	// Token: 0x060083BB RID: 33723 RVA: 0x00322DCC File Offset: 0x00320FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshSelectionMode()
	{
		XUiC_SizeBar.<>c__DisplayClass27_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		if (this.allowance <= 0f || this.used <= 0f)
		{
			return;
		}
		float num = Mathf.Max(this.allowance, this.used);
		CS$<>8__locals1.pixelScaleFactor = ((num > 0f) ? ((float)this.fullWidth / num) : 0f);
		int num2 = Mathf.CeilToInt(CS$<>8__locals1.pixelScaleFactor * this.used);
		this.bar_used.IsVisible = true;
		this.bar_used.Size = new Vector2i(Math.Max(num2, this.minBarWidth), this.fullHeight);
		CS$<>8__locals1.maxPosition = this.fullWidth;
		if (this.used > this.allowance)
		{
			this.bar_excess.IsVisible = true;
			int num3 = Mathf.Min(Mathf.CeilToInt(CS$<>8__locals1.pixelScaleFactor * this.allowance), CS$<>8__locals1.maxPosition - this.minBarWidth);
			int num4 = Math.Max(Mathf.FloorToInt(CS$<>8__locals1.pixelScaleFactor * (this.used - this.allowance)), this.minBarWidth);
			this.bar_excess.Position = new Vector2i(num3, 0);
			this.bar_excess.Size = new Vector2i(num4, this.fullHeight);
		}
		this.<RefreshSelectionMode>g__UpdateRegion|27_1(this.selectedRegion, this.bar_selected_fill, this.bar_selected_outline, ref CS$<>8__locals1);
	}

	// Token: 0x060083BC RID: 33724 RVA: 0x00322F28 File Offset: 0x00321128
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "showwarningoverlay")
		{
			_value = (this.used > this.allowance).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x060083BF RID: 33727 RVA: 0x00322FAC File Offset: 0x003211AC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <RefreshSelectionMode>g__GetPixelValues|27_0(XUiC_SizeBar.BarRegionFloat region, int maxPosition, out int position, out int width, ref XUiC_SizeBar.<>c__DisplayClass27_0 A_5)
	{
		position = Mathf.Min(Mathf.FloorToInt(A_5.pixelScaleFactor * region.Start), maxPosition - this.minBarWidth);
		width = Mathf.Max(Mathf.CeilToInt(A_5.pixelScaleFactor * region.Size), this.minBarWidth);
	}

	// Token: 0x060083C0 RID: 33728 RVA: 0x00322FFC File Offset: 0x003211FC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <RefreshSelectionMode>g__UpdateRegion|27_1(XUiC_SizeBar.BarRegionFloat region, XUiV_Sprite fillSprite, XUiV_Sprite outlineSprite, ref XUiC_SizeBar.<>c__DisplayClass27_0 A_4)
	{
		if (this.hoveredRegion.Size > 0f)
		{
			int num;
			int num2;
			this.<RefreshSelectionMode>g__GetPixelValues|27_0(this.hoveredRegion, A_4.maxPosition, out num, out num2, ref A_4);
			this.bar_hovered_outline.IsVisible = true;
			this.bar_hovered_outline.Position = new Vector2i(num - 1, 0);
			this.bar_hovered_outline.Size = new Vector2i(num2 + 2, this.fullHeight);
		}
		if (region.Size <= 0f)
		{
			return;
		}
		int num3;
		int num4;
		this.<RefreshSelectionMode>g__GetPixelValues|27_0(region, A_4.maxPosition, out num3, out num4, ref A_4);
		fillSprite.IsVisible = true;
		fillSprite.Position = new Vector2i(num3, 0);
		fillSprite.Size = new Vector2i(num4, this.fullHeight);
		fillSprite.Color = this.selectionFillColor;
		outlineSprite.IsVisible = true;
		outlineSprite.Position = fillSprite.Position;
		outlineSprite.Size = fillSprite.Size;
		outlineSprite.Color = XUiC_SizeBar.selectionOutlineColor;
		A_4.maxPosition = num3 + num4;
	}

	// Token: 0x0400618E RID: 24974
	[PublicizedFrom(EAccessModifier.Private)]
	public int minBarWidth = 1;

	// Token: 0x0400618F RID: 24975
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x04006190 RID: 24976
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006191 RID: 24977
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_used;

	// Token: 0x04006192 RID: 24978
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_excess;

	// Token: 0x04006193 RID: 24979
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_fill;

	// Token: 0x04006194 RID: 24980
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_outline;

	// Token: 0x04006195 RID: 24981
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_hovered_outline;

	// Token: 0x04006196 RID: 24982
	[PublicizedFrom(EAccessModifier.Private)]
	public float used;

	// Token: 0x04006197 RID: 24983
	[PublicizedFrom(EAccessModifier.Private)]
	public float allowance;

	// Token: 0x04006198 RID: 24984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat selectedRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x04006199 RID: 24985
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat hoveredRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x0400619A RID: 24986
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x0400619B RID: 24987
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x0400619C RID: 24988
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x0400619D RID: 24989
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x0400619E RID: 24990
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor;

	// Token: 0x02001038 RID: 4152
	public struct BarRegionFloat : IEquatable<XUiC_SizeBar.BarRegionFloat>
	{
		// Token: 0x060083C1 RID: 33729 RVA: 0x003230FF File Offset: 0x003212FF
		public BarRegionFloat(float offset, float size)
		{
			this.Start = offset;
			this.Size = size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x060083C2 RID: 33730 RVA: 0x00323122 File Offset: 0x00321322
		public bool Equals(XUiC_SizeBar.BarRegionFloat other)
		{
			return this.Start == other.Start && this.Size == other.Size;
		}

		// Token: 0x0400619F RID: 24991
		public readonly float Start;

		// Token: 0x040061A0 RID: 24992
		public readonly float Size;

		// Token: 0x040061A1 RID: 24993
		public readonly float End;

		// Token: 0x040061A2 RID: 24994
		public static readonly XUiC_SizeBar.BarRegionFloat None = new XUiC_SizeBar.BarRegionFloat(0f, 0f);
	}
}
