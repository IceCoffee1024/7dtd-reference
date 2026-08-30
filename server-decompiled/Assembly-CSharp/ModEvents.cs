using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x02000729 RID: 1833
public static class ModEvents
{
	// Token: 0x04002D5D RID: 11613
	public static readonly ModEvents.ModEvent<ModEvents.SGameFocusData> GameFocus = new ModEvents.ModEvent<ModEvents.SGameFocusData>("GameFocus");

	// Token: 0x04002D5E RID: 11614
	public static readonly ModEvents.ModEvent<ModEvents.SGameAwakeData> GameAwake = new ModEvents.ModEvent<ModEvents.SGameAwakeData>("GameAwake");

	// Token: 0x04002D5F RID: 11615
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartingData> GameStarting = new ModEvents.ModEvent<ModEvents.SGameStartingData>("GameStarting");

	// Token: 0x04002D60 RID: 11616
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData> MainMenuOpening = new ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData>("MainMenuOpening");

	// Token: 0x04002D61 RID: 11617
	public static readonly ModEvents.ModEvent<ModEvents.SMainMenuOpenedData> MainMenuOpened = new ModEvents.ModEvent<ModEvents.SMainMenuOpenedData>("MainMenuOpened");

	// Token: 0x04002D62 RID: 11618
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartDoneData> GameStartDone = new ModEvents.ModEvent<ModEvents.SGameStartDoneData>("GameStartDone");

	// Token: 0x04002D63 RID: 11619
	public static readonly ModEvents.ModEvent<ModEvents.SCreateWorldDoneData> CreateWorldDone = new ModEvents.ModEvent<ModEvents.SCreateWorldDoneData>("CreateWorldDone");

	// Token: 0x04002D64 RID: 11620
	public static readonly ModEvents.ModEvent<ModEvents.SGameUpdateData> GameUpdate = new ModEvents.ModEvent<ModEvents.SGameUpdateData>("GameUpdate");

	// Token: 0x04002D65 RID: 11621
	public static readonly ModEvents.ModEvent<ModEvents.SWorldShuttingDownData> WorldShuttingDown = new ModEvents.ModEvent<ModEvents.SWorldShuttingDownData>("WorldShuttingDown");

	// Token: 0x04002D66 RID: 11622
	public static readonly ModEvents.ModEvent<ModEvents.SGameShutdownData> GameShutdown = new ModEvents.ModEvent<ModEvents.SGameShutdownData>("GameShutdown");

	// Token: 0x04002D67 RID: 11623
	public static readonly ModEvents.ModEvent<ModEvents.SServerRegisteredData> ServerRegistered = new ModEvents.ModEvent<ModEvents.SServerRegisteredData>("ServerRegistered");

	// Token: 0x04002D68 RID: 11624
	public static readonly ModEvents.ModEvent<ModEvents.SUnityUpdateData> UnityUpdate = new ModEvents.ModEvent<ModEvents.SUnityUpdateData>("UnityUpdate");

	// Token: 0x04002D69 RID: 11625
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData> PlayerLogin = new ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData>("PlayerLogin");

	// Token: 0x04002D6A RID: 11626
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData> PlayerJoinedGame = new ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData>("PlayerJoinedGame");

	// Token: 0x04002D6B RID: 11627
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawningData> PlayerSpawning = new ModEvents.ModEvent<ModEvents.SPlayerSpawningData>("PlayerSpawning");

	// Token: 0x04002D6C RID: 11628
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData> PlayerSpawnedInWorld = new ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData>("PlayerSpawnedInWorld");

	// Token: 0x04002D6D RID: 11629
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData> PlayerDisconnected = new ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData>("PlayerDisconnected");

	// Token: 0x04002D6E RID: 11630
	public static readonly ModEvents.ModEvent<ModEvents.SSavePlayerDataData> SavePlayerData = new ModEvents.ModEvent<ModEvents.SSavePlayerDataData>("SavePlayerData");

	// Token: 0x04002D6F RID: 11631
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SGameMessageData> GameMessage = new ModEvents.ModEventInterruptible<ModEvents.SGameMessageData>("GameMessage");

	// Token: 0x04002D70 RID: 11632
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SChatMessageData> ChatMessage = new ModEvents.ModEventInterruptible<ModEvents.SChatMessageData>("ChatMessage");

	// Token: 0x04002D71 RID: 11633
	public static readonly ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData> CalcChunkColorsDone = new ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData>("CalcChunkColorsDone");

	// Token: 0x04002D72 RID: 11634
	public static readonly ModEvents.ModEvent<ModEvents.SEntityKilledData> EntityKilled = new ModEvents.ModEvent<ModEvents.SEntityKilledData>("EntityKilled");

	// Token: 0x0200072A RID: 1834
	public struct SGameFocusData
	{
		// Token: 0x06003777 RID: 14199 RVA: 0x0016E057 File Offset: 0x0016C257
		public SGameFocusData(bool _isFocused)
		{
			this.IsFocused = _isFocused;
		}

		// Token: 0x04002D73 RID: 11635
		public readonly bool IsFocused;
	}

	// Token: 0x0200072B RID: 1835
	public struct SGameAwakeData
	{
	}

	// Token: 0x0200072C RID: 1836
	public struct SGameStartingData
	{
		// Token: 0x06003778 RID: 14200 RVA: 0x0016E060 File Offset: 0x0016C260
		public SGameStartingData(bool _asServer)
		{
			this.AsServer = _asServer;
		}

		// Token: 0x04002D74 RID: 11636
		public readonly bool AsServer;
	}

	// Token: 0x0200072D RID: 1837
	public struct SMainMenuOpeningData
	{
		// Token: 0x06003779 RID: 14201 RVA: 0x0016E069 File Offset: 0x0016C269
		public SMainMenuOpeningData(bool _openedBefore)
		{
			this.OpenedBefore = _openedBefore;
		}

		// Token: 0x04002D75 RID: 11637
		public readonly bool OpenedBefore;
	}

	// Token: 0x0200072E RID: 1838
	public struct SMainMenuOpenedData
	{
		// Token: 0x0600377A RID: 14202 RVA: 0x0016E072 File Offset: 0x0016C272
		public SMainMenuOpenedData(bool _firstTimeOpen)
		{
			this.FirstTimeOpen = _firstTimeOpen;
		}

		// Token: 0x04002D76 RID: 11638
		public readonly bool FirstTimeOpen;
	}

	// Token: 0x0200072F RID: 1839
	public struct SGameStartDoneData
	{
	}

	// Token: 0x02000730 RID: 1840
	public struct SCreateWorldDoneData
	{
	}

	// Token: 0x02000731 RID: 1841
	public struct SGameUpdateData
	{
	}

	// Token: 0x02000732 RID: 1842
	public struct SWorldShuttingDownData
	{
	}

	// Token: 0x02000733 RID: 1843
	public struct SGameShutdownData
	{
	}

	// Token: 0x02000734 RID: 1844
	public struct SServerRegisteredData
	{
	}

	// Token: 0x02000735 RID: 1845
	public struct SUnityUpdateData
	{
	}

	// Token: 0x02000736 RID: 1846
	public struct SPlayerLoginData
	{
		// Token: 0x0600377B RID: 14203 RVA: 0x0016E07B File Offset: 0x0016C27B
		public SPlayerLoginData(ClientInfo _clientInfo, string _compatibilityVersion)
		{
			this.ClientInfo = _clientInfo;
			this.CompatibilityVersion = _compatibilityVersion;
			this.CustomMessage = null;
		}

		// Token: 0x04002D77 RID: 11639
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D78 RID: 11640
		public readonly string CompatibilityVersion;

		// Token: 0x04002D79 RID: 11641
		public string CustomMessage;
	}

	// Token: 0x02000737 RID: 1847
	public struct SPlayerJoinedGameData
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x0016E092 File Offset: 0x0016C292
		public SPlayerJoinedGameData(ClientInfo _clientInfo)
		{
			this.ClientInfo = _clientInfo;
		}

		// Token: 0x04002D7A RID: 11642
		public readonly ClientInfo ClientInfo;
	}

	// Token: 0x02000738 RID: 1848
	public readonly struct SPlayerSpawningData
	{
		// Token: 0x0600377D RID: 14205 RVA: 0x0016E09B File Offset: 0x0016C29B
		public SPlayerSpawningData(ClientInfo _clientInfo, int _chunkViewDim, PlayerProfile _playerProfile)
		{
			this.ClientInfo = _clientInfo;
			this.ChunkViewDim = _chunkViewDim;
			this.PlayerProfile = _playerProfile;
		}

		// Token: 0x04002D7B RID: 11643
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D7C RID: 11644
		public readonly int ChunkViewDim;

		// Token: 0x04002D7D RID: 11645
		public readonly PlayerProfile PlayerProfile;
	}

	// Token: 0x02000739 RID: 1849
	public struct SPlayerSpawnedInWorldData
	{
		// Token: 0x0600377E RID: 14206 RVA: 0x0016E0B2 File Offset: 0x0016C2B2
		public SPlayerSpawnedInWorldData(ClientInfo _clientInfo, bool _isLocalPlayer, int _entityId, RespawnType _respawnType, Vector3i _position)
		{
			this.ClientInfo = _clientInfo;
			this.IsLocalPlayer = _isLocalPlayer;
			this.EntityId = _entityId;
			this.RespawnType = _respawnType;
			this.Position = _position;
		}

		// Token: 0x04002D7E RID: 11646
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D7F RID: 11647
		public readonly bool IsLocalPlayer;

		// Token: 0x04002D80 RID: 11648
		public readonly int EntityId;

		// Token: 0x04002D81 RID: 11649
		public readonly RespawnType RespawnType;

		// Token: 0x04002D82 RID: 11650
		public readonly Vector3i Position;
	}

	// Token: 0x0200073A RID: 1850
	public struct SPlayerDisconnectedData
	{
		// Token: 0x0600377F RID: 14207 RVA: 0x0016E0D9 File Offset: 0x0016C2D9
		public SPlayerDisconnectedData(ClientInfo _clientInfo, bool _gameShuttingDown)
		{
			this.ClientInfo = _clientInfo;
			this.GameShuttingDown = _gameShuttingDown;
		}

		// Token: 0x04002D83 RID: 11651
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D84 RID: 11652
		public readonly bool GameShuttingDown;
	}

	// Token: 0x0200073B RID: 1851
	public struct SSavePlayerDataData
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x0016E0E9 File Offset: 0x0016C2E9
		public SSavePlayerDataData(ClientInfo _clientInfo, PlayerDataFile _playerDataFile)
		{
			this.ClientInfo = _clientInfo;
			this.PlayerDataFile = _playerDataFile;
		}

		// Token: 0x04002D85 RID: 11653
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D86 RID: 11654
		public readonly PlayerDataFile PlayerDataFile;
	}

	// Token: 0x0200073C RID: 1852
	public struct SGameMessageData
	{
		// Token: 0x06003781 RID: 14209 RVA: 0x0016E0F9 File Offset: 0x0016C2F9
		public SGameMessageData(ClientInfo _clientInfo, EnumGameMessages _messageType, string _mainName, string _secondaryName)
		{
			this.ClientInfo = _clientInfo;
			this.MessageType = _messageType;
			this.MainName = _mainName;
			this.SecondaryName = _secondaryName;
		}

		// Token: 0x04002D87 RID: 11655
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D88 RID: 11656
		public readonly EnumGameMessages MessageType;

		// Token: 0x04002D89 RID: 11657
		public readonly string MainName;

		// Token: 0x04002D8A RID: 11658
		public readonly string SecondaryName;
	}

	// Token: 0x0200073D RID: 1853
	public struct SChatMessageData
	{
		// Token: 0x06003782 RID: 14210 RVA: 0x0016E118 File Offset: 0x0016C318
		public SChatMessageData(ClientInfo _clientInfo, EChatType _chatType, int _senderEntityId, string _message, string _mainName, List<int> _recipientEntityIds)
		{
			this.ClientInfo = _clientInfo;
			this.ChatType = _chatType;
			this.SenderEntityId = _senderEntityId;
			this.Message = _message;
			this.MainName = _mainName;
			this.RecipientEntityIds = _recipientEntityIds;
		}

		// Token: 0x04002D8B RID: 11659
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D8C RID: 11660
		public readonly EChatType ChatType;

		// Token: 0x04002D8D RID: 11661
		public readonly int SenderEntityId;

		// Token: 0x04002D8E RID: 11662
		public readonly string Message;

		// Token: 0x04002D8F RID: 11663
		public readonly string MainName;

		// Token: 0x04002D90 RID: 11664
		public readonly List<int> RecipientEntityIds;
	}

	// Token: 0x0200073E RID: 1854
	public struct SCalcChunkColorsDoneData
	{
		// Token: 0x06003783 RID: 14211 RVA: 0x0016E147 File Offset: 0x0016C347
		public SCalcChunkColorsDoneData(Chunk _chunk)
		{
			this.Chunk = _chunk;
		}

		// Token: 0x04002D91 RID: 11665
		public readonly Chunk Chunk;
	}

	// Token: 0x0200073F RID: 1855
	public struct SEntityKilledData
	{
		// Token: 0x06003784 RID: 14212 RVA: 0x0016E150 File Offset: 0x0016C350
		public SEntityKilledData(Entity _killedEntitiy, Entity _killingEntity)
		{
			this.KilledEntitiy = _killedEntitiy;
			this.KillingEntity = _killingEntity;
		}

		// Token: 0x04002D92 RID: 11666
		public readonly Entity KilledEntitiy;

		// Token: 0x04002D93 RID: 11667
		public readonly Entity KillingEntity;
	}

	// Token: 0x02000740 RID: 1856
	public enum EModEventResult
	{
		// Token: 0x04002D95 RID: 11669
		Continue,
		// Token: 0x04002D96 RID: 11670
		StopHandlersRunVanilla,
		// Token: 0x04002D97 RID: 11671
		StopHandlersAndVanilla
	}

	// Token: 0x02000741 RID: 1857
	// (Invoke) Token: 0x06003786 RID: 14214
	public delegate void ModEventHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x02000742 RID: 1858
	// (Invoke) Token: 0x0600378A RID: 14218
	public delegate ModEvents.EModEventResult ModEventInterruptibleHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x02000743 RID: 1859
	public abstract class ModEventAbs<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x0600378D RID: 14221 RVA: 0x0016E160 File Offset: 0x0016C360
		[PublicizedFrom(EAccessModifier.Protected)]
		public ModEventAbs(string _eventName)
		{
			this.eventName = _eventName;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0016E17C File Offset: 0x0016C37C
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

		// Token: 0x0600378F RID: 14223 RVA: 0x0016E1E4 File Offset: 0x0016C3E4
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

		// Token: 0x06003790 RID: 14224 RVA: 0x0016E238 File Offset: 0x0016C438
		[PublicizedFrom(EAccessModifier.Protected)]
		public void LogError(Exception _e, ModEvents.ModEventAbs<TDelegate>.Receiver _currentMod)
		{
			Log.Error(string.Concat(new string[] { "[MODS] Error while executing ModEvent \"", this.eventName, "\" on mod \"", _currentMod.ModName, "\"" }));
			Log.Exception(_e);
		}

		// Token: 0x04002D98 RID: 11672
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string eventName;

		// Token: 0x04002D99 RID: 11673
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly List<ModEvents.ModEventAbs<TDelegate>.Receiver> receivers = new List<ModEvents.ModEventAbs<TDelegate>.Receiver>();

		// Token: 0x02000744 RID: 1860
		[PublicizedFrom(EAccessModifier.Protected)]
		public class Receiver
		{
			// Token: 0x1700057C RID: 1404
			// (get) Token: 0x06003791 RID: 14225 RVA: 0x0016E285 File Offset: 0x0016C485
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

			// Token: 0x06003792 RID: 14226 RVA: 0x0016E2AE File Offset: 0x0016C4AE
			public Receiver(Mod _mod, TDelegate _handler, bool _coreGame = false)
			{
				this.Mod = _mod;
				this.DelegateFunc = _handler;
				this.coreGame = _coreGame;
			}

			// Token: 0x04002D9A RID: 11674
			public readonly Mod Mod;

			// Token: 0x04002D9B RID: 11675
			public readonly TDelegate DelegateFunc;

			// Token: 0x04002D9C RID: 11676
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly bool coreGame;
		}
	}

	// Token: 0x02000745 RID: 1861
	public class ModEvent<TData> : ModEvents.ModEventAbs<ModEvents.ModEventHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x06003793 RID: 14227 RVA: 0x0016E2CB File Offset: 0x0016C4CB
		public ModEvent([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0016E2D4 File Offset: 0x0016C4D4
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

	// Token: 0x02000746 RID: 1862
	public class ModEventInterruptible<TData> : ModEvents.ModEventAbs<ModEvents.ModEventInterruptibleHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x06003795 RID: 14229 RVA: 0x0016E348 File Offset: 0x0016C548
		public ModEventInterruptible([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x0016E354 File Offset: 0x0016C554
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
