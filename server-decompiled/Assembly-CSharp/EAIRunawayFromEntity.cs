using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044D RID: 1101
[Preserve]
public class EAIRunawayFromEntity : EAIRunAway
{
	// Token: 0x0600219D RID: 8605 RVA: 0x000C68F8 File Offset: 0x000C4AF8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000CB4FC File Offset: 0x000C96FC
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		this.targetClasses = new List<Type>();
		string text;
		if (data.TryGetValue("class", out text))
		{
			string[] array = text.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i += 2)
			{
				Type entityType = EntityFactory.GetEntityType(array[i]);
				this.targetClasses.Add(entityType);
			}
		}
		base.GetData(data, "safeDistance", ref this.safeDistance);
		base.GetData(data, "minSneakDistance", ref this.minSneakDistance);
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000CB57B File Offset: 0x000C977B
	public override bool CanExecute()
	{
		this.FindEnemy();
		return !(this.enemy == null) && base.CanExecute();
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000CB59C File Offset: 0x000C979C
	public void FindEnemy()
	{
		this.enemy = null;
		if (this.theEntity.noisePlayer && this.theEntity.noisePlayerVolume >= 8f)
		{
			this.enemy = this.theEntity.noisePlayer;
			return;
		}
		float seeDistance = this.theEntity.GetSeeDistance();
		Bounds bounds = BoundsUtils.ExpandBounds(this.theEntity.boundingBox, seeDistance, seeDistance, seeDistance);
		for (int i = 0; i < this.targetClasses.Count; i++)
		{
			Type type = this.targetClasses[i];
			this.theEntity.world.GetEntitiesInBounds(type, bounds, EAIRunawayFromEntity.list);
			if (type == typeof(EntityPlayer))
			{
				float num = float.MaxValue;
				for (int j = 0; j < EAIRunawayFromEntity.list.Count; j++)
				{
					EntityPlayer entityPlayer = EAIRunawayFromEntity.list[j] as EntityPlayer;
					float seeDistance2 = this.manager.GetSeeDistance(entityPlayer);
					if (seeDistance2 < num && this.theEntity.CanSee(entityPlayer) && this.theEntity.CanSeeStealth(seeDistance2, entityPlayer.Stealth.lightLevel) && !entityPlayer.IsIgnoredByAI())
					{
						num = seeDistance2;
						this.enemy = entityPlayer;
					}
				}
			}
			else
			{
				float num2 = float.MaxValue;
				for (int k = 0; k < EAIRunawayFromEntity.list.Count; k++)
				{
					EntityAlive entityAlive = EAIRunawayFromEntity.list[k] as EntityAlive;
					float distanceSq = this.theEntity.GetDistanceSq(entityAlive);
					if (distanceSq <= this.minSneakDistance * this.minSneakDistance)
					{
						this.enemy = entityAlive;
						break;
					}
					if (distanceSq < num2 && this.theEntity.CanSee(entityAlive) && !entityAlive.IsIgnoredByAI())
					{
						num2 = distanceSq;
						this.enemy = entityAlive;
					}
				}
			}
			EAIRunawayFromEntity.list.Clear();
			if (this.enemy)
			{
				break;
			}
		}
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000CB786 File Offset: 0x000C9986
	public override bool Continue()
	{
		return this.theEntity.GetDistanceSq(this.enemy) < this.safeDistance * this.safeDistance && base.Continue();
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000CB7B0 File Offset: 0x000C99B0
	public override void Update()
	{
		base.Update();
		this.theEntity.navigator.setMoveSpeed(this.theEntity.IsSwimming() ? this.theEntity.GetMoveSpeed() : this.theEntity.GetMoveSpeedPanic());
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000CB7ED File Offset: 0x000C99ED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetFleeFromPos()
	{
		return this.enemy.position;
	}

	// Token: 0x0400173A RID: 5946
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Type> targetClasses;

	// Token: 0x0400173B RID: 5947
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRunNoiseVolume = 8;

	// Token: 0x0400173C RID: 5948
	[PublicizedFrom(EAccessModifier.Private)]
	public float safeDistance = 38f;

	// Token: 0x0400173D RID: 5949
	[PublicizedFrom(EAccessModifier.Private)]
	public float minSneakDistance = 3.5f;

	// Token: 0x0400173E RID: 5950
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Entity> list = new List<Entity>();
}
