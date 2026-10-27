using System;
using System.Collections.Generic;
using System.Linq;
using Platform;

// Token: 0x020012E4 RID: 4836
public class PlayerInteractions
{
	// Token: 0x17001251 RID: 4689
	// (get) Token: 0x060098CA RID: 39114 RVA: 0x0039B03F File Offset: 0x0039923F
	public static PlayerInteractions Instance { get; } = new PlayerInteractions();

	// Token: 0x140000FD RID: 253
	// (add) Token: 0x060098CB RID: 39115 RVA: 0x0039B048 File Offset: 0x00399248
	// (remove) Token: 0x060098CC RID: 39116 RVA: 0x0039B080 File Offset: 0x00399280
	public event PlayerIteractionEvent OnNewPlayerInteraction;

	// Token: 0x060098CD RID: 39117 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteractions()
	{
	}

	// Token: 0x060098CE RID: 39118 RVA: 0x0039B0B5 File Offset: 0x003992B5
	public void JoinedMultiplayerServer(PersistentPlayerList ppl)
	{
		Log.Out("[PlayerInteractions] JoinedMultplayerServer");
		this.RecordInteractionForActivePersistentPlayers(ppl, PlayerInteractionType.Login);
		this.SetPlayersList(ppl);
	}

	// Token: 0x060098CF RID: 39119 RVA: 0x0039B0D0 File Offset: 0x003992D0
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

	// Token: 0x060098D0 RID: 39120 RVA: 0x0039B168 File Offset: 0x00399368
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

	// Token: 0x060098D1 RID: 39121 RVA: 0x0039B1BC File Offset: 0x003993BC
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

	// Token: 0x060098D2 RID: 39122 RVA: 0x0039B23F File Offset: 0x0039943F
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

	// Token: 0x060098D3 RID: 39123 RVA: 0x0039B280 File Offset: 0x00399480
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

	// Token: 0x060098D4 RID: 39124 RVA: 0x0039B344 File Offset: 0x00399544
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInteraction CreateInteraction(PersistentPlayerData ppd, PlayerInteractionType type)
	{
		return new PlayerInteraction(ppd.PlayerData, type);
	}

	// Token: 0x040072A7 RID: 29351
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList playerList;
}
