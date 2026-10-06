using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB5 RID: 4021
[Preserve]
public class XUiC_PartyWindow : XUiController
{
	// Token: 0x06007E8E RID: 32398 RVA: 0x0030282C File Offset: 0x00300A2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "partyvisible")
		{
			_value = ((this.xui.playerUI.entityPlayer != null) ? (this.xui.playerUI.entityPlayer.Party != null && !this.playerDead).ToString() : "false");
			return true;
		}
		if (_bindingName == "isleader")
		{
			_value = ((this.xui.playerUI.entityPlayer != null) ? (this.xui.playerUI.entityPlayer.Party != null && this.xui.playerUI.entityPlayer.Party.Leader == this.xui.playerUI.entityPlayer).ToString() : "false");
			return true;
		}
		if (_bindingName == "voicevisible")
		{
			_value = VoiceHelpers.InAnyVoiceChat.ToString();
			return true;
		}
		if (!(_bindingName == "voiceactive"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.voiceActive.ToString();
		return true;
	}

	// Token: 0x06007E8F RID: 32399 RVA: 0x00302964 File Offset: 0x00300B64
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PartyJoined += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave += this.Player_PartyChanged;
		this.playerDead = this.xui.playerUI.entityPlayer.IsDead();
	}

	// Token: 0x06007E90 RID: 32400 RVA: 0x003029F8 File Offset: 0x00300BF8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PartyJoined -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave -= this.Player_PartyChanged;
	}

	// Token: 0x06007E91 RID: 32401 RVA: 0x00302A70 File Offset: 0x00300C70
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.xui.playerUI.entityPlayer == null)
		{
			return;
		}
		float time = Time.time;
		bool flag = time > this.updateTime;
		bool flag2 = this.xui.playerUI.entityPlayer.IsDead();
		bool flag3 = flag2 != this.playerDead;
		this.playerDead = flag2;
		if (flag || flag3 || this.updateVoiceState())
		{
			this.updateTime = time + 1f;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007E92 RID: 32402 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Player_PartyChanged(Party _affectedParty, EntityPlayer _player)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007E93 RID: 32403 RVA: 0x00302AF8 File Offset: 0x00300CF8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateVoiceState()
	{
		if (GameManager.IsDedicatedServer)
		{
			return false;
		}
		if (GameStats.GetInt(EnumGameStats.GameState) != 1)
		{
			return false;
		}
		if (this.xui.playerUI.entityPlayer == null)
		{
			return false;
		}
		bool flag = VoiceHelpers.LocalPlayerTalking();
		if (flag == this.voiceActive)
		{
			return false;
		}
		this.voiceActive = flag;
		return true;
	}

	// Token: 0x04005DF1 RID: 24049
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ForcedRefreshTime = 1f;

	// Token: 0x04005DF2 RID: 24050
	[PublicizedFrom(EAccessModifier.Private)]
	public bool voiceActive;

	// Token: 0x04005DF3 RID: 24051
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDead;

	// Token: 0x04005DF4 RID: 24052
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
