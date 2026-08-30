using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02000C9E RID: 3230
public static class VoiceHelpers
{
	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x06006341 RID: 25409 RVA: 0x0026F54B File Offset: 0x0026D74B
	public static bool VoiceAllowed
	{
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x06006342 RID: 25410 RVA: 0x0026F552 File Offset: 0x0026D752
	public static bool PlatformVoiceEnabled
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsVoiceChatEnabled) && VoiceHelpers.VoiceAllowed;
		}
	}

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x06006343 RID: 25411 RVA: 0x0026F564 File Offset: 0x0026D764
	public static bool InAnyVoiceChat
	{
		get
		{
			return VoiceHelpers.VoiceAllowed && (DiscordManager.Instance.ActiveVoiceLobby != null || PartyVoice.Instance.InVoice);
		}
	}

	// Token: 0x06006344 RID: 25412 RVA: 0x0026F588 File Offset: 0x0026D788
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool pushToTalkButtonValid(LocalPlayerUI _playerUI)
	{
		bool controlKeyPressed = InputUtils.ControlKeyPressed;
		bool flag = _playerUI.windowManager.IsInputActive();
		bool flag2 = PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && GameManager.Instance.isAnyCursorWindowOpen(null);
		return (!GameManager.Instance.IsEditMode() || !controlKeyPressed) && !flag && !flag2;
	}

	// Token: 0x06006345 RID: 25413 RVA: 0x0026F5E0 File Offset: 0x0026D7E0
	public static bool PushToTalkPressed()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		return !(uiforPrimaryPlayer == null) && uiforPrimaryPlayer.playerInput != null && uiforPrimaryPlayer.playerInput.PermanentActions.PushToTalk.IsPressed && VoiceHelpers.pushToTalkButtonValid(uiforPrimaryPlayer);
	}

	// Token: 0x06006346 RID: 25414 RVA: 0x0026F628 File Offset: 0x0026D828
	public static bool PushToTalkWasPressed()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		return !(uiforPrimaryPlayer == null) && uiforPrimaryPlayer.playerInput != null && uiforPrimaryPlayer.playerInput.PermanentActions.PushToTalk.WasPressed && VoiceHelpers.pushToTalkButtonValid(uiforPrimaryPlayer);
	}

	// Token: 0x06006347 RID: 25415 RVA: 0x0026F66D File Offset: 0x0026D86D
	public static bool LocalPlayerTalking()
	{
		if (PartyVoice.Instance.SendingVoice)
		{
			return true;
		}
		if (DiscordManager.Instance.IsReady)
		{
			DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
			return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.IsSpeaking;
		}
		return false;
	}

	// Token: 0x06006348 RID: 25416 RVA: 0x0026F6A8 File Offset: 0x0026D8A8
	public static IPartyVoice.EVoiceMemberState GetPlayerVoiceState(EntityPlayer _player, bool _partyOnly = false)
	{
		if (_player == null)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		IPartyVoice.EVoiceMemberState playerDiscordVoiceState = VoiceHelpers.GetPlayerDiscordVoiceState(_player, _partyOnly);
		IPartyVoice.EVoiceMemberState evoiceMemberState;
		if (GamePrefs.GetBool(EnumGamePrefs.OptionsVoiceChatEnabled))
		{
			PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_player.entityId);
			evoiceMemberState = ((playerDataFromEntityID == null) ? IPartyVoice.EVoiceMemberState.Disabled : PartyVoice.Instance.GetVoiceMemberState(playerDataFromEntityID.PrimaryId));
		}
		else
		{
			evoiceMemberState = IPartyVoice.EVoiceMemberState.Disabled;
		}
		if (playerDiscordVoiceState < evoiceMemberState)
		{
			return evoiceMemberState;
		}
		return playerDiscordVoiceState;
	}

	// Token: 0x06006349 RID: 25417 RVA: 0x0026F70C File Offset: 0x0026D90C
	public static IPartyVoice.EVoiceMemberState GetPlayerDiscordVoiceState(EntityPlayer _player, bool _partyOnly = false)
	{
		if (_player == null)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		if (!DiscordManager.Instance.IsReady)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
		if (activeVoiceLobby == null)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		if (!activeVoiceLobby.IsInVoice)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		if (_partyOnly && activeVoiceLobby.LobbyType != DiscordManager.ELobbyType.Party)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		DiscordManager.DiscordUser discordUser;
		if (!DiscordManager.Instance.TryGetUserFromEntity(_player, out discordUser) || !discordUser.InCurrentVoice)
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		return discordUser.VoiceState;
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x0026F77C File Offset: 0x0026D97C
	public static bool IsSnapWithoutMicPermission()
	{
		if (VoiceHelpers.snapWithoutMicPermission != null)
		{
			return VoiceHelpers.snapWithoutMicPermission.Value;
		}
		if (GameIO.IsRunningAsSnap() == null)
		{
			VoiceHelpers.snapWithoutMicPermission = new bool?(false);
			return false;
		}
		VoiceHelpers.snapWithoutMicPermission = new bool?(!VoiceHelpers.<IsSnapWithoutMicPermission>g__SnapHasMicPermission|13_0());
		return VoiceHelpers.snapWithoutMicPermission.Value;
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x0026F7D0 File Offset: 0x0026D9D0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <IsSnapWithoutMicPermission>g__SnapHasMicPermission|13_0()
	{
		ProcessStartInfo processStartInfo;
		if (GameIO.IsRunningInSteamRuntime())
		{
			processStartInfo = new ProcessStartInfo
			{
				FileName = "/usr/bin/steam-runtime-launch-client",
				Arguments = " --alongside-steam -- /usr/bin/snapctl is-connected audio-record"
			};
		}
		else
		{
			processStartInfo = new ProcessStartInfo
			{
				FileName = "/usr/bin/snapctl",
				Arguments = "is-connected audio-record"
			};
		}
		processStartInfo.CreateNoWindow = true;
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardError = true;
		processStartInfo.RedirectStandardOutput = true;
		bool flag;
		try
		{
			Process process = Process.Start(processStartInfo);
			if (process == null)
			{
				Log.Out("Snap microphone permission check: Could not run snapctl");
				flag = false;
			}
			else
			{
				string text = process.StandardError.ReadToEnd();
				process.WaitForExit(150);
				if (!process.HasExited)
				{
					process.Kill();
					Log.Error("Snap microphone permission check: snapctl did not terminate");
					flag = false;
				}
				else
				{
					bool flag2 = process.ExitCode == 0;
					Log.Out(string.Format("Snap microphone permission check done: Has permission: {0}", flag2));
					if (text.Length > 5)
					{
						Log.Out("ErrOut: " + text);
					}
					flag = flag2;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("Snap microphone permission check failed with exception: " + ex.Message);
			flag = false;
		}
		return flag;
	}

	// Token: 0x04004D03 RID: 19715
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool? snapWithoutMicPermission;
}
