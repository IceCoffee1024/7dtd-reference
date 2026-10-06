using System;
using Audio;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class AnimationEventBridge : RootTransformRefEntity
{
	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600035C RID: 860 RVA: 0x0001895C File Offset: 0x00016B5C
	public EntityAlive entity
	{
		get
		{
			if (!this._entity && this.RootTransform)
			{
				this._entity = this.RootTransform.GetComponent<EntityAlive>();
			}
			return this._entity;
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0001898F File Offset: 0x00016B8F
	public void PlaySound(AnimationEvent ae)
	{
		this.playSound(ae.stringParameter, ae);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000189A0 File Offset: 0x00016BA0
	public void playSound(string name, AnimationEvent ae = null)
	{
		if (this.entity != null)
		{
			EntityPlayer entityPlayer = this.entity as EntityPlayer;
			if (entityPlayer != null && entityPlayer.IsReloadCancelled())
			{
				return;
			}
			this.entity.PlayOneShot(name, false, true, false, ae, 1f);
		}
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000189F0 File Offset: 0x00016BF0
	public void PlayWeaponSound(AnimationEvent ae)
	{
		EntityPlayer entityPlayer = this.entity as EntityPlayer;
		if (entityPlayer != null && entityPlayer.inventory.holdingItem.HoldType.Value == ae.intParameter)
		{
			this.playSound(ae.stringParameter, ae);
		}
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00018A36 File Offset: 0x00016C36
	public void PlayLocalSound(string name)
	{
		this.playSound(name, null);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00018A40 File Offset: 0x00016C40
	public void PlayStepSound(AnimationEvent _animEvent)
	{
		if (this.entity && _animEvent.animatorClipInfo.weight >= 0.3f)
		{
			float time = Time.time;
			if (time - this.lastTimeStepPlayed >= 0.1f)
			{
				this.lastTimeStepPlayed = time;
				float num = _animEvent.floatParameter;
				if (num == 0f)
				{
					num = 1f;
				}
				this.entity.PlayStepSound(num);
			}
		}
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00018AAC File Offset: 0x00016CAC
	public void DeathImpactLight()
	{
		if (this.entity != null && this.entity.IsDead())
		{
			Manager.Play(this.entity, "impactbodylight", 1f, false);
		}
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00018AE0 File Offset: 0x00016CE0
	public void DeathImpactHeavy()
	{
		if (this.entity != null && this.entity.IsDead())
		{
			Manager.Play(this.entity, "impactbodyheavy", 1f, false);
		}
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00018B14 File Offset: 0x00016D14
	public void HideHoldingItem()
	{
		EntityAlive entity = this.entity;
		if (entity && entity.emodel)
		{
			entity.ShowHoldingItem(false);
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00018B44 File Offset: 0x00016D44
	public void ShowHoldingItem()
	{
		EntityAlive entity = this.entity;
		if (entity && entity.emodel)
		{
			entity.ShowHoldingItem(true);
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00018B74 File Offset: 0x00016D74
	public void Hit()
	{
		Entity entity = this.entity;
		if (entity && entity.emodel && entity.emodel.avatarController)
		{
			entity.emodel.avatarController.SetAttackImpact();
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00018BC0 File Offset: 0x00016DC0
	public void ConsumeComplete()
	{
		EntityPlayerLocal entityPlayerLocal = this.entity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			ItemClass holdingItem = entityPlayerLocal.inventory.holdingItem;
			for (int i = 0; i < holdingItem.Actions.Length; i++)
			{
				ItemActionEat itemActionEat = holdingItem.Actions[i] as ItemActionEat;
				if (itemActionEat != null && itemActionEat.PercentDone(entityPlayerLocal.inventory.holdingItemData.actionData[i]) > 0f)
				{
					itemActionEat.Completed(entityPlayerLocal.inventory.holdingItemData.actionData[i]);
					return;
				}
			}
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00018C54 File Offset: 0x00016E54
	public void HeldEntityStress()
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			ItemClassHeldEntity itemClassHeldEntity = primaryPlayer.inventory.holdingItem as ItemClassHeldEntity;
			if (itemClassHeldEntity != null)
			{
				itemClassHeldEntity.PlayStressSound();
			}
		}
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00018C94 File Offset: 0x00016E94
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		EntityAlive entity = this.entity;
		if (entity != null && entity.emodel != null)
		{
			entity.emodel.avatarController.SetCrouching(entity.IsCrouching);
			entity.SetVehiclePoseMode(entity.vehiclePoseMode);
		}
	}

	// Token: 0x040003CE RID: 974
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive _entity;

	// Token: 0x040003CF RID: 975
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeStepPlayed;
}
