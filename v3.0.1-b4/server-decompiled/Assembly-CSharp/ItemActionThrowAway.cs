using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005DB RID: 1499
[Preserve]
public class ItemActionThrowAway : ItemAction
{
	// Token: 0x060030AE RID: 12462 RVA: 0x0013C060 File Offset: 0x0013A260
	public ItemActionThrowAway()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.35f));
		texture2D.Apply();
		this.progressBarStyle = new GUIStyle();
		this.progressBarStyle.normal.background = texture2D;
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x0013C0C0 File Offset: 0x0013A2C0
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

	// Token: 0x060030B0 RID: 12464 RVA: 0x0013C128 File Offset: 0x0013A328
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionThrowAway.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x0013C134 File Offset: 0x0013A334
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

	// Token: 0x060030B2 RID: 12466 RVA: 0x0013C1BE File Offset: 0x0013A3BE
	public override void StartHolding(ItemActionData _data)
	{
		this.originalType = _data.invData.holdingEntity.inventory.holdingItemItemValue.type;
		base.StartHolding(_data);
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x0013C1E8 File Offset: 0x0013A3E8
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

	// Token: 0x060030B4 RID: 12468 RVA: 0x0013C244 File Offset: 0x0013A444
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

	// Token: 0x060030B5 RID: 12469 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x0013C278 File Offset: 0x0013A478
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

	// Token: 0x060030B7 RID: 12471 RVA: 0x0013C40C File Offset: 0x0013A60C
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		return myInventoryData.m_bActivated || (myInventoryData.m_LastThrowTime > 0f && Time.time - myInventoryData.m_LastThrowTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast);
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x0013C470 File Offset: 0x0013A670
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

	// Token: 0x060030B9 RID: 12473 RVA: 0x0013C5B4 File Offset: 0x0013A7B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void throwAway(ItemActionThrowAway.MyInventoryData _actionData)
	{
		ItemInventoryData invData = _actionData.invData;
		EntityAlive holdingEntity = invData.holdingEntity;
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
			invData.gameManager.ItemDropServer(new ItemStack(holdingEntity.inventory.holdingItemItemValue, 1), vector, Vector3.zero, lookVector * _actionData.m_ThrowStrength, holdingEntity.entityId, 60f, true, -1);
			if (!holdingEntity.inventory.DecHoldingItem(1))
			{
				_actionData.invData.holdingEntity.emodel.avatarController.CancelEvent(AvatarController.itemThrownAwayTriggerHash);
			}
		}
	}

	// Token: 0x040025C1 RID: 9665
	[PublicizedFrom(EAccessModifier.Protected)]
	public const float SHORT_CLICK_TIME = 0.2f;

	// Token: 0x040025C2 RID: 9666
	[PublicizedFrom(EAccessModifier.Protected)]
	public float defaultThrowStrength;

	// Token: 0x040025C3 RID: 9667
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxThrowStrength;

	// Token: 0x040025C4 RID: 9668
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxStrainTime;

	// Token: 0x040025C5 RID: 9669
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle progressBarStyle;

	// Token: 0x040025C6 RID: 9670
	[PublicizedFrom(EAccessModifier.Private)]
	public int originalType;

	// Token: 0x020005DC RID: 1500
	public class MyInventoryData : ItemActionData
	{
		// Token: 0x060030BA RID: 12474 RVA: 0x0013C755 File Offset: 0x0013A955
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025C7 RID: 9671
		public float m_ActivateTime;

		// Token: 0x040025C8 RID: 9672
		public bool m_bActivated;

		// Token: 0x040025C9 RID: 9673
		public bool m_bReleased;

		// Token: 0x040025CA RID: 9674
		public float m_LastThrowTime;

		// Token: 0x040025CB RID: 9675
		public float m_ThrowStrength;

		// Token: 0x040025CC RID: 9676
		public bool m_bCanceled;

		// Token: 0x040025CD RID: 9677
		public bool isCooldown;
	}
}
