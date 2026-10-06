using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EA1 RID: 3745
[Preserve]
public class XUiC_PlayerName : XUiController
{
	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x0600740D RID: 29709 RVA: 0x002C2434 File Offset: 0x002C0634
	// (set) Token: 0x0600740E RID: 29710 RVA: 0x002C2441 File Offset: 0x002C0641
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

	// Token: 0x0600740F RID: 29711 RVA: 0x002C245C File Offset: 0x002C065C
	public override void Init()
	{
		base.Init();
		this.rect.IsNavigatable = (this.rect.IsSnappable = false);
		base.OnPress += this.PlayerName_OnPress;
	}

	// Token: 0x06007410 RID: 29712 RVA: 0x002C249B File Offset: 0x002C069B
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnPress -= this.PlayerName_OnPress;
	}

	// Token: 0x06007411 RID: 29713 RVA: 0x002C24B5 File Offset: 0x002C06B5
	public void SetGenericName(string _name)
	{
		this.UpdatePlayerData(null, false, _name);
	}

	// Token: 0x06007412 RID: 29714 RVA: 0x002C24C0 File Offset: 0x002C06C0
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

	// Token: 0x06007413 RID: 29715 RVA: 0x002C2603 File Offset: 0x002C0803
	public void ClearPlayerData()
	{
		this.playerData = null;
		this.lblName.Text = string.Empty;
		this.lblNameCrossplay.Text = string.Empty;
		this.sprIconCrossplay.IsVisible = false;
	}

	// Token: 0x06007414 RID: 29716 RVA: 0x002C2638 File Offset: 0x002C0838
	public bool CanShowProfile()
	{
		return this.playerData != null && ((this.playerData.NativeId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.NativeId)) || (this.playerData.PrimaryId != null && PlatformManager.MultiPlatform.User.CanShowProfile(this.playerData.PrimaryId)));
	}

	// Token: 0x06007415 RID: 29717 RVA: 0x002C26A8 File Offset: 0x002C08A8
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

	// Token: 0x06007416 RID: 29718 RVA: 0x002C2745 File Offset: 0x002C0945
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerName_OnPress(XUiController _sender, int _mousebutton)
	{
		this.ShowProfile();
	}

	// Token: 0x040056AA RID: 22186
	[XuiBindComponent("playerName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Rect rect;

	// Token: 0x040056AB RID: 22187
	[XuiBindComponent("name", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblName;

	// Token: 0x040056AC RID: 22188
	[XuiBindComponent("nameCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblNameCrossplay;

	// Token: 0x040056AD RID: 22189
	[XuiBindComponent("iconCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite sprIconCrossplay;

	// Token: 0x040056AE RID: 22190
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData playerData;
}
