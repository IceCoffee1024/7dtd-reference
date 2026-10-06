using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x0200072B RID: 1835
public static class ModEvents
{
	// Token: 0x04002D56 RID: 11606
	public static readonly ModEvents.ModEvent<ModEvents.SGameFocusData> GameFocus = new ModEvents.ModEvent<ModEvents.SGameFocusData>("GameFocus");

	// Token: 0x04002D57 RID: 11607
	public static readonly ModEvents.ModEvent<ModEvents.SGameAwakeData> GameAwake = new ModEvents.ModEvent<ModEvents.SGameAwakeData>("GameAwake");

	// Token: 0x04002D58 RID: 11608
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartingData> GameStarting = new ModEvents.ModEvent<ModEvents.SGameStartingData>("GameStarting");

	// Token: 0x04002D59 RID: 11609
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData> MainMenuOpening = new ModEvents.ModEventInterruptible<ModEvents.SMainMenuOpeningData>("MainMenuOpening");

	// Token: 0x04002D5A RID: 11610
	public static readonly ModEvents.ModEvent<ModEvents.SMainMenuOpenedData> MainMenuOpened = new ModEvents.ModEvent<ModEvents.SMainMenuOpenedData>("MainMenuOpened");

	// Token: 0x04002D5B RID: 11611
	public static readonly ModEvents.ModEvent<ModEvents.SGameStartDoneData> GameStartDone = new ModEvents.ModEvent<ModEvents.SGameStartDoneData>("GameStartDone");

	// Token: 0x04002D5C RID: 11612
	public static readonly ModEvents.ModEvent<ModEvents.SCreateWorldDoneData> CreateWorldDone = new ModEvents.ModEvent<ModEvents.SCreateWorldDoneData>("CreateWorldDone");

	// Token: 0x04002D5D RID: 11613
	public static readonly ModEvents.ModEvent<ModEvents.SGameUpdateData> GameUpdate = new ModEvents.ModEvent<ModEvents.SGameUpdateData>("GameUpdate");

	// Token: 0x04002D5E RID: 11614
	public static readonly ModEvents.ModEvent<ModEvents.SWorldShuttingDownData> WorldShuttingDown = new ModEvents.ModEvent<ModEvents.SWorldShuttingDownData>("WorldShuttingDown");

	// Token: 0x04002D5F RID: 11615
	public static readonly ModEvents.ModEvent<ModEvents.SGameShutdownData> GameShutdown = new ModEvents.ModEvent<ModEvents.SGameShutdownData>("GameShutdown");

	// Token: 0x04002D60 RID: 11616
	public static readonly ModEvents.ModEvent<ModEvents.SServerRegisteredData> ServerRegistered = new ModEvents.ModEvent<ModEvents.SServerRegisteredData>("ServerRegistered");

	// Token: 0x04002D61 RID: 11617
	public static readonly ModEvents.ModEvent<ModEvents.SUnityUpdateData> UnityUpdate = new ModEvents.ModEvent<ModEvents.SUnityUpdateData>("UnityUpdate");

	// Token: 0x04002D62 RID: 11618
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData> PlayerLogin = new ModEvents.ModEventInterruptible<ModEvents.SPlayerLoginData>("PlayerLogin");

	// Token: 0x04002D63 RID: 11619
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData> PlayerJoinedGame = new ModEvents.ModEvent<ModEvents.SPlayerJoinedGameData>("PlayerJoinedGame");

	// Token: 0x04002D64 RID: 11620
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawningData> PlayerSpawning = new ModEvents.ModEvent<ModEvents.SPlayerSpawningData>("PlayerSpawning");

	// Token: 0x04002D65 RID: 11621
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData> PlayerSpawnedInWorld = new ModEvents.ModEvent<ModEvents.SPlayerSpawnedInWorldData>("PlayerSpawnedInWorld");

	// Token: 0x04002D66 RID: 11622
	public static readonly ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData> PlayerDisconnected = new ModEvents.ModEvent<ModEvents.SPlayerDisconnectedData>("PlayerDisconnected");

	// Token: 0x04002D67 RID: 11623
	public static readonly ModEvents.ModEvent<ModEvents.SSavePlayerDataData> SavePlayerData = new ModEvents.ModEvent<ModEvents.SSavePlayerDataData>("SavePlayerData");

	// Token: 0x04002D68 RID: 11624
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SGameMessageData> GameMessage = new ModEvents.ModEventInterruptible<ModEvents.SGameMessageData>("GameMessage");

	// Token: 0x04002D69 RID: 11625
	public static readonly ModEvents.ModEventInterruptible<ModEvents.SChatMessageData> ChatMessage = new ModEvents.ModEventInterruptible<ModEvents.SChatMessageData>("ChatMessage");

	// Token: 0x04002D6A RID: 11626
	public static readonly ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData> CalcChunkColorsDone = new ModEvents.ModEvent<ModEvents.SCalcChunkColorsDoneData>("CalcChunkColorsDone");

	// Token: 0x04002D6B RID: 11627
	public static readonly ModEvents.ModEvent<ModEvents.SEntityKilledData> EntityKilled = new ModEvents.ModEvent<ModEvents.SEntityKilledData>("EntityKilled");

	// Token: 0x0200072C RID: 1836
	public struct SGameFocusData
	{
		// Token: 0x060037BA RID: 14266 RVA: 0x0016E9BB File Offset: 0x0016CBBB
		public SGameFocusData(bool _isFocused)
		{
			this.IsFocused = _isFocused;
		}

		// Token: 0x04002D6C RID: 11628
		public readonly bool IsFocused;
	}

	// Token: 0x0200072D RID: 1837
	public struct SGameAwakeData
	{
	}

	// Token: 0x0200072E RID: 1838
	public struct SGameStartingData
	{
		// Token: 0x060037BB RID: 14267 RVA: 0x0016E9C4 File Offset: 0x0016CBC4
		public SGameStartingData(bool _asServer)
		{
			this.AsServer = _asServer;
		}

		// Token: 0x04002D6D RID: 11629
		public readonly bool AsServer;
	}

	// Token: 0x0200072F RID: 1839
	public struct SMainMenuOpeningData
	{
		// Token: 0x060037BC RID: 14268 RVA: 0x0016E9CD File Offset: 0x0016CBCD
		public SMainMenuOpeningData(bool _openedBefore)
		{
			this.OpenedBefore = _openedBefore;
		}

		// Token: 0x04002D6E RID: 11630
		public readonly bool OpenedBefore;
	}

	// Token: 0x02000730 RID: 1840
	public struct SMainMenuOpenedData
	{
		// Token: 0x060037BD RID: 14269 RVA: 0x0016E9D6 File Offset: 0x0016CBD6
		public SMainMenuOpenedData(bool _firstTimeOpen)
		{
			this.FirstTimeOpen = _firstTimeOpen;
		}

		// Token: 0x04002D6F RID: 11631
		public readonly bool FirstTimeOpen;
	}

	// Token: 0x02000731 RID: 1841
	public struct SGameStartDoneData
	{
	}

	// Token: 0x02000732 RID: 1842
	public struct SCreateWorldDoneData
	{
	}

	// Token: 0x02000733 RID: 1843
	public struct SGameUpdateData
	{
	}

	// Token: 0x02000734 RID: 1844
	public struct SWorldShuttingDownData
	{
	}

	// Token: 0x02000735 RID: 1845
	public struct SGameShutdownData
	{
	}

	// Token: 0x02000736 RID: 1846
	public struct SServerRegisteredData
	{
	}

	// Token: 0x02000737 RID: 1847
	public struct SUnityUpdateData
	{
	}

	// Token: 0x02000738 RID: 1848
	public struct SPlayerLoginData
	{
		// Token: 0x060037BE RID: 14270 RVA: 0x0016E9DF File Offset: 0x0016CBDF
		public SPlayerLoginData(ClientInfo _clientInfo, string _compatibilityVersion)
		{
			this.ClientInfo = _clientInfo;
			this.CompatibilityVersion = _compatibilityVersion;
			this.CustomMessage = null;
		}

		// Token: 0x04002D70 RID: 11632
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D71 RID: 11633
		public readonly string CompatibilityVersion;

		// Token: 0x04002D72 RID: 11634
		public string CustomMessage;
	}

	// Token: 0x02000739 RID: 1849
	public struct SPlayerJoinedGameData
	{
		// Token: 0x060037BF RID: 14271 RVA: 0x0016E9F6 File Offset: 0x0016CBF6
		public SPlayerJoinedGameData(ClientInfo _clientInfo)
		{
			this.ClientInfo = _clientInfo;
		}

		// Token: 0x04002D73 RID: 11635
		public readonly ClientInfo ClientInfo;
	}

	// Token: 0x0200073A RID: 1850
	public readonly struct SPlayerSpawningData
	{
		// Token: 0x060037C0 RID: 14272 RVA: 0x0016E9FF File Offset: 0x0016CBFF
		public SPlayerSpawningData(ClientInfo _clientInfo, int _chunkViewDim, PlayerProfile _playerProfile)
		{
			this.ClientInfo = _clientInfo;
			this.ChunkViewDim = _chunkViewDim;
			this.PlayerProfile = _playerProfile;
		}

		// Token: 0x04002D74 RID: 11636
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D75 RID: 11637
		public readonly int ChunkViewDim;

		// Token: 0x04002D76 RID: 11638
		public readonly PlayerProfile PlayerProfile;
	}

	// Token: 0x0200073B RID: 1851
	public struct SPlayerSpawnedInWorldData
	{
		// Token: 0x060037C1 RID: 14273 RVA: 0x0016EA16 File Offset: 0x0016CC16
		public SPlayerSpawnedInWorldData(ClientInfo _clientInfo, bool _isLocalPlayer, int _entityId, RespawnType _respawnType, Vector3i _position)
		{
			this.ClientInfo = _clientInfo;
			this.IsLocalPlayer = _isLocalPlayer;
			this.EntityId = _entityId;
			this.RespawnType = _respawnType;
			this.Position = _position;
		}

		// Token: 0x04002D77 RID: 11639
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D78 RID: 11640
		public readonly bool IsLocalPlayer;

		// Token: 0x04002D79 RID: 11641
		public readonly int EntityId;

		// Token: 0x04002D7A RID: 11642
		public readonly RespawnType RespawnType;

		// Token: 0x04002D7B RID: 11643
		public readonly Vector3i Position;
	}

	// Token: 0x0200073C RID: 1852
	public struct SPlayerDisconnectedData
	{
		// Token: 0x060037C2 RID: 14274 RVA: 0x0016EA3D File Offset: 0x0016CC3D
		public SPlayerDisconnectedData(ClientInfo _clientInfo, bool _gameShuttingDown)
		{
			this.ClientInfo = _clientInfo;
			this.GameShuttingDown = _gameShuttingDown;
		}

		// Token: 0x04002D7C RID: 11644
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D7D RID: 11645
		public readonly bool GameShuttingDown;
	}

	// Token: 0x0200073D RID: 1853
	public struct SSavePlayerDataData
	{
		// Token: 0x060037C3 RID: 14275 RVA: 0x0016EA4D File Offset: 0x0016CC4D
		public SSavePlayerDataData(ClientInfo _clientInfo, PlayerDataFile _playerDataFile)
		{
			this.ClientInfo = _clientInfo;
			this.PlayerDataFile = _playerDataFile;
		}

		// Token: 0x04002D7E RID: 11646
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D7F RID: 11647
		public readonly PlayerDataFile PlayerDataFile;
	}

	// Token: 0x0200073E RID: 1854
	public struct SGameMessageData
	{
		// Token: 0x060037C4 RID: 14276 RVA: 0x0016EA5D File Offset: 0x0016CC5D
		public SGameMessageData(ClientInfo _clientInfo, EnumGameMessages _messageType, string _mainName, string _secondaryName)
		{
			this.ClientInfo = _clientInfo;
			this.MessageType = _messageType;
			this.MainName = _mainName;
			this.SecondaryName = _secondaryName;
		}

		// Token: 0x04002D80 RID: 11648
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D81 RID: 11649
		public readonly EnumGameMessages MessageType;

		// Token: 0x04002D82 RID: 11650
		public readonly string MainName;

		// Token: 0x04002D83 RID: 11651
		public readonly string SecondaryName;
	}

	// Token: 0x0200073F RID: 1855
	public struct SChatMessageData
	{
		// Token: 0x060037C5 RID: 14277 RVA: 0x0016EA7C File Offset: 0x0016CC7C
		public SChatMessageData(ClientInfo _clientInfo, EChatType _chatType, int _senderEntityId, string _message, string _mainName, List<int> _recipientEntityIds)
		{
			this.ClientInfo = _clientInfo;
			this.ChatType = _chatType;
			this.SenderEntityId = _senderEntityId;
			this.Message = _message;
			this.MainName = _mainName;
			this.RecipientEntityIds = _recipientEntityIds;
		}

		// Token: 0x04002D84 RID: 11652
		public readonly ClientInfo ClientInfo;

		// Token: 0x04002D85 RID: 11653
		public readonly EChatType ChatType;

		// Token: 0x04002D86 RID: 11654
		public readonly int SenderEntityId;

		// Token: 0x04002D87 RID: 11655
		public readonly string Message;

		// Token: 0x04002D88 RID: 11656
		public readonly string MainName;

		// Token: 0x04002D89 RID: 11657
		public readonly List<int> RecipientEntityIds;
	}

	// Token: 0x02000740 RID: 1856
	public struct SCalcChunkColorsDoneData
	{
		// Token: 0x060037C6 RID: 14278 RVA: 0x0016EAAB File Offset: 0x0016CCAB
		public SCalcChunkColorsDoneData(Chunk _chunk)
		{
			this.Chunk = _chunk;
		}

		// Token: 0x04002D8A RID: 11658
		public readonly Chunk Chunk;
	}

	// Token: 0x02000741 RID: 1857
	public struct SEntityKilledData
	{
		// Token: 0x060037C7 RID: 14279 RVA: 0x0016EAB4 File Offset: 0x0016CCB4
		public SEntityKilledData(Entity _killedEntitiy, Entity _killingEntity)
		{
			this.KilledEntitiy = _killedEntitiy;
			this.KillingEntity = _killingEntity;
		}

		// Token: 0x04002D8B RID: 11659
		public readonly Entity KilledEntitiy;

		// Token: 0x04002D8C RID: 11660
		public readonly Entity KillingEntity;
	}

	// Token: 0x02000742 RID: 1858
	public enum EModEventResult
	{
		// Token: 0x04002D8E RID: 11662
		Continue,
		// Token: 0x04002D8F RID: 11663
		StopHandlersRunVanilla,
		// Token: 0x04002D90 RID: 11664
		StopHandlersAndVanilla
	}

	// Token: 0x02000743 RID: 1859
	// (Invoke) Token: 0x060037C9 RID: 14281
	public delegate void ModEventHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x02000744 RID: 1860
	// (Invoke) Token: 0x060037CD RID: 14285
	public delegate ModEvents.EModEventResult ModEventInterruptibleHandlerDelegate<TData>(ref TData _data) where TData : struct;

	// Token: 0x02000745 RID: 1861
	public abstract class ModEventAbs<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x060037D0 RID: 14288 RVA: 0x0016EAC4 File Offset: 0x0016CCC4
		[PublicizedFrom(EAccessModifier.Protected)]
		public ModEventAbs(string _eventName)
		{
			this.eventName = _eventName;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0016EAE0 File Offset: 0x0016CCE0
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

		// Token: 0x060037D2 RID: 14290 RVA: 0x0016EB48 File Offset: 0x0016CD48
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

		// Token: 0x060037D3 RID: 14291 RVA: 0x0016EB9C File Offset: 0x0016CD9C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void LogError(Exception _e, ModEvents.ModEventAbs<TDelegate>.Receiver _currentMod)
		{
			Log.Error(string.Concat(new string[] { "[MODS] Error while executing ModEvent \"", this.eventName, "\" on mod \"", _currentMod.ModName, "\"" }));
			Log.Exception(_e);
		}

		// Token: 0x04002D91 RID: 11665
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string eventName;

		// Token: 0x04002D92 RID: 11666
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly List<ModEvents.ModEventAbs<TDelegate>.Receiver> receivers = new List<ModEvents.ModEventAbs<TDelegate>.Receiver>();

		// Token: 0x02000746 RID: 1862
		[PublicizedFrom(EAccessModifier.Protected)]
		public class Receiver
		{
			// Token: 0x170005A2 RID: 1442
			// (get) Token: 0x060037D4 RID: 14292 RVA: 0x0016EBE9 File Offset: 0x0016CDE9
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

			// Token: 0x060037D5 RID: 14293 RVA: 0x0016EC12 File Offset: 0x0016CE12
			public Receiver(Mod _mod, TDelegate _handler, bool _coreGame = false)
			{
				this.Mod = _mod;
				this.DelegateFunc = _handler;
				this.coreGame = _coreGame;
			}

			// Token: 0x04002D93 RID: 11667
			public readonly Mod Mod;

			// Token: 0x04002D94 RID: 11668
			public readonly TDelegate DelegateFunc;

			// Token: 0x04002D95 RID: 11669
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly bool coreGame;
		}
	}

	// Token: 0x02000747 RID: 1863
	public class ModEvent<TData> : ModEvents.ModEventAbs<ModEvents.ModEventHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x060037D6 RID: 14294 RVA: 0x0016EC2F File Offset: 0x0016CE2F
		public ModEvent([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0016EC38 File Offset: 0x0016CE38
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

	// Token: 0x02000748 RID: 1864
	public class ModEventInterruptible<TData> : ModEvents.ModEventAbs<ModEvents.ModEventInterruptibleHandlerDelegate<TData>> where TData : struct
	{
		// Token: 0x060037D8 RID: 14296 RVA: 0x0016ECAC File Offset: 0x0016CEAC
		public ModEventInterruptible([CallerMemberName] string _eventName = null)
			: base(_eventName)
		{
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x0016ECB8 File Offset: 0x0016CEB8
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
