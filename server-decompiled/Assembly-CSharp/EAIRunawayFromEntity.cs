using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000453 RID: 1107
[Preserve]
public class EAIRunawayFromEntity : EAIRunAway
{
	// Token: 0x060021A9 RID: 8617 RVA: 0x000C71B8 File Offset: 0x000C53B8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060021AA RID: 8618 RVA: 0x000CBDBC File Offset: 0x000C9FBC
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

	// Token: 0x060021AB RID: 8619 RVA: 0x000CBE2A File Offset: 0x000CA02A
	public override bool CanExecute()
	{
		this.FindEnemy();
		return this.enemy && base.CanExecute();
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x000CBE48 File Offset: 0x000CA048
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

	// Token: 0x060021AD RID: 8621 RVA: 0x000CBFD7 File Offset: 0x000CA1D7
	public override bool Continue()
	{
		return this.theEntity.GetDistanceSq(this.enemy) < this.safeDistance * this.safeDistance && base.Continue();
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x000CC001 File Offset: 0x000CA201
	public override void Update()
	{
		base.Update();
		this.theEntity.navigator.setMoveSpeed(this.theEntity.IsSwimming() ? this.theEntity.GetMoveSpeed() : this.theEntity.GetMoveSpeedPanic());
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x000CC03E File Offset: 0x000CA23E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetFleeFromPos()
	{
		return this.enemy.position;
	}

	// Token: 0x04001746 RID: 5958
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRunNoiseVolume = 8;

	// Token: 0x04001747 RID: 5959
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags flags;

	// Token: 0x04001748 RID: 5960
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags safeFlags;

	// Token: 0x04001749 RID: 5961
	[PublicizedFrom(EAccessModifier.Private)]
	public float safeDistance = 38f;

	// Token: 0x0400174A RID: 5962
	[PublicizedFrom(EAccessModifier.Private)]
	public float dangerDistance = 4.5f;

	// Token: 0x0400174B RID: 5963
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Entity> entityList = new List<Entity>();
}
