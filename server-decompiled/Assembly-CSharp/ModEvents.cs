using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x02000733 RID: 1843
public static class ModEvents
{
	// Token: 0x04002D72 RID: 11634
	public static readonly ModEvents.ModEvent<ModEvents.SGameFocusData> GameFocus = new ModEvents.ModEvent<ModEvents.SGameFocusData>("GameFocus");

	// Token: 0x04002D73 RID: 11635
	public static readonly ModEvents.ModEvent<ModEvents.SGameAwakeData> GameAwake = new ModEvents.ModEvent<ModEvents.SGameAwakeData>("GameAwake");

	// Token: 0x04002D74 RID: 11636
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartingData> GameStarting = new ModEvents.ModEvent<ModEvents.SGameStartingData>("GameStarting");

	// Token: 0x04002D75 RID: 11637
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData> MainMenuOpening = new ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData>("MainMenuOpening");

	// Token: 0x04002D76 RID: 11638
	public static readonly ModEvents.ModEvent<ModEvents.SMainMenuOpenedData> MainMenuOpened = new ModEvents.ModEvent<ModEvents.SMainMenuOpenedData>("MainMenuOpened");

	// Token: 0x04002D77 RID: 11639
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartDoneData> GameStartDone = new ModEvents.ModEvent<ModEvents.SGameStartDoneData>("GameStartDone");

	// Token: 0x04002D78 RID: 11640
	public static readonly ModEvents.ModEvent<ModEvents.SCreateWorldDoneData> CreateWorldDone = new ModEvents.ModEvent<ModEvents.SCreateWorldDoneData>("CreateWorldDone");

	// Token: 0x04002D79 RID: 11641
	public static readonly ModEvents.ModEvent<ModEvents.SGameUpdateData> GameUpdate = new ModEvents.ModEvent<ModEvents.SGameUpdateData>("GameUpdate");

	// Token: 0x04002D7A RID: 11642
	public static readonly ModEvents.ModEvent<ModEvents.SWorldShuttingDownData> WorldShuttingDown = new ModEvents.ModEvent<ModEvents.SWorldShuttingDownData>("WorldShuttingDown");

	// Token: 0x04002D7B RID: 11643
	public static readonly ModEvents.ModEvent<ModEvents.SGameShutdownData> GameShutdown = new ModEvents.ModEvent<ModEvents.SGameShutdownData>("GameShutdown");

	// Token: 0x04002D7C RID: 11644
	public static readonly ModEvents.ModEvent<ModEvents.SServerRegisteredData> ServerRegistered = new ModEvents.ModEvent<ModEvents.SServerRegisteredData>("ServerRegistered");

	// Token: 0x04002D7D RID: 11645
	public static readonly ModEvents.ModEvent<ModEvents.SUnityUpdateData> UnityUpdate = new ModEvents.ModEvent<ModEvents.SUnityUpdateData>("UnityUpdate");

	// Token: 0x04002D7E RID: 11646
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData> PlayerLogin = new ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData>("PlayerLogin");

	// Token: 0x04002D7F RID: 11647
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData> PlayerJoinedGame = new ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData>("PlayerJoinedGame");

	// Token: 0x04002D80 RID: 11648
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawningData> PlayerSpawning = new ModEvents.ModEvent<ModEvents.SPlayerSpawningData>("PlayerSpawning");

	// Token: 0x04002D81 RID: 11649
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData> PlayerSpawnedInWorld = new ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData>("PlayerSpawnedInWorld");

	// Token: 0x04002D82 RID: 11650
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData> PlayerDisconnected = new ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData>("PlayerDisconnected");

	// Token: 0x04002D83 RID: 11651
	public static readonly ModEvents.ModEvent<ModEvents.SSavePlayerDataData> SavePlayerData = new ModEvents.ModEvent<ModEvents.SSavePlayerDataData>("SavePlayerData");

	// Token: 0x04002D84 RID: 11652
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SGameMessageData> GameMessage = new ModEvents.ModEventInterruptible<ModEvents.SGameMessageData>("GameMessage");

	// Token: 0x04002D85 RID: 11653
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SChatMessageData> ChatMessage = new ModEvents.ModEventInterruptible<ModEvents.SChatMessageData>("ChatMessage");

	// Token: 0x04002D86 RID: 11654
	public static readonly ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData> CalcChunkColorsDone = new ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData>("CalcChunkColorsDone");

	// Token: 0x04002D87 RID: 11655
	public static readonly ModEvents.ModEvent<ModEvents.SEntityKilledData> EntityKilled = new ModEvents.ModEvent<ModEvents.SEntityKilledData>("EntityKilled");

	// Token: 0x02000734 RID: 1844
	public struct SGameFocusData
	{
		// Token: 0x060037D1 RID: 14289 RVA: 0x0016F953 File Offset: 0x0016DB53
		public SGameFocusData(bool _isFocused)
		{
			this.IsFocused = _isFocused;
		}

		// Token: 0x04002D88 RID: 11656
		public readonly bool IsFocused;
	}

	// Token: 0x02000735 RID: 1845
	public struct SGameAwakeData
	{
	}

	// Token: 0x02000736 RID: 1846
	public struct SGameStartingData
	{
		// Token: 0x060037D2 RID: 14290 RVA: 0x0016F95C File Offset: 0x0016DB5C
		public SGameStartingData(bool _asServer)
		{
			this.AsServer = _asServer;
		}

		// Token: 0x04002D89 RID: 11657
		public readonly bool AsServer;
	}

	// Token: 0x02000737 RID: 1847
	public struct SMainMenuOpeningData
	{
		// Token: 0x060037D3 RID: 14291 RVA: 0x0016F965 File Offset: 0x0016DB65
		public SMainMenuOpeningData(bool _openedBefore)
		{
			this.OpenedBefore = _openedBefore;
		}

		// Token: 0x04002D8A RID: 11658
		public readonly bool OpenedBefore;
	}

	// Token: 0x02000738 RID: 1848
	public struct SMainMenuOpenedData
	{
		// Token: 0x060037D4 RID: 14292 RVA: 0x0016F96E File Offset: 0x0016DB6E
		public SMainMenuOpenedData(bool _firstTimeOpen)
		{
			this.FirstTimeOpen = _firstTimeOpen;
		}

		// Token: 0x04002D8B RID: 11659
		public readonly bool FirstTimeOpen;
	}

	// Token: 0x02000739 RID: 1849
	public struct SGameStartDoneData
	{
	}

	// Token: 0x0200073A RID: 1850
	public struct SCreateWorldDoneData
	{
	}

	// Token: 0x0200073B RID: 1851
	public struct SGameUpdateData
	{
	}

	// Token: 0x0200073C RID: 1852
	public struct SWorldShuttingDownData
	{
	}

	// Token: 0x0200073D RID: 1853
	public struct SGameShutdownData
	{
	}

	// Token: 0x0200073E RID: 1854
	public struct SServerRegisteredData
	{
	}

	// Token: 0x0200073F RID: 1855
	public struct SUnityUpdateData
	{
	}

	// Token: 0x02000740 RID: 1856
	public struct SPlayerLoginData
	{
		// Token: 0x060037D5 RID: 14293 RVA: 0x0016F977 File Offset: 0x0016DB77
		public SPlayerLoginData(ClientInfo _clientInfo, string _compatibilityVersion)
		{
			this.ClientInfo = _clientInfo;
			this.CompatibilityVersion = _compatibilityVersion;
			this.CustomMessage = null;
		}

		// Token: 0x04002D8C RID: 11660
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D8D RID: 11661
		public readonly string CompatibilityVersion;

		// Token: 0x04002D8E RID: 11662
		public string CustomMessage;
	}

	// Token: 0x02000741 RID: 1857
	public struct SPlayerJoinedGameData
	{
		// Token: 0x060037D6 RID: 14294 RVA: 0x0016F98E File Offset: 0x0016DB8E
		public SPlayerJoinedGameData(ClientInfo _clientInfo)
		{
			this.ClientInfo = _clientInfo;
		}

		// Token: 0x04002D8F RID: 11663
		public readonly ClientInfo ClientInfo;
	}

	// Token: 0x02000742 RID: 1858
	public readonly struct SPlayerSpawningData
	{
		// Token: 0x060037D7 RID: 14295 RVA: 0x0016F997 File Offset: 0x0016DB97
		public SPlayerSpawningData(ClientInfo _clientInfo, int _chunkViewDim, PlayerProfile _playerProfile)
		{
			this.ClientInfo = _clientInfo;
			this.ChunkViewDim = _chunkViewDim;
			this.PlayerProfile = _playerProfile;
		}

		// Token: 0x04002D90 RID: 11664
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D91 RID: 11665
		public readonly int ChunkViewDim;

		// Token: 0x04002D92 RID: 11666
		public readonly PlayerProfile PlayerProfile;
	}

	// Token: 0x02000743 RID: 1859
	public struct SPlayerSpawnedInWorldData
	{
		// Token: 0x060037D8 RID: 14296 RVA: 0x0016F9AE File Offset: 0x0016DBAE
		public SPlayerSpawnedInWorldData(ClientInfo _clientInfo, bool _isLocalPlayer, int _entityId, RespawnType _respawnType, Vector3i _position)
		{
			this.ClientInfo = _clientInfo;
			this.IsLocalPlayer = _isLocalPlayer;
			this.EntityId = _entityId;
			this.RespawnType = _respawnType;
			this.Position = _position;
		}

		// Token: 0x04002D93 RID: 11667
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D94 RID: 11668
		public readonly bool IsLocalPlayer;

		// Token: 0x04002D95 RID: 11669
		public readonly int EntityId;

		// Token: 0x04002D96 RID: 11670
		public readonly RespawnType RespawnType;

		// Token: 0x04002D97 RID: 11671
		public readonly Vector3i Position;
	}

	// Token: 0x02000744 RID: 1860
	public struct SPlayerDisconnectedData
	{
		// Token: 0x060037D9 RID: 14297 RVA: 0x0016F9D5 File Offset: 0x0016DBD5
		public SPlayerDisconnectedData(ClientInfo _clientInfo, bool _gameShuttingDown)
		{
			this.ClientInfo = _clientInfo;
			this.GameShuttingDown = _gameShuttingDown;
		}

		// Token: 0x04002D98 RID: 11672
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D99 RID: 11673
		public readonly bool GameShuttingDown;
	}

	// Token: 0x02000745 RID: 1861
	public struct SSavePlayerDataData
	{
		// Token: 0x060037DA RID: 14298 RVA: 0x0016F9E5 File Offset: 0x0016DBE5
		public SSavePlayerDataData(ClientInfo _clientInfo, PlayerDataFile _playerDataFile)
		{
			this.ClientInfo = _clientInfo;
			this.PlayerDataFile = _playerDataFile;
		}

		// Token: 0x04002D9A RID: 11674
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D9B RID: 11675
		public readonly PlayerDataFile PlayerDataFile;
	}

	// Token: 0x02000746 RID: 1862
	public struct SGameMessageData
	{
		// Token: 0x060037DB RID: 14299 RVA: 0x0016F9F5 File Offset: 0x0016DBF5
		public SGameMessageData(ClientInfo _clientInfo, EnumGameMessages _messageType, string _mainName, string _secondaryName)
		{
			this.ClientInfo = _clientInfo;
			this.MessageType = _messageType;
			this.MainName = _mainName;
			this.SecondaryName = _secondaryName;
		}

		// Token: 0x04002D9C RID: 11676
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D9D RID: 11677
		public readonly EnumGameMessages MessageType;

		// Token: 0x04002D9E RID: 11678
		public readonly string MainName;

		// Token: 0x04002D9F RID: 11679
		public readonly string SecondaryName;
	}

	// Token: 0x02000747 RID: 1863
	public struct SChatMessageData
	{
		// Token: 0x060037DC RID: 14300 RVA: 0x0016FA14 File Offset: 0x0016DC14
		public SChatMessageData(ClientInfo _clientInfo, EChatType _chatType, int _senderEntityId, string _message, string _mainName, List<int> _recipientEntityIds)
		{
			this.ClientInfo = _clientInfo;
			this.ChatType = _chatType;
			this.SenderEntityId = _senderEntityId;
			this.Message = _message;
			this.MainName = _mainName;
			this.RecipientEntityIds = _recipientEntityIds;
		}

		// Token: 0x04002DA0 RID: 11680
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002DA1 RID: 11681
		public readonly EChatType ChatType;

		// Token: 0x04002DA2 RID: 11682
		public readonly int SenderEntityId;

		// Token: 0x04002DA3 RID: 11683
		public readonly string Message;

		// Token: 0x04002DA4 RID: 11684
		public readonly string MainName;

		// Token: 0x04002DA5 RID: 11685
		public readonly List<int> RecipientEntityIds;
	}

	// Token: 0x02000748 RID: 1864
	public struct SCalcChunkColorsDoneData
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x0016FA43 File Offset: 0x0016DC43
		public SCalcChunkColorsDoneData(Chunk _chunk)
		{
			this.Chunk = _chunk;
		}

		// Token: 0x04002DA6 RID: 11686
		public readonly Chunk Chunk;
	}

	// Token: 0x02000749 RID: 1865
	public struct SEntityKilledData
	{
		// Token: 0x060037DE RID: 14302 RVA: 0x0016FA4C File Offset: 0x0016DC4C
		public SEntityKilledData(Entity _killedEntitiy, Entity _killingEntity)
		{
			this.KilledEntitiy = _killedEntitiy;
			this.KillingEntity = _killingEntity;
		}

		// Token: 0x04002DA7 RID: 11687
		public readonly Entity KilledEntitiy;

		// Token: 0x04002DA8 RID: 11688
		public readonly Entity KillingEntity;
	}

	// Token: 0x0200074A RID: 1866
	public enum EModEventResult
	{
		// Token: 0x04002DAA RID: 11690
		Continue,
		// Token: 0x04002DAB RID: 11691
		StopHandlersRunVanilla,
		// Token: 0x04002DAC RID: 11692
		StopHandlersAndVanilla
	}

	// Token: 0x0200074B RID: 1867
	// (Invoke) Token: 0x060037E0 RID: 14304
	public delegate void ModEventHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x0200074C RID: 1868
	// (Invoke) Token: 0x060037E4 RID: 14308
	public delegate ModEvents.EModEventResult ModEventInterruptibleHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x0200074D RID: 1869
	public abstract class ModEventAbs<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x0016FA5C File Offset: 0x0016DC5C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ModEventAbs(string _eventName)
		{
			this.eventName = _eventName;
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0016FA78 File Offset: 0x0016DC78
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RegisterHandler(TDelegate _handlerFunc)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			Assembly assembly = typeof(ModEvents).Assembly;
			bool flag = false;
			Mod mod = null;
			if (callingAssembly.Equals(assembly))
			{
				flag = true;
			}
			else
			{
				mod = ModManager.GetModForAssembly(callingAssembly);
				if (mod == null)
				{
					Log.Warning("[MODS] Could not find mod that tries to register a handler for event " + this.eventName);
				}
			}
			this.receivers.Add(new ModEvents.ModEventAbs<TDelegate>.Receiver(mod, _handlerFunc, flag));
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x0016FAE0 File Offset: 0x0016DCE0
		public void UnregisterHandler(TDelegate _handlerFunc)
		{
			for (int i = this.receivers.Count - 1; i >= 0; i--)
			{
				if (this.receivers[i].DelegateFunc.Equals(_handlerFunc))
				{
					this.receivers.RemoveAt(i);
				}
			}
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x0016FB34 File Offset: 0x0016DD34
		[PublicizedFrom(EAccessModifier.Protected)]
		public void LogError(Exception _e, ModEvents.ModEventAbs<TDelegate>.Receiver _currentMod)
		{
			Log.Error(string.Concat(new string[] { "[MODS] Error while executing ModEvent \"", this.eventName, "\" on mod \"", _currentMod.ModName, "\"" }));
			Log.Exception(_e);
		}

		// Token: 0x04002DAD RID: 11693
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string eventName;

		// Token: 0x04002DAE RID: 11694
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly List<ModEvents.ModEventAbs<TDelegate>.Receiver> receivers = new List<ModEvents.ModEventAbs<TDelegate>.Receiver>();

		// Token: 0x0200074E RID: 1870
		[PublicizedFrom(EAccessModifier.Protected)]
		public class Receiver
		{
			// Token: 0x170005A6 RID: 1446
			// (get) Token: 0x060037EB RID: 14315 RVA: 0x0016FB81 File Offset: 0x0016DD81
			public string ModName
			{
				get
				{
					if (this.Mod != null)
					{
						return this.Mod.Name;
					}
					if (this.coreGame)
					{
						return "-GameCore-";
					}
					return "-UnknownMod-";
				}
			}

			// Token: 0x060037EC RID: 14316 RVA: 0x0016FBAA File Offset: 0x0016DDAA
			public Receiver(Mod _mod, TDelegate _handler, bool _coreGame = false)
			{
				this.Mod = _mod;
				this.DelegateFunc = _handler;
				this.coreGame = _coreGame;
			}

			// Token: 0x04002DAF RID: 11695
			public readonly Mod Mod;

			// Token: 0x04002DB0 RID: 11696
			public readonly TDelegate DelegateFunc;

			// Token: 0x04002DB1 RID: 11697
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly bool coreGame;
		}
	}

	// Token: 0x0200074F RID: 1871
	public class ModEvent<TData> : ModEvents.ModEventAbs<ModEvents.ModEventHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x060037ED RID: 14317 RVA: 0x0016FBC7 File Offset: 0x0016DDC7
		public ModEvent([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x0016FBD0 File Offset: 0x0016DDD0
		public void Invoke(ref TData _data)
		{
			foreach (ModEvents.ModEventAbs<ModEvents.ModEventHandlerDelegate<TData>>.Receiver receiver in this.receivers)
			{
				try
				{
					receiver.DelegateFunc(ref _data);
				}
				catch (Exception ex)
				{
					base.LogError(ex, receiver);
				}
			}
		}
	}

	// Token: 0x02000750 RID: 1872
	public class ModEventInterruptible<TData> : ModEvents.ModEventAbs<ModEvents.ModEventInterruptibleHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x060037EF RID: 14319 RVA: 0x0016FC44 File Offset: 0x0016DE44
		public ModEventInterruptible([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0016FC50 File Offset: 0x0016DE50
		public ValueTuple<ModEvents.EModEventResult, Mod> Invoke(ref TData _data)
		{
			foreach (ModEvents.ModEventAbs<ModEvents.ModEventInterruptibleHandlerDelegate<TData>>.Receiver receiver in this.receivers)
			{
				try
				{
					ModEvents.EModEventResult emodEventResult = receiver.DelegateFunc(ref _data);
					if (emodEventResult != ModEvents.EModEventResult.Continue)
					{
						return new ValueTuple<ModEvents.EModEventResult, Mod>(emodEventResult, receiver.Mod);
					}
				}
				catch (Exception ex)
				{
					base.LogError(ex, receiver);
				}
			}
			return new ValueTuple<ModEvents.EModEventResult, Mod>(ModEvents.EModEventResult.Continue, null);
		}
	}
}
