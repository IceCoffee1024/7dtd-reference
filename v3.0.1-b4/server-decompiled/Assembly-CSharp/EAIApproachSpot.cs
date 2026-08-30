using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200042B RID: 1067
[Preserve]
public class EAIApproachSpot : EAIBase
{
	// Token: 0x060020CB RID: 8395 RVA: 0x000C5027 File Offset: 0x000C3227
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 0.1f;
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000C64E8 File Offset: 0x000C46E8
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

	// Token: 0x060020CD RID: 8397 RVA: 0x000C6541 File Offset: 0x000C4741
	public override void Start()
	{
		this.hadPath = false;
		this.updatePath();
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x000C6550 File Offset: 0x000C4750
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

	// Token: 0x060020CF RID: 8399 RVA: 0x000C660C File Offset: 0x000C480C
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

	// Token: 0x060020D0 RID: 8400 RVA: 0x000C6680 File Offset: 0x000C4880
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

	// Token: 0x060020D1 RID: 8401 RVA: 0x000C6700 File Offset: 0x000C4900
	public override void Reset()
	{
		this.theEntity.moveHelper.Stop();
		this.theEntity.SetLookPosition(Vector3.zero);
		this.manager.lookTime = 5f + base.RandomFloat * 3f;
		this.manager.interestDistance = 2f;
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x000C675C File Offset: 0x000C495C
	public override string ToString()
	{
		return string.Format("{0}, {1} dist{2}", base.ToString(), this.theEntity.navigator.noPathAndNotPlanningOne() ? "(-path)" : (this.theEntity.navigator.noPath() ? "(!path)" : ""), (this.theEntity.position - this.seekPos).magnitude.ToCultureInvariantString());
	}

	// Token: 0x0400167C RID: 5756
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cInvestigateChangeDist = 2f;

	// Token: 0x0400167D RID: 5757
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCloseDist = 2f;

	// Token: 0x0400167E RID: 5758
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTimeMin = 5f;

	// Token: 0x0400167F RID: 5759
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTimeMax = 8f;

	// Token: 0x04001680 RID: 5760
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 investigatePos;

	// Token: 0x04001681 RID: 5761
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 seekPos;

	// Token: 0x04001682 RID: 5762
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadPath;

	// Token: 0x04001683 RID: 5763
	[PublicizedFrom(EAccessModifier.Private)]
	public int investigateTicks;

	// Token: 0x04001684 RID: 5764
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathRecalculateTicks;
}
