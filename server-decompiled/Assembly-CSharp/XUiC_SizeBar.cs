using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001050 RID: 4176
[Preserve]
public class XUiC_SizeBar : XUiController
{
	// Token: 0x060084A4 RID: 33956 RVA: 0x003273E8 File Offset: 0x003255E8
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

	// Token: 0x060084A5 RID: 33957 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060084A6 RID: 33958 RVA: 0x002CCDB9 File Offset: 0x002CAFB9
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x060084A7 RID: 33959 RVA: 0x00327510 File Offset: 0x00325710
	public void SetSelectedRegion(XUiC_SizeBar.BarRegionFloat primaryRegion)
	{
		if (!this.selectedRegion.Equals(primaryRegion))
		{
			this.selectedRegion = primaryRegion;
			this.IsDirty = true;
		}
	}

	// Token: 0x060084A8 RID: 33960 RVA: 0x0032752E File Offset: 0x0032572E
	public void SetHoveredRegion(XUiC_SizeBar.BarRegionFloat region)
	{
		if (!this.hoveredRegion.Equals(region))
		{
			this.hoveredRegion = region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060084A9 RID: 33961 RVA: 0x0032754C File Offset: 0x0032574C
	public void SetUsed(float used)
	{
		if (this.used == used)
		{
			return;
		}
		this.used = used;
		this.IsDirty = true;
	}

	// Token: 0x060084AA RID: 33962 RVA: 0x00327566 File Offset: 0x00325766
	public void SetAllowance(float allowance)
	{
		if (this.allowance == allowance)
		{
			return;
		}
		this.allowance = allowance;
		this.IsDirty = true;
	}

	// Token: 0x060084AB RID: 33963 RVA: 0x00327580 File Offset: 0x00325780
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x060084AC RID: 33964 RVA: 0x00327598 File Offset: 0x00325798
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

	// Token: 0x060084AD RID: 33965 RVA: 0x003275F4 File Offset: 0x003257F4
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

	// Token: 0x060084AE RID: 33966 RVA: 0x00327750 File Offset: 0x00325950
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

	// Token: 0x060084B1 RID: 33969 RVA: 0x003277D4 File Offset: 0x003259D4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <RefreshSelectionMode>g__GetPixelValues|27_0(XUiC_SizeBar.BarRegionFloat region, int maxPosition, out int position, out int width, ref XUiC_SizeBar.<>c__DisplayClass27_0 A_5)
	{
		position = Mathf.Min(Mathf.FloorToInt(A_5.pixelScaleFactor * region.Start), maxPosition - this.minBarWidth);
		width = Mathf.Max(Mathf.CeilToInt(A_5.pixelScaleFactor * region.Size), this.minBarWidth);
	}

	// Token: 0x060084B2 RID: 33970 RVA: 0x00327824 File Offset: 0x00325A24
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

	// Token: 0x04006255 RID: 25173
	[PublicizedFrom(EAccessModifier.Private)]
	public int minBarWidth = 1;

	// Token: 0x04006256 RID: 25174
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x04006257 RID: 25175
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006258 RID: 25176
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_used;

	// Token: 0x04006259 RID: 25177
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_excess;

	// Token: 0x0400625A RID: 25178
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_fill;

	// Token: 0x0400625B RID: 25179
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_selected_outline;

	// Token: 0x0400625C RID: 25180
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite bar_hovered_outline;

	// Token: 0x0400625D RID: 25181
	[PublicizedFrom(EAccessModifier.Private)]
	public float used;

	// Token: 0x0400625E RID: 25182
	[PublicizedFrom(EAccessModifier.Private)]
	public float allowance;

	// Token: 0x0400625F RID: 25183
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat selectedRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x04006260 RID: 25184
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar.BarRegionFloat hoveredRegion = XUiC_SizeBar.BarRegionFloat.None;

	// Token: 0x04006261 RID: 25185
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x04006262 RID: 25186
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x04006263 RID: 25187
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x04006264 RID: 25188
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x04006265 RID: 25189
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor;

	// Token: 0x02001051 RID: 4177
	public struct BarRegionFloat : IEquatable<XUiC_SizeBar.BarRegionFloat>
	{
		// Token: 0x060084B3 RID: 33971 RVA: 0x00327927 File Offset: 0x00325B27
		public BarRegionFloat(float offset, float size)
		{
			this.Start = offset;
			this.Size = size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x060084B4 RID: 33972 RVA: 0x0032794A File Offset: 0x00325B4A
		public bool Equals(XUiC_SizeBar.BarRegionFloat other)
		{
			return this.Start == other.Start && this.Size == other.Size;
		}

		// Token: 0x04006266 RID: 25190
		public readonly float Start;

		// Token: 0x04006267 RID: 25191
		public readonly float Size;

		// Token: 0x04006268 RID: 25192
		public readonly float End;

		// Token: 0x04006269 RID: 25193
		public static readonly XUiC_SizeBar.BarRegionFloat None = new XUiC_SizeBar.BarRegionFloat(0f, 0f);
	}
}
