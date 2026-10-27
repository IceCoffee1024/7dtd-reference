using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000433 RID: 1075
[Preserve]
public class EAIApproachSpot : EAIBase
{
	// Token: 0x060020DC RID: 8412 RVA: 0x000C5C93 File Offset: 0x000C3E93
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 0.1f;
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000C7154 File Offset: 0x000C5354
	public override bool CanExecute()
	{
		if (!this.theEntity.HasInvestigatePosition)
		{
			return false;
		}
		if (this.theEntity.IsSleeping)
		{
			return false;
		}
		this.investigatePos = this.theEntity.InvestigatePosition;
		this.seekPos = this.theEntity.world.FindSupportingBlockPos(this.investigatePos);
		return true;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000C71AD File Offset: 0x000C53AD
	public override void Start()
	{
		this.hadPath = false;
		this.updatePath();
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000C71BC File Offset: 0x000C53BC
	public override bool Continue()
	{
		PathEntity path = this.theEntity.navigator.getPath();
		if (this.hadPath && path == null)
		{
			return false;
		}
		int num = this.investigateTicks + 1;
		this.investigateTicks = num;
		if (num > 40)
		{
			this.investigateTicks = 0;
			if (!this.theEntity.HasInvestigatePosition)
			{
				return false;
			}
			if ((this.investigatePos - this.theEntity.InvestigatePosition).sqrMagnitude >= 4f)
			{
				return false;
			}
		}
		if ((this.seekPos - this.theEntity.position).sqrMagnitude <= 4f || (path != null && path.isFinished()))
		{
			this.theEntity.ClearInvestigatePosition();
			return false;
		}
		return true;
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000C7278 File Offset: 0x000C5478
	public override void Update()
	{
		if (this.theEntity.navigator.getPath() != null)
		{
			this.hadPath = true;
			this.theEntity.moveHelper.CalcIfUnreachablePos();
		}
		Vector3 vector = this.investigatePos;
		vector.y += 0.8f;
		this.theEntity.SetLookPosition(vector);
		int num = this.pathRecalculateTicks - 1;
		this.pathRecalculateTicks = num;
		if (num <= 0)
		{
			this.updatePath();
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000C72EC File Offset: 0x000C54EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePath()
	{
		if (this.theEntity.IsScoutZombie)
		{
			AstarManager.Instance.AddLocationLine(this.theEntity.position, this.seekPos, 32);
		}
		if (PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
		{
			return;
		}
		this.pathRecalculateTicks = 40 + base.GetRandom(20);
		this.theEntity.FindPath(this.seekPos, this.theEntity.GetMoveSpeedAggro(), true, this);
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000C736C File Offset: 0x000C556C
	public override void Reset()
	{
		this.theEntity.moveHelper.Stop();
		this.theEntity.SetLookPosition(Vector3.zero);
		this.manager.lookTime = 5f + base.RandomFloat * 3f;
		this.manager.interestDistance = 2f;
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000C73C8 File Offset: 0x000C55C8
	public override string ToString()
	{
		return string.Format("{0}, {1} dist{2}", base.ToString(), this.theEntity.navigator.noPathAndNotPlanningOne() ? "(-path)" : (this.theEntity.navigator.noPath() ? "(!path)" : ""), (this.theEntity.position - this.seekPos).magnitude.ToCultureInvariantString());
	}

	// Token: 0x04001698 RID: 5784
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cInvestigateChangeDist = 2f;

	// Token: 0x04001699 RID: 5785
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCloseDist = 2f;

	// Token: 0x0400169A RID: 5786
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTimeMin = 5f;

	// Token: 0x0400169B RID: 5787
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTimeMax = 8f;

	// Token: 0x0400169C RID: 5788
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 investigatePos;

	// Token: 0x0400169D RID: 5789
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 seekPos;

	// Token: 0x0400169E RID: 5790
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadPath;

	// Token: 0x0400169F RID: 5791
	[PublicizedFrom(EAccessModifier.Private)]
	public int investigateTicks;

	// Token: 0x040016A0 RID: 5792
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathRecalculateTicks;
}
