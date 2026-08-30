using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E5 RID: 1509
[Preserve]
public class ItemActionVomit : ItemActionLauncher
{
	// Token: 0x06003111 RID: 12561 RVA: 0x0013E996 File Offset: 0x0013CB96
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionVomit.ItemActionDataVomit(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x0013E9A0 File Offset: 0x0013CBA0
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

	// Token: 0x06003113 RID: 12563 RVA: 0x0013EA0A File Offset: 0x0013CC0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetAttack(ItemActionVomit.ItemActionDataVomit _actionData)
	{
		_actionData.numWarningsPlayed = 0;
		_actionData.warningTime = 0f;
		_actionData.bAttackStarted = false;
		_actionData.isActive = false;
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x0013EA2C File Offset: 0x0013CC2C
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

	// Token: 0x06003115 RID: 12565 RVA: 0x0013EB54 File Offset: 0x0013CD54
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

	// Token: 0x06003116 RID: 12566 RVA: 0x0013EC34 File Offset: 0x0013CE34
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

	// Token: 0x06003117 RID: 12567 RVA: 0x0013ED75 File Offset: 0x0013CF75
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionVomit.ItemActionDataVomit)_actionData).isActive;
	}

	// Token: 0x04002616 RID: 9750
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadius = -1f;

	// Token: 0x04002617 RID: 9751
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x04002618 RID: 9752
	[PublicizedFrom(EAccessModifier.Private)]
	public float warningDelay;

	// Token: 0x04002619 RID: 9753
	[PublicizedFrom(EAccessModifier.Private)]
	public int warningMax;

	// Token: 0x0400261A RID: 9754
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarning;

	// Token: 0x020005E6 RID: 1510
	public class ItemActionDataVomit : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x06003119 RID: 12569 RVA: 0x0012AD89 File Offset: 0x00128F89
		public ItemActionDataVomit(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400261B RID: 9755
		public float warningTime;

		// Token: 0x0400261C RID: 9756
		public int numWarningsPlayed;

		// Token: 0x0400261D RID: 9757
		public int numVomits;

		// Token: 0x0400261E RID: 9758
		public bool bAttackStarted;

		// Token: 0x0400261F RID: 9759
		public bool isActive;
	}
}
