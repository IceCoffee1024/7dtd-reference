using System;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D7B RID: 3451
[Preserve]
public class XUiC_DataManagementBar : XUiController
{
	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x060068CA RID: 26826 RVA: 0x00295F26 File Offset: 0x00294126
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x060068CB RID: 26827 RVA: 0x00295F38 File Offset: 0x00294138
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

	// Token: 0x060068CC RID: 26828 RVA: 0x00295F8C File Offset: 0x0029418C
	public override void Init()
	{
		base.Init();
		this.selectionFillColor = this.barSelectedPrimaryFill.Color;
		this.fullWidth = this.background.Size.x;
		this.fullHeight = this.background.Size.y;
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x00295FF0 File Offset: 0x002941F0
	public void SetDisplayMode(XUiC_DataManagementBar.DisplayMode _displayMode)
	{
		if (this.displayMode == _displayMode)
		{
			return;
		}
		this.displayMode = _displayMode;
		this.IsDirty = true;
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x0029600A File Offset: 0x0029420A
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, XUiC_DataManagementBar.BarRegion.None, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x060068D0 RID: 26832 RVA: 0x0029601D File Offset: 0x0029421D
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion, XUiC_DataManagementBar.BarRegion _secondaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, _secondaryRegion, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x060068D1 RID: 26833 RVA: 0x0029602C File Offset: 0x0029422C
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

	// Token: 0x060068D2 RID: 26834 RVA: 0x0029608D File Offset: 0x0029428D
	public void SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.hoveredByteRegion.Equals(_region))
		{
			this.hoveredByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068D3 RID: 26835 RVA: 0x002960AB File Offset: 0x002942AB
	public void SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.archivePreviewByteRegion.Equals(_region))
		{
			this.archivePreviewByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068D4 RID: 26836 RVA: 0x002960C9 File Offset: 0x002942C9
	public void SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth _selectionDepth)
	{
		if (this.focusedSelectionDepth != _selectionDepth)
		{
			this.focusedSelectionDepth = _selectionDepth;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x002960E2 File Offset: 0x002942E2
	public void SetDeleteHovered(bool _hovered)
	{
		if (this.deleteHovered != _hovered)
		{
			this.deleteHovered = _hovered;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068D6 RID: 26838 RVA: 0x002960FB File Offset: 0x002942FB
	public void SetDeleteWindowDisplayed(bool _displayed)
	{
		if (this.deleteWindowDisplayed != _displayed)
		{
			this.deleteWindowDisplayed = _displayed;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068D7 RID: 26839 RVA: 0x00296114 File Offset: 0x00294314
	public void SetUsedBytes(long _usedBytes)
	{
		if (this.usedBytes == _usedBytes)
		{
			return;
		}
		this.usedBytes = _usedBytes;
		this.IsDirty = true;
	}

	// Token: 0x060068D8 RID: 26840 RVA: 0x0029612E File Offset: 0x0029432E
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

	// Token: 0x060068D9 RID: 26841 RVA: 0x00296168 File Offset: 0x00294368
	public void SetPendingBytes(long _pendingBytes)
	{
		if (this.pendingBytes == _pendingBytes)
		{
			return;
		}
		this.pendingBytes = _pendingBytes;
		this.IsDirty = true;
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x00296182 File Offset: 0x00294382
	public long GetPendingBytes()
	{
		return this.pendingBytes;
	}

	// Token: 0x060068DB RID: 26843 RVA: 0x0029618A File Offset: 0x0029438A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.refresh();
		}
	}

	// Token: 0x060068DC RID: 26844 RVA: 0x002961A4 File Offset: 0x002943A4
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

	// Token: 0x060068DD RID: 26845 RVA: 0x00296254 File Offset: 0x00294454
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

	// Token: 0x060068DE RID: 26846 RVA: 0x002963BC File Offset: 0x002945BC
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

	// Token: 0x060068E1 RID: 26849 RVA: 0x0029651E File Offset: 0x0029471E
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <refreshSelectionMode>g__GetPixelValues|55_0(XUiC_DataManagementBar.BarRegion _byteRegion, int _maxPosition, out int _position, out int _width, ref XUiC_DataManagementBar.<>c__DisplayClass55_0 A_5)
	{
		_position = Mathf.Min(Mathf.FloorToInt(this.bytesToPixels * (float)_byteRegion.Start), _maxPosition - 8);
		_width = Mathf.Max(Mathf.CeilToInt(this.bytesToPixels * (float)_byteRegion.Size), 8);
	}

	// Token: 0x060068E2 RID: 26850 RVA: 0x0029655C File Offset: 0x0029475C
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

	// Token: 0x04005077 RID: 20599
	[XuiBindComponent("background", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite background;

	// Token: 0x04005078 RID: 20600
	[XuiBindComponent("bar_used", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barUsed;

	// Token: 0x04005079 RID: 20601
	[XuiBindComponent("bar_required", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barRequired;

	// Token: 0x0400507A RID: 20602
	[XuiBindComponent("bar_pending", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barPending;

	// Token: 0x0400507B RID: 20603
	[XuiBindComponent("bar_selected_primary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryFill;

	// Token: 0x0400507C RID: 20604
	[XuiBindComponent("bar_selected_secondary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryFill;

	// Token: 0x0400507D RID: 20605
	[XuiBindComponent("bar_selected_tertiary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryFill;

	// Token: 0x0400507E RID: 20606
	[XuiBindComponent("bar_selected_primary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryOutline;

	// Token: 0x0400507F RID: 20607
	[XuiBindComponent("bar_selected_secondary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryOutline;

	// Token: 0x04005080 RID: 20608
	[XuiBindComponent("bar_selected_tertiary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryOutline;

	// Token: 0x04005081 RID: 20609
	[XuiBindComponent("bar_hovered_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barHoveredOutline;

	// Token: 0x04005082 RID: 20610
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.DisplayMode displayMode;

	// Token: 0x04005083 RID: 20611
	[PublicizedFrom(EAccessModifier.Private)]
	public long usedBytes;

	// Token: 0x04005084 RID: 20612
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x04005085 RID: 20613
	[PublicizedFrom(EAccessModifier.Private)]
	public long allowanceBytes;

	// Token: 0x04005086 RID: 20614
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion primaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04005087 RID: 20615
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion secondaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04005088 RID: 20616
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion tertiaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04005089 RID: 20617
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion hoveredByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x0400508A RID: 20618
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion archivePreviewByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x0400508B RID: 20619
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.SelectionDepth focusedSelectionDepth;

	// Token: 0x0400508C RID: 20620
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteHovered;

	// Token: 0x0400508D RID: 20621
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteWindowDisplayed;

	// Token: 0x0400508E RID: 20622
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x0400508F RID: 20623
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x04005090 RID: 20624
	[PublicizedFrom(EAccessModifier.Private)]
	public float bytesToPixels;

	// Token: 0x04005091 RID: 20625
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x04005092 RID: 20626
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorFade = new Color32(250, byte.MaxValue, 163, 86);

	// Token: 0x04005093 RID: 20627
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorDelete = new Color32(234, 67, 53, byte.MaxValue);

	// Token: 0x04005094 RID: 20628
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x02000D7C RID: 3452
	public enum SelectionDepth
	{
		// Token: 0x04005096 RID: 20630
		Primary,
		// Token: 0x04005097 RID: 20631
		Secondary,
		// Token: 0x04005098 RID: 20632
		Tertiary
	}

	// Token: 0x02000D7D RID: 3453
	public readonly struct BarRegion : IEquatable<XUiC_DataManagementBar.BarRegion>
	{
		// Token: 0x060068E3 RID: 26851 RVA: 0x00296787 File Offset: 0x00294987
		public BarRegion(long _offset, long _size)
		{
			this.Start = _offset;
			this.Size = _size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x060068E4 RID: 26852 RVA: 0x002967AA File Offset: 0x002949AA
		public bool Equals(XUiC_DataManagementBar.BarRegion _other)
		{
			return this.Start == _other.Start && this.Size == _other.Size;
		}

		// Token: 0x04005099 RID: 20633
		public readonly long Start;

		// Token: 0x0400509A RID: 20634
		public readonly long Size;

		// Token: 0x0400509B RID: 20635
		public readonly long End;

		// Token: 0x0400509C RID: 20636
		public static readonly XUiC_DataManagementBar.BarRegion None = new XUiC_DataManagementBar.BarRegion(0L, 0L);
	}

	// Token: 0x02000D7E RID: 3454
	public enum DisplayMode
	{
		// Token: 0x0400509E RID: 20638
		Selection,
		// Token: 0x0400509F RID: 20639
		Preview
	}
}
