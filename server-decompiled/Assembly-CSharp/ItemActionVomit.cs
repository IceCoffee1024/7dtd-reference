using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E0 RID: 1504
[Preserve]
public class ItemActionVomit : ItemActionLauncher
{
	// Token: 0x060030E3 RID: 12515 RVA: 0x0013D926 File Offset: 0x0013BB26
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionVomit.ItemActionDataVomit(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x0013D930 File Offset: 0x0013BB30
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		_props.ParseInt("AnimType", ref this.animType);
		this.warningDelay = 1.2f;
		_props.ParseFloat("WarningDelay", ref this.warningDelay);
		this.warningMax = 3;
		_props.ParseInt("WarningMax", ref this.warningMax);
		_props.ParseString("Sound_warning", ref this.soundWarning);
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0013D99A File Offset: 0x0013BB9A
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetAttack(ItemActionVomit.ItemActionDataVomit _actionData)
	{
		_actionData.numWarningsPlayed = 0;
		_actionData.warningTime = 0f;
		_actionData.bAttackStarted = false;
		_actionData.isActive = false;
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x0013D9BC File Offset: 0x0013BBBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetActionEffectsValues(ItemActionData _actionData, out Vector3 _startPos, out Vector3 _direction)
	{
		ItemActionVomit.ItemActionDataVomit itemActionDataVomit = (ItemActionVomit.ItemActionDataVomit)_actionData;
		Ray lookRay = itemActionDataVomit.invData.holdingEntity.GetLookRay();
		_startPos = lookRay.origin;
		_direction = lookRay.direction;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue holdingItemItemValue = holdingEntity.inventory.holdingItemItemValue;
		ItemClass forId = ItemClass.GetForId(ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false).type);
		if (((ItemActionProjectile)((forId.Actions[0] is ItemActionProjectile) ? forId.Actions[0] : forId.Actions[1])).FlyTime < 0f)
		{
			Transform transform = itemActionDataVomit.projectileJointT;
			if (!transform)
			{
				transform = holdingEntity.emodel.GetRightHandTransform();
			}
			_startPos = transform.position + Origin.position;
			EntityAlive attackTarget = holdingEntity.GetAttackTarget();
			if (attackTarget)
			{
				Vector3 chestPosition = attackTarget.getChestPosition();
				if (holdingEntity.IsInFrontOfMe(chestPosition))
				{
					_direction = chestPosition;
					return 1;
				}
			}
			return 0;
		}
		_direction = this.getDirectionOffset(itemActionDataVomit, _direction, 0);
		return 0;
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x0013DAE4 File Offset: 0x0013BCE4
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		ItemActionVomit.ItemActionDataVomit itemActionDataVomit = (ItemActionVomit.ItemActionDataVomit)_actionData;
		if (itemActionDataVomit.muzzle == null)
		{
			itemActionDataVomit.muzzle = _actionData.invData.holdingEntity.emodel.GetRightHandTransform();
		}
		if (_firingState != 0)
		{
			itemActionDataVomit.numVomits++;
			bool flag = _userData > 0;
			Vector3 vector = _direction;
			Vector3 direction = itemActionDataVomit.invData.holdingEntity.GetLookRay().direction;
			int burstCount = this.GetBurstCount(_actionData);
			for (int i = 0; i < burstCount; i++)
			{
				if (!flag)
				{
					vector = this.getDirectionRandomOffset(itemActionDataVomit, direction);
				}
				base.instantiateProjectile(_actionData, default(Vector3)).GetComponent<ProjectileMoveScript>().Fire(_startPos, vector, _actionData.invData.holdingEntity, this.hitmaskOverride, -1f, flag);
			}
		}
		base.ItemActionEffects(_gameManager, _actionData, _firingState, _startPos, _direction, _userData);
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x0013DBC4 File Offset: 0x0013BDC4
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionVomit.ItemActionDataVomit itemActionDataVomit = (ItemActionVomit.ItemActionDataVomit)_actionData;
		if (_bReleased)
		{
			base.ExecuteAction(_actionData, _bReleased);
			this.resetAttack(itemActionDataVomit);
			return;
		}
		itemActionDataVomit.isActive = true;
		float time = Time.time;
		if (time - itemActionDataVomit.m_LastShotTime < this.Delay)
		{
			return;
		}
		if (itemActionDataVomit.warningTime > 0f && time < itemActionDataVomit.warningTime)
		{
			return;
		}
		if (!itemActionDataVomit.bAttackStarted)
		{
			EntityAlive holdingEntity = _actionData.invData.holdingEntity;
			if (itemActionDataVomit.numWarningsPlayed < this.warningMax - 1 && holdingEntity.rand.RandomFloat < 0.5f)
			{
				itemActionDataVomit.numWarningsPlayed++;
				itemActionDataVomit.warningTime = time + this.warningDelay;
				holdingEntity.PlayOneShot(this.soundWarning, false, false, false, null, 1f);
				holdingEntity.Raging = true;
				return;
			}
			itemActionDataVomit.bAttackStarted = true;
			itemActionDataVomit.numVomits = 0;
			holdingEntity.StartAnimAction(this.animType + 3000);
			if (this.warningMax > 0)
			{
				holdingEntity.PlayOneShot(this.soundWarning, false, false, false, null, 1f);
				itemActionDataVomit.warningTime = time + this.warningDelay;
				return;
			}
		}
		if (itemActionDataVomit.numVomits >= this.GetMaxAmmoCount(itemActionDataVomit))
		{
			itemActionDataVomit.isActive = false;
			return;
		}
		itemActionDataVomit.curBurstCount = 0;
		base.ExecuteAction(_actionData, _bReleased);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x0013DD05 File Offset: 0x0013BF05
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionVomit.ItemActionDataVomit)_actionData).isActive;
	}

	// Token: 0x040025E2 RID: 9698
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadius = -1f;

	// Token: 0x040025E3 RID: 9699
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x040025E4 RID: 9700
	[PublicizedFrom(EAccessModifier.Private)]
	public float warningDelay;

	// Token: 0x040025E5 RID: 9701
	[PublicizedFrom(EAccessModifier.Private)]
	public int warningMax;

	// Token: 0x040025E6 RID: 9702
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarning;

	// Token: 0x020005E1 RID: 1505
	public class ItemActionDataVomit : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x060030EB RID: 12523 RVA: 0x00129D19 File Offset: 0x00127F19
		public ItemActionDataVomit(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025E7 RID: 9703
		public float warningTime;

		// Token: 0x040025E8 RID: 9704
		public int numWarningsPlayed;

		// Token: 0x040025E9 RID: 9705
		public int numVomits;

		// Token: 0x040025EA RID: 9706
		public bool bAttackStarted;

		// Token: 0x040025EB RID: 9707
		public bool isActive;
	}
}
