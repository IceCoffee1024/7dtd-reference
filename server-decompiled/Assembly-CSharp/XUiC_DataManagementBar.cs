using System;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D95 RID: 3477
[Preserve]
public class XUiC_DataManagementBar : XUiController
{
	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x060068E7 RID: 26855 RVA: 0x0029A7DA File Offset: 0x002989DA
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x060068E8 RID: 26856 RVA: 0x0029A7EC File Offset: 0x002989EC
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

	// Token: 0x060068E9 RID: 26857 RVA: 0x0029A840 File Offset: 0x00298A40
	public override void Init()
	{
		base.Init();
		this.selectionFillColor = this.barSelectedPrimaryFill.Color;
		this.fullWidth = this.background.Size.x;
		this.fullHeight = this.background.Size.y;
	}

	// Token: 0x060068EA RID: 26858 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060068EB RID: 26859 RVA: 0x0029A8A4 File Offset: 0x00298AA4
	public void SetDisplayMode(XUiC_DataManagementBar.DisplayMode _displayMode)
	{
		if (this.displayMode == _displayMode)
		{
			return;
		}
		this.displayMode = _displayMode;
		this.IsDirty = true;
	}

	// Token: 0x060068EC RID: 26860 RVA: 0x0029A8BE File Offset: 0x00298ABE
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, XUiC_DataManagementBar.BarRegion.None, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x060068ED RID: 26861 RVA: 0x0029A8D1 File Offset: 0x00298AD1
	public void SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion _primaryRegion, XUiC_DataManagementBar.BarRegion _secondaryRegion)
	{
		this.SetSelectedByteRegion(_primaryRegion, _secondaryRegion, XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x060068EE RID: 26862 RVA: 0x0029A8E0 File Offset: 0x00298AE0
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

	// Token: 0x060068EF RID: 26863 RVA: 0x0029A941 File Offset: 0x00298B41
	public void SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.hoveredByteRegion.Equals(_region))
		{
			this.hoveredByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068F0 RID: 26864 RVA: 0x0029A95F File Offset: 0x00298B5F
	public void SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion _region)
	{
		if (!this.archivePreviewByteRegion.Equals(_region))
		{
			this.archivePreviewByteRegion = _region;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068F1 RID: 26865 RVA: 0x0029A97D File Offset: 0x00298B7D
	public void SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth _selectionDepth)
	{
		if (this.focusedSelectionDepth != _selectionDepth)
		{
			this.focusedSelectionDepth = _selectionDepth;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068F2 RID: 26866 RVA: 0x0029A996 File Offset: 0x00298B96
	public void SetDeleteHovered(bool _hovered)
	{
		if (this.deleteHovered != _hovered)
		{
			this.deleteHovered = _hovered;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068F3 RID: 26867 RVA: 0x0029A9AF File Offset: 0x00298BAF
	public void SetDeleteWindowDisplayed(bool _displayed)
	{
		if (this.deleteWindowDisplayed != _displayed)
		{
			this.deleteWindowDisplayed = _displayed;
			this.IsDirty = true;
		}
	}

	// Token: 0x060068F4 RID: 26868 RVA: 0x0029A9C8 File Offset: 0x00298BC8
	public void SetUsedBytes(long _usedBytes)
	{
		if (this.usedBytes == _usedBytes)
		{
			return;
		}
		this.usedBytes = _usedBytes;
		this.IsDirty = true;
	}

	// Token: 0x060068F5 RID: 26869 RVA: 0x0029A9E2 File Offset: 0x00298BE2
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

	// Token: 0x060068F6 RID: 26870 RVA: 0x0029AA1C File Offset: 0x00298C1C
	public void SetPendingBytes(long _pendingBytes)
	{
		if (this.pendingBytes == _pendingBytes)
		{
			return;
		}
		this.pendingBytes = _pendingBytes;
		this.IsDirty = true;
	}

	// Token: 0x060068F7 RID: 26871 RVA: 0x0029AA36 File Offset: 0x00298C36
	public long GetPendingBytes()
	{
		return this.pendingBytes;
	}

	// Token: 0x060068F8 RID: 26872 RVA: 0x0029AA3E File Offset: 0x00298C3E
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.refresh();
		}
	}

	// Token: 0x060068F9 RID: 26873 RVA: 0x0029AA58 File Offset: 0x00298C58
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

	// Token: 0x060068FA RID: 26874 RVA: 0x0029AB08 File Offset: 0x00298D08
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

	// Token: 0x060068FB RID: 26875 RVA: 0x0029AC70 File Offset: 0x00298E70
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

	// Token: 0x060068FE RID: 26878 RVA: 0x0029ADD2 File Offset: 0x00298FD2
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <refreshSelectionMode>g__GetPixelValues|55_0(XUiC_DataManagementBar.BarRegion _byteRegion, int _maxPosition, out int _position, out int _width, ref XUiC_DataManagementBar.<>c__DisplayClass55_0 A_5)
	{
		_position = Mathf.Min(Mathf.FloorToInt(this.bytesToPixels * (float)_byteRegion.Start), _maxPosition - 8);
		_width = Mathf.Max(Mathf.CeilToInt(this.bytesToPixels * (float)_byteRegion.Size), 8);
	}

	// Token: 0x060068FF RID: 26879 RVA: 0x0029AE10 File Offset: 0x00299010
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

	// Token: 0x040050D3 RID: 20691
	[XuiBindComponent("background", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite background;

	// Token: 0x040050D4 RID: 20692
	[XuiBindComponent("bar_used", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barUsed;

	// Token: 0x040050D5 RID: 20693
	[XuiBindComponent("bar_required", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barRequired;

	// Token: 0x040050D6 RID: 20694
	[XuiBindComponent("bar_pending", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barPending;

	// Token: 0x040050D7 RID: 20695
	[XuiBindComponent("bar_selected_primary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryFill;

	// Token: 0x040050D8 RID: 20696
	[XuiBindComponent("bar_selected_secondary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryFill;

	// Token: 0x040050D9 RID: 20697
	[XuiBindComponent("bar_selected_tertiary_fill", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryFill;

	// Token: 0x040050DA RID: 20698
	[XuiBindComponent("bar_selected_primary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedPrimaryOutline;

	// Token: 0x040050DB RID: 20699
	[XuiBindComponent("bar_selected_secondary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedSecondaryOutline;

	// Token: 0x040050DC RID: 20700
	[XuiBindComponent("bar_selected_tertiary_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barSelectedTertiaryOutline;

	// Token: 0x040050DD RID: 20701
	[XuiBindComponent("bar_hovered_outline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite barHoveredOutline;

	// Token: 0x040050DE RID: 20702
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.DisplayMode displayMode;

	// Token: 0x040050DF RID: 20703
	[PublicizedFrom(EAccessModifier.Private)]
	public long usedBytes;

	// Token: 0x040050E0 RID: 20704
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x040050E1 RID: 20705
	[PublicizedFrom(EAccessModifier.Private)]
	public long allowanceBytes;

	// Token: 0x040050E2 RID: 20706
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion primaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050E3 RID: 20707
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion secondaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050E4 RID: 20708
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion tertiaryByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050E5 RID: 20709
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion hoveredByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050E6 RID: 20710
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.BarRegion archivePreviewByteRegion = XUiC_DataManagementBar.BarRegion.None;

	// Token: 0x040050E7 RID: 20711
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar.SelectionDepth focusedSelectionDepth;

	// Token: 0x040050E8 RID: 20712
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteHovered;

	// Token: 0x040050E9 RID: 20713
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deleteWindowDisplayed;

	// Token: 0x040050EA RID: 20714
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullWidth;

	// Token: 0x040050EB RID: 20715
	[PublicizedFrom(EAccessModifier.Private)]
	public int fullHeight;

	// Token: 0x040050EC RID: 20716
	[PublicizedFrom(EAccessModifier.Private)]
	public float bytesToPixels;

	// Token: 0x040050ED RID: 20717
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColor = new Color32(250, byte.MaxValue, 163, 193);

	// Token: 0x040050EE RID: 20718
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorFade = new Color32(250, byte.MaxValue, 163, 86);

	// Token: 0x040050EF RID: 20719
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 selectionOutlineColorDelete = new Color32(234, 67, 53, byte.MaxValue);

	// Token: 0x040050F0 RID: 20720
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectionFillColor;

	// Token: 0x02000D96 RID: 3478
	public enum SelectionDepth
	{
		// Token: 0x040050F2 RID: 20722
		Primary,
		// Token: 0x040050F3 RID: 20723
		Secondary,
		// Token: 0x040050F4 RID: 20724
		Tertiary
	}

	// Token: 0x02000D97 RID: 3479
	public readonly struct BarRegion : IEquatable<XUiC_DataManagementBar.BarRegion>
	{
		// Token: 0x06006900 RID: 26880 RVA: 0x0029B03B File Offset: 0x0029923B
		public BarRegion(long _offset, long _size)
		{
			this.Start = _offset;
			this.Size = _size;
			this.End = this.Start + this.Size;
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x0029B05E File Offset: 0x0029925E
		public bool Equals(XUiC_DataManagementBar.BarRegion _other)
		{
			return this.Start == _other.Start && this.Size == _other.Size;
		}

		// Token: 0x040050F5 RID: 20725
		public readonly long Start;

		// Token: 0x040050F6 RID: 20726
		public readonly long Size;

		// Token: 0x040050F7 RID: 20727
		public readonly long End;

		// Token: 0x040050F8 RID: 20728
		public static readonly XUiC_DataManagementBar.BarRegion None = new XUiC_DataManagementBar.BarRegion(0L, 0L);
	}

	// Token: 0x02000D98 RID: 3480
	public enum DisplayMode
	{
		// Token: 0x040050FA RID: 20730
		Selection,
		// Token: 0x040050FB RID: 20731
		Preview
	}
}
