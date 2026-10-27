using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EAC RID: 3756
[Preserve]
public class XUiC_PlayerName : XUiController
{
	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x06007432 RID: 29746 RVA: 0x002C39A0 File Offset: 0x002C1BA0
	// (set) Token: 0x06007433 RID: 29747 RVA: 0x002C39AD File Offset: 0x002C1BAD
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

	// Token: 0x06007434 RID: 29748 RVA: 0x002C39C8 File Offset: 0x002C1BC8
	public override void Init()
	{
		base.Init();
		this.rect.IsNavigatable = (this.rect.IsSnappable = false);
		base.OnPress += this.PlayerName_OnPress;
	}

	// Token: 0x06007435 RID: 29749 RVA: 0x002C3A07 File Offset: 0x002C1C07
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnPress -= this.PlayerName_OnPress;
	}

	// Token: 0x06007436 RID: 29750 RVA: 0x002C3A21 File Offset: 0x002C1C21
	public void SetGenericName(string _name)
	{
		this.UpdatePlayerData(null, false, _name);
	}

	// Token: 0x06007437 RID: 29751 RVA: 0x002C3A2C File Offset: 0x002C1C2C
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

	// Token: 0x06007438 RID: 29752 RVA: 0x002C3B6F File Offset: 0x002C1D6F
	public void ClearPlayerData()
	{
		this.playerData = null;
		this.lblName.Text = string.Empty;
		this.lblNameCrossplay.Text = string.Empty;
		this.sprIconCrossplay.IsVisible = false;
	}

	// Token: 0x06007439 RID: 29753 RVA: 0x002C3BA4 File Offset: 0x002C1DA4
	public bool CanShowProfile()
	{
		return this.playerData != null && ((this.playerData.NativeId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.NativeId)) || (this.playerData.PrimaryId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.PrimaryId)));
	}

	// Token: 0x0600743A RID: 29754 RVA: 0x002C3C14 File Offset: 0x002C1E14
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

	// Token: 0x0600743B RID: 29755 RVA: 0x002C3CB1 File Offset: 0x002C1EB1
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerName_OnPress(XUiController _sender, int _mousebutton)
	{
		this.ShowProfile();
	}

	// Token: 0x040056CF RID: 22223
	[XuiBindComponent("playerName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Rect rect;

	// Token: 0x040056D0 RID: 22224
	[XuiBindComponent("name", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblName;

	// Token: 0x040056D1 RID: 22225
	[XuiBindComponent("nameCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblNameCrossplay;

	// Token: 0x040056D2 RID: 22226
	[XuiBindComponent("iconCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite sprIconCrossplay;

	// Token: 0x040056D3 RID: 22227
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData playerData;
}
