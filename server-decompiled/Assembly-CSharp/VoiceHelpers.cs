using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02000CB0 RID: 3248
public static class VoiceHelpers
{
	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x060063CD RID: 25549 RVA: 0x00272447 File Offset: 0x00270647
	public static bool VoiceAllowed
	{
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x060063CE RID: 25550 RVA: 0x0027244E File Offset: 0x0027064E
	public static bool PlatformVoiceEnabled
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsVoiceChatEnabled) && VoiceHelpers.VoiceAllowed;
		}
	}

	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x060063CF RID: 25551 RVA: 0x00272460 File Offset: 0x00270660
	public static bool InAnyVoiceChat
	{
		get
		{
			return VoiceHelpers.VoiceAllowed && (DiscordManager.Instance.ActiveVoiceLobby != null || PartyVoice.Instance.InVoice);
		}
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x00272484 File Offset: 0x00270684
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool pushToTalkButtonValid(LocalPlayerUI _playerUI)
	{
		bool controlKeyPressed = InputUtils.ControlKeyPressed;
		bool flag = _playerUI.windowManager.IsInputActive();
		bool flag2 = PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && GameManager.Instance.isAnyCursorWindowOpen(null);
		return (!GameManager.Instance.IsEditMode() || !controlKeyPressed) && !flag && !flag2;
	}

	// Token: 0x060063D1 RID: 25553 RVA: 0x002724DC File Offset: 0x002706DC
	public static bool PushToTalkPressed()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		return !(uiforPrimaryPlayer == null) && uiforPrimaryPlayer.playerInput != null && uiforPrimaryPlayer.playerInput.PermanentActions.PushToTalk.IsPressed && VoiceHelpers.pushToTalkButtonValid(uiforPrimaryPlayer);
	}

	// Token: 0x060063D2 RID: 25554 RVA: 0x00272524 File Offset: 0x00270724
	public static bool PushToTalkWasPressed()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		return !(uiforPrimaryPlayer == null) && uiforPrimaryPlayer.playerInput != null && uiforPrimaryPlayer.playerInput.PermanentActions.PushToTalk.WasPressed && VoiceHelpers.pushToTalkButtonValid(uiforPrimaryPlayer);
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x00272569 File Offset: 0x00270769
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

	// Token: 0x060063D4 RID: 25556 RVA: 0x002725A4 File Offset: 0x002707A4
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

	// Token: 0x060063D5 RID: 25557 RVA: 0x00272608 File Offset: 0x00270808
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

	// Token: 0x060063D6 RID: 25558 RVA: 0x00272678 File Offset: 0x00270878
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

	// Token: 0x060063D7 RID: 25559 RVA: 0x002726CC File Offset: 0x002708CC
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

	// Token: 0x04004DA1 RID: 19873
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool? snapWithoutMicPermission;
}
