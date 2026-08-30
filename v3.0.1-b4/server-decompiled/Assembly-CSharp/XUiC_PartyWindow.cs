using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F8D RID: 3981
[Preserve]
public class XUiC_PartyWindow : XUiController
{
	// Token: 0x06007D8D RID: 32141 RVA: 0x002FB080 File Offset: 0x002F9280
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

	// Token: 0x06007D8E RID: 32142 RVA: 0x002FB1B8 File Offset: 0x002F93B8
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PartyJoined += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged += this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave += this.Player_PartyChanged;
		this.playerDead = this.xui.playerUI.entityPlayer.IsDead();
	}

	// Token: 0x06007D8F RID: 32143 RVA: 0x002FB24C File Offset: 0x002F944C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PartyJoined -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyChanged -= this.Player_PartyChanged;
		this.xui.playerUI.entityPlayer.PartyLeave -= this.Player_PartyChanged;
	}

	// Token: 0x06007D90 RID: 32144 RVA: 0x002FB2C4 File Offset: 0x002F94C4
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

	// Token: 0x06007D91 RID: 32145 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Player_PartyChanged(Party _affectedParty, EntityPlayer _player)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007D92 RID: 32146 RVA: 0x002FB34C File Offset: 0x002F954C
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

	// Token: 0x04005D01 RID: 23809
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ForcedRefreshTime = 1f;

	// Token: 0x04005D02 RID: 23810
	[PublicizedFrom(EAccessModifier.Private)]
	public bool voiceActive;

	// Token: 0x04005D03 RID: 23811
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDead;

	// Token: 0x04005D04 RID: 23812
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
