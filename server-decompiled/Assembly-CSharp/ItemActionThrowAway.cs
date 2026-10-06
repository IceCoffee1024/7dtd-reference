using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E1 RID: 1505
[Preserve]
public class ItemActionThrowAway : ItemAction
{
	// Token: 0x06003123 RID: 12579 RVA: 0x0013DB98 File Offset: 0x0013BD98
	public ItemActionThrowAway()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.35f));
		texture2D.Apply();
		this.progressBarStyle = new GUIStyle();
		this.progressBarStyle.normal.background = texture2D;
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x0013DBF8 File Offset: 0x0013BDF8
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.defaultThrowStrength = 1.1f;
		_props.ParseFloat("Throw_strength_default", ref this.defaultThrowStrength);
		this.maxThrowStrength = 5f;
		_props.ParseFloat("Throw_strength_max", ref this.maxThrowStrength);
		this.maxStrainTime = 2f;
		_props.ParseFloat("Max_strain_time", ref this.maxStrainTime);
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x0013DC60 File Offset: 0x0013BE60
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionThrowAway.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x0013DC6C File Offset: 0x0013BE6C
	public override void OnScreenOverlay(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		EntityPlayerLocal entityPlayerLocal = myInventoryData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			LocalPlayerUI playerUI = entityPlayerLocal.PlayerUI;
			if (!myInventoryData.isCooldown && myInventoryData.m_bActivated && Time.time - myInventoryData.m_ActivateTime > 0.2f)
			{
				float num = Mathf.Min(this.maxStrainTime, Time.time - myInventoryData.m_ActivateTime) / this.maxStrainTime;
				XUiC_ThrowPower.Status(playerUI, num);
				return;
			}
			XUiC_ThrowPower.Status(playerUI, -1f);
		}
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x0013DCF6 File Offset: 0x0013BEF6
	public override void StartHolding(ItemActionData _data)
	{
		this.originalType = _data.invData.holdingEntity.inventory.holdingItemItemValue.type;
		base.StartHolding(_data);
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x0013DD20 File Offset: 0x0013BF20
	public override void StopHolding(ItemActionData _actionData)
	{
		base.StopHolding(_actionData);
		this.CancelAction(_actionData);
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		myInventoryData.m_bActivated = false;
		EntityPlayerLocal entityPlayerLocal = myInventoryData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			XUiC_ThrowPower.Status(entityPlayerLocal.PlayerUI, -1f);
		}
		myInventoryData.m_LastThrowTime = -1f;
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x0013DD7C File Offset: 0x0013BF7C
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		if (myInventoryData.m_bActivated)
		{
			myInventoryData.m_bActivated = false;
			myInventoryData.m_bCanceled = true;
			myInventoryData.m_bReleased = false;
		}
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x0013DDB0 File Offset: 0x0013BFB0
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		if (!_bReleased)
		{
			if (!myInventoryData.m_bActivated)
			{
				myInventoryData.m_bActivated = true;
				myInventoryData.m_ActivateTime = Time.time;
			}
			return;
		}
		if (!myInventoryData.m_bActivated)
		{
			return;
		}
		if (myInventoryData.isCooldown)
		{
			return;
		}
		myInventoryData.m_bReleased = true;
		if (holdingEntity.inventory.holdingCount != 0)
		{
			holdingEntity.emodel.avatarController.TriggerEvent(AvatarController.itemThrownAwayTriggerHash);
		}
		if (Time.time - myInventoryData.m_ActivateTime < 0.2f || this.maxStrainTime == 0f)
		{
			myInventoryData.m_ThrowStrength = this.defaultThrowStrength;
		}
		else
		{
			myInventoryData.m_ThrowStrength = Mathf.Min(this.maxStrainTime, Time.time - myInventoryData.m_ActivateTime) / this.maxStrainTime * this.maxThrowStrength;
		}
		if (holdingEntity.inventory.holdingItemItemValue.Meta == 0 && EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			myInventoryData.m_LastThrowTime = Time.time + 1f;
			myInventoryData.m_bActivated = false;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return;
		}
		myInventoryData.m_LastThrowTime = Time.time;
		myInventoryData.m_bActivated = false;
		myInventoryData.invData.holdingEntity.RightArmAnimationAttack = true;
		if (this.soundStart != null)
		{
			myInventoryData.invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
		}
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x0013DF44 File Offset: 0x0013C144
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		return myInventoryData.m_bActivated || (myInventoryData.m_LastThrowTime > 0f && Time.time - myInventoryData.m_LastThrowTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast);
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x0013DFA8 File Offset: 0x0013C1A8
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		if (myInventoryData.isCooldown)
		{
			holdingEntity.emodel.avatarController.CancelEvent(AvatarController.itemThrownAwayTriggerHash);
			myInventoryData.isCooldown = Time.time - myInventoryData.m_LastThrowTime < this.Delay;
			if (myInventoryData.m_bActivated)
			{
				myInventoryData.m_ActivateTime = Time.time;
			}
		}
		if (holdingEntity.inventory.holdingItemItemValue.type != this.originalType)
		{
			holdingEntity.emodel.avatarController.CancelEvent(AvatarController.itemThrownAwayTriggerHash);
			myInventoryData.m_bActivated = false;
			myInventoryData.m_bReleased = false;
			return;
		}
		EntityPlayerLocal entityPlayerLocal = holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && myInventoryData.m_bActivated)
		{
			entityPlayerLocal.StartTPCameraLockTimer();
		}
		if (myInventoryData.m_bReleased)
		{
			if (holdingEntity.inventory.holdingCount == 1)
			{
				holdingEntity.emodel.avatarController.TriggerEvent(AvatarController.itemThrownAwayTriggerHash);
			}
			float rayCast = AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
			if (myInventoryData.m_LastThrowTime <= 0f || Time.time - myInventoryData.m_LastThrowTime < rayCast)
			{
				return;
			}
			myInventoryData.m_LastThrowTime = Time.time;
			myInventoryData.m_bReleased = false;
			this.throwAway(myInventoryData);
		}
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x0013E0EC File Offset: 0x0013C2EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void throwAway(ItemActionThrowAway.MyInventoryData _actionData)
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		EntityPlayerLocal entityPlayerLocal = holdingEntity as EntityPlayerLocal;
		bool flag = entityPlayerLocal != null && !entityPlayerLocal.bFirstPersonView;
		if (holdingEntity.inventory.holdingItemItemValue.Meta == 0 && EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			_actionData.m_bActivated = false;
			return;
		}
		EntityPlayerLocal entityPlayerLocal2 = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal2 != null && !entityPlayerLocal2.TPCameraCheckPassed)
		{
			return;
		}
		Vector3 lookVector = holdingEntity.GetLookVector();
		Vector3 headPosition = holdingEntity.getHeadPosition();
		Vector3 vector = ((EntityPlayerLocal)holdingEntity).GetCrosshairPosition3D(0f, 0f, headPosition);
		Ray ray;
		if (flag)
		{
			ray = new Ray(headPosition, vector - Origin.position);
		}
		else
		{
			ray = new Ray(vector - Origin.position, lookVector);
		}
		RaycastHit raycastHit;
		if (!Physics.Raycast(ray, out raycastHit, 0.28f, -555274253))
		{
			vector += 0.23f * lookVector;
			vector -= headPosition;
			GameManager.Instance.ItemDropServer(new ItemStack(holdingEntity.inventory.holdingItemItemValue.Clone(), 1), vector, Vector3.zero, lookVector * _actionData.m_ThrowStrength, holdingEntity.entityId, 60f, true, -1);
			if (!holdingEntity.inventory.DecHoldingItem(1))
			{
				_actionData.invData.holdingEntity.emodel.avatarController.CancelEvent(AvatarController.itemThrownAwayTriggerHash);
			}
		}
	}

	// Token: 0x040025EF RID: 9711
	[PublicizedFrom(EAccessModifier.Protected)]
	public const float SHORT_CLICK_TIME = 0.2f;

	// Token: 0x040025F0 RID: 9712
	[PublicizedFrom(EAccessModifier.Protected)]
	public float defaultThrowStrength;

	// Token: 0x040025F1 RID: 9713
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxThrowStrength;

	// Token: 0x040025F2 RID: 9714
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxStrainTime;

	// Token: 0x040025F3 RID: 9715
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle progressBarStyle;

	// Token: 0x040025F4 RID: 9716
	[PublicizedFrom(EAccessModifier.Private)]
	public int originalType;

	// Token: 0x020005E2 RID: 1506
	public class MyInventoryData : ItemActionData
	{
		// Token: 0x0600312F RID: 12591 RVA: 0x0013E28B File Offset: 0x0013C48B
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025F5 RID: 9717
		public float m_ActivateTime;

		// Token: 0x040025F6 RID: 9718
		public bool m_bActivated;

		// Token: 0x040025F7 RID: 9719
		public bool m_bReleased;

		// Token: 0x040025F8 RID: 9720
		public float m_LastThrowTime;

		// Token: 0x040025F9 RID: 9721
		public float m_ThrowStrength;

		// Token: 0x040025FA RID: 9722
		public bool m_bCanceled;

		// Token: 0x040025FB RID: 9723
		public bool isCooldown;
	}
}
