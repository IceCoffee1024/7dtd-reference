using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000435 RID: 1077
[Preserve]
public class EAIDodge : EAIBase
{
	// Token: 0x06002106 RID: 8454 RVA: 0x000C75F7 File Offset: 0x000C57F7
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.executeDelay = 0.1f;
		this.cooldown = 3f;
		this.actionkDuration = 1f;
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000C7624 File Offset: 0x000C5824
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		string text;
		if (data.TryGetValue("tags", out text))
		{
			this.tags = FastTags<TagGroup.Global>.Parse(text);
		}
		base.GetData(data, "maxXZDistance", ref this.maxXZDistance);
		base.GetData(data, "cooldown", ref this.baseCooldown);
		base.GetData(data, "duration", ref this.actionkDuration);
		base.GetData(data, "minRange", ref this.minRange);
		base.GetData(data, "maxRange", ref this.maxRange);
		base.GetData(data, "unreachableRange", ref this.unreachableRange);
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x000C76C0 File Offset: 0x000C58C0
	public override bool CanExecute()
	{
		if (this.theEntity.IsDancing)
		{
			return false;
		}
		if (this.cooldown > 0f)
		{
			this.cooldown -= this.executeWaitTime;
			return false;
		}
		this.theEntity.world.GetEntitiesInBounds(this.tags, BoundsUtils.ExpandBounds(this.theEntity.boundingBox, this.maxXZDistance, 8f, this.maxXZDistance), EAIDodge.entityList);
		this.entityTarget = null;
		for (int i = 0; i < EAIDodge.entityList.Count; i++)
		{
			EntityAlive entityAlive = EAIDodge.entityList[i] as EntityAlive;
			if (entityAlive && !entityAlive.IsDead() && entityAlive.emodel.avatarController.IsAnimationToDodge())
			{
				this.entityTarget = entityAlive;
				break;
			}
		}
		EAIDodge.entityList.Clear();
		return !(this.entityTarget == null) && this.InRange() && this.theEntity.CanSee(this.entityTarget);
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000C77C7 File Offset: 0x000C59C7
	public override void Start()
	{
		this.actionTime = 0f;
		this.theEntity.emodel.avatarController.StartAnimationDodge(base.Random.RandomFloat);
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000C77F4 File Offset: 0x000C59F4
	public override bool Continue()
	{
		return this.entityTarget && this.entityTarget.IsAlive() && this.actionTime < this.actionkDuration && this.theEntity.hasBeenAttackedTime <= 0;
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000C7834 File Offset: 0x000C5A34
	public override void Update()
	{
		this.actionTime += 0.05f;
		if (this.actionTime < this.actionkDuration * 0.5f)
		{
			Vector3 headPosition = this.entityTarget.getHeadPosition();
			if (this.theEntity.IsInFrontOfMe(headPosition))
			{
				this.theEntity.SetLookPosition(headPosition);
			}
		}
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Reset()
	{
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000C7890 File Offset: 0x000C5A90
	[PublicizedFrom(EAccessModifier.Private)]
	public bool InRange()
	{
		float distanceSq = this.entityTarget.GetDistanceSq(this.theEntity);
		return distanceSq >= this.minRange * this.minRange && distanceSq <= this.maxRange * this.maxRange;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000C78D4 File Offset: 0x000C5AD4
	public override string ToString()
	{
		bool flag = this.entityTarget && this.InRange();
		return string.Format("{0} {1}, inRange{2}, Time {3}", new object[]
		{
			base.ToString(),
			this.entityTarget ? this.entityTarget.EntityName : "",
			flag,
			this.actionTime.ToCultureInvariantString("0.00")
		});
	}

	// Token: 0x040016B5 RID: 5813
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags;

	// Token: 0x040016B6 RID: 5814
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxXZDistance = 100f;

	// Token: 0x040016B7 RID: 5815
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseCooldown;

	// Token: 0x040016B8 RID: 5816
	[PublicizedFrom(EAccessModifier.Private)]
	public float cooldown;

	// Token: 0x040016B9 RID: 5817
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entityTarget;

	// Token: 0x040016BA RID: 5818
	[PublicizedFrom(EAccessModifier.Private)]
	public float actionTime;

	// Token: 0x040016BB RID: 5819
	[PublicizedFrom(EAccessModifier.Private)]
	public float actionkDuration;

	// Token: 0x040016BC RID: 5820
	[PublicizedFrom(EAccessModifier.Private)]
	public float minRange = 4f;

	// Token: 0x040016BD RID: 5821
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxRange = 25f;

	// Token: 0x040016BE RID: 5822
	[PublicizedFrom(EAccessModifier.Private)]
	public float unreachableRange;

	// Token: 0x040016BF RID: 5823
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Entity> entityList = new List<Entity>();
}
