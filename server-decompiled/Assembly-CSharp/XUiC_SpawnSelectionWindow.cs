using System;
using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200107B RID: 4219
[Preserve]
public class XUiC_SpawnSelectionWindow : XUiController
{
	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x06008608 RID: 34312 RVA: 0x00331949 File Offset: 0x0032FB49
	// (set) Token: 0x06008609 RID: 34313 RVA: 0x00331951 File Offset: 0x0032FB51
	public XUiC_SpawnSelectionWindow.ESpawnWindowMode WindowMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.windowMode;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.windowMode != value)
			{
				this.windowMode = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x0600860A RID: 34314 RVA: 0x0033196A File Offset: 0x0032FB6A
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "spawn_sound")
		{
			this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _clip)
			{
				this.spawnReadyClip = _clip;
			});
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0600860B RID: 34315 RVA: 0x0033199C File Offset: 0x0032FB9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 3003985336U)
		{
			if (num != 79689725U)
			{
				if (num != 848152551U)
				{
					if (num == 3003985336U)
					{
						if (_bindingName == "progressVisible")
						{
							_value = (this.WindowMode == XUiC_SpawnSelectionWindow.ESpawnWindowMode.Progress).ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "firstTimeSpawn")
				{
					_value = this.bFirstTimeSpawn.ToString();
					return true;
				}
			}
			else if (_bindingName == "hasBedroll")
			{
				_value = this.bHasBedroll.ToString();
				return true;
			}
		}
		else if (num <= 3221349377U)
		{
			if (num != 3073415995U)
			{
				if (num == 3221349377U)
				{
					if (_bindingName == "enteringGame")
					{
						_value = this.bEnteringGame.ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "showNearFriends")
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					_value = false.ToString();
				}
				else if (XUiC_SpawnNearFriendsList.SpawnNearFriendMode == AllowSpawnNearFriend.Disabled)
				{
					_value = false.ToString();
				}
				else
				{
					_value = this.bFirstTimeSpawn.ToString();
				}
				return true;
			}
		}
		else if (num != 4053612733U)
		{
			if (num == 4166775256U)
			{
				if (_bindingName == "buttonsVisible")
				{
					_value = (this.WindowMode == XUiC_SpawnSelectionWindow.ESpawnWindowMode.SpawnSelection).ToString();
					return true;
				}
			}
		}
		else if (_bindingName == "hasBackpack")
		{
			GameServerInfo currentGameServerInfoServerOrClient = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient;
			bool flag = currentGameServerInfoServerOrClient == null || currentGameServerInfoServerOrClient.GetValue(GameInfoBool.AllowSpawnNearBackpack);
			_value = (flag && this.bHasBackpack).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x0600860C RID: 34316 RVA: 0x00331B60 File Offset: 0x0032FD60
	public override void Init()
	{
		base.Init();
		XUiC_SpawnSelectionWindow.ID = base.WindowGroup.Id;
		this.lblProgress = (XUiV_Label)base.GetChildById("lblProgress").ViewComponent;
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnLeave") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += delegate(XUiController _, int _)
			{
				this.xui.playerUI.windowManager.Close(this.windowGroup, false);
				GameManager.Instance.Disconnect();
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnSpawnFirstTime") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += delegate(XUiController _, int _)
			{
				this.SpawnButtonPressed(SpawnMethod.Invalid, -1);
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton3 = base.GetChildById("btnSpawn") as XUiC_SimpleButton;
		if (xuiC_SimpleButton3 != null)
		{
			xuiC_SimpleButton3.OnPressed += delegate(XUiController _, int _)
			{
				this.SpawnButtonPressed(SpawnMethod.Invalid, -1);
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton4 = base.GetChildById("btnRespawn") as XUiC_SimpleButton;
		if (xuiC_SimpleButton4 != null)
		{
			xuiC_SimpleButton4.OnPressed += delegate(XUiController _, int _)
			{
				if (GameStats.GetInt(EnumGameStats.DeathPenalty) == 3)
				{
					this.SpawnButtonPressed(SpawnMethod.NewRandomSpawn, -1);
					return;
				}
				this.SpawnButtonPressed(SpawnMethod.NearDeath, -1);
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton5 = base.GetChildById("btnNearBackpack") as XUiC_SimpleButton;
		if (xuiC_SimpleButton5 != null)
		{
			xuiC_SimpleButton5.OnPressed += delegate(XUiController _, int _)
			{
				this.SpawnButtonPressed(SpawnMethod.NearBackpack, -1);
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton6 = base.GetChildById("btnOnBedroll") as XUiC_SimpleButton;
		if (xuiC_SimpleButton6 != null)
		{
			xuiC_SimpleButton6.OnPressed += delegate(XUiController _, int _)
			{
				this.SpawnButtonPressed(SpawnMethod.OnBedRoll, -1);
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton7 = base.GetChildById("btnNearBedroll") as XUiC_SimpleButton;
		if (xuiC_SimpleButton7 != null)
		{
			xuiC_SimpleButton7.OnPressed += delegate(XUiController _, int _)
			{
				this.SpawnButtonPressed(SpawnMethod.NearBedroll, -1);
			};
		}
		XUiC_SpawnNearFriendsList childByType = base.GetChildByType<XUiC_SpawnNearFriendsList>();
		if (childByType != null)
		{
			childByType.SpawnClicked += delegate(PersistentPlayerData _ppd)
			{
				this.SpawnButtonPressed(SpawnMethod.NearFriend, _ppd.EntityId);
			};
		}
	}

	// Token: 0x0600860D RID: 34317 RVA: 0x00331CD0 File Offset: 0x0032FED0
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshButtons()
	{
		if (this.WindowMode == XUiC_SpawnSelectionWindow.ESpawnWindowMode.Progress && !this.bEnteringGame)
		{
			return;
		}
		if (this.bEnteringGame)
		{
			return;
		}
		World world = GameManager.Instance.World;
		EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		if (entityPlayerLocal == null)
		{
			Log.Warning("Refresh buttons cannot process without an EntityPlayerLocal");
			return;
		}
		SpawnPosition spawnPoint;
		Vector3i vector3i;
		if (entityPlayerLocal != null)
		{
			spawnPoint = entityPlayerLocal.GetSpawnPoint();
			vector3i = entityPlayerLocal.GetLastDroppedBackpackPosition();
		}
		else
		{
			spawnPoint = new SpawnPosition(Vector3.forward, 0f);
			vector3i = Vector3i.down;
		}
		this.bHasBedroll = !spawnPoint.IsUndef();
		this.bHasBackpack = vector3i != Vector3i.zero;
	}

	// Token: 0x0600860E RID: 34318 RVA: 0x00331D74 File Offset: 0x0032FF74
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnButtonPressed(SpawnMethod _method, int _nearEntityId = -1)
	{
		this.xui.playerUI.xui.PlayMenuConfirmSound();
		this.spawnMethod = _method;
		if (this.bEnteringGame)
		{
			this.WindowMode = XUiC_SpawnSelectionWindow.ESpawnWindowMode.Progress;
			this.xui.playerUI.CursorController.SetNavigationTarget(null);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				GameManager.Instance.canSpawnPlayer = true;
				return;
			}
			GameManager.Instance.RequestToSpawn(_nearEntityId);
			if (_method == SpawnMethod.NearFriend)
			{
				ThreadManager.StartCoroutine(this.sendFriendRequest(_nearEntityId));
				return;
			}
		}
		else
		{
			World world = GameManager.Instance.World;
			EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
			if (entityPlayerLocal == null)
			{
				Log.Warning("Spawn button cannot process without an EntityPlayerLocal");
				return;
			}
			SpawnPosition spawnPoint;
			Vector3i vector3i;
			Vector3 vector;
			if (entityPlayerLocal != null)
			{
				spawnPoint = entityPlayerLocal.GetSpawnPoint();
				vector3i = entityPlayerLocal.GetLastDroppedBackpackPosition();
				vector = entityPlayerLocal.position;
			}
			else
			{
				spawnPoint = new SpawnPosition(Vector3.forward, 0f);
				vector3i = Vector3i.down;
				vector = Vector3.down;
			}
			SpawnPosition spawnPosition = new SpawnPosition(vector3i, 0f);
			SpawnPosition spawnPosition2 = new SpawnPosition(vector, 0f);
			SpawnPosition randomSpawnPosition = GameManager.Instance.GetSpawnPointList().GetRandomSpawnPosition(entityPlayerLocal.world, null, 0, 0);
			SpawnPosition spawnPosition3;
			switch (this.spawnMethod)
			{
			case SpawnMethod.Invalid:
				spawnPosition3 = SpawnPosition.Undef;
				break;
			case SpawnMethod.NewRandomSpawn:
				spawnPosition3 = randomSpawnPosition;
				break;
			case SpawnMethod.NearDeath:
				spawnPosition3 = spawnPosition2;
				break;
			case SpawnMethod.OnBedRoll:
				spawnPosition3 = spawnPoint;
				break;
			case SpawnMethod.NearBedroll:
				spawnPosition3 = spawnPoint;
				break;
			case SpawnMethod.NearBackpack:
				spawnPosition3 = spawnPosition;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.spawnTarget = spawnPosition3;
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x0600860F RID: 34319 RVA: 0x00331F17 File Offset: 0x00330117
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator sendFriendRequest(int _entityId)
	{
		if (_entityId == -1)
		{
			yield break;
		}
		while (GameManager.Instance.World != null)
		{
			EntityPlayerLocal epl = GameManager.Instance.World.GetPrimaryPlayer();
			yield return null;
			if (!(epl == null) && epl.Spawned)
			{
				EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(_entityId) as EntityPlayer;
				if (entityPlayer != null)
				{
					if (!entityPlayer.partyInvites.Contains(epl))
					{
						entityPlayer.AddPartyInvite(epl.entityId);
					}
					NetPackagePartyActions netPackagePartyActions = NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SendInvite, epl.entityId, _entityId, null, null);
					if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackagePartyActions, false);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackagePartyActions, false, -1, -1, -1, null, 192, false);
					}
				}
				epl.PlayerUI.xui.GetChildByType<XUiC_PlayersList>().AddInvitePress(_entityId);
				yield break;
			}
		}
		yield break;
	}

	// Token: 0x06008610 RID: 34320 RVA: 0x00331F28 File Offset: 0x00330128
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.RefreshButtons();
			base.RefreshBindings();
			if (this.xui.playerUI.CursorController.navigationTarget == null && !this.xui.playerUI.CursorController.CursorModeActive)
			{
				base.GetChildById("content").SelectCursorElement(true, false);
			}
			this.IsDirty = false;
		}
		this.updateLoadState();
	}

	// Token: 0x06008611 RID: 34321 RVA: 0x00331FA0 File Offset: 0x003301A0
	public override void OnOpen()
	{
		base.OnOpen();
		this.delayCountdownTime = 1f;
		this.spawnMethod = SpawnMethod.Invalid;
		this.lblProgress.Text = Localization.Get("msgBuildingEnvironment", false, null);
		this.showSpawningComponents(XUiC_SpawnSelectionWindow.ESpawnWindowMode.Progress);
		base.RefreshBindings();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
		this.setCursor = false;
		this.xui.playerUI.CursorController.SetCursorHidden(true);
	}

	// Token: 0x06008612 RID: 34322 RVA: 0x0033201B File Offset: 0x0033021B
	public override void OnClose()
	{
		base.OnClose();
		this.bChooseSpawnPosition = false;
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		this.setCursor = false;
	}

	// Token: 0x06008613 RID: 34323 RVA: 0x00332048 File Offset: 0x00330248
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateLoadState()
	{
		if (!GameManager.Instance.gameStateManager.IsGameStarted())
		{
			if (this.bEnteringGame)
			{
				this.showSpawningComponents(XUiC_SpawnSelectionWindow.ESpawnWindowMode.SpawnSelection);
			}
			return;
		}
		if (GameStats.GetInt(EnumGameStats.GameState) == 2)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			return;
		}
		if (this.delayCountdownTime > 0f)
		{
			this.delayCountdownTime -= Time.deltaTime;
			return;
		}
		int displayedChunkGameObjectsCount = GameManager.Instance.World.m_ChunkManager.GetDisplayedChunkGameObjectsCount();
		int viewDistance = GameUtils.GetViewDistance();
		int num = ((!GameManager.Instance.World.ChunkCache.IsFixedSize) ? (viewDistance * viewDistance - 10) : 0);
		if (displayedChunkGameObjectsCount < num)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.lblProgress.Text = Localization.Get("msgStartingGame", false, null);
				return;
			}
		}
		else if (DistantTerrain.Instance != null && !DistantTerrain.Instance.IsTerrainReady)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				this.lblProgress.Text = Localization.Get("msgGeneratingDistantTerrain", false, null);
				return;
			}
		}
		else
		{
			if (!LocalPlayerUI.GetUIForPrimaryPlayer().xui.IsReady)
			{
				this.lblProgress.Text = Localization.Get("msgLoadingUI", false, null);
				return;
			}
			if (this.bChooseSpawnPosition && GameManager.Instance.World.GetPrimaryPlayer() != null)
			{
				this.showSpawningComponents(XUiC_SpawnSelectionWindow.ESpawnWindowMode.SpawnSelection);
				return;
			}
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x06008614 RID: 34324 RVA: 0x003321C8 File Offset: 0x003303C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void showSpawningComponents(XUiC_SpawnSelectionWindow.ESpawnWindowMode _windowMode)
	{
		GameManager.Instance.SetCursorEnabledOverride(false, false);
		if (_windowMode == XUiC_SpawnSelectionWindow.ESpawnWindowMode.SpawnSelection && AutomationRunner.Instance.IsRunning)
		{
			this.SpawnButtonPressed(SpawnMethod.Invalid, -1);
			return;
		}
		if (!this.setCursor)
		{
			this.WindowMode = _windowMode;
			this.xui.playerUI.CursorController.SetCursorHidden(false);
			this.setCursor = true;
			if (this.WindowMode == XUiC_SpawnSelectionWindow.ESpawnWindowMode.SpawnSelection)
			{
				Manager.PlayXUiSound(this.spawnReadyClip, 1f);
			}
		}
	}

	// Token: 0x06008615 RID: 34325 RVA: 0x0033223F File Offset: 0x0033043F
	public static void Open(LocalPlayerUI _playerUi, bool _chooseSpawnPosition, bool _enteringGame, bool _firstTimeSpawn = false)
	{
		XUiC_SpawnSelectionWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SpawnSelectionWindow.ID).GetChildByType<XUiC_SpawnSelectionWindow>();
		childByType.bChooseSpawnPosition = _chooseSpawnPosition;
		childByType.bEnteringGame = _enteringGame;
		childByType.bFirstTimeSpawn = _firstTimeSpawn;
		_playerUi.windowManager.Open(XUiC_SpawnSelectionWindow.ID, true, true);
	}

	// Token: 0x06008616 RID: 34326 RVA: 0x0033227C File Offset: 0x0033047C
	public static void Close(LocalPlayerUI _playerUi)
	{
		_playerUi.windowManager.Close(XUiC_SpawnSelectionWindow.ID);
	}

	// Token: 0x06008617 RID: 34327 RVA: 0x0033228E File Offset: 0x0033048E
	public static bool IsOpenInUI(LocalPlayerUI _playerUi)
	{
		return _playerUi.windowManager.IsWindowOpen(XUiC_SpawnSelectionWindow.ID);
	}

	// Token: 0x06008618 RID: 34328 RVA: 0x003322A0 File Offset: 0x003304A0
	public static bool IsInSpawnSelection(LocalPlayerUI _playerUi)
	{
		return XUiC_SpawnSelectionWindow.IsOpenInUI(_playerUi) && XUiC_SpawnSelectionWindow.GetWindow(_playerUi).WindowMode == XUiC_SpawnSelectionWindow.ESpawnWindowMode.SpawnSelection;
	}

	// Token: 0x06008619 RID: 34329 RVA: 0x003322BA File Offset: 0x003304BA
	public static XUiC_SpawnSelectionWindow GetWindow(LocalPlayerUI _playerUi)
	{
		return _playerUi.xui.FindWindowGroupByName(XUiC_SpawnSelectionWindow.ID).GetChildByType<XUiC_SpawnSelectionWindow>();
	}

	// Token: 0x04006394 RID: 25492
	public static string ID = "";

	// Token: 0x04006395 RID: 25493
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnSelectionWindow.ESpawnWindowMode windowMode;

	// Token: 0x04006396 RID: 25494
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblProgress;

	// Token: 0x04006397 RID: 25495
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bChooseSpawnPosition;

	// Token: 0x04006398 RID: 25496
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEnteringGame;

	// Token: 0x04006399 RID: 25497
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFirstTimeSpawn;

	// Token: 0x0400639A RID: 25498
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHasBedroll;

	// Token: 0x0400639B RID: 25499
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHasBackpack;

	// Token: 0x0400639C RID: 25500
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayCountdownTime;

	// Token: 0x0400639D RID: 25501
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip spawnReadyClip;

	// Token: 0x0400639E RID: 25502
	public SpawnMethod spawnMethod;

	// Token: 0x0400639F RID: 25503
	public SpawnPosition spawnTarget;

	// Token: 0x040063A0 RID: 25504
	[PublicizedFrom(EAccessModifier.Private)]
	public bool setCursor;

	// Token: 0x0200107C RID: 4220
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ESpawnWindowMode
	{
		// Token: 0x040063A2 RID: 25506
		Progress,
		// Token: 0x040063A3 RID: 25507
		SpawnSelection
	}
}
