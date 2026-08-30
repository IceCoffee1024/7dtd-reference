using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E7D RID: 3709
[Preserve]
public class XUiC_PlayerName : XUiController
{
	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x06007375 RID: 29557 RVA: 0x002BBB58 File Offset: 0x002B9D58
	// (set) Token: 0x06007376 RID: 29558 RVA: 0x002BBB65 File Offset: 0x002B9D65
	public Color Color
	{
		get
		{
			return this.lblName.Color;
		}
		set
		{
			this.lblName.Color = value;
			this.lblNameCrossplay.Color = value;
		}
	}

	// Token: 0x06007377 RID: 29559 RVA: 0x002BBB80 File Offset: 0x002B9D80
	public override void Init()
	{
		base.Init();
		this.rect.IsNavigatable = (this.rect.IsSnappable = false);
		base.OnPress += this.PlayerName_OnPress;
	}

	// Token: 0x06007378 RID: 29560 RVA: 0x002BBBBF File Offset: 0x002B9DBF
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnPress -= this.PlayerName_OnPress;
	}

	// Token: 0x06007379 RID: 29561 RVA: 0x002BBBD9 File Offset: 0x002B9DD9
	public void SetGenericName(string _name)
	{
		this.UpdatePlayerData(null, false, _name);
	}

	// Token: 0x0600737A RID: 29562 RVA: 0x002BBBE4 File Offset: 0x002B9DE4
	public void UpdatePlayerData(PlayerData _playerData, bool _showCrossplay, string _displayName = null)
	{
		this.playerData = _playerData;
		bool flag = false;
		if (_displayName != null)
		{
			XUiV_Label xuiV_Label = this.lblName;
			this.lblNameCrossplay.Text = _displayName;
			xuiV_Label.Text = _displayName;
			flag = true;
		}
		else if (this.playerData != null)
		{
			GeneratedTextManager.GetDisplayText(this.playerData.PlayerName, delegate(string _name)
			{
				XUiV_Label xuiV_Label2 = this.lblName;
				this.lblNameCrossplay.Text = _name;
				xuiV_Label2.Text = _name;
			}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
			flag = true;
		}
		this.rect.EventOnPress = flag && this.CanShowProfile();
		this.rect.IsNavigatable = (this.rect.IsSnappable = flag);
		if (_showCrossplay && this.playerData != null && this.playerData.PlayGroup != EPlayGroup.Unknown)
		{
			this.sprIconCrossplay.SpriteName = PlatformManager.NativePlatform.Utils.GetCrossplayPlayerIcon(this.playerData.PlayGroup, true, this.playerData.NativeId.PlatformIdentifier);
			this.sprIconCrossplay.UIAtlas = "SymbolAtlas";
			this.sprIconCrossplay.IsVisible = true;
			this.lblName.IsVisible = false;
			this.lblNameCrossplay.IsVisible = true;
		}
		else
		{
			this.sprIconCrossplay.IsVisible = false;
			this.lblName.IsVisible = true;
			this.lblNameCrossplay.IsVisible = false;
		}
		base.RefreshBindings();
	}

	// Token: 0x0600737B RID: 29563 RVA: 0x002BBD27 File Offset: 0x002B9F27
	public void ClearPlayerData()
	{
		this.playerData = null;
		this.lblName.Text = string.Empty;
		this.lblNameCrossplay.Text = string.Empty;
		this.sprIconCrossplay.IsVisible = false;
	}

	// Token: 0x0600737C RID: 29564 RVA: 0x002BBD5C File Offset: 0x002B9F5C
	public bool CanShowProfile()
	{
		return this.playerData != null && ((this.playerData.NativeId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.NativeId)) || (this.playerData.PrimaryId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.PrimaryId)));
	}

	// Token: 0x0600737D RID: 29565 RVA: 0x002BBDCC File Offset: 0x002B9FCC
	public void ShowProfile()
	{
		if (this.playerData == null)
		{
			return;
		}
		if (this.playerData.NativeId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.NativeId))
		{
			PlatformManager.MultiPlatform.User.ShowProfile(this.playerData.NativeId);
			return;
		}
		if (this.playerData.PrimaryId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.PrimaryId))
		{
			PlatformManager.MultiPlatform.User.ShowProfile(this.playerData.PrimaryId);
		}
	}

	// Token: 0x0600737E RID: 29566 RVA: 0x002BBE69 File Offset: 0x002BA069
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerName_OnPress(XUiController _sender, int _mousebutton)
	{
		this.ShowProfile();
	}

	// Token: 0x040055CF RID: 21967
	[XuiBindComponent("playerName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Rect rect;

	// Token: 0x040055D0 RID: 21968
	[XuiBindComponent("name", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblName;

	// Token: 0x040055D1 RID: 21969
	[XuiBindComponent("nameCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblNameCrossplay;

	// Token: 0x040055D2 RID: 21970
	[XuiBindComponent("iconCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite sprIconCrossplay;

	// Token: 0x040055D3 RID: 21971
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData playerData;
}
