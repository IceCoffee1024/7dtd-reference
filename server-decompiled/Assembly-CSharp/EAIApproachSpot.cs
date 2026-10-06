using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000431 RID: 1073
[Preserve]
public class EAIApproachSpot : EAIBase
{
	// Token: 0x060020D7 RID: 8407 RVA: 0x000C58E7 File Offset: 0x000C3AE7
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 0.1f;
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x000C6DA8 File Offset: 0x000C4FA8
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

	// Token: 0x060020D9 RID: 8409 RVA: 0x000C6E01 File Offset: 0x000C5001
	public override void Start()
	{
		this.hadPath = false;
		this.updatePath();
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x000C6E10 File Offset: 0x000C5010
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

	// Token: 0x060020DB RID: 8411 RVA: 0x000C6ECC File Offset: 0x000C50CC
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

	// Token: 0x060020DC RID: 8412 RVA: 0x000C6F40 File Offset: 0x000C5140
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

	// Token: 0x060020DD RID: 8413 RVA: 0x000C6FC0 File Offset: 0x000C51C0
	public override void Reset()
	{
		this.theEntity.moveHelper.Stop();
		this.theEntity.SetLookPosition(Vector3.zero);
		this.manager.lookTime = 5f + base.RandomFloat * 3f;
		this.manager.interestDistance = 2f;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000C701C File Offset: 0x000C521C
	public override string ToString()
	{
		return string.Format("{0}, {1} dist{2}", base.ToString(), this.theEntity.navigator.noPathAndNotPlanningOne() ? "(-path)" : (this.theEntity.navigator.noPath() ? "(!path)" : ""), (this.theEntity.position - this.seekPos).magnitude.ToCultureInvariantString());
	}

	// Token: 0x04001688 RID: 5768
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cInvestigateChangeDist = 2f;

	// Token: 0x04001689 RID: 5769
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCloseDist = 2f;

	// Token: 0x0400168A RID: 5770
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTimeMin = 5f;

	// Token: 0x0400168B RID: 5771
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTimeMax = 8f;

	// Token: 0x0400168C RID: 5772
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 investigatePos;

	// Token: 0x0400168D RID: 5773
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 seekPos;

	// Token: 0x0400168E RID: 5774
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadPath;

	// Token: 0x0400168F RID: 5775
	[PublicizedFrom(EAccessModifier.Private)]
	public int investigateTicks;

	// Token: 0x04001690 RID: 5776
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathRecalculateTicks;
}
