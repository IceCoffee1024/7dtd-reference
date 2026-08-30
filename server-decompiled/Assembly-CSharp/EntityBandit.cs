using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200048A RID: 1162
[Preserve]
public class EntityBandit : EntityHuman
{
	// Token: 0x0600241C RID: 9244 RVA: 0x000DA7A4 File Offset: 0x000D89A4
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

	// Token: 0x0600241D RID: 9245 RVA: 0x000DA871 File Offset: 0x000D8A71
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000DA879 File Offset: 0x000D8A79
	public override void SetupHandItem()
	{
		this.ShowHoldingItem(true);
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x000DA884 File Offset: 0x000D8A84
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

	// Token: 0x06002420 RID: 9248 RVA: 0x000DA8F0 File Offset: 0x000D8AF0
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

	// Token: 0x06002421 RID: 9249 RVA: 0x000DA91A File Offset: 0x000D8B1A
	public override bool GetAimTarget(out Vector3 aimTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out aimTarget) || base.GetAimTarget(out aimTarget);
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000DA934 File Offset: 0x000D8B34
	public override bool GetHeadLookTarget(out Vector3 lookTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out lookTarget) || base.GetHeadLookTarget(out lookTarget);
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000DA950 File Offset: 0x000D8B50
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

	// Token: 0x04001997 RID: 6551
	public readonly AIFocus<AIFocusBody> focusBody = new AIFocus<AIFocusBody>(true);

	// Token: 0x04001998 RID: 6552
	public readonly AIFocus<AIFocusAim> focusAim = new AIFocus<AIFocusAim>(true);

	// Token: 0x0200048B RID: 1163
	[PublicizedFrom(EAccessModifier.Private)]
	public enum InvSlots
	{
		// Token: 0x0400199A RID: 6554
		Melee,
		// Token: 0x0400199B RID: 6555
		Ranged,
		// Token: 0x0400199C RID: 6556
		Thrown,
		// Token: 0x0400199D RID: 6557
		Heal
	}
}
