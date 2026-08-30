using System;
using System.Collections.Generic;
using System.Linq;
using Platform;

// Token: 0x02001299 RID: 4761
public class PlayerInteractions
{
	// Token: 0x170011C8 RID: 4552
	// (get) Token: 0x060096D1 RID: 38609 RVA: 0x0038EFCA File Offset: 0x0038D1CA
	public static PlayerInteractions Instance { get; } = new PlayerInteractions();

	// Token: 0x140000FD RID: 253
	// (add) Token: 0x060096D2 RID: 38610 RVA: 0x0038EFD4 File Offset: 0x0038D1D4
	// (remove) Token: 0x060096D3 RID: 38611 RVA: 0x0038F00C File Offset: 0x0038D20C
	public event PlayerIteractionEvent OnNewPlayerInteraction;

	// Token: 0x060096D4 RID: 38612 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteractions()
	{
	}

	// Token: 0x060096D5 RID: 38613 RVA: 0x0038F041 File Offset: 0x0038D241
	public void JoinedMultiplayerServer(PersistentPlayerList ppl)
	{
		Log.Out("[PlayerInteractions] JoinedMultplayerServer");
		this.RecordInteractionForActivePersistentPlayers(ppl, PlayerInteractionType.Login);
		this.SetPlayersList(ppl);
	}

	// Token: 0x060096D6 RID: 38614 RVA: 0x0038F05C File Offset: 0x0038D25C
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

	// Token: 0x060096D7 RID: 38615 RVA: 0x0038F0F4 File Offset: 0x0038D2F4
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

	// Token: 0x060096D8 RID: 38616 RVA: 0x0038F148 File Offset: 0x0038D348
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

	// Token: 0x060096D9 RID: 38617 RVA: 0x0038F1CB File Offset: 0x0038D3CB
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

	// Token: 0x060096DA RID: 38618 RVA: 0x0038F20C File Offset: 0x0038D40C
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

	// Token: 0x060096DB RID: 38619 RVA: 0x0038F2D0 File Offset: 0x0038D4D0
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteraction CreateInteraction(PersistentPlayerData ppd, PlayerInteractionType type)
	{
		return new PlayerInteraction(ppd.PlayerData, type);
	}

	// Token: 0x0400711D RID: 28957
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList playerList;
}
