using System;
using System.Collections.Generic;
using System.Linq;
using Platform;

// Token: 0x020012D2 RID: 4818
public class PlayerInteractions
{
	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x06009872 RID: 39026 RVA: 0x00398C3B File Offset: 0x00396E3B
	public static PlayerInteractions Instance { get; } = new PlayerInteractions();

	// Token: 0x140000FD RID: 253
	// (add) Token: 0x06009873 RID: 39027 RVA: 0x00398C44 File Offset: 0x00396E44
	// (remove) Token: 0x06009874 RID: 39028 RVA: 0x00398C7C File Offset: 0x00396E7C
	public event PlayerIteractionEvent OnNewPlayerInteraction;

	// Token: 0x06009875 RID: 39029 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteractions()
	{
	}

	// Token: 0x06009876 RID: 39030 RVA: 0x00398CB1 File Offset: 0x00396EB1
	public void JoinedMultiplayerServer(PersistentPlayerList ppl)
	{
		Log.Out("[PlayerInteractions] JoinedMultplayerServer");
		this.RecordInteractionForActivePersistentPlayers(ppl, PlayerInteractionType.Login);
		this.SetPlayersList(ppl);
	}

	// Token: 0x06009877 RID: 39031 RVA: 0x00398CCC File Offset: 0x00396ECC
	public void PlayerSpawnedInMultiplayerServer(PersistentPlayerList ppl, int spawningEntityId, RespawnType respawnReason)
	{
		this.SetPlayersList(ppl);
		if (respawnReason != RespawnType.NewGame && respawnReason != RespawnType.EnterMultiplayer && respawnReason != RespawnType.JoinMultiplayer && respawnReason != RespawnType.LoadedGame)
		{
			return;
		}
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		EntityPlayerLocal entityPlayerLocal = ((uiforPrimaryPlayer != null) ? uiforPrimaryPlayer.entityPlayer : null);
		int? num = ((entityPlayerLocal != null) ? new int?(entityPlayerLocal.entityId) : null);
		if (((spawningEntityId == num.GetValueOrDefault()) & (num != null)) && spawningEntityId != -1)
		{
			this.RecordInteractionForActivePersistentPlayers(ppl, PlayerInteractionType.FirstSpawn);
			return;
		}
		PersistentPlayerData playerDataFromEntityID = ppl.GetPlayerDataFromEntityID(spawningEntityId);
		PlayerIteractionEvent onNewPlayerInteraction = this.OnNewPlayerInteraction;
		if (onNewPlayerInteraction == null)
		{
			return;
		}
		onNewPlayerInteraction(this.CreateInteraction(playerDataFromEntityID, PlayerInteractionType.FirstSpawn));
	}

	// Token: 0x06009878 RID: 39032 RVA: 0x00398D64 File Offset: 0x00396F64
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetPlayersList(PersistentPlayerList ppl)
	{
		if (ppl == this.playerList)
		{
			return;
		}
		if (this.playerList != null)
		{
			this.playerList.RemovePlayerEventHandler(new PersistentPlayerData.PlayerEventHandler(this.OnPersistentPlayerEvent));
		}
		this.playerList = ppl;
		this.playerList.AddPlayerEventHandler(new PersistentPlayerData.PlayerEventHandler(this.OnPersistentPlayerEvent));
	}

	// Token: 0x06009879 RID: 39033 RVA: 0x00398DB8 File Offset: 0x00396FB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPersistentPlayerEvent(PersistentPlayerData ppData, PersistentPlayerData otherPlayer, EnumPersistentPlayerDataReason reason)
	{
		PlayerInteraction playerInteraction = default(PlayerInteraction);
		if (reason != EnumPersistentPlayerDataReason.Login)
		{
			if (reason == EnumPersistentPlayerDataReason.Disconnected)
			{
				Log.Out("[PlayerInteractions] persistent player disconnect");
				playerInteraction = this.CreateInteraction(ppData, PlayerInteractionType.Disconnect);
				IPlayerInteractionsRecorder playerInteractionsRecorder = PlatformManager.MultiPlatform.PlayerInteractionsRecorder;
				if (playerInteractionsRecorder != null)
				{
					playerInteractionsRecorder.RecordPlayerInteraction(playerInteraction);
				}
			}
		}
		else
		{
			Log.Out("[PlayerInteractions] persistent player login");
			playerInteraction = this.CreateInteraction(ppData, PlayerInteractionType.Login);
			IPlayerInteractionsRecorder playerInteractionsRecorder2 = PlatformManager.MultiPlatform.PlayerInteractionsRecorder;
			if (playerInteractionsRecorder2 != null)
			{
				playerInteractionsRecorder2.RecordPlayerInteraction(playerInteraction);
			}
		}
		PlayerIteractionEvent onNewPlayerInteraction = this.OnNewPlayerInteraction;
		if (onNewPlayerInteraction == null)
		{
			return;
		}
		onNewPlayerInteraction(playerInteraction);
	}

	// Token: 0x0600987A RID: 39034 RVA: 0x00398E3B File Offset: 0x0039703B
	public void Shutdown()
	{
		if (this.playerList != null)
		{
			Log.Out("[PlayerInteractions] Shutdown, record disconnect for all currently connected players");
			this.RecordInteractionForActivePersistentPlayers(this.playerList, PlayerInteractionType.Disconnect);
			this.playerList.RemovePlayerEventHandler(new PersistentPlayerData.PlayerEventHandler(this.OnPersistentPlayerEvent));
			this.playerList = null;
		}
	}

	// Token: 0x0600987B RID: 39035 RVA: 0x00398E7C File Offset: 0x0039707C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecordInteractionForActivePersistentPlayers(PersistentPlayerList ppl, PlayerInteractionType interactionType)
	{
		IEnumerable<PlayerInteraction> enumerable = from ppd in ppl.Players.Values.ToList<PersistentPlayerData>()
			where ppd.EntityId != -1
			select this.CreateInteraction(ppd, interactionType);
		IPlayerInteractionsRecorder playerInteractionsRecorder = PlatformManager.MultiPlatform.PlayerInteractionsRecorder;
		if (playerInteractionsRecorder != null)
		{
			playerInteractionsRecorder.RecordPlayerInteractions(enumerable);
		}
		foreach (PlayerInteraction playerInteraction in enumerable)
		{
			PlayerIteractionEvent onNewPlayerInteraction = this.OnNewPlayerInteraction;
			if (onNewPlayerInteraction != null)
			{
				onNewPlayerInteraction(playerInteraction);
			}
		}
	}

	// Token: 0x0600987C RID: 39036 RVA: 0x00398F40 File Offset: 0x00397140
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteraction CreateInteraction(PersistentPlayerData ppd, PlayerInteractionType type)
	{
		return new PlayerInteraction(ppd.PlayerData, type);
	}

	// Token: 0x04007266 RID: 29286
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList playerList;
}
