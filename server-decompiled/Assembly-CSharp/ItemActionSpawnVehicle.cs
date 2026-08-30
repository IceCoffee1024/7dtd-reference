using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005CF RID: 1487
[Preserve]
public class ItemActionSpawnVehicle : ItemAction
{
	// Token: 0x06003056 RID: 12374 RVA: 0x00138714 File Offset: 0x00136914
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnVehicle.ItemActionDataSpawnVehicle(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x00138720 File Offset: 0x00136920
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Vehicle"))
		{
			this.entityToSpawn = _props.Values["Vehicle"];
		}
		this.vehicleSize = new Vector3(1f, 1.9f, 2f);
		if (_props.Values.ContainsKey("VehicleSize"))
		{
			this.vehicleSize = StringParsers.ParseVector3(_props.Values["VehicleSize"], 0, -1);
		}
		foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
		{
			if (keyValuePair.Value.entityClassName == this.entityToSpawn)
			{
				this.entityId = keyValuePair.Key;
				break;
			}
		}
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x00138810 File Offset: 0x00136A10
	public override void StartHolding(ItemActionData _actionData)
	{
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataSpawnVehicle.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		if (itemActionDataSpawnVehicle.VehiclePreviewT)
		{
			global::UnityEngine.Object.DestroyImmediate(itemActionDataSpawnVehicle.VehiclePreviewT.gameObject);
		}
		GameObject gameObject = DataLoader.LoadAsset<GameObject>(entityPlayerLocal.inventory.holdingItem.MeshFile, false);
		itemActionDataSpawnVehicle.VehiclePreviewT = global::UnityEngine.Object.Instantiate<GameObject>(gameObject).transform;
		Vehicle.SetupPreview(itemActionDataSpawnVehicle.VehiclePreviewT);
		this.SetupPreview(itemActionDataSpawnVehicle);
		GameManager.Instance.StartCoroutine(this.UpdatePreview(itemActionDataSpawnVehicle));
	}

	// Token: 0x06003059 RID: 12377 RVA: 0x001388A8 File Offset: 0x00136AA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupPreview(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle data)
	{
		data.ValidPosition = false;
		if (data.PreviewRenderers == null || data.PreviewRenderers.Length == 0 || data.PreviewRenderers[0] == null)
		{
			data.PreviewRenderers = data.VehiclePreviewT.GetComponentsInChildren<Renderer>();
		}
		for (int i = 0; i < data.PreviewRenderers.Length; i++)
		{
			data.PreviewRenderers[i].material.color = new Color(2f, 0.25f, 0.25f);
		}
	}

	// Token: 0x0600305A RID: 12378 RVA: 0x00138927 File Offset: 0x00136B27
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator UpdatePreview(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle data)
	{
		World world = data.invData.world;
		while (data.VehiclePreviewT)
		{
			bool flag = this.CalcSpawnPosition(data, ref data.Position) && world.CanPlaceBlockAt(new Vector3i(data.Position), world.GetGameManager().GetPersistentLocalPlayer(), false);
			if (data.ValidPosition != flag)
			{
				data.ValidPosition = flag;
				if (data.PreviewRenderers == null || data.PreviewRenderers.Length == 0 || data.PreviewRenderers[0] == null)
				{
					data.PreviewRenderers = data.VehiclePreviewT.GetComponentsInChildren<Renderer>();
				}
				Color color = (flag ? new Color(0.25f, 2f, 0.25f) : new Color(2f, 0.25f, 0.25f));
				for (int i = 0; i < data.PreviewRenderers.Length; i++)
				{
					data.PreviewRenderers[i].material.color = color;
				}
			}
			if (data.Position.y < 9999f)
			{
				Quaternion quaternion = Quaternion.Euler(0f, data.invData.holdingEntity.rotation.y + 90f, 0f);
				data.VehiclePreviewT.localRotation = quaternion;
				data.VehiclePreviewT.position = data.Position - Origin.position;
				data.VehiclePreviewT.gameObject.SetActive(true);
			}
			else
			{
				data.VehiclePreviewT.gameObject.SetActive(false);
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x00138940 File Offset: 0x00136B40
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		if (itemActionDataSpawnVehicle.VehiclePreviewT && itemActionDataSpawnVehicle.invData.holdingEntity is EntityPlayerLocal)
		{
			global::UnityEngine.Object.Destroy(itemActionDataSpawnVehicle.VehiclePreviewT.gameObject);
		}
	}

	// Token: 0x0600305C RID: 12380 RVA: 0x00138984 File Offset: 0x00136B84
	public override void StopHolding(ItemActionData _actionData)
	{
		base.StopHolding(_actionData);
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		if (itemActionDataSpawnVehicle.VehiclePreviewT && itemActionDataSpawnVehicle.invData.holdingEntity is EntityPlayerLocal)
		{
			global::UnityEngine.Object.Destroy(itemActionDataSpawnVehicle.VehiclePreviewT.gameObject);
		}
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x001389D0 File Offset: 0x00136BD0
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
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
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		if (!itemActionDataSpawnVehicle.ValidPosition)
		{
			return;
		}
		if (this.entityId < 0)
		{
			foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
			{
				if (keyValuePair.Value.entityClassName == this.entityToSpawn)
				{
					this.entityId = keyValuePair.Key;
					break;
				}
			}
			if (this.entityId == 0)
			{
				return;
			}
		}
		_actionData.lastUseTime = time;
		ItemValue holdingItemItemValue = entityPlayerLocal.inventory.holdingItemItemValue;
		if (VehicleManager.CanAddMoreVehicles())
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageVehicleSpawn>().Setup(this.entityId, itemActionDataSpawnVehicle.Position, new Vector3(0f, entityPlayerLocal.rotation.y + 90f, 0f), holdingItemItemValue.Clone(), entityPlayerLocal.entityId), true);
			}
			else
			{
				Entity entity = EntityFactory.CreateEntity(this.entityId, itemActionDataSpawnVehicle.Position + Vector3.up * 0.25f, new Vector3(0f, entityPlayerLocal.rotation.y + 90f, 0f));
				entity.SetSpawnerSource(EnumSpawnerSource.StaticSpawner);
				EntityVehicle entityVehicle = entity as EntityVehicle;
				if (entityVehicle != null)
				{
					entityVehicle.GetVehicle().SetItemValue(holdingItemItemValue);
					entityVehicle.SetOwner(PlatformManager.InternalLocalUserIdentifier);
				}
				else
				{
					EntityAlive entityAlive = entity as EntityAlive;
					if (entityAlive != null)
					{
						entityAlive.factionId = entityPlayerLocal.factionId;
						entityAlive.belongsPlayerId = entityPlayerLocal.entityId;
						entityAlive.factionRank = entityPlayerLocal.factionRank - 1;
					}
				}
				GameManager.Instance.World.SpawnEntityInWorld(entity);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageVehicleCount>().Setup(), false, -1, -1, -1, null, 192, false);
			}
			entityPlayerLocal.RightArmAnimationUse = true;
			entityPlayerLocal.DropTimeDelay = 0.5f;
			entityPlayerLocal.inventory.DecHoldingItem(1);
			entityPlayerLocal.PlayOneShot((this.soundStart != null) ? this.soundStart : "placeblock", false, false, false, null, 1f);
			this.ClearPreview(_actionData);
			return;
		}
		GameManager.ShowTooltip(entityPlayerLocal, "uiCannotAddVehicle", false, false, 0f);
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x00138C70 File Offset: 0x00136E70
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CalcSpawnPosition(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle _actionData, ref Vector3 position)
	{
		World world = _actionData.invData.world;
		Ray lookRay = _actionData.invData.holdingEntity.GetLookRay();
		if (Vector3.Dot(lookRay.direction, Vector3.up) == 1f)
		{
			return false;
		}
		position.y = float.MaxValue;
		float num = 4f + this.vehicleSize.x;
		if (Voxel.Raycast(world, lookRay, num + 1.5f, 8454144, 69, 0f))
		{
			if ((Voxel.voxelRayHitInfo.hit.pos - lookRay.origin).magnitude > num)
			{
				position = Voxel.voxelRayHitInfo.hit.pos;
				return false;
			}
			for (float num2 = 0.14f; num2 < 1.15f; num2 += 0.25f)
			{
				position = Voxel.voxelRayHitInfo.hit.pos;
				position.y += num2;
				Vector3 normalized = Vector3.Cross(lookRay.direction, Vector3.up).normalized;
				Vector3 vector = Vector3.Cross(normalized, Vector3.up);
				Vector3 vector2 = position - Origin.position;
				vector2.y += this.vehicleSize.y * 0.5f + 0.05f;
				if (this.CheckForSpace(vector2, normalized, this.vehicleSize.z, vector, this.vehicleSize.x, Vector3.up, this.vehicleSize.y) && this.CheckForSpace(vector2, vector, this.vehicleSize.x, normalized, this.vehicleSize.z, Vector3.up, this.vehicleSize.y) && this.CheckForSpace(vector2, Vector3.up, this.vehicleSize.y, normalized, this.vehicleSize.z, vector, this.vehicleSize.x))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x00138E6C File Offset: 0x0013706C
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

	// Token: 0x06003061 RID: 12385 RVA: 0x00138F24 File Offset: 0x00137124
	public void ClearPreview(ItemActionData _data)
	{
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = _data as ItemActionSpawnVehicle.ItemActionDataSpawnVehicle;
		this.ClearPreview(itemActionDataSpawnVehicle);
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x00138F3F File Offset: 0x0013713F
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearPreview(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle _actionData)
	{
		if (_actionData.VehiclePreviewT)
		{
			global::UnityEngine.Object.Destroy(_actionData.VehiclePreviewT.gameObject);
		}
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x00138F60 File Offset: 0x00137160
	public override void Cleanup(ItemActionData _data)
	{
		base.Cleanup(_data);
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = _data as ItemActionSpawnVehicle.ItemActionDataSpawnVehicle;
		if (itemActionDataSpawnVehicle != null && itemActionDataSpawnVehicle.invData != null && itemActionDataSpawnVehicle.invData.holdingEntity is EntityPlayerLocal)
		{
			this.ClearPreview(itemActionDataSpawnVehicle);
		}
	}

	// Token: 0x04002580 RID: 9600
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cColliderMask = 28901376;

	// Token: 0x04002581 RID: 9601
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x04002582 RID: 9602
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId = -1;

	// Token: 0x04002583 RID: 9603
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 vehicleSize;

	// Token: 0x020005D0 RID: 1488
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnVehicle : ItemActionAttackData
	{
		// Token: 0x06003064 RID: 12388 RVA: 0x00129694 File Offset: 0x00127894
		public ItemActionDataSpawnVehicle(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002584 RID: 9604
		public Transform VehiclePreviewT;

		// Token: 0x04002585 RID: 9605
		public Renderer[] PreviewRenderers;

		// Token: 0x04002586 RID: 9606
		public bool ValidPosition;

		// Token: 0x04002587 RID: 9607
		public Vector3 Position;
	}
}
