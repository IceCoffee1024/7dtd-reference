using System;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D67 RID: 3431
[Preserve]
public class XUiC_DataManagementBar : XUiController
{
	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x0600682E RID: 26670 RVA: 0x00292DD2 File Offset: 0x00290FD2
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x0600682F RID: 26671 RVA: 0x00292DE4 File Offset: 0x00290FE4
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

	// Token: 0x06006830 RID: 26672 RVA: 0x00292E38 File Offset: 0x00291038
	public override void Init()
	{
		base.Init();
		this.selectionFillColor = this.barSelectedPrimaryFill.Color;
		this.fullWidth = this.background.Size.x;
		this.fullHeight = this.background.Size.y;
	}

	// Token: 0x06006831 RID: 26673 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06006832 RID: 26674 RVA: 0x00292E9C File Offset: 0x0029109C
	public void SetDisplayMode(XUiC_DataManagementBar.DisplayMode _displayMode)
	{
		if (this.displayMode == _displayMode)
		{
			return;
		}
		this.displayMode = _displayMode;
		this.IsDirty = true;
	}

	// Token: 0x06006833 RID: 26675 RVA: 0x00292EB6 File Offset: 0x002910B6
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, XUiC_DataManagementBar.BarRegion.None, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x06006834 RID: 26676 RVA: 0x00292EC9 File Offset: 0x002910C9
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion, XUiC_DataManagementBar.BarRegion _secondaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, _secondaryRegion, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x06006835 RID: 26677 RVA: 0x00292ED8 File Offset: 0x002910D8
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

	// Token: 0x06006836 RID: 26678 RVA: 0x00292F39 File Offset: 0x00291139
	public void SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.hoveredByteRegion.Equals(_region))
		{
			this.hoveredByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006837 RID: 26679 RVA: 0x00292F57 File Offset: 0x00291157
	public void SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.archivePreviewByteRegion.Equals(_region))
		{
			this.archivePreviewByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006838 RID: 26680 RVA: 0x00292F75 File Offset: 0x00291175
	public void SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth _selectionDepth)
	{
		if (this.focusedSelectionDepth != _selectionDepth)
		{
			this.focusedSelectionDepth = _selectionDepth;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006839 RID: 26681 RVA: 0x00292F8E File Offset: 0x0029118E
	public void SetDeleteHovered(bool _hovered)
	{
		if (this.deleteHovered != _hovered)
		{
			this.deleteHovered = _hovered;
			this.IsDirty = true;
		}
	}

	// Token: 0x0600683A RID: 26682 RVA: 0x00292FA7 File Offset: 0x002911A7
	public void SetDeleteWindowDisplayed(bool _displayed)
	{
		if (this.deleteWindowDisplayed != _displayed)
		{
			this.deleteWindowDisplayed = _displayed;
			this.IsDirty = true;
		}
	}

	// Token: 0x0600683B RID: 26683 RVA: 0x00292FC0 File Offset: 0x002911C0
	public void SetUsedBytes(long _usedBytes)
	{
		if (this.usedBytes == _usedBytes)
		{
			return;
		}
		this.usedBytes = _usedBytes;
		this.IsDirty = true;
	}

	// Token: 0x0600683C RID: 26684 RVA: 0x00292FDA File Offset: 0x002911DA
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

	// Token: 0x0600683D RID: 26685 RVA: 0x00293014 File Offset: 0x00291214
	public void SetPendingBytes(long _pendingBytes)
	{
		if (this.pendingBytes == _pendingBytes)
		{
			return;
		}
		this.pendingBytes = _pendingBytes;
		this.IsDirty = true;
	}

	// Token: 0x0600683E RID: 26686 RVA: 0x0029302E File Offset: 0x0029122E
	public long GetPendingBytes()
	{
		return this.pendingBytes;
	}

	// Token: 0x0600683F RID: 26687 RVA: 0x00293036 File Offset: 0x00291236
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.refresh();
		}
	}

	// Token: 0x06006840 RID: 26688 RVA: 0x00293050 File Offset: 0x00291250
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

	// Token: 0x06006841 RID: 26689 RVA: 0x00293100 File Offset: 0x00291300
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

	// Token: 0x06006842 RID: 26690 RVA: 0x00293268 File Offset: 0x00291468
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

	// Token: 0x06006845 RID: 26693 RVA: 0x002933CA File Offset: 0x002915CA
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <refreshSelectionMode>g__GetPixelValues|55_0(XUiC_DataManagementBar.BarRegion _byteRegion, int _maxPosition, out int _position, out int _width, ref XUiC_DataManagementBar.<>c__DisplayClass55_0 A_5)
	{
		_position = Mathf.Min(Mathf.FloorToInt(this.bytesToPixels * (float)_byteRegion.Start), _maxPosition - 8);
		_width = Mathf.Max(Mathf.CeilToInt(this.bytesToPixels * (float)_byteRegion.Size), 8);
	}

	// Token: 0x06006846 RID: 26694 RVA: 0x00293408 File Offset: 0x00291608
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

	// Token: 0x04004FD5 RID: 20437
	[XuiBindComponent("background", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite background;

	// Token: 0x04004FD6 RID: 20438
	[XuiBindComponent("bar_used", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barUsed;

	// Token: 0x04004FD7 RID: 20439
	[XuiBindComponent("bar_required", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barRequired;

	// Token: 0x04004FD8 RID: 20440
	[XuiBindComponent("bar_pending", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barPending;

	// Token: 0x04004FD9 RID: 20441
	[XuiBindComponent("bar_selected_primary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryFill;

	// Token: 0x04004FDA RID: 20442
	[XuiBindComponent("bar_selected_secondary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryFill;

	// Token: 0x04004FDB RID: 20443
	[XuiBindComponent("bar_selected_tertiary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryFill;

	// Token: 0x04004FDC RID: 20444
	[XuiBindComponent("bar_selected_primary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryOutline;

	// Token: 0x04004FDD RID: 20445
	[XuiBindComponent("bar_selected_secondary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryOutline;

	// Token: 0x04004FDE RID: 20446
	[XuiBindComponent("bar_selected_tertiary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryOutline;

	// Token: 0x04004FDF RID: 20447
	[XuiBindComponent("bar_hovered_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barHoveredOutline;

	// Token: 0x04004FE0 RID: 20448
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.DisplayMode displayMode;

	// Token: 0x04004FE1 RID: 20449
	[PublicizedFrom(EAccessModifier.Private)]
	public long usedBytes;

	// Token: 0x04004FE2 RID: 20450
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x04004FE3 RID: 20451
	[PublicizedFrom(EAccessModifier.Private)]
	public long allowanceBytes;

	// Token: 0x04004FE4 RID: 20452
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion primaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04004FE5 RID: 20453
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion secondaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04004FE6 RID: 20454
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion tertiaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04004FE7 RID: 20455
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion hoveredByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04004FE8 RID: 20456
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion archivePreviewByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x04004FE9 RID: 20457
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.SelectionDepth focusedSelectionDepth;

	// Token: 0x04004FEA RID: 20458
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteHovered;

	// Token: 0x04004FEB RID: 20459
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteWindowDisplayed;

	// Token: 0x04004FEC RID: 20460
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x04004FED RID: 20461
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x04004FEE RID: 20462
	[PublicizedFrom(EAccessModifier.Private)]
	public float bytesToPixels;

	// Token: 0x04004FEF RID: 20463
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x04004FF0 RID: 20464
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorFade = new Color32(250, byte.MaxValue, 163, 86);

	// Token: 0x04004FF1 RID: 20465
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorDelete = new Color32(234, 67, 53, byte.MaxValue);

	// Token: 0x04004FF2 RID: 20466
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x02000D68 RID: 3432
	public enum SelectionDepth
	{
		// Token: 0x04004FF4 RID: 20468
		Primary,
		// Token: 0x04004FF5 RID: 20469
		Secondary,
		// Token: 0x04004FF6 RID: 20470
		Tertiary
	}

	// Token: 0x02000D69 RID: 3433
	public readonly struct BarRegion : IEquatable<XUiC_DataManagementBar.BarRegion>
	{
		// Token: 0x06006847 RID: 26695 RVA: 0x00293633 File Offset: 0x00291833
		public BarRegion(long _offset, long _size)
		{
			this.Start = _offset;
			this.Size = _size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x06006848 RID: 26696 RVA: 0x00293656 File Offset: 0x00291856
		public bool Equals(XUiC_DataManagementBar.BarRegion _other)
		{
			return this.Start == _other.Start && this.Size == _other.Size;
		}

		// Token: 0x04004FF7 RID: 20471
		public readonly long Start;

		// Token: 0x04004FF8 RID: 20472
		public readonly long Size;

		// Token: 0x04004FF9 RID: 20473
		public readonly long End;

		// Token: 0x04004FFA RID: 20474
		public static readonly XUiC_DataManagementBar.BarRegion None = new XUiC_DataManagementBar.BarRegion(0L, 0L);
	}

	// Token: 0x02000D6A RID: 3434
	public enum DisplayMode
	{
		// Token: 0x04004FFC RID: 20476
		Selection,
		// Token: 0x04004FFD RID: 20477
		Preview
	}
}
