using System;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D8A RID: 3466
[Preserve]
public class XUiC_DataManagementBar : XUiController
{
	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x060068C2 RID: 26818 RVA: 0x002992B2 File Offset: 0x002974B2
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x060068C3 RID: 26819 RVA: 0x002992C4 File Offset: 0x002974C4
	[XuiXmlBinding("warningtext")]
	public string WarningText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			long num = this.allowanceBytes - this.usedBytes;
			if (this.displayMode == XUiC_DataManagementBar.DisplayMode.Selection || num >= this.pendingBytes)
			{
				return string.Empty;
			}
			long num2 = this.pendingBytes - num;
			return string.Format(Localization.Get("xuiDmBarRequiredSpaceWarning", false, null), ValueDisplayFormatters.MemoryMiB(num2));
		}
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x00299318 File Offset: 0x00297518
	public override void Init()
	{
		base.Init();
		this.selectionFillColor = this.barSelectedPrimaryFill.Color;
		this.fullWidth = this.background.Size.x;
		this.fullHeight = this.background.Size.y;
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x0029937C File Offset: 0x0029757C
	public void SetDisplayMode(XUiC_DataManagementBar.DisplayMode _displayMode)
	{
		if (this.displayMode == _displayMode)
		{
			return;
		}
		this.displayMode = _displayMode;
		this.IsDirty = true;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x00299396 File Offset: 0x00297596
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, XUiC_DataManagementBar.BarRegion.None, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x002993A9 File Offset: 0x002975A9
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion, XUiC_DataManagementBar.BarRegion _secondaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, _secondaryRegion, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x002993B8 File Offset: 0x002975B8
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion, XUiC_DataManagementBar.BarRegion _secondaryRegion, XUiC_DataManagementBar.BarRegion _tertiaryRegion)
	{
		if (!this.primaryByteRegion.Equals(_primaryRegion))
		{
			this.primaryByteRegion = _primaryRegion;
			this.IsDirty = true;
		}
		if (!this.secondaryByteRegion.Equals(_secondaryRegion))
		{
			this.secondaryByteRegion = _secondaryRegion;
			this.IsDirty = true;
		}
		if (!this.tertiaryByteRegion.Equals(_tertiaryRegion))
		{
			this.tertiaryByteRegion = _tertiaryRegion;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068CA RID: 26826 RVA: 0x00299419 File Offset: 0x00297619
	public void SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.hoveredByteRegion.Equals(_region))
		{
			this.hoveredByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068CB RID: 26827 RVA: 0x00299437 File Offset: 0x00297637
	public void SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.archivePreviewByteRegion.Equals(_region))
		{
			this.archivePreviewByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068CC RID: 26828 RVA: 0x00299455 File Offset: 0x00297655
	public void SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth _selectionDepth)
	{
		if (this.focusedSelectionDepth != _selectionDepth)
		{
			this.focusedSelectionDepth = _selectionDepth;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x0029946E File Offset: 0x0029766E
	public void SetDeleteHovered(bool _hovered)
	{
		if (this.deleteHovered != _hovered)
		{
			this.deleteHovered = _hovered;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x00299487 File Offset: 0x00297687
	public void SetDeleteWindowDisplayed(bool _displayed)
	{
		if (this.deleteWindowDisplayed != _displayed)
		{
			this.deleteWindowDisplayed = _displayed;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x002994A0 File Offset: 0x002976A0
	public void SetUsedBytes(long _usedBytes)
	{
		if (this.usedBytes == _usedBytes)
		{
			return;
		}
		this.usedBytes = _usedBytes;
		this.IsDirty = true;
	}

	// Token: 0x060068D0 RID: 26832 RVA: 0x002994BA File Offset: 0x002976BA
	public void SetAllowanceBytes(long _allowanceBytes)
	{
		if (this.allowanceBytes == _allowanceBytes)
		{
			return;
		}
		this.allowanceBytes = _allowanceBytes;
		this.bytesToPixels = (((float)_allowanceBytes > 0f) ? ((float)this.fullWidth / (float)_allowanceBytes) : 0f);
		this.IsDirty = true;
	}

	// Token: 0x060068D1 RID: 26833 RVA: 0x002994F4 File Offset: 0x002976F4
	public void SetPendingBytes(long _pendingBytes)
	{
		if (this.pendingBytes == _pendingBytes)
		{
			return;
		}
		this.pendingBytes = _pendingBytes;
		this.IsDirty = true;
	}

	// Token: 0x060068D2 RID: 26834 RVA: 0x0029950E File Offset: 0x0029770E
	public long GetPendingBytes()
	{
		return this.pendingBytes;
	}

	// Token: 0x060068D3 RID: 26835 RVA: 0x00299516 File Offset: 0x00297716
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.refresh();
		}
	}

	// Token: 0x060068D4 RID: 26836 RVA: 0x00299530 File Offset: 0x00297730
	[PublicizedFrom(EAccessModifier.Private)]
	public void refresh()
	{
		this.barUsed.IsVisible = false;
		this.barSelectedPrimaryFill.IsVisible = false;
		this.barSelectedSecondaryFill.IsVisible = false;
		this.barSelectedTertiaryFill.IsVisible = false;
		this.barSelectedPrimaryOutline.IsVisible = false;
		this.barSelectedSecondaryOutline.IsVisible = false;
		this.barSelectedTertiaryOutline.IsVisible = false;
		this.barHoveredOutline.IsVisible = false;
		this.barRequired.IsVisible = false;
		this.barPending.IsVisible = false;
		XUiC_DataManagementBar.DisplayMode displayMode = this.displayMode;
		if (displayMode != XUiC_DataManagementBar.DisplayMode.Selection && displayMode == XUiC_DataManagementBar.DisplayMode.Preview)
		{
			this.refreshPreviewMode();
		}
		else
		{
			this.refreshSelectionMode();
		}
		base.RefreshBindings();
		this.IsDirty = false;
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x002995E0 File Offset: 0x002977E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void refreshPreviewMode()
	{
		if (this.allowanceBytes <= 0L || (this.usedBytes <= 0L && this.pendingBytes <= 0L))
		{
			return;
		}
		int num = Mathf.CeilToInt(this.bytesToPixels * (float)this.usedBytes);
		int num2 = 0;
		int num3 = 0;
		int num6;
		if (this.pendingBytes > 0L)
		{
			long num4 = this.allowanceBytes - this.usedBytes;
			if (num4 < this.pendingBytes)
			{
				num3 = this.fullWidth - num;
				long num5 = this.pendingBytes - num4;
				num2 = Math.Clamp(Mathf.CeilToInt(this.bytesToPixels * (float)num5), 3, num);
				num6 = num - num2;
			}
			else
			{
				num3 = Math.Clamp(Mathf.CeilToInt(this.bytesToPixels * (float)this.pendingBytes), 3, this.fullWidth - 3);
				num6 = Math.Min(num, this.fullWidth - num3);
			}
		}
		else
		{
			num6 = num;
		}
		if (num6 > 0)
		{
			this.barUsed.IsVisible = true;
			this.barUsed.Size = new Vector2i(num6, this.fullHeight);
		}
		if (num2 > 0)
		{
			this.barRequired.IsVisible = true;
			this.barRequired.Position = new Vector2i(num6, 0);
			this.barRequired.Size = new Vector2i(num2, this.fullHeight);
		}
		if (num3 > 0)
		{
			this.barPending.IsVisible = true;
			this.barPending.Position = new Vector2i(num6 + num2, 0);
			this.barPending.Size = new Vector2i(num3, this.fullHeight);
		}
	}

	// Token: 0x060068D6 RID: 26838 RVA: 0x00299748 File Offset: 0x00297948
	[PublicizedFrom(EAccessModifier.Private)]
	public void refreshSelectionMode()
	{
		XUiC_DataManagementBar.<>c__DisplayClass55_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		if (this.allowanceBytes <= 0L || this.usedBytes <= 0L)
		{
			return;
		}
		int num = Mathf.CeilToInt(this.bytesToPixels * (float)this.usedBytes);
		this.barUsed.IsVisible = true;
		this.barUsed.Size = new Vector2i(Math.Max(num, 8), this.fullHeight);
		CS$<>8__locals1.maxPosition = this.fullWidth;
		this.<refreshSelectionMode>g__UpdateRegion|55_1(this.primaryByteRegion, this.barSelectedPrimaryFill, this.barSelectedPrimaryOutline, XUiC_DataManagementBar.SelectionDepth.Primary, ref CS$<>8__locals1);
		this.<refreshSelectionMode>g__UpdateRegion|55_1(this.secondaryByteRegion, this.barSelectedSecondaryFill, this.barSelectedSecondaryOutline, XUiC_DataManagementBar.SelectionDepth.Secondary, ref CS$<>8__locals1);
		this.<refreshSelectionMode>g__UpdateRegion|55_1(this.tertiaryByteRegion, this.barSelectedTertiaryFill, this.barSelectedTertiaryOutline, XUiC_DataManagementBar.SelectionDepth.Tertiary, ref CS$<>8__locals1);
	}

	// Token: 0x060068D9 RID: 26841 RVA: 0x002998AA File Offset: 0x00297AAA
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <refreshSelectionMode>g__GetPixelValues|55_0(XUiC_DataManagementBar.BarRegion _byteRegion, int _maxPosition, out int _position, out int _width, ref XUiC_DataManagementBar.<>c__DisplayClass55_0 A_5)
	{
		_position = Mathf.Min(Mathf.FloorToInt(this.bytesToPixels * (float)_byteRegion.Start), _maxPosition - 8);
		_width = Mathf.Max(Mathf.CeilToInt(this.bytesToPixels * (float)_byteRegion.Size), 8);
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x002998E8 File Offset: 0x00297AE8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <refreshSelectionMode>g__UpdateRegion|55_1(XUiC_DataManagementBar.BarRegion _byteRegion, XUiV_Sprite _fillSprite, XUiV_Sprite _outlineSprite, XUiC_DataManagementBar.SelectionDepth _depth, ref XUiC_DataManagementBar.<>c__DisplayClass55_0 A_5)
	{
		if (this.focusedSelectionDepth == _depth && this.hoveredByteRegion.Size > 0L)
		{
			int num;
			int num2;
			this.<refreshSelectionMode>g__GetPixelValues|55_0(this.hoveredByteRegion, A_5.maxPosition, out num, out num2, ref A_5);
			this.barHoveredOutline.IsVisible = true;
			this.barHoveredOutline.Position = new Vector2i(num, 0);
			this.barHoveredOutline.Size = new Vector2i(num2, this.fullHeight);
		}
		if (_byteRegion.Size <= 0L)
		{
			return;
		}
		int num3;
		int num4;
		this.<refreshSelectionMode>g__GetPixelValues|55_0(_byteRegion, A_5.maxPosition, out num3, out num4, ref A_5);
		_fillSprite.IsVisible = true;
		_fillSprite.Position = new Vector2i(num3, 0);
		_fillSprite.Size = new Vector2i(num4, this.fullHeight);
		float num5 = ((this.focusedSelectionDepth == XUiC_DataManagementBar.SelectionDepth.Secondary) ? 1f : 0.5f) * (float)(this.focusedSelectionDepth - _depth);
		Color color = this.selectionFillColor;
		color = Color.Lerp(color, Color.white, 0.5f * Mathf.Abs(num5));
		bool flag = this.deleteHovered || this.deleteWindowDisplayed;
		if (num5 > 0f)
		{
			color.a = Mathf.Lerp(color.a, 0.5f, num5);
		}
		else if (flag)
		{
			color = Color.Lerp(color, XUiC_DataManagementBar.selectionOutlineColorDelete, 0.2f);
		}
		_fillSprite.Color = color;
		if (!flag || num5 >= 0f)
		{
			_outlineSprite.IsVisible = true;
			_outlineSprite.Position = _fillSprite.Position;
			_outlineSprite.Size = _fillSprite.Size;
			Color color2;
			if (this.focusedSelectionDepth == _depth)
			{
				color2 = (flag ? XUiC_DataManagementBar.selectionOutlineColorDelete : XUiC_DataManagementBar.selectionOutlineColor);
			}
			else
			{
				color2 = XUiC_DataManagementBar.selectionOutlineColorFade;
			}
			_outlineSprite.Color = color2;
		}
		if (this.focusedSelectionDepth == _depth && this.archivePreviewByteRegion.Size > 0L)
		{
			int num6;
			int num7;
			this.<refreshSelectionMode>g__GetPixelValues|55_0(this.archivePreviewByteRegion, A_5.maxPosition, out num6, out num7, ref A_5);
			this.barPending.IsVisible = true;
			this.barPending.Position = new Vector2i(num6, 0);
			this.barPending.Size = new Vector2i(num7, this.fullHeight);
		}
		A_5.maxPosition = num3 + num4;
	}

	// Token: 0x040050AE RID: 20654
	[XuiBindComponent("background", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite background;

	// Token: 0x040050AF RID: 20655
	[XuiBindComponent("bar_used", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barUsed;

	// Token: 0x040050B0 RID: 20656
	[XuiBindComponent("bar_required", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barRequired;

	// Token: 0x040050B1 RID: 20657
	[XuiBindComponent("bar_pending", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barPending;

	// Token: 0x040050B2 RID: 20658
	[XuiBindComponent("bar_selected_primary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryFill;

	// Token: 0x040050B3 RID: 20659
	[XuiBindComponent("bar_selected_secondary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryFill;

	// Token: 0x040050B4 RID: 20660
	[XuiBindComponent("bar_selected_tertiary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryFill;

	// Token: 0x040050B5 RID: 20661
	[XuiBindComponent("bar_selected_primary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryOutline;

	// Token: 0x040050B6 RID: 20662
	[XuiBindComponent("bar_selected_secondary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryOutline;

	// Token: 0x040050B7 RID: 20663
	[XuiBindComponent("bar_selected_tertiary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryOutline;

	// Token: 0x040050B8 RID: 20664
	[XuiBindComponent("bar_hovered_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barHoveredOutline;

	// Token: 0x040050B9 RID: 20665
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.DisplayMode displayMode;

	// Token: 0x040050BA RID: 20666
	[PublicizedFrom(EAccessModifier.Private)]
	public long usedBytes;

	// Token: 0x040050BB RID: 20667
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x040050BC RID: 20668
	[PublicizedFrom(EAccessModifier.Private)]
	public long allowanceBytes;

	// Token: 0x040050BD RID: 20669
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion primaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050BE RID: 20670
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion secondaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050BF RID: 20671
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion tertiaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050C0 RID: 20672
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion hoveredByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050C1 RID: 20673
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion archivePreviewByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050C2 RID: 20674
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.SelectionDepth focusedSelectionDepth;

	// Token: 0x040050C3 RID: 20675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteHovered;

	// Token: 0x040050C4 RID: 20676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteWindowDisplayed;

	// Token: 0x040050C5 RID: 20677
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x040050C6 RID: 20678
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x040050C7 RID: 20679
	[PublicizedFrom(EAccessModifier.Private)]
	public float bytesToPixels;

	// Token: 0x040050C8 RID: 20680
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x040050C9 RID: 20681
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorFade = new Color32(250, byte.MaxValue, 163, 86);

	// Token: 0x040050CA RID: 20682
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorDelete = new Color32(234, 67, 53, byte.MaxValue);

	// Token: 0x040050CB RID: 20683
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x02000D8B RID: 3467
	public enum SelectionDepth
	{
		// Token: 0x040050CD RID: 20685
		Primary,
		// Token: 0x040050CE RID: 20686
		Secondary,
		// Token: 0x040050CF RID: 20687
		Tertiary
	}

	// Token: 0x02000D8C RID: 3468
	public readonly struct BarRegion : IEquatable<XUiC_DataManagementBar.BarRegion>
	{
		// Token: 0x060068DB RID: 26843 RVA: 0x00299B13 File Offset: 0x00297D13
		public BarRegion(long _offset, long _size)
		{
			this.Start = _offset;
			this.Size = _size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x00299B36 File Offset: 0x00297D36
		public bool Equals(XUiC_DataManagementBar.BarRegion _other)
		{
			return this.Start == _other.Start && this.Size == _other.Size;
		}

		// Token: 0x040050D0 RID: 20688
		public readonly long Start;

		// Token: 0x040050D1 RID: 20689
		public readonly long Size;

		// Token: 0x040050D2 RID: 20690
		public readonly long End;

		// Token: 0x040050D3 RID: 20691
		public static readonly XUiC_DataManagementBar.BarRegion None = new XUiC_DataManagementBar.BarRegion(0L, 0L);
	}

	// Token: 0x02000D8D RID: 3469
	public enum DisplayMode
	{
		// Token: 0x040050D5 RID: 20693
		Selection,
		// Token: 0x040050D6 RID: 20694
		Preview
	}
}
