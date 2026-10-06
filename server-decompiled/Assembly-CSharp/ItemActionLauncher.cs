using System;
using System.Collections.Generic;
using AssetManagement;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B4 RID: 1460
[Preserve]
public class ItemActionLauncher : ItemActionRanged
{
	// Token: 0x06002FE6 RID: 12262 RVA: 0x001315E3 File Offset: 0x0012F7E3
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionLauncher.ItemActionDataLauncher(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FE7 RID: 12263 RVA: 0x001315EC File Offset: 0x0012F7EC
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x001315F8 File Offset: 0x0012F7F8
	public override void StartHolding(ItemActionData _actionData)
	{
		base.StartHolding(_actionData);
		this.DeleteProjectiles(_actionData);
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		if (_actionData.invData.itemValue.Meta != 0 && this.GetMaxAmmoCount(itemActionDataLauncher) != 0)
		{
			for (int i = 0; i < _actionData.invData.itemValue.Meta; i++)
			{
				itemActionDataLauncher.projectileTs.Add(this.instantiateProjectile(_actionData, default(Vector3)));
			}
		}
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x0013166B File Offset: 0x0012F86B
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		this.DeleteProjectiles(_data);
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x0013167C File Offset: 0x0012F87C
	public void DeleteProjectiles(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		for (int i = 0; i < itemActionDataLauncher.projectileTs.Count; i++)
		{
			Transform transform = itemActionDataLauncher.projectileTs[i];
			if (transform)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		itemActionDataLauncher.projectileTs.Clear();
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x001316D4 File Offset: 0x0012F8D4
	public override void ReloadGun(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		if (itemActionDataLauncher != null)
		{
			itemActionDataLauncher.isReloadRequested = false;
			Manager.StopSequence(_actionData.invData.holdingEntity, ((ItemActionRanged.ItemActionDataRanged)_actionData).SoundStart);
			if (!_actionData.invData.holdingEntity.isEntityRemote)
			{
				_actionData.invData.holdingEntity.OnReloadStart();
			}
		}
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x00131730 File Offset: 0x0012F930
	public override void CancelReload(ItemActionData _actionData, bool holsterWeapon)
	{
		base.CancelReload(_actionData, holsterWeapon);
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		this.ClampAmmoCount(itemActionDataLauncher);
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x00131754 File Offset: 0x0012F954
	public override void SwapAmmoType(EntityAlive _entity, int _selectedIndex = -1)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_entity.inventory.holdingItemData.actionData[0];
		this.ClampAmmoCount(itemActionDataLauncher);
		base.SwapAmmoType(_entity, _selectedIndex);
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x0013178C File Offset: 0x0012F98C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClampAmmoCount(ItemActionLauncher.ItemActionDataLauncher actionData)
	{
		int maxAmmoCount = this.GetMaxAmmoCount(actionData);
		int num = actionData.projectileTs.Count - maxAmmoCount;
		if (num > 0)
		{
			for (int i = maxAmmoCount; i < actionData.projectileTs.Count; i++)
			{
				if (actionData.projectileTs[i] != null)
				{
					global::UnityEngine.Object.Destroy(actionData.projectileTs[i].gameObject);
				}
			}
			actionData.projectileTs.RemoveRange(maxAmmoCount, num);
		}
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x00131800 File Offset: 0x0012FA00
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 fireShot(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData, ref bool hitEntity)
	{
		hitEntity = true;
		return Vector3.zero;
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x0013180C File Offset: 0x0012FA0C
	public Transform instantiateProjectile(ItemActionData _actionData, Vector3 _positionOffset = default(Vector3))
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue holdingItemItemValue = holdingEntity.inventory.holdingItemItemValue;
		ItemValue item = ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false);
		this.LastProjectileType = item;
		ItemClass forId = ItemClass.GetForId(item.type);
		if (forId == null)
		{
			return null;
		}
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		itemActionDataLauncher.lastAttackStrainPercent = itemActionDataLauncher.strainPercent;
		ItemValue itemValue = new ItemValue(forId.Id, false);
		InstanceHandle<GameObject> instanceHandle = forId.CloneModel(itemValue, null, BlockShape.MeshPurpose.World);
		Transform transform = instanceHandle.Instance.transform;
		Transform transform2 = itemActionDataLauncher.projectileJointT;
		if (!transform2)
		{
			transform2 = holdingEntity.emodel.GetRightHandTransform();
		}
		if (transform2)
		{
			transform.SetParent(transform2, false);
			transform.SetLocalPositionAndRotation(_positionOffset, Quaternion.identity);
			transform.localScale = Vector3.one;
		}
		else
		{
			transform.parent = null;
			transform.localScale = Vector3.one;
		}
		Utils.SetLayerRecursively(transform.gameObject, transform2 ? transform2.gameObject.layer : 0);
		ProjectileMoveScript projectileMoveScript = transform.gameObject.AddComponent<ProjectileMoveScript>();
		projectileMoveScript.itemProjectile = forId;
		projectileMoveScript.itemValueProjectile = itemValue;
		projectileMoveScript.itemValueLauncher = holdingItemItemValue;
		projectileMoveScript.itemActionProjectile = (ItemActionProjectile)((forId.Actions[0] is ItemActionProjectile) ? forId.Actions[0] : forId.Actions[1]);
		projectileMoveScript.ProjectileOwnerID = holdingEntity.entityId;
		projectileMoveScript.actionData = itemActionDataLauncher;
		projectileMoveScript.assetHandle = instanceHandle;
		transform.gameObject.SetActive(true);
		return transform;
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x00131998 File Offset: 0x0012FB98
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		base.ItemActionEffects(_gameManager, _actionData, _firingState, _startPos, _direction, _userData);
		if (_firingState == 0)
		{
			return;
		}
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		int num = this.GetBurstCount(_actionData);
		if (num <= 0)
		{
			return;
		}
		for (int i = itemActionDataLauncher.projectileTs.Count - 1; i >= 0; i--)
		{
			Transform transform = itemActionDataLauncher.projectileTs[i];
			if (transform)
			{
				transform.GetComponent<ProjectileMoveScript>().Fire(_startPos, this.getDirectionOffset(itemActionDataLauncher, _direction, i), _actionData.invData.holdingEntity, this.hitmaskOverride, 0f, false);
			}
			itemActionDataLauncher.projectileTs.RemoveAt(i);
			if (--num <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x00131A3B File Offset: 0x0012FC3B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ConsumeAmmo(ItemActionData _actionData)
	{
		_actionData.invData.itemValue.Meta -= this.GetBurstCount(_actionData);
	}

	// Token: 0x06002FF3 RID: 12275 RVA: 0x00131A5C File Offset: 0x0012FC5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetActionEffectsValues(ItemActionData _actionData, out Vector3 _startPos, out Vector3 _direction)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		Ray lookRay = itemActionDataLauncher.invData.holdingEntity.GetLookRay();
		_startPos = lookRay.origin;
		_direction = lookRay.direction;
		_direction = this.getDirectionOffset(itemActionDataLauncher, _direction, 0);
		return 0;
	}

	// Token: 0x040024E6 RID: 9446
	public ItemValue LastProjectileType;

	// Token: 0x020005B5 RID: 1461
	public class ItemActionDataLauncher : ItemActionRanged.ItemActionDataRanged
	{
		// Token: 0x06002FF5 RID: 12277 RVA: 0x00131AB8 File Offset: 0x0012FCB8
		public ItemActionDataLauncher(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
			this.projectileJointT = ((_invData.model != null) ? _invData.model.FindInChilds("ProjectileJoint", false) : null);
			this.projectileTs = new List<Transform>();
		}

		// Token: 0x040024E7 RID: 9447
		public Transform projectileJointT;

		// Token: 0x040024E8 RID: 9448
		public List<Transform> projectileTs;

		// Token: 0x040024E9 RID: 9449
		public float strainPercent = 1f;

		// Token: 0x040024EA RID: 9450
		public float lastAttackStrainPercent;
	}
}
