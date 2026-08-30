using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA6 RID: 4006
[Preserve]
public class XUiC_PartyWindow : XUiController
{
	// Token: 0x06007E7F RID: 32383 RVA: 0x002FF854 File Offset: 0x002FDA54
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

	// Token: 0x06007E80 RID: 32384 RVA: 0x002FF98C File Offset: 0x002FDB8C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PartyJoined += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave += this.Player_PartyChanged;
		this.playerDead = this.xui.playerUI.entityPlayer.IsDead();
	}

	// Token: 0x06007E81 RID: 32385 RVA: 0x002FFA20 File Offset: 0x002FDC20
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PartyJoined -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave -= this.Player_PartyChanged;
	}

	// Token: 0x06007E82 RID: 32386 RVA: 0x002FFA98 File Offset: 0x002FDC98
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

	// Token: 0x06007E83 RID: 32387 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Player_PartyChanged(Party _affectedParty, EntityPlayer _player)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007E84 RID: 32388 RVA: 0x002FFB20 File Offset: 0x002FDD20
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

	// Token: 0x04005DC8 RID: 24008
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ForcedRefreshTime = 1f;

	// Token: 0x04005DC9 RID: 24009
	[PublicizedFrom(EAccessModifier.Private)]
	public bool voiceActive;

	// Token: 0x04005DCA RID: 24010
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDead;

	// Token: 0x04005DCB RID: 24011
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
