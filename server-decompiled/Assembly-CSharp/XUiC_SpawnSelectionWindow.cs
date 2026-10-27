using System;
using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200109C RID: 4252
[Preserve]
public class XUiC_SpawnSelectionWindow : XUiController
{
	// Token: 0x060086A1 RID: 34465 RVA: 0x003375D9 File Offset: 0x003357D9
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

	// Token: 0x060086A2 RID: 34466 RVA: 0x0033760C File Offset: 0x0033580C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "buttonsVisible")
		{
			_value = (this.bChooseSpawnPosition || this.bEnteringGame).ToString();
			return true;
		}
		if (_bindingName == "enteringGame")
		{
			_value = this.bEnteringGame.ToString();
			return true;
		}
		if (_bindingName == "firstTimeSpawn")
		{
			_value = this.bFirstTimeSpawn.ToString();
			return true;
		}
		if (_bindingName == "showNearFriends")
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
		if (_bindingName == "hasBackpack")
		{
			GameServerInfo currentGameServerInfoServerOrClient = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient;
			bool flag = currentGameServerInfoServerOrClient == null || currentGameServerInfoServerOrClient.GetValue(GameInfoBool.AllowSpawnNearBackpack);
			_value = (flag && this.bHasBackpack).ToString();
			return true;
		}
		if (!(_bindingName == "hasBedroll"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.bHasBedroll.ToString();
		return true;
	}

	// Token: 0x060086A3 RID: 34467 RVA: 0x00337734 File Offset: 0x00335934
	public override void Init()
	{
		base.Init();
		XUiC_SpawnSelectionWindow.ID = base.WindowGroup.Id;
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

	// Token: 0x060086A4 RID: 34468 RVA: 0x0033788C File Offset: 0x00335A8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshButtons()
	{
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

	// Token: 0x060086A5 RID: 34469 RVA: 0x00337920 File Offset: 0x00335B20
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnButtonPressed(SpawnMethod _method, int _nearEntityId = -1)
	{
		this.xui.playerUI.xui.PlayMenuConfirmSound();
		this.spawnMethod = _method;
		if (this.bEnteringGame)
		{
			this.xui.playerUI.CursorController.SetNavigationTarget(null);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				GameManager.Instance.canSpawnPlayer = true;
			}
			else
			{
				GameManager.Instance.RequestToSpawn(_nearEntityId);
				if (_method == SpawnMethod.NearFriend)
				{
					ThreadManager.StartCoroutine(this.sendFriendRequest(_nearEntityId));
				}
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
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, Localization.Get("uiLoadCreatingPlayer", false, null), null, true, true, false);
	}

	// Token: 0x060086A6 RID: 34470 RVA: 0x00337ADE File Offset: 0x00335CDE
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

	// Token: 0x060086A7 RID: 34471 RVA: 0x00337AF0 File Offset: 0x00335CF0
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
		if (GameStats.GetInt(EnumGameStats.GameState) == 2)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			return;
		}
	}

	// Token: 0x060086A8 RID: 34472 RVA: 0x00337B88 File Offset: 0x00335D88
	public override void OnOpen()
	{
		base.OnOpen();
		this.delayCountdownTime = 1f;
		this.spawnMethod = SpawnMethod.Invalid;
		this.showSpawningComponents();
		this.RefreshButtons();
		base.RefreshBindings();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
	}

	// Token: 0x060086A9 RID: 34473 RVA: 0x00337BD4 File Offset: 0x00335DD4
	public override void OnClose()
	{
		base.OnClose();
		this.bChooseSpawnPosition = false;
	}

	// Token: 0x060086AA RID: 34474 RVA: 0x00337BE3 File Offset: 0x00335DE3
	[PublicizedFrom(EAccessModifier.Private)]
	public void showSpawningComponents()
	{
		GameManager.Instance.SetCursorEnabledOverride(false, false);
		if (AutomationRunner.Instance.IsRunning)
		{
			this.SpawnButtonPressed(SpawnMethod.Invalid, -1);
			return;
		}
		Manager.PlayXUiSound(this.spawnReadyClip, 1f);
	}

	// Token: 0x060086AB RID: 34475 RVA: 0x00337C16 File Offset: 0x00335E16
	public static void Open(LocalPlayerUI _playerUi, bool _chooseSpawnPosition, bool _enteringGame, bool _firstTimeSpawn = false)
	{
		XUiC_SpawnSelectionWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SpawnSelectionWindow.ID).GetChildByType<XUiC_SpawnSelectionWindow>();
		childByType.bChooseSpawnPosition = _chooseSpawnPosition;
		childByType.bEnteringGame = _enteringGame;
		childByType.bFirstTimeSpawn = _firstTimeSpawn;
		_playerUi.windowManager.Open(XUiC_SpawnSelectionWindow.ID, true, true);
	}

	// Token: 0x060086AC RID: 34476 RVA: 0x00337C53 File Offset: 0x00335E53
	public static void Close(LocalPlayerUI _playerUi)
	{
		_playerUi.windowManager.Close(XUiC_SpawnSelectionWindow.ID);
	}

	// Token: 0x060086AD RID: 34477 RVA: 0x00337C65 File Offset: 0x00335E65
	public static bool IsOpenInUI(LocalPlayerUI _playerUi)
	{
		return _playerUi.windowManager.IsWindowOpen(XUiC_SpawnSelectionWindow.ID);
	}

	// Token: 0x060086AE RID: 34478 RVA: 0x00337C77 File Offset: 0x00335E77
	public static XUiC_SpawnSelectionWindow GetWindow(LocalPlayerUI _playerUi)
	{
		return _playerUi.xui.FindWindowGroupByName(XUiC_SpawnSelectionWindow.ID).GetChildByType<XUiC_SpawnSelectionWindow>();
	}

	// Token: 0x04006418 RID: 25624
	public static string ID = "";

	// Token: 0x04006419 RID: 25625
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bChooseSpawnPosition;

	// Token: 0x0400641A RID: 25626
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEnteringGame;

	// Token: 0x0400641B RID: 25627
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFirstTimeSpawn;

	// Token: 0x0400641C RID: 25628
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHasBedroll;

	// Token: 0x0400641D RID: 25629
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHasBackpack;

	// Token: 0x0400641E RID: 25630
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayCountdownTime;

	// Token: 0x0400641F RID: 25631
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip spawnReadyClip;

	// Token: 0x04006420 RID: 25632
	public SpawnMethod spawnMethod;

	// Token: 0x04006421 RID: 25633
	public SpawnPosition spawnTarget;

	// Token: 0x04006422 RID: 25634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool setCursor;
}
