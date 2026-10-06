using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005D3 RID: 1491
[Preserve]
public class ItemActionSpawnTurret : ItemAction
{
	// Token: 0x060030BA RID: 12474 RVA: 0x0013968A File Offset: 0x0013788A
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnTurret.ItemActionDataSpawnTurret(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x00139694 File Offset: 0x00137894
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Turret"))
		{
			this.entityToSpawn = _props.Values["Turret"];
		}
		this.turretSize = new Vector3(0.5f, 0.5f, 0.5f);
		if (_props.Values.ContainsKey("Scale"))
		{
			this.turretSize = StringParsers.ParseVector3(_props.Values["Scale"], 0, -1);
		}
		this.previewSize = new Vector3(1f, 1f, 1f);
		if (_props.Values.ContainsKey("PreviewSize"))
		{
			this.previewSize = StringParsers.ParseVector3(_props.Values["PreviewSize"], 0, -1);
		}
		this.canPlaceInAir = false;
		if (_props.Values.ContainsKey("CanPlaceInAir"))
		{
			this.canPlaceInAir = StringParsers.ParseBool(_props.Values["CanPlaceInAir"], 0, -1, true);
		}
		foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
		{
			if (keyValuePair.Value.entityClassName == this.entityToSpawn)
			{
				this.entityClassId = keyValuePair.Key;
				break;
			}
		}
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x00139804 File Offset: 0x00137A04
	public override void StartHolding(ItemActionData _actionData)
	{
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = (ItemActionSpawnTurret.ItemActionDataSpawnTurret)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataSpawnTurret.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		if (itemActionDataSpawnTurret.TurretPreviewT != null)
		{
			global::UnityEngine.Object.DestroyImmediate(itemActionDataSpawnTurret.TurretPreviewT.gameObject);
		}
		GameObject gameObject = DataLoader.LoadAsset<GameObject>(entityPlayerLocal.inventory.holdingItem.MeshFile, false);
		itemActionDataSpawnTurret.TurretPreviewT = global::UnityEngine.Object.Instantiate<GameObject>(gameObject).transform;
		this.setupPreview(itemActionDataSpawnTurret);
		this.updatePreview(itemActionDataSpawnTurret);
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x00139888 File Offset: 0x00137A88
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupPreview(ItemActionSpawnTurret.ItemActionDataSpawnTurret data)
	{
		if (data.PreviewRenderers == null || data.PreviewRenderers.Length == 0 || data.PreviewRenderers[0] == null)
		{
			data.PreviewRenderers = data.TurretPreviewT.GetComponentsInChildren<Renderer>();
		}
		data.TurretPreviewT.localScale = this.previewSize;
		data.CanPlaceInAir = this.canPlaceInAir;
		data.TurretPreviewT.GetComponent<SphereCollider>().enabled = false;
		for (int i = 0; i < data.PreviewRenderers.Length; i++)
		{
			data.PreviewRenderers[i].material.color = new Color(2f, 0.25f, 0.25f);
		}
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x00139930 File Offset: 0x00137B30
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePreview(ItemActionSpawnTurret.ItemActionDataSpawnTurret data)
	{
		if (data.PreviewRenderers == null || data.PreviewRenderers.Length == 0 || data.PreviewRenderers[0] == null)
		{
			data.PreviewRenderers = data.TurretPreviewT.GetComponentsInChildren<Renderer>();
		}
		World world = data.invData.world;
		bool flag = this.CalcSpawnPosition(data, ref data.Position) && world.CanPlaceBlockAt(new Vector3i(data.Position), world.GetGameManager().GetPersistentLocalPlayer(), false);
		data.ValidPosition = flag;
		for (int i = 0; i < data.PreviewRenderers.Length; i++)
		{
			data.PreviewRenderers[i].material.color = (flag ? new Color(0.25f, 2f, 0.25f) : new Color(2f, 0.25f, 0.25f));
		}
		Quaternion localRotation = data.TurretPreviewT.localRotation;
		localRotation.eulerAngles = new Vector3(0f, data.invData.holdingEntity.rotation.y, 0f);
		data.TurretPreviewT.localRotation = localRotation;
		data.TurretPreviewT.position = data.Position - Origin.position;
		data.TurretPreviewT.gameObject.SetActive(data.Placing);
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x00139A78 File Offset: 0x00137C78
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = (ItemActionSpawnTurret.ItemActionDataSpawnTurret)_actionData;
		if (itemActionDataSpawnTurret.TurretPreviewT != null && itemActionDataSpawnTurret.invData.holdingEntity is EntityPlayerLocal)
		{
			global::UnityEngine.Object.Destroy(itemActionDataSpawnTurret.TurretPreviewT.gameObject);
		}
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x00139ABC File Offset: 0x00137CBC
	public override void StopHolding(ItemActionData _actionData)
	{
		base.StopHolding(_actionData);
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = (ItemActionSpawnTurret.ItemActionDataSpawnTurret)_actionData;
		if (itemActionDataSpawnTurret.TurretPreviewT != null && itemActionDataSpawnTurret.invData.holdingEntity is EntityPlayerLocal)
		{
			global::UnityEngine.Object.Destroy(itemActionDataSpawnTurret.TurretPreviewT.gameObject);
		}
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x00139B08 File Offset: 0x00137D08
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = (ItemActionSpawnTurret.ItemActionDataSpawnTurret)_actionData;
		if (itemActionDataSpawnTurret.invData.item.Actions[0] != null && itemActionDataSpawnTurret.invData.item.Actions[0].IsActionRunning(itemActionDataSpawnTurret.invData.actionData[0]))
		{
			itemActionDataSpawnTurret.Placing = false;
		}
		if (itemActionDataSpawnTurret.TurretPreviewT != null && itemActionDataSpawnTurret.invData.holdingEntity is EntityPlayerLocal)
		{
			this.updatePreview(itemActionDataSpawnTurret);
		}
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x00139B88 File Offset: 0x00137D88
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = (ItemActionSpawnTurret.ItemActionDataSpawnTurret)_actionData;
		if (!(_actionData.invData.holdingEntity is EntityPlayerLocal))
		{
			return;
		}
		if (!itemActionDataSpawnTurret.Placing)
		{
			if (_bReleased)
			{
				itemActionDataSpawnTurret.Placing = true;
			}
			return;
		}
		if (!itemActionDataSpawnTurret.Placing || !_bReleased)
		{
			return;
		}
		float time = Time.time;
		if (time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		if (time - _actionData.lastUseTime < 2f)
		{
			return;
		}
		if (!itemActionDataSpawnTurret.ValidPosition)
		{
			return;
		}
		ItemInventoryData invData = _actionData.invData;
		if (this.entityClassId < 0)
		{
			foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
			{
				if (keyValuePair.Value.entityClassName == this.entityToSpawn)
				{
					this.entityClassId = keyValuePair.Key;
					break;
				}
			}
			if (this.entityClassId == 0)
			{
				return;
			}
		}
		_actionData.lastUseTime = time;
		bool flag = false;
		bool flag2 = false;
		ItemClass item = itemActionDataSpawnTurret.invData.item;
		if (item.HasAnyTags(EntityClass.droneTag))
		{
			flag2 = true;
			if (DroneManager.CanAddMoreDrones() && EntityDrone.IsValidForLocalPlayer())
			{
				flag = true;
			}
		}
		else if ((item.HasAnyTags(EntityClass.turretRangedTag) || item.HasAnyTags(EntityClass.turretMeleeTag)) && TurretTracker.CanAddMoreTurrets())
		{
			flag = true;
		}
		if (flag)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTurretSpawn>().Setup(this.entityClassId, itemActionDataSpawnTurret.Position, new Vector3(0f, invData.holdingEntity.rotation.y, 0f), invData.holdingEntity.inventory.holdingItemItemValue.Clone(), invData.holdingEntity.entityId), true);
			}
			else
			{
				Entity entity = EntityFactory.CreateEntity(this.entityClassId, itemActionDataSpawnTurret.Position, new Vector3(0f, _actionData.invData.holdingEntity.rotation.y, 0f));
				entity.SetSpawnerSource(EnumSpawnerSource.StaticSpawner);
				EntityTurret entityTurret = entity as EntityTurret;
				if (entityTurret != null)
				{
					entityTurret.factionId = itemActionDataSpawnTurret.invData.holdingEntity.factionId;
					entityTurret.belongsPlayerId = itemActionDataSpawnTurret.invData.holdingEntity.entityId;
					entityTurret.factionRank = itemActionDataSpawnTurret.invData.holdingEntity.factionRank - 1;
					entityTurret.OriginalItemValue = itemActionDataSpawnTurret.invData.itemValue.Clone();
					entityTurret.groundPosition = itemActionDataSpawnTurret.Position;
					entityTurret.OwnerID = PlatformManager.InternalLocalUserIdentifier;
					entityTurret.ForceOn = true;
					entityTurret.rotation = new Vector3(0f, _actionData.invData.holdingEntity.rotation.y, 0f);
					itemActionDataSpawnTurret.invData.holdingEntity.AddOwnedEntity(entityTurret);
				}
				else
				{
					EntityDrone entityDrone = entity as EntityDrone;
					if (entityDrone != null)
					{
						entityDrone.factionId = itemActionDataSpawnTurret.invData.holdingEntity.factionId;
						entityDrone.belongsPlayerId = itemActionDataSpawnTurret.invData.holdingEntity.entityId;
						entityDrone.factionRank = itemActionDataSpawnTurret.invData.holdingEntity.factionRank - 1;
						entityDrone.OriginalItemValue = itemActionDataSpawnTurret.invData.itemValue.Clone();
						entityDrone.SetItemValueToLoad(entityDrone.OriginalItemValue);
						entityDrone.OwnerID = PlatformManager.InternalLocalUserIdentifier;
						entityDrone.PrepareToSpawn();
						itemActionDataSpawnTurret.invData.holdingEntity.AddOwnedEntity(entityDrone);
					}
				}
				GameManager.Instance.World.SpawnEntityInWorld(entity);
			}
			itemActionDataSpawnTurret.Placing = false;
			invData.holdingEntity.RightArmAnimationUse = true;
			(invData.holdingEntity as EntityPlayerLocal).DropTimeDelay = 0.5f;
			if (itemActionDataSpawnTurret.TurretPreviewT != null && itemActionDataSpawnTurret.invData.holdingEntity is EntityPlayerLocal)
			{
				this.ClearPreview(itemActionDataSpawnTurret);
			}
			invData.holdingEntity.inventory.DecHoldingItem(1);
			invData.holdingEntity.PlayOneShot((this.soundStart != null) ? this.soundStart : "placeblock", false, false, false, null, 1f);
			return;
		}
		if (flag2)
		{
			GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "xuiMaxDeployedDronesReached", false, false, 0f);
			return;
		}
		GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "uiCannotAddTurret", false, false, 0f);
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x060030C4 RID: 12484 RVA: 0x00139FF0 File Offset: 0x001381F0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CalcSpawnPosition(ItemActionSpawnTurret.ItemActionDataSpawnTurret _actionData, ref Vector3 position)
	{
		World world = _actionData.invData.world;
		Ray lookRay = _actionData.invData.holdingEntity.GetLookRay();
		int num = (_actionData.CanPlaceInAir ? 8454144 : 65536);
		if (Voxel.Raycast(world, lookRay, 4f + this.turretSize.x, num, 69, 0f))
		{
			position = Voxel.voxelRayHitInfo.hit.pos;
		}
		else
		{
			position = lookRay.origin + lookRay.direction * (4f + this.turretSize.x);
			if (!_actionData.CanPlaceInAir)
			{
				return false;
			}
		}
		Collider[] array = Physics.OverlapSphere(position - Origin.position + Vector3.up * 0.525f, 0.5f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.layer != 18 && !(array[i].gameObject == _actionData.TurretPreviewT.gameObject))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x0013A10C File Offset: 0x0013830C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckForSpace(Vector3 localPos, Vector3 dirN, float length, Vector3 axis1N, float axis1Length, Vector3 axis2N, float axis2Length)
	{
		Vector3 vector = dirN * length * 0.5f;
		for (float num = -axis1Length * 0.5f; num <= axis1Length * 0.5f; num += 0.2499f)
		{
			Vector3 vector2 = localPos + axis1N * num;
			for (float num2 = -axis2Length * 0.5f; num2 <= axis2Length * 0.5f; num2 += 0.2499f)
			{
				Vector3 vector3 = vector2 + axis2N * num2;
				if (Physics.Raycast(vector3 - vector, dirN, length, 28901376))
				{
					return false;
				}
				if (Physics.Raycast(vector3 + vector, -dirN, length, 28901376))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060030C6 RID: 12486 RVA: 0x0013A1C4 File Offset: 0x001383C4
	public void ClearPreview(ItemActionData _data)
	{
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = _data as ItemActionSpawnTurret.ItemActionDataSpawnTurret;
		this.ClearPreview(itemActionDataSpawnTurret);
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x0013A1DF File Offset: 0x001383DF
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearPreview(ItemActionSpawnTurret.ItemActionDataSpawnTurret _actionData)
	{
		if (_actionData.TurretPreviewT)
		{
			global::UnityEngine.Object.Destroy(_actionData.TurretPreviewT.gameObject);
		}
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x0013A200 File Offset: 0x00138400
	public override void Cleanup(ItemActionData _data)
	{
		base.Cleanup(_data);
		ItemActionSpawnTurret.ItemActionDataSpawnTurret itemActionDataSpawnTurret = _data as ItemActionSpawnTurret.ItemActionDataSpawnTurret;
		if (itemActionDataSpawnTurret != null && itemActionDataSpawnTurret.TurretPreviewT != null && itemActionDataSpawnTurret.invData != null && itemActionDataSpawnTurret.invData.holdingEntity is EntityPlayerLocal)
		{
			this.ClearPreview(itemActionDataSpawnTurret);
		}
	}

	// Token: 0x040025A2 RID: 9634
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cColliderMask = 28901376;

	// Token: 0x040025A3 RID: 9635
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x040025A4 RID: 9636
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityClassId = -1;

	// Token: 0x040025A5 RID: 9637
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 turretSize;

	// Token: 0x040025A6 RID: 9638
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 previewSize;

	// Token: 0x040025A7 RID: 9639
	public bool canPlaceInAir;

	// Token: 0x020005D4 RID: 1492
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnTurret : ItemActionAttackData
	{
		// Token: 0x060030C9 RID: 12489 RVA: 0x0012B224 File Offset: 0x00129424
		public ItemActionDataSpawnTurret(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025A8 RID: 9640
		public Transform TurretPreviewT;

		// Token: 0x040025A9 RID: 9641
		public Renderer[] PreviewRenderers;

		// Token: 0x040025AA RID: 9642
		public bool ValidPosition;

		// Token: 0x040025AB RID: 9643
		public Vector3 Position;

		// Token: 0x040025AC RID: 9644
		public bool Placing;

		// Token: 0x040025AD RID: 9645
		public bool CanPlaceInAir;
	}
}
