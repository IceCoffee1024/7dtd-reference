using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000486 RID: 1158
[Preserve]
public class EntityBandit : EntityHuman
{
	// Token: 0x060023FB RID: 9211 RVA: 0x000D9CCC File Offset: 0x000D7ECC
	public override void PostInit()
	{
		string @string = EntityClass.list[this.entityClass].Properties.GetString(EntityClass.PropHandItem);
		if (@string.Length > 0)
		{
			base.SetInventorySlots(@string);
		}
		if (this.inventory.GetItem(0).IsEmpty())
		{
			ItemValue bareHandItemValue = this.inventory.GetBareHandItemValue();
			bareHandItemValue.Quality = (ushort)this.rand.RandomRange(1, 3);
			bareHandItemValue.UseTimes = (float)bareHandItemValue.MaxUseTimes * 0.7f - 1f;
			this.inventory.SetItem(0, bareHandItemValue, 1, true);
		}
		int num = 1;
		if (!this.inventory.GetItem(num).IsEmpty())
		{
			this.inventory.SetHoldingItemIdx(num);
		}
		if (this.moveHelper != null)
		{
			this.moveHelper.CanOpenDoors = true;
		}
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000D9D99 File Offset: 0x000D7F99
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000D9DA1 File Offset: 0x000D7FA1
	public override void SetupHandItem()
	{
		this.ShowHoldingItem(true);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000D9DAC File Offset: 0x000D7FAC
	public override bool UseHoldingItem(int _actionIndex, bool _isReleased)
	{
		if (!_isReleased)
		{
			ItemActionAttackData itemActionAttackData = this.inventory.holdingItemData.actionData[0] as ItemActionAttackData;
			if (itemActionAttackData != null)
			{
				ItemValue itemValue = itemActionAttackData.invData.itemValue;
				itemValue.UseTimes = (float)itemValue.MaxUseTimes * 0.8f - 1f;
				if (itemActionAttackData is ItemActionRanged.ItemActionDataRanged)
				{
					itemValue.Meta = 2;
				}
			}
		}
		return base.UseHoldingItem(_actionIndex, _isReleased);
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000D9E18 File Offset: 0x000D8018
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTasks()
	{
		base.updateTasks();
		Vector3 vector;
		if (AIFocusAim.GetActiveFocus(this, this.focusAim, out vector))
		{
			base.SetLookPosition(vector);
		}
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000D9E42 File Offset: 0x000D8042
	public override bool GetAimTarget(out Vector3 aimTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out aimTarget) || base.GetAimTarget(out aimTarget);
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000D9E5C File Offset: 0x000D805C
	public override bool GetHeadLookTarget(out Vector3 lookTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out lookTarget) || base.GetHeadLookTarget(out lookTarget);
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000D9E78 File Offset: 0x000D8078
	public override bool CalcStrafeYawOffset(float _moveX, float _moveZ, ref float _desiredyaw, ref float _yawOffset)
	{
		float num;
		if (AIFocusBody.GetActiveFocus(this, this.focusBody, out num))
		{
			float num2 = Mathf.Atan2(_moveX, _moveZ) * 57.29578f;
			_desiredyaw = num;
			_yawOffset = MathUtils.NormalizeAxis(num2 - _desiredyaw);
			return true;
		}
		return false;
	}

	// Token: 0x04001980 RID: 6528
	public readonly AIFocus<AIFocusBody> focusBody = new AIFocus<AIFocusBody>(true);

	// Token: 0x04001981 RID: 6529
	public readonly AIFocus<AIFocusAim> focusAim = new AIFocus<AIFocusAim>(true);

	// Token: 0x02000487 RID: 1159
	[PublicizedFrom(EAccessModifier.Private)]
	public enum InvSlots
	{
		// Token: 0x04001983 RID: 6531
		Melee,
		// Token: 0x04001984 RID: 6532
		Ranged,
		// Token: 0x04001985 RID: 6533
		Thrown,
		// Token: 0x04001986 RID: 6534
		Heal
	}
}
