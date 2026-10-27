using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001071 RID: 4209
[Preserve]
public class XUiC_SizeBar : XUiController
{
	// Token: 0x0600853C RID: 34108 RVA: 0x0032CCF8 File Offset: 0x0032AEF8
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

	// Token: 0x0600853D RID: 34109 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x0600853E RID: 34110 RVA: 0x002D16E9 File Offset: 0x002CF8E9
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x0600853F RID: 34111 RVA: 0x0032CE20 File Offset: 0x0032B020
	public void SetSelectedRegion(XUiC_SizeBar.BarRegionFloat primaryRegion)
	{
		if (!this.selectedRegion.Equals(primaryRegion))
		{
			this.selectedRegion = primaryRegion;
			this.IsDirty = true;
		}
	}

	// Token: 0x06008540 RID: 34112 RVA: 0x0032CE3E File Offset: 0x0032B03E
	public void SetHoveredRegion(XUiC_SizeBar.BarRegionFloat region)
	{
		if (!this.hoveredRegion.Equals(region))
		{
			this.hoveredRegion = region;
			this.IsDirty = true;
		}
	}

	// Token: 0x06008541 RID: 34113 RVA: 0x0032CE5C File Offset: 0x0032B05C
	public void SetUsed(float used)
	{
		if (this.used == used)
		{
			return;
		}
		this.used = used;
		this.IsDirty = true;
	}

	// Token: 0x06008542 RID: 34114 RVA: 0x0032CE76 File Offset: 0x0032B076
	public void SetAllowance(float allowance)
	{
		if (this.allowance == allowance)
		{
			return;
		}
		this.allowance = allowance;
		this.IsDirty = true;
	}

	// Token: 0x06008543 RID: 34115 RVA: 0x0032CE90 File Offset: 0x0032B090
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x06008544 RID: 34116 RVA: 0x0032CEA8 File Offset: 0x0032B0A8
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

	// Token: 0x06008545 RID: 34117 RVA: 0x0032CF04 File Offset: 0x0032B104
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

	// Token: 0x06008546 RID: 34118 RVA: 0x0032D060 File Offset: 0x0032B260
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

	// Token: 0x06008549 RID: 34121 RVA: 0x0032D0E4 File Offset: 0x0032B2E4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <RefreshSelectionMode>g__GetPixelValues|27_0(XUiC_SizeBar.BarRegionFloat region, int maxPosition, out int position, out int width, ref XUiC_SizeBar.<>c__DisplayClass27_0 A_5)
	{
		position = Mathf.Min(Mathf.FloorToInt(A_5.pixelScaleFactor * region.Start), maxPosition - this.minBarWidth);
		width = Mathf.Max(Mathf.CeilToInt(A_5.pixelScaleFactor * region.Size), this.minBarWidth);
	}

	// Token: 0x0600854A RID: 34122 RVA: 0x0032D134 File Offset: 0x0032B334
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

	// Token: 0x040062D8 RID: 25304
	[PublicizedFrom(EAccessModifier.Private)]
	public int minBarWidth = 1;

	// Token: 0x040062D9 RID: 25305
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x040062DA RID: 25306
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040062DB RID: 25307
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_used;

	// Token: 0x040062DC RID: 25308
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_excess;

	// Token: 0x040062DD RID: 25309
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_fill;

	// Token: 0x040062DE RID: 25310
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_outline;

	// Token: 0x040062DF RID: 25311
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_hovered_outline;

	// Token: 0x040062E0 RID: 25312
	[PublicizedFrom(EAccessModifier.Private)]
	public float used;

	// Token: 0x040062E1 RID: 25313
	[PublicizedFrom(EAccessModifier.Private)]
	public float allowance;

	// Token: 0x040062E2 RID: 25314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat selectedRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x040062E3 RID: 25315
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat hoveredRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x040062E4 RID: 25316
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x040062E5 RID: 25317
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x040062E6 RID: 25318
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x040062E7 RID: 25319
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x040062E8 RID: 25320
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor;

	// Token: 0x02001072 RID: 4210
	public struct BarRegionFloat : IEquatable<XUiC_SizeBar.BarRegionFloat>
	{
		// Token: 0x0600854B RID: 34123 RVA: 0x0032D237 File Offset: 0x0032B437
		public BarRegionFloat(float offset, float size)
		{
			this.Start = offset;
			this.Size = size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x0600854C RID: 34124 RVA: 0x0032D25A File Offset: 0x0032B45A
		public bool Equals(XUiC_SizeBar.BarRegionFloat other)
		{
			return this.Start == other.Start && this.Size == other.Size;
		}

		// Token: 0x040062E9 RID: 25321
		public readonly float Start;

		// Token: 0x040062EA RID: 25322
		public readonly float Size;

		// Token: 0x040062EB RID: 25323
		public readonly float End;

		// Token: 0x040062EC RID: 25324
		public static readonly XUiC_SizeBar.BarRegionFloat None = new XUiC_SizeBar.BarRegionFloat(0f, 0f);
	}
}
