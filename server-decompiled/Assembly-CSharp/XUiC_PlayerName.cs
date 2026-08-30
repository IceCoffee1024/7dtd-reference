using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E92 RID: 3730
[Preserve]
public class XUiC_PlayerName : XUiController
{
	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x06007415 RID: 29717 RVA: 0x002BF084 File Offset: 0x002BD284
	// (set) Token: 0x06007416 RID: 29718 RVA: 0x002BF091 File Offset: 0x002BD291
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

	// Token: 0x06007417 RID: 29719 RVA: 0x002BF0AC File Offset: 0x002BD2AC
	public override void Init()
	{
		base.Init();
		this.rect.IsNavigatable = (this.rect.IsSnappable = false);
		base.OnPress += this.PlayerName_OnPress;
	}

	// Token: 0x06007418 RID: 29720 RVA: 0x002BF0EB File Offset: 0x002BD2EB
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnPress -= this.PlayerName_OnPress;
	}

	// Token: 0x06007419 RID: 29721 RVA: 0x002BF105 File Offset: 0x002BD305
	public void SetGenericName(string _name)
	{
		this.UpdatePlayerData(null, false, _name);
	}

	// Token: 0x0600741A RID: 29722 RVA: 0x002BF110 File Offset: 0x002BD310
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

	// Token: 0x0600741B RID: 29723 RVA: 0x002BF253 File Offset: 0x002BD453
	public void ClearPlayerData()
	{
		this.playerData = null;
		this.lblName.Text = string.Empty;
		this.lblNameCrossplay.Text = string.Empty;
		this.sprIconCrossplay.IsVisible = false;
	}

	// Token: 0x0600741C RID: 29724 RVA: 0x002BF288 File Offset: 0x002BD488
	public bool CanShowProfile()
	{
		return this.playerData != null && ((this.playerData.NativeId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.NativeId)) || (this.playerData.PrimaryId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.PrimaryId)));
	}

	// Token: 0x0600741D RID: 29725 RVA: 0x002BF2F8 File Offset: 0x002BD4F8
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

	// Token: 0x0600741E RID: 29726 RVA: 0x002BF395 File Offset: 0x002BD595
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerName_OnPress(XUiController _sender, int _mousebutton)
	{
		this.ShowProfile();
	}

	// Token: 0x04005673 RID: 22131
	[XuiBindComponent("playerName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Rect rect;

	// Token: 0x04005674 RID: 22132
	[XuiBindComponent("name", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblName;

	// Token: 0x04005675 RID: 22133
	[XuiBindComponent("nameCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblNameCrossplay;

	// Token: 0x04005676 RID: 22134
	[XuiBindComponent("iconCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite sprIconCrossplay;

	// Token: 0x04005677 RID: 22135
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData playerData;
}
