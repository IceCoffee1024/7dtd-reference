using System;
using System.Collections.Generic;
using System.Linq;
using Platform;

// Token: 0x020012B3 RID: 4787
public class PlayerInteractions
{
	// Token: 0x17001201 RID: 4609
	// (get) Token: 0x060097CC RID: 38860 RVA: 0x00394126 File Offset: 0x00392326
	public static PlayerInteractions Instance { get; } = new PlayerInteractions();

	// Token: 0x140000FE RID: 254
	// (add) Token: 0x060097CD RID: 38861 RVA: 0x00394130 File Offset: 0x00392330
	// (remove) Token: 0x060097CE RID: 38862 RVA: 0x00394168 File Offset: 0x00392368
	public event PlayerIteractionEvent OnNewPlayerInteraction;

	// Token: 0x060097CF RID: 38863 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteractions()
	{
	}

	// Token: 0x060097D0 RID: 38864 RVA: 0x0039419D File Offset: 0x0039239D
	public void JoinedMultiplayerServer(PersistentPlayerList ppl)
	{
		Log.Out("[PlayerInteractions] JoinedMultplayerServer");
		this.RecordInteractionForActivePersistentPlayers(ppl, PlayerInteractionType.Login);
		this.SetPlayersList(ppl);
	}

	// Token: 0x060097D1 RID: 38865 RVA: 0x003941B8 File Offset: 0x003923B8
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

	// Token: 0x060097D2 RID: 38866 RVA: 0x00394250 File Offset: 0x00392450
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

	// Token: 0x060097D3 RID: 38867 RVA: 0x003942A4 File Offset: 0x003924A4
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

	// Token: 0x060097D4 RID: 38868 RVA: 0x00394327 File Offset: 0x00392527
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

	// Token: 0x060097D5 RID: 38869 RVA: 0x00394368 File Offset: 0x00392568
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

	// Token: 0x060097D6 RID: 38870 RVA: 0x0039442C File Offset: 0x0039262C
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteraction CreateInteraction(PersistentPlayerData ppd, PlayerInteractionType type)
	{
		return new PlayerInteraction(ppd.PlayerData, type);
	}

	// Token: 0x040071F2 RID: 29170
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList playerList;
}
