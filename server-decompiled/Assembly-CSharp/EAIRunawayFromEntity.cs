using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000455 RID: 1109
[Preserve]
public class EAIRunawayFromEntity : EAIRunAway
{
	// Token: 0x060021AE RID: 8622 RVA: 0x000C7564 File Offset: 0x000C5764
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x000CC168 File Offset: 0x000CA368
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		string text;
		if (data.TryGetValue("flags", out text))
		{
			EntityClass.ParseEntityFlags(text, ref this.flags);
		}
		if (data.TryGetValue("safeFlags", out text))
		{
			EntityClass.ParseEntityFlags(text, ref this.safeFlags);
		}
		base.GetData(data, "safeDistance", ref this.safeDistance);
		base.GetData(data, "dangerDistance", ref this.dangerDistance);
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x000CC1D6 File Offset: 0x000CA3D6
	public override bool CanExecute()
	{
		this.FindEnemy();
		return this.enemy && base.CanExecute();
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x000CC1F4 File Offset: 0x000CA3F4
	public void FindEnemy()
	{
		this.enemy = null;
		if (this.theEntity.noisePlayer && this.theEntity.noisePlayerVolume >= 8f)
		{
			this.enemy = this.theEntity.noisePlayer;
			return;
		}
		float num = Utils.FastMin(this.theEntity.GetSeeDistance(), this.safeDistance) * 0.8f;
		this.theEntity.world.GetEntitiesAround(EntityFlags.None, this.safeFlags, this.theEntity.position, num, EAIRunawayFromEntity.entityList);
		float num2 = float.MaxValue;
		for (int i = 0; i < EAIRunawayFromEntity.entityList.Count; i++)
		{
			Entity entity = EAIRunawayFromEntity.entityList[i];
			if ((entity.entityFlags & this.flags) != EntityFlags.None)
			{
				EntityPlayer entityPlayer = entity as EntityPlayer;
				if (entityPlayer != null)
				{
					float seeDistance = this.manager.GetSeeDistance(entityPlayer);
					float num3 = seeDistance * seeDistance;
					if (num3 < num2 && this.theEntity.CanSee(entityPlayer) && this.theEntity.CanSeeStealth(seeDistance, entityPlayer.Stealth.lightLevel) && !entityPlayer.IsIgnoredByAI())
					{
						num2 = num3;
						this.enemy = entityPlayer;
					}
				}
				else
				{
					EntityAlive entityAlive = entity as EntityAlive;
					if (entityAlive != null)
					{
						float distanceSq = this.theEntity.GetDistanceSq(entityAlive);
						if (distanceSq < num2 && (distanceSq <= this.dangerDistance * this.dangerDistance || (this.theEntity.CanSee(entityAlive) && !entityAlive.IsIgnoredByAI())))
						{
							num2 = distanceSq;
							this.enemy = entityAlive;
						}
					}
				}
			}
		}
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x000CC383 File Offset: 0x000CA583
	public override bool Continue()
	{
		return this.theEntity.GetDistanceSq(this.enemy) < this.safeDistance * this.safeDistance && base.Continue();
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x000CC3AD File Offset: 0x000CA5AD
	public override void Update()
	{
		base.Update();
		this.theEntity.navigator.setMoveSpeed(this.theEntity.IsSwimming() ? this.theEntity.GetMoveSpeed() : this.theEntity.GetMoveSpeedPanic());
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x000CC3EA File Offset: 0x000CA5EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetFleeFromPos()
	{
		return this.enemy.position;
	}

	// Token: 0x04001756 RID: 5974
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRunNoiseVolume = 8;

	// Token: 0x04001757 RID: 5975
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags flags;

	// Token: 0x04001758 RID: 5976
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags safeFlags;

	// Token: 0x04001759 RID: 5977
	[PublicizedFrom(EAccessModifier.Private)]
	public float safeDistance = 38f;

	// Token: 0x0400175A RID: 5978
	[PublicizedFrom(EAccessModifier.Private)]
	public float dangerDistance = 4.5f;

	// Token: 0x0400175B RID: 5979
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Entity> entityList = new List<Entity>();
}
