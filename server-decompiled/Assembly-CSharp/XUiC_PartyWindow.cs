using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC5 RID: 4037
[Preserve]
public class XUiC_PartyWindow : XUiController
{
	// Token: 0x06007EDD RID: 32477 RVA: 0x00304348 File Offset: 0x00302548
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

	// Token: 0x06007EDE RID: 32478 RVA: 0x00304480 File Offset: 0x00302680
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PartyJoined += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave += this.Player_PartyChanged;
		this.playerDead = this.xui.playerUI.entityPlayer.IsDead();
	}

	// Token: 0x06007EDF RID: 32479 RVA: 0x00304514 File Offset: 0x00302714
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PartyJoined -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave -= this.Player_PartyChanged;
	}

	// Token: 0x06007EE0 RID: 32480 RVA: 0x0030458C File Offset: 0x0030278C
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

	// Token: 0x06007EE1 RID: 32481 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Player_PartyChanged(Party _affectedParty, EntityPlayer _player)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007EE2 RID: 32482 RVA: 0x00304614 File Offset: 0x00302814
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

	// Token: 0x04005E2C RID: 24108
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ForcedRefreshTime = 1f;

	// Token: 0x04005E2D RID: 24109
	[PublicizedFrom(EAccessModifier.Private)]
	public bool voiceActive;

	// Token: 0x04005E2E RID: 24110
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDead;

	// Token: 0x04005E2F RID: 24111
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
