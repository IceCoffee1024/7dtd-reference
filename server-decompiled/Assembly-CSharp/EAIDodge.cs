using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200043B RID: 1083
[Preserve]
public class EAIDodge : EAIBase
{
	// Token: 0x06002112 RID: 8466 RVA: 0x000C7EB7 File Offset: 0x000C60B7
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.executeDelay = 0.1f;
		this.cooldown = 3f;
		this.actionkDuration = 1f;
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000C7EE4 File Offset: 0x000C60E4
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

	// Token: 0x06002114 RID: 8468 RVA: 0x000C7F80 File Offset: 0x000C6180
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

	// Token: 0x06002115 RID: 8469 RVA: 0x000C8087 File Offset: 0x000C6287
	public override void Start()
	{
		this.actionTime = 0f;
		this.theEntity.emodel.avatarController.StartAnimationDodge(base.Random.RandomFloat);
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000C80B4 File Offset: 0x000C62B4
	public override bool Continue()
	{
		return this.entityTarget && this.entityTarget.IsAlive() && this.actionTime < this.actionkDuration && this.theEntity.hasBeenAttackedTime <= 0;
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000C80F4 File Offset: 0x000C62F4
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

	// Token: 0x06002118 RID: 8472 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Reset()
	{
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x000C8150 File Offset: 0x000C6350
	[PublicizedFrom(EAccessModifier.Private)]
	public bool InRange()
	{
		float distanceSq = this.entityTarget.GetDistanceSq(this.theEntity);
		return distanceSq >= this.minRange * this.minRange && distanceSq <= this.maxRange * this.maxRange;
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x000C8194 File Offset: 0x000C6394
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

	// Token: 0x040016C1 RID: 5825
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tags;

	// Token: 0x040016C2 RID: 5826
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxXZDistance = 100f;

	// Token: 0x040016C3 RID: 5827
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseCooldown;

	// Token: 0x040016C4 RID: 5828
	[PublicizedFrom(EAccessModifier.Private)]
	public float cooldown;

	// Token: 0x040016C5 RID: 5829
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entityTarget;

	// Token: 0x040016C6 RID: 5830
	[PublicizedFrom(EAccessModifier.Private)]
	public float actionTime;

	// Token: 0x040016C7 RID: 5831
	[PublicizedFrom(EAccessModifier.Private)]
	public float actionkDuration;

	// Token: 0x040016C8 RID: 5832
	[PublicizedFrom(EAccessModifier.Private)]
	public float minRange = 4f;

	// Token: 0x040016C9 RID: 5833
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxRange = 25f;

	// Token: 0x040016CA RID: 5834
	[PublicizedFrom(EAccessModifier.Private)]
	public float unreachableRange;

	// Token: 0x040016CB RID: 5835
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Entity> entityList = new List<Entity>();
}
