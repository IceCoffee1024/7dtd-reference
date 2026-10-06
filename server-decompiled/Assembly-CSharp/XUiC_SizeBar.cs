using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001061 RID: 4193
[Preserve]
public class XUiC_SizeBar : XUiController
{
	// Token: 0x060084EA RID: 34026 RVA: 0x0032AFF4 File Offset: 0x003291F4
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

	// Token: 0x060084EB RID: 34027 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060084EC RID: 34028 RVA: 0x002D0191 File Offset: 0x002CE391
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x060084ED RID: 34029 RVA: 0x0032B11C File Offset: 0x0032931C
	public void SetSelectedRegion(XUiC_SizeBar.BarRegionFloat primaryRegion)
	{
		if (!this.selectedRegion.Equals(primaryRegion))
		{
			this.selectedRegion = primaryRegion;
			this.IsDirty = true;
		}
	}

	// Token: 0x060084EE RID: 34030 RVA: 0x0032B13A File Offset: 0x0032933A
	public void SetHoveredRegion(XUiC_SizeBar.BarRegionFloat region)
	{
		if (!this.hoveredRegion.Equals(region))
		{
			this.hoveredRegion = region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060084EF RID: 34031 RVA: 0x0032B158 File Offset: 0x00329358
	public void SetUsed(float used)
	{
		if (this.used == used)
		{
			return;
		}
		this.used = used;
		this.IsDirty = true;
	}

	// Token: 0x060084F0 RID: 34032 RVA: 0x0032B172 File Offset: 0x00329372
	public void SetAllowance(float allowance)
	{
		if (this.allowance == allowance)
		{
			return;
		}
		this.allowance = allowance;
		this.IsDirty = true;
	}

	// Token: 0x060084F1 RID: 34033 RVA: 0x0032B18C File Offset: 0x0032938C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x060084F2 RID: 34034 RVA: 0x0032B1A4 File Offset: 0x003293A4
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

	// Token: 0x060084F3 RID: 34035 RVA: 0x0032B200 File Offset: 0x00329400
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

	// Token: 0x060084F4 RID: 34036 RVA: 0x0032B35C File Offset: 0x0032955C
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

	// Token: 0x060084F7 RID: 34039 RVA: 0x0032B3E0 File Offset: 0x003295E0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <RefreshSelectionMode>g__GetPixelValues|27_0(XUiC_SizeBar.BarRegionFloat region, int maxPosition, out int position, out int width, ref XUiC_SizeBar.<>c__DisplayClass27_0 A_5)
	{
		position = Mathf.Min(Mathf.FloorToInt(A_5.pixelScaleFactor * region.Start), maxPosition - this.minBarWidth);
		width = Mathf.Max(Mathf.CeilToInt(A_5.pixelScaleFactor * region.Size), this.minBarWidth);
	}

	// Token: 0x060084F8 RID: 34040 RVA: 0x0032B430 File Offset: 0x00329630
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

	// Token: 0x0400629A RID: 25242
	[PublicizedFrom(EAccessModifier.Private)]
	public int minBarWidth = 1;

	// Token: 0x0400629B RID: 25243
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x0400629C RID: 25244
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400629D RID: 25245
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_used;

	// Token: 0x0400629E RID: 25246
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_excess;

	// Token: 0x0400629F RID: 25247
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_fill;

	// Token: 0x040062A0 RID: 25248
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_outline;

	// Token: 0x040062A1 RID: 25249
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_hovered_outline;

	// Token: 0x040062A2 RID: 25250
	[PublicizedFrom(EAccessModifier.Private)]
	public float used;

	// Token: 0x040062A3 RID: 25251
	[PublicizedFrom(EAccessModifier.Private)]
	public float allowance;

	// Token: 0x040062A4 RID: 25252
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat selectedRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x040062A5 RID: 25253
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat hoveredRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x040062A6 RID: 25254
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x040062A7 RID: 25255
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x040062A8 RID: 25256
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x040062A9 RID: 25257
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x040062AA RID: 25258
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor;

	// Token: 0x02001062 RID: 4194
	public struct BarRegionFloat : IEquatable<XUiC_SizeBar.BarRegionFloat>
	{
		// Token: 0x060084F9 RID: 34041 RVA: 0x0032B533 File Offset: 0x00329733
		public BarRegionFloat(float offset, float size)
		{
			this.Start = offset;
			this.Size = size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x060084FA RID: 34042 RVA: 0x0032B556 File Offset: 0x00329756
		public bool Equals(XUiC_SizeBar.BarRegionFloat other)
		{
			return this.Start == other.Start && this.Size == other.Size;
		}

		// Token: 0x040062AB RID: 25259
		public readonly float Start;

		// Token: 0x040062AC RID: 25260
		public readonly float Size;

		// Token: 0x040062AD RID: 25261
		public readonly float End;

		// Token: 0x040062AE RID: 25262
		public static readonly XUiC_SizeBar.BarRegionFloat None = new XUiC_SizeBar.BarRegionFloat(0f, 0f);
	}
}
