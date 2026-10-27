using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02000CC9 RID: 3273
public static class VoiceHelpers
{
	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x060063E5 RID: 25573 RVA: 0x00276D73 File Offset: 0x00274F73
	public static bool VoiceAllowed
	{
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x060063E6 RID: 25574 RVA: 0x00276D7A File Offset: 0x00274F7A
	public static bool PlatformVoiceEnabled
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsVoiceChatEnabled) && VoiceHelpers.VoiceAllowed;
		}
	}

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x060063E7 RID: 25575 RVA: 0x00276D8C File Offset: 0x00274F8C
	public static bool InAnyVoiceChat
	{
		get
		{
			return VoiceHelpers.VoiceAllowed && (DiscordManager.Instance.ActiveVoiceLobby != null || PartyVoice.Instance.InVoice);
		}
	}

	// Token: 0x060063E8 RID: 25576 RVA: 0x00276DB0 File Offset: 0x00274FB0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool pushToTalkButtonValid(LocalPlayerUI _playerUI)
	{
		bool controlKeyPressed = InputUtils.ControlKeyPressed;
		bool flag = _playerUI.windowManager.IsInputActive();
		bool flag2 = PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && GameManager.Instance.isAnyCursorWindowOpen(null);
		return (!GameManager.Instance.IsEditMode() || !controlKeyPressed) && !flag && !flag2;
	}

	// Token: 0x060063E9 RID: 25577 RVA: 0x00276E08 File Offset: 0x00275008
	public static bool PushToTalkPressed()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		return !(uiforPrimaryPlayer == null) && uiforPrimaryPlayer.playerInput != null && uiforPrimaryPlayer.playerInput.PermanentActions.PushToTalk.IsPressed && VoiceHelpers.pushToTalkButtonValid(uiforPrimaryPlayer);
	}

	// Token: 0x060063EA RID: 25578 RVA: 0x00276E50 File Offset: 0x00275050
	public static bool PushToTalkWasPressed()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		return !(uiforPrimaryPlayer == null) && uiforPrimaryPlayer.playerInput != null && uiforPrimaryPlayer.playerInput.PermanentActions.PushToTalk.WasPressed && VoiceHelpers.pushToTalkButtonValid(uiforPrimaryPlayer);
	}

	// Token: 0x060063EB RID: 25579 RVA: 0x00276E95 File Offset: 0x00275095
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

	// Token: 0x060063EC RID: 25580 RVA: 0x00276ED0 File Offset: 0x002750D0
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

	// Token: 0x060063ED RID: 25581 RVA: 0x00276F34 File Offset: 0x00275134
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

	// Token: 0x060063EE RID: 25582 RVA: 0x00276FA4 File Offset: 0x002751A4
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

	// Token: 0x060063EF RID: 25583 RVA: 0x00276FF8 File Offset: 0x002751F8
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

	// Token: 0x04004DFC RID: 19964
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool? snapWithoutMicPermission;
}
