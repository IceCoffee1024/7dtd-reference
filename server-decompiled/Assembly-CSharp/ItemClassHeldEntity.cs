using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005FA RID: 1530
public class ItemClassHeldEntity : ItemClass
{
	// Token: 0x060031E5 RID: 12773 RVA: 0x001442B0 File Offset: 0x001424B0
	public override void Init()
	{
		base.Init();
		this.Properties.ParseFloat(ItemClassHeldEntity.PropFreakoutTime, ref this.FreakoutTime);
		this.Properties.ParseInt(ItemClassHeldEntity.PropStressMaxLevel, ref this.StressMaxLevel);
		this.Properties.ParseFloat(ItemClassHeldEntity.PropSneakModifier, ref this.SneakModifier);
		this.Properties.ParseFloat(ItemClassHeldEntity.PropRunModifier, ref this.RunModifier);
		this.Properties.ParseFloat(ItemClassHeldEntity.PropDecayModifier, ref this.DecayModifier);
		this.Properties.ParseFloat(ItemClassHeldEntity.PropFullStressDropStress, ref this.FullStressDropStress);
		this.Properties.ParseString(ItemClassHeldEntity.PropPickupPlayerBuff, ref this.PickupPlayerBuff);
		this.Properties.ParseString(ItemClassHeldEntity.PropPickedUpEntityName, ref this.PickedUpEntityName);
		this.Properties.ParseString(ItemClassHeldEntity.PropPickedUpAltEntityName, ref this.PickedUpAltEntityName);
		this.Properties.ParseString(ItemClassHeldEntity.PropPickupSound, ref this.PickupSound);
		this.Properties.ParseString(ItemClassHeldEntity.PropDropSound, ref this.DropSound);
		this.Properties.ParseString(ItemClassHeldEntity.PropDropSoundMaxStress, ref this.DropSoundMaxStress);
		this.Properties.ParseString(ItemClassHeldEntity.PropStressSound, ref this.StressSound);
		this.Properties.ParseString(ItemClassHeldEntity.PropMaxStressSound, ref this.MaxStressSound);
		this.Properties.ParseString(ItemClassHeldEntity.PropDropEntityBuff, ref this.DropEntityBuff);
		this.Properties.ParseString(ItemClassHeldEntity.PropFullStressEvent, ref this.FullStressEvent);
	}

	// Token: 0x060031E6 RID: 12774 RVA: 0x00144423 File Offset: 0x00142623
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isMaxStress()
	{
		return GameManager.Instance.World.GetPrimaryPlayer().GetCVar(ItemClassHeldEntity.CVarEntityStress) >= 100f;
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x00144448 File Offset: 0x00142648
	public void PlayStressSound()
	{
		if (this.isMaxStress())
		{
			Manager.PlayInsidePlayerHead(this.MaxStressSound, -1, 0f, false, false);
			return;
		}
		Manager.PlayInsidePlayerHead(this.StressSound, -1, 0f, false, false);
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x0014447C File Offset: 0x0014267C
	public override void OnPlacedAsCatalyst(ItemInventoryData _data)
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		primaryPlayer.SetCVar(this.CVarLastHeldEntitySlot, -1f);
		this.removeBuff(primaryPlayer);
		primaryPlayer.MountEvent.RemoveListener(new UnityAction<bool>(this.handleMountEvent));
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x001444C8 File Offset: 0x001426C8
	public override bool CanMoveToSlot(XUiC_ItemStack.StackLocationTypes locationType, int slot)
	{
		bool flag = true;
		if (locationType == XUiC_ItemStack.StackLocationTypes.ToolBelt)
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				flag = flag && primaryPlayer.inventory.holdingItemIdx == slot;
			}
		}
		return flag;
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x0014450C File Offset: 0x0014270C
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal getLocalPlayer(ItemInventoryData _data)
	{
		EntityPlayerLocal entityPlayerLocal = GameManager.Instance.World.GetPrimaryPlayer();
		if (entityPlayerLocal != null && entityPlayerLocal != _data.holdingEntity)
		{
			entityPlayerLocal = null;
		}
		return entityPlayerLocal;
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x00144544 File Offset: 0x00142744
	public override void OnHoldingUpdate(ItemInventoryData _data)
	{
		base.OnHoldingUpdate(_data);
		EntityPlayerLocal localPlayer = this.getLocalPlayer(_data);
		if (localPlayer != null)
		{
			this.updateTimer(localPlayer, 50f, false);
		}
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x00144578 File Offset: 0x00142778
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool HandleFullStress(EntityPlayerLocal epl)
	{
		if (this.FullStressEvent != "")
		{
			GameEventManager.Current.HandleAction(this.FullStressEvent, null, epl, false, "", "", false, true, "", null);
		}
		return false;
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x001445C0 File Offset: 0x001427C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateTimer(EntityPlayerLocal epl, float deltaTime, bool ignoreMove)
	{
		float num = epl.GetCVar(this.PickupStressTimer);
		int num2 = (int)(num / this.FreakoutTime);
		bool flag = epl.moveDirection.magnitude > 0.001f || (!epl.onGround && !epl.IsOnLadder());
		float num3 = 10f * this.FreakoutTime;
		if (flag && !ignoreMove)
		{
			if (epl.MovementRunning)
			{
				deltaTime *= this.RunModifier;
			}
			else if (epl.Crouching)
			{
				deltaTime *= this.SneakModifier;
			}
			num += deltaTime;
		}
		else
		{
			num = Utils.FastMin(num, num3 + 500f);
			num -= deltaTime * this.DecayModifier;
		}
		num = Mathf.Max(0f, num);
		float time = Time.time;
		int num4 = (int)(num / this.FreakoutTime);
		if (num2 < num4)
		{
			num4 = Mathf.Min(num4, 10);
			if (num4 == 10 && this.HandleFullStress(epl))
			{
				epl.SetCVar(this.PickupStressTimer, this.FullStressDropStress);
				Inventory inventory = epl.inventory;
				inventory.SetItem(inventory.holdingItemIdx, ItemStack.Empty);
				this.DropEntity(true);
				this.removeBuff(epl);
			}
			num = Mathf.Max(0f, Mathf.Min(num, num3));
			epl.emodel.avatarController.StartAnimationUse();
			Transform rightHandTransform = epl.emodel.GetRightHandTransform();
			if (rightHandTransform != null)
			{
				ParticleSystem[] componentsInChildren = rightHandTransform.GetComponentsInChildren<ParticleSystem>();
				if (componentsInChildren != null)
				{
					ParticleSystem[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Play();
					}
				}
			}
		}
		epl.SetCVar(this.PickupStressTimer, num);
		epl.SetCVar(ItemClassHeldEntity.CVarEntityStress, (float)((int)((float)num4 * 100f / 10f)));
		float num5 = num % this.FreakoutTime / this.FreakoutTime;
		float num6 = 1f - num5;
		Color color = Color.red * num5 + Color.green * num6;
		if (num >= num3)
		{
			float num7 = Mathf.PingPong(time, 0.5f);
			LocalPlayerUI.GetUIForPrimaryPlayer().xui.BuffPopoutList.SetBuffBarLengthAndColor(this.PickupPlayerBuff, 1f, Color.Lerp(Color.red, this.darkerRed, num7 * 2f));
		}
		else
		{
			LocalPlayerUI.GetUIForPrimaryPlayer().xui.BuffPopoutList.SetBuffBarLengthAndColor(this.PickupPlayerBuff, num5, color);
		}
		epl.emodel.avatarController.SetStress(num / num3);
		epl.emodel.avatarController.GetRightHandTransform();
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x00144828 File Offset: 0x00142A28
	public override void StartHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		base.StartHolding(_data, _modelTransform);
		EntityPlayerLocal localPlayer = this.getLocalPlayer(_data);
		if (localPlayer != null)
		{
			Manager.PlayInsidePlayerHead(this.PickupSound, -1, 0f, false, false);
			localPlayer.SetCVar(this.CVarLastHeldEntitySlot, (float)_data.slotIdx);
			localPlayer.MountEvent.AddListener(new UnityAction<bool>(this.handleMountEvent));
			if (!localPlayer.Buffs.HasCustomVar(ItemClassHeldEntity.CVarEntityStress))
			{
				localPlayer.SetCVar(ItemClassHeldEntity.CVarEntityStress, 0f);
			}
			if (!localPlayer.Buffs.HasCustomVar(ItemClassHeldEntity.CVarLastDroppedEntityId))
			{
				localPlayer.SetCVar(ItemClassHeldEntity.CVarLastDroppedEntityId, -1f);
			}
		}
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x001448D2 File Offset: 0x00142AD2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void addBuff(EntityPlayerLocal epl)
	{
		if (!this.hasBuff(epl))
		{
			epl.Buffs.AddBuff(this.PickupPlayerBuff, -1, true, false, -1f);
		}
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x001448F7 File Offset: 0x00142AF7
	[PublicizedFrom(EAccessModifier.Protected)]
	public void removeBuff(EntityPlayerLocal epl)
	{
		if (this.hasBuff(epl))
		{
			epl.Buffs.RemoveBuff(this.PickupPlayerBuff, -1, true);
		}
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x00144915 File Offset: 0x00142B15
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool hasBuff(EntityPlayerLocal epl)
	{
		return epl.Buffs.HasBuff(this.PickupPlayerBuff);
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x00144928 File Offset: 0x00142B28
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleMountEvent(bool mounted)
	{
		if (mounted)
		{
			Inventory inventory = GameManager.Instance.World.GetPrimaryPlayer().inventory;
			if (inventory.holdingItem == this)
			{
				this.DropEntity(false);
				inventory.SetItem(inventory.holdingItemIdx, ItemStack.Empty);
			}
		}
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x00144970 File Offset: 0x00142B70
	public void DropEntity(bool forceFullStress = false)
	{
		XUiC_DragAndDropWindow dragAndDropWindow = LocalPlayerUI.GetUIForPrimaryPlayer().xui.DragAndDropWindow;
		if (dragAndDropWindow.CurrentStack.itemValue.ItemClass != this)
		{
			dragAndDropWindow.CurrentStack.itemValue = ItemValue.None;
		}
		if (forceFullStress || this.isMaxStress())
		{
			Manager.PlayInsidePlayerHead(this.DropSoundMaxStress, -1, 0f, false, false);
		}
		else
		{
			Manager.PlayInsidePlayerHead(this.DropSound, -1, 0f, false, false);
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		primaryPlayer.SetCVar(this.CVarLastHeldEntitySlot, -1f);
		primaryPlayer.MountEvent.RemoveListener(new UnityAction<bool>(this.handleMountEvent));
		bool flag = primaryPlayer.GetCVar(this.CVarUseAltVersion) == 1f;
		primaryPlayer.SetCVar(this.CVarUseAltVersion, 0f);
		int id = EntityClass.GetId(flag ? this.PickedUpAltEntityName : this.PickedUpEntityName);
		Vector3 vector = primaryPlayer.position + primaryPlayer.transform.forward * 0.5f;
		vector.y += 0.5f;
		ItemClassHeldEntity.cleanupWaypoints(primaryPlayer);
		EntityCreationData entityCreationData = EntityFactory.SetupEntityCreationData(id, -1, vector, primaryPlayer.rotation);
		entityCreationData.stressAmount = primaryPlayer.GetCVar(this.PickupStressTimer);
		primaryPlayer.SetCVar(ItemClassHeldEntity.CVarLastDroppedEntityId, (float)entityCreationData.id);
		primaryPlayer.Buffs.RemoveBuff(this.PickupPlayerBuff, -1, true);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			ItemClassHeldEntity.entitySpawned(GameManager.Instance.SpawnEntityServer(entityCreationData));
			return;
		}
		primaryPlayer.RequestToSpawnEntityServer(entityCreationData, new Action<Entity>(ItemClassHeldEntity.entitySpawned), true);
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x00144B04 File Offset: 0x00142D04
	[PublicizedFrom(EAccessModifier.Private)]
	public static void entitySpawned(Entity requestedEntity)
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			EntityAlive entityAlive = requestedEntity as EntityAlive;
			if (entityAlive != null)
			{
				primaryPlayer.MinEventContext.Other = entityAlive;
				primaryPlayer.FireEvent(MinEventTypes.onSelfDroppedOther, true);
				EntityAnimal entityAnimal = entityAlive as EntityAnimal;
				if (entityAnimal != null)
				{
					entityAnimal.SetDistressed(true, 2f, 5f, primaryPlayer.entityId);
				}
			}
		}
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x00144B70 File Offset: 0x00142D70
	[PublicizedFrom(EAccessModifier.Private)]
	public string getItemValueName(ItemValue itemValue)
	{
		string text = null;
		ItemClass itemClass = itemValue.ItemClass;
		if (itemClass != null)
		{
			text = itemClass.Name;
		}
		return text;
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x00144B94 File Offset: 0x00142D94
	public override void StopHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		base.StopHolding(_data, _modelTransform);
		EntityPlayerLocal localPlayer = this.getLocalPlayer(_data);
		if (localPlayer != null)
		{
			int num = (int)localPlayer.GetCVar(this.CVarLastHeldEntitySlot);
			if (num >= 0)
			{
				XUiC_DragAndDropWindow dragAndDropWindow = LocalPlayerUI.GetUIForPrimaryPlayer().xui.DragAndDropWindow;
				if (localPlayer.inventory.holdingItemIdx == num)
				{
					if (string.Compare(this.getItemValueName(dragAndDropWindow.CurrentStack.itemValue), this.GetItemName()) == 0 && dragAndDropWindow.LastStack.itemValue.ItemClass != null)
					{
						dragAndDropWindow.CurrentStack.itemValue = ItemValue.None;
						this.DropEntity(false);
						this.waitForBuff(localPlayer);
						return;
					}
				}
				else
				{
					this.DropEntity(false);
					localPlayer.inventory.SetItem(num, ItemStack.Empty);
					this.waitForBuff(localPlayer);
				}
			}
		}
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x00144C64 File Offset: 0x00142E64
	[PublicizedFrom(EAccessModifier.Private)]
	public static void cleanupWaypoints(EntityPlayerLocal epl)
	{
		List<Waypoint> list = new List<Waypoint>();
		foreach (Waypoint waypoint in epl.Waypoints.Collection.list)
		{
			if (waypoint.lastKnownPositionEntityType == eLastKnownPositionEntityType.Animal)
			{
				list.Add(waypoint);
			}
		}
		foreach (Waypoint waypoint2 in list)
		{
			epl.Waypoints.Collection.Remove(waypoint2);
			NavObjectManager.Instance.UnRegisterNavObject(waypoint2.navObject);
		}
		EntityAlive entityAlive = GameManager.Instance.World.GetEntity((int)epl.GetCVar(ItemClassHeldEntity.CVarLastDroppedEntityId)) as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.ClearDistressed();
		}
		epl.SetCVar(ItemClassHeldEntity.CVarLastDroppedEntityId, -1f);
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x00144D6C File Offset: 0x00142F6C
	public static void CheckSpawnValues(EntityPlayerLocal epl)
	{
		ItemClassHeldEntity.cleanupWaypoints(epl);
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x00144D6C File Offset: 0x00142F6C
	public static void CleanupOnPlayerRemove(EntityPlayerLocal epl)
	{
		ItemClassHeldEntity.cleanupWaypoints(epl);
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x00144D74 File Offset: 0x00142F74
	[PublicizedFrom(EAccessModifier.Protected)]
	public void waitForBuff(EntityPlayerLocal epl)
	{
		if (epl != null && this.hasBuff(epl))
		{
			GameManager.Instance.StartCoroutine(this.waitForBuffCoroutine());
		}
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x00144D99 File Offset: 0x00142F99
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator waitForBuffCoroutine()
	{
		EntityPlayerLocal entityPlayerLocal2;
		for (;;)
		{
			GameManager instance = GameManager.Instance;
			EntityPlayerLocal entityPlayerLocal;
			if (instance == null)
			{
				entityPlayerLocal = null;
			}
			else
			{
				World world = instance.World;
				entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
			}
			entityPlayerLocal2 = entityPlayerLocal;
			if (entityPlayerLocal2 != null)
			{
				if (entityPlayerLocal2.GetCVar(this.CVarLastHeldEntitySlot) >= 0f)
				{
					goto IL_00A6;
				}
				this.updateTimer(entityPlayerLocal2, Time.deltaTime * 1000f, true);
				if (entityPlayerLocal2.GetCVar(this.PickupStressTimer) <= 0f)
				{
					break;
				}
			}
			yield return null;
		}
		this.removeBuff(entityPlayerLocal2);
		IL_00A6:
		yield break;
	}

	// Token: 0x04002762 RID: 10082
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxFreakLevel = 10;

	// Token: 0x04002763 RID: 10083
	[PublicizedFrom(EAccessModifier.Protected)]
	public float RunModifier = 3f;

	// Token: 0x04002764 RID: 10084
	[PublicizedFrom(EAccessModifier.Protected)]
	public float SneakModifier = -0.1f;

	// Token: 0x04002765 RID: 10085
	[PublicizedFrom(EAccessModifier.Protected)]
	public float DecayModifier = 1f;

	// Token: 0x04002766 RID: 10086
	[PublicizedFrom(EAccessModifier.Protected)]
	public float FreakoutTime = 5000f;

	// Token: 0x04002767 RID: 10087
	[PublicizedFrom(EAccessModifier.Protected)]
	public int StressMaxLevel = 10;

	// Token: 0x04002768 RID: 10088
	[PublicizedFrom(EAccessModifier.Protected)]
	public string PickupPlayerBuff;

	// Token: 0x04002769 RID: 10089
	[PublicizedFrom(EAccessModifier.Protected)]
	public string PickedUpEntityName;

	// Token: 0x0400276A RID: 10090
	[PublicizedFrom(EAccessModifier.Protected)]
	public string PickedUpAltEntityName;

	// Token: 0x0400276B RID: 10091
	[PublicizedFrom(EAccessModifier.Protected)]
	public string PickupSound = "chickenpickupdrop";

	// Token: 0x0400276C RID: 10092
	[PublicizedFrom(EAccessModifier.Protected)]
	public string DropSound = "chickenpickupdrop";

	// Token: 0x0400276D RID: 10093
	[PublicizedFrom(EAccessModifier.Protected)]
	public string DropSoundMaxStress = "chickenpickupdrop";

	// Token: 0x0400276E RID: 10094
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StressSound = "";

	// Token: 0x0400276F RID: 10095
	[PublicizedFrom(EAccessModifier.Protected)]
	public string MaxStressSound = "";

	// Token: 0x04002770 RID: 10096
	[PublicizedFrom(EAccessModifier.Protected)]
	public string PickupStressTimer = "stressTimer";

	// Token: 0x04002771 RID: 10097
	[PublicizedFrom(EAccessModifier.Protected)]
	public string DropEntityBuff;

	// Token: 0x04002772 RID: 10098
	[PublicizedFrom(EAccessModifier.Protected)]
	public string FullStressEvent = "";

	// Token: 0x04002773 RID: 10099
	[PublicizedFrom(EAccessModifier.Protected)]
	public float FullStressDropStress = 40000f;

	// Token: 0x04002774 RID: 10100
	public static string CVarEntityStress = "%stressAmount";

	// Token: 0x04002775 RID: 10101
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFreakoutTime = "FreakoutTime";

	// Token: 0x04002776 RID: 10102
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStressMaxLevel = "StressMaxLevel";

	// Token: 0x04002777 RID: 10103
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSneakModifier = "SneakModifier";

	// Token: 0x04002778 RID: 10104
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropRunModifier = "RunModifier";

	// Token: 0x04002779 RID: 10105
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDecayModifier = "DecayModifier";

	// Token: 0x0400277A RID: 10106
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropPickedUpEntityName = "PickedUpEntityName";

	// Token: 0x0400277B RID: 10107
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropPickedUpAltEntityName = "PickedUpAltEntityName";

	// Token: 0x0400277C RID: 10108
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropPickupStressCvar = "PickupStressCVar";

	// Token: 0x0400277D RID: 10109
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropPickupSound = "PickupSound";

	// Token: 0x0400277E RID: 10110
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDropSound = "DropSound";

	// Token: 0x0400277F RID: 10111
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDropSoundMaxStress = "DropSoundMaxStress";

	// Token: 0x04002780 RID: 10112
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStressSound = "StressSound";

	// Token: 0x04002781 RID: 10113
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropMaxStressSound = "MaxStressSound";

	// Token: 0x04002782 RID: 10114
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropPickupPlayerBuff = "PickupPlayerBuff";

	// Token: 0x04002783 RID: 10115
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDropEntityBuff = "DropEntityBuff";

	// Token: 0x04002784 RID: 10116
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFullStressEvent = "FullStressEvent";

	// Token: 0x04002785 RID: 10117
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFullStressDropStress = "FullStressDropStress";

	// Token: 0x04002786 RID: 10118
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropEntityStressCVar = "EntityStressCVar";

	// Token: 0x04002787 RID: 10119
	[PublicizedFrom(EAccessModifier.Protected)]
	public string CVarLastHeldEntitySlot = "_lastHeldEntitySlot";

	// Token: 0x04002788 RID: 10120
	[PublicizedFrom(EAccessModifier.Protected)]
	public string CVarIsFullStress = ".isFullStress";

	// Token: 0x04002789 RID: 10121
	[PublicizedFrom(EAccessModifier.Protected)]
	public string CVarUseAltVersion = ".UseAltEntity";

	// Token: 0x0400278A RID: 10122
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string CVarLastDroppedEntityId = "_lastDroppedEntityId";

	// Token: 0x0400278B RID: 10123
	[PublicizedFrom(EAccessModifier.Private)]
	public Color darkerRed = new Color32(100, 0, 0, byte.MaxValue);
}
