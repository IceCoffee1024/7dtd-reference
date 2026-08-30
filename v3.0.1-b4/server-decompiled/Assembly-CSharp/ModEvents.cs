using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x0200071F RID: 1823
public static class ModEvents
{
	// Token: 0x04002CE5 RID: 11493
	public static readonly ModEvents.ModEvent<ModEvents.SGameFocusData> GameFocus = new ModEvents.ModEvent<ModEvents.SGameFocusData>("GameFocus");

	// Token: 0x04002CE6 RID: 11494
	public static readonly ModEvents.ModEvent<ModEvents.SGameAwakeData> GameAwake = new ModEvents.ModEvent<ModEvents.SGameAwakeData>("GameAwake");

	// Token: 0x04002CE7 RID: 11495
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartingData> GameStarting = new ModEvents.ModEvent<ModEvents.SGameStartingData>("GameStarting");

	// Token: 0x04002CE8 RID: 11496
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData> MainMenuOpening = new ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData>("MainMenuOpening");

	// Token: 0x04002CE9 RID: 11497
	public static readonly ModEvents.ModEvent<ModEvents.SMainMenuOpenedData> MainMenuOpened = new ModEvents.ModEvent<ModEvents.SMainMenuOpenedData>("MainMenuOpened");

	// Token: 0x04002CEA RID: 11498
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartDoneData> GameStartDone = new ModEvents.ModEvent<ModEvents.SGameStartDoneData>("GameStartDone");

	// Token: 0x04002CEB RID: 11499
	public static readonly ModEvents.ModEvent<ModEvents.SCreateWorldDoneData> CreateWorldDone = new ModEvents.ModEvent<ModEvents.SCreateWorldDoneData>("CreateWorldDone");

	// Token: 0x04002CEC RID: 11500
	public static readonly ModEvents.ModEvent<ModEvents.SGameUpdateData> GameUpdate = new ModEvents.ModEvent<ModEvents.SGameUpdateData>("GameUpdate");

	// Token: 0x04002CED RID: 11501
	public static readonly ModEvents.ModEvent<ModEvents.SWorldShuttingDownData> WorldShuttingDown = new ModEvents.ModEvent<ModEvents.SWorldShuttingDownData>("WorldShuttingDown");

	// Token: 0x04002CEE RID: 11502
	public static readonly ModEvents.ModEvent<ModEvents.SGameShutdownData> GameShutdown = new ModEvents.ModEvent<ModEvents.SGameShutdownData>("GameShutdown");

	// Token: 0x04002CEF RID: 11503
	public static readonly ModEvents.ModEvent<ModEvents.SServerRegisteredData> ServerRegistered = new ModEvents.ModEvent<ModEvents.SServerRegisteredData>("ServerRegistered");

	// Token: 0x04002CF0 RID: 11504
	public static readonly ModEvents.ModEvent<ModEvents.SUnityUpdateData> UnityUpdate = new ModEvents.ModEvent<ModEvents.SUnityUpdateData>("UnityUpdate");

	// Token: 0x04002CF1 RID: 11505
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData> PlayerLogin = new ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData>("PlayerLogin");

	// Token: 0x04002CF2 RID: 11506
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData> PlayerJoinedGame = new ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData>("PlayerJoinedGame");

	// Token: 0x04002CF3 RID: 11507
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawningData> PlayerSpawning = new ModEvents.ModEvent<ModEvents.SPlayerSpawningData>("PlayerSpawning");

	// Token: 0x04002CF4 RID: 11508
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData> PlayerSpawnedInWorld = new ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData>("PlayerSpawnedInWorld");

	// Token: 0x04002CF5 RID: 11509
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData> PlayerDisconnected = new ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData>("PlayerDisconnected");

	// Token: 0x04002CF6 RID: 11510
	public static readonly ModEvents.ModEvent<ModEvents.SSavePlayerDataData> SavePlayerData = new ModEvents.ModEvent<ModEvents.SSavePlayerDataData>("SavePlayerData");

	// Token: 0x04002CF7 RID: 11511
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SGameMessageData> GameMessage = new ModEvents.ModEventInterruptible<ModEvents.SGameMessageData>("GameMessage");

	// Token: 0x04002CF8 RID: 11512
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SChatMessageData> ChatMessage = new ModEvents.ModEventInterruptible<ModEvents.SChatMessageData>("ChatMessage");

	// Token: 0x04002CF9 RID: 11513
	public static readonly ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData> CalcChunkColorsDone = new ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData>("CalcChunkColorsDone");

	// Token: 0x04002CFA RID: 11514
	public static readonly ModEvents.ModEvent<ModEvents.SEntityKilledData> EntityKilled = new ModEvents.ModEvent<ModEvents.SEntityKilledData>("EntityKilled");

	// Token: 0x02000720 RID: 1824
	public struct SGameFocusData
	{
		// Token: 0x06003721 RID: 14113 RVA: 0x0016BE3B File Offset: 0x0016A03B
		public SGameFocusData(bool _isFocused)
		{
			this.IsFocused = _isFocused;
		}

		// Token: 0x04002CFB RID: 11515
		public readonly bool IsFocused;
	}

	// Token: 0x02000721 RID: 1825
	public struct SGameAwakeData
	{
	}

	// Token: 0x02000722 RID: 1826
	public struct SGameStartingData
	{
		// Token: 0x06003722 RID: 14114 RVA: 0x0016BE44 File Offset: 0x0016A044
		public SGameStartingData(bool _asServer)
		{
			this.AsServer = _asServer;
		}

		// Token: 0x04002CFC RID: 11516
		public readonly bool AsServer;
	}

	// Token: 0x02000723 RID: 1827
	public struct SMainMenuOpeningData
	{
		// Token: 0x06003723 RID: 14115 RVA: 0x0016BE4D File Offset: 0x0016A04D
		public SMainMenuOpeningData(bool _openedBefore)
		{
			this.OpenedBefore = _openedBefore;
		}

		// Token: 0x04002CFD RID: 11517
		public readonly bool OpenedBefore;
	}

	// Token: 0x02000724 RID: 1828
	public struct SMainMenuOpenedData
	{
		// Token: 0x06003724 RID: 14116 RVA: 0x0016BE56 File Offset: 0x0016A056
		public SMainMenuOpenedData(bool _firstTimeOpen)
		{
			this.FirstTimeOpen = _firstTimeOpen;
		}

		// Token: 0x04002CFE RID: 11518
		public readonly bool FirstTimeOpen;
	}

	// Token: 0x02000725 RID: 1829
	public struct SGameStartDoneData
	{
	}

	// Token: 0x02000726 RID: 1830
	public struct SCreateWorldDoneData
	{
	}

	// Token: 0x02000727 RID: 1831
	public struct SGameUpdateData
	{
	}

	// Token: 0x02000728 RID: 1832
	public struct SWorldShuttingDownData
	{
	}

	// Token: 0x02000729 RID: 1833
	public struct SGameShutdownData
	{
	}

	// Token: 0x0200072A RID: 1834
	public struct SServerRegisteredData
	{
	}

	// Token: 0x0200072B RID: 1835
	public struct SUnityUpdateData
	{
	}

	// Token: 0x0200072C RID: 1836
	public struct SPlayerLoginData
	{
		// Token: 0x06003725 RID: 14117 RVA: 0x0016BE5F File Offset: 0x0016A05F
		public SPlayerLoginData(ClientInfo _clientInfo, string _compatibilityVersion)
		{
			this.ClientInfo = _clientInfo;
			this.CompatibilityVersion = _compatibilityVersion;
			this.CustomMessage = null;
		}

		// Token: 0x04002CFF RID: 11519
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D00 RID: 11520
		public readonly string CompatibilityVersion;

		// Token: 0x04002D01 RID: 11521
		public string CustomMessage;
	}

	// Token: 0x0200072D RID: 1837
	public struct SPlayerJoinedGameData
	{
		// Token: 0x06003726 RID: 14118 RVA: 0x0016BE76 File Offset: 0x0016A076
		public SPlayerJoinedGameData(ClientInfo _clientInfo)
		{
			this.ClientInfo = _clientInfo;
		}

		// Token: 0x04002D02 RID: 11522
		public readonly ClientInfo ClientInfo;
	}

	// Token: 0x0200072E RID: 1838
	public readonly struct SPlayerSpawningData
	{
		// Token: 0x06003727 RID: 14119 RVA: 0x0016BE7F File Offset: 0x0016A07F
		public SPlayerSpawningData(ClientInfo _clientInfo, int _chunkViewDim, PlayerProfile _playerProfile)
		{
			this.ClientInfo = _clientInfo;
			this.ChunkViewDim = _chunkViewDim;
			this.PlayerProfile = _playerProfile;
		}

		// Token: 0x04002D03 RID: 11523
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D04 RID: 11524
		public readonly int ChunkViewDim;

		// Token: 0x04002D05 RID: 11525
		public readonly PlayerProfile PlayerProfile;
	}

	// Token: 0x0200072F RID: 1839
	public struct SPlayerSpawnedInWorldData
	{
		// Token: 0x06003728 RID: 14120 RVA: 0x0016BE96 File Offset: 0x0016A096
		public SPlayerSpawnedInWorldData(ClientInfo _clientInfo, bool _isLocalPlayer, int _entityId, RespawnType _respawnType, Vector3i _position)
		{
			this.ClientInfo = _clientInfo;
			this.IsLocalPlayer = _isLocalPlayer;
			this.EntityId = _entityId;
			this.RespawnType = _respawnType;
			this.Position = _position;
		}

		// Token: 0x04002D06 RID: 11526
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D07 RID: 11527
		public readonly bool IsLocalPlayer;

		// Token: 0x04002D08 RID: 11528
		public readonly int EntityId;

		// Token: 0x04002D09 RID: 11529
		public readonly RespawnType RespawnType;

		// Token: 0x04002D0A RID: 11530
		public readonly Vector3i Position;
	}

	// Token: 0x02000730 RID: 1840
	public struct SPlayerDisconnectedData
	{
		// Token: 0x06003729 RID: 14121 RVA: 0x0016BEBD File Offset: 0x0016A0BD
		public SPlayerDisconnectedData(ClientInfo _clientInfo, bool _gameShuttingDown)
		{
			this.ClientInfo = _clientInfo;
			this.GameShuttingDown = _gameShuttingDown;
		}

		// Token: 0x04002D0B RID: 11531
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D0C RID: 11532
		public readonly bool GameShuttingDown;
	}

	// Token: 0x02000731 RID: 1841
	public struct SSavePlayerDataData
	{
		// Token: 0x0600372A RID: 14122 RVA: 0x0016BECD File Offset: 0x0016A0CD
		public SSavePlayerDataData(ClientInfo _clientInfo, PlayerDataFile _playerDataFile)
		{
			this.ClientInfo = _clientInfo;
			this.PlayerDataFile = _playerDataFile;
		}

		// Token: 0x04002D0D RID: 11533
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D0E RID: 11534
		public readonly PlayerDataFile PlayerDataFile;
	}

	// Token: 0x02000732 RID: 1842
	public struct SGameMessageData
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x0016BEDD File Offset: 0x0016A0DD
		public SGameMessageData(ClientInfo _clientInfo, EnumGameMessages _messageType, string _mainName, string _secondaryName)
		{
			this.ClientInfo = _clientInfo;
			this.MessageType = _messageType;
			this.MainName = _mainName;
			this.SecondaryName = _secondaryName;
		}

		// Token: 0x04002D0F RID: 11535
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D10 RID: 11536
		public readonly EnumGameMessages MessageType;

		// Token: 0x04002D11 RID: 11537
		public readonly string MainName;

		// Token: 0x04002D12 RID: 11538
		public readonly string SecondaryName;
	}

	// Token: 0x02000733 RID: 1843
	public struct SChatMessageData
	{
		// Token: 0x0600372C RID: 14124 RVA: 0x0016BEFC File Offset: 0x0016A0FC
		public SChatMessageData(ClientInfo _clientInfo, EChatType _chatType, int _senderEntityId, string _message, string _mainName, List<int> _recipientEntityIds)
		{
			this.ClientInfo = _clientInfo;
			this.ChatType = _chatType;
			this.SenderEntityId = _senderEntityId;
			this.Message = _message;
			this.MainName = _mainName;
			this.RecipientEntityIds = _recipientEntityIds;
		}

		// Token: 0x04002D13 RID: 11539
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D14 RID: 11540
		public readonly EChatType ChatType;

		// Token: 0x04002D15 RID: 11541
		public readonly int SenderEntityId;

		// Token: 0x04002D16 RID: 11542
		public readonly string Message;

		// Token: 0x04002D17 RID: 11543
		public readonly string MainName;

		// Token: 0x04002D18 RID: 11544
		public readonly List<int> RecipientEntityIds;
	}

	// Token: 0x02000734 RID: 1844
	public struct SCalcChunkColorsDoneData
	{
		// Token: 0x0600372D RID: 14125 RVA: 0x0016BF2B File Offset: 0x0016A12B
		public SCalcChunkColorsDoneData(Chunk _chunk)
		{
			this.Chunk = _chunk;
		}

		// Token: 0x04002D19 RID: 11545
		public readonly Chunk Chunk;
	}

	// Token: 0x02000735 RID: 1845
	public struct SEntityKilledData
	{
		// Token: 0x0600372E RID: 14126 RVA: 0x0016BF34 File Offset: 0x0016A134
		public SEntityKilledData(Entity _killedEntitiy, Entity _killingEntity)
		{
			this.KilledEntitiy = _killedEntitiy;
			this.KillingEntity = _killingEntity;
		}

		// Token: 0x04002D1A RID: 11546
		public readonly Entity KilledEntitiy;

		// Token: 0x04002D1B RID: 11547
		public readonly Entity KillingEntity;
	}

	// Token: 0x02000736 RID: 1846
	public enum EModEventResult
	{
		// Token: 0x04002D1D RID: 11549
		Continue,
		// Token: 0x04002D1E RID: 11550
		StopHandlersRunVanilla,
		// Token: 0x04002D1F RID: 11551
		StopHandlersAndVanilla
	}

	// Token: 0x02000737 RID: 1847
	// (Invoke) Token: 0x06003730 RID: 14128
	public delegate void ModEventHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x02000738 RID: 1848
	// (Invoke) Token: 0x06003734 RID: 14132
	public delegate ModEvents.EModEventResult ModEventInterruptibleHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x02000739 RID: 1849
	public abstract class ModEventAbs<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x06003737 RID: 14135 RVA: 0x0016BF44 File Offset: 0x0016A144
		[PublicizedFrom(EAccessModifier.Protected)]
		public ModEventAbs(string _eventName)
		{
			this.eventName = _eventName;
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x0016BF60 File Offset: 0x0016A160
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

		// Token: 0x06003739 RID: 14137 RVA: 0x0016BFC8 File Offset: 0x0016A1C8
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

		// Token: 0x0600373A RID: 14138 RVA: 0x0016C01C File Offset: 0x0016A21C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void LogError(Exception _e, ModEvents.ModEventAbs<TDelegate>.Receiver _currentMod)
		{
			Log.Error(string.Concat(new string[] { "[MODS] Error while executing ModEvent \"", this.eventName, "\" on mod \"", _currentMod.ModName, "\"" }));
			Log.Exception(_e);
		}

		// Token: 0x04002D20 RID: 11552
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string eventName;

		// Token: 0x04002D21 RID: 11553
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly List<ModEvents.ModEventAbs<TDelegate>.Receiver> receivers = new List<ModEvents.ModEventAbs<TDelegate>.Receiver>();

		// Token: 0x0200073A RID: 1850
		[PublicizedFrom(EAccessModifier.Protected)]
		public class Receiver
		{
			// Token: 0x17000573 RID: 1395
			// (get) Token: 0x0600373B RID: 14139 RVA: 0x0016C069 File Offset: 0x0016A269
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

			// Token: 0x0600373C RID: 14140 RVA: 0x0016C092 File Offset: 0x0016A292
			public Receiver(Mod _mod, TDelegate _handler, bool _coreGame = false)
			{
				this.Mod = _mod;
				this.DelegateFunc = _handler;
				this.coreGame = _coreGame;
			}

			// Token: 0x04002D22 RID: 11554
			public readonly Mod Mod;

			// Token: 0x04002D23 RID: 11555
			public readonly TDelegate DelegateFunc;

			// Token: 0x04002D24 RID: 11556
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly bool coreGame;
		}
	}

	// Token: 0x0200073B RID: 1851
	public class ModEvent<TData> : ModEvents.ModEventAbs<ModEvents.ModEventHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x0600373D RID: 14141 RVA: 0x0016C0AF File Offset: 0x0016A2AF
		public ModEvent([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x0016C0B8 File Offset: 0x0016A2B8
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

	// Token: 0x0200073C RID: 1852
	public class ModEventInterruptible<TData> : ModEvents.ModEventAbs<ModEvents.ModEventInterruptibleHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x0600373F RID: 14143 RVA: 0x0016C12C File Offset: 0x0016A32C
		public ModEventInterruptible([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x0016C138 File Offset: 0x0016A338
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
