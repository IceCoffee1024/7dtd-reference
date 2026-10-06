using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044F RID: 1103
[Preserve]
public class EAIPathTest : EAIBase
{
	// Token: 0x0600218D RID: 8589 RVA: 0x000CB2F3 File Offset: 0x000C94F3
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
		this.banditEntity = _theEntity as EntityBandit;
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000CB30F File Offset: 0x000C950F
	public void SetTargetMove(Vector3 _targetWorld, EntityAlive _targetYaw, bool _canBreakBlocks)
	{
		if (this.banditEntity != null)
		{
			this.targetWorldPosition = _targetWorld;
			this.targetYaw = _targetYaw;
			this.newTarget = true;
			this.cancel = false;
			this.canBreakBlocks = _canBreakBlocks;
		}
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x000CB342 File Offset: 0x000C9542
	public void CancelTargetMove()
	{
		this.targetWorldPosition = Vector3.zero;
		this.newTarget = false;
		this.cancel = true;
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x000CB35D File Offset: 0x000C955D
	public override bool CanExecute()
	{
		return this.newTarget;
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000CB365 File Offset: 0x000C9565
	public override bool Continue()
	{
		return !this.cancel;
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000CB370 File Offset: 0x000C9570
	public override void Update()
	{
		base.Update();
		if (this.newTarget)
		{
			this.newTarget = false;
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(this.theEntity, this.targetWorldPosition, this.canBreakBlocks, this.theEntity.GetMoveSpeed(), null);
			PathFinderThread.Instance.FindPath(pathInfoSingleTarget);
			if (this.banditEntity != null)
			{
				this.banditEntity.focusBody.SetFocus(FocusPriority.Highest, new AIFocusBody(this.targetYaw)
				{
					ConditionDistance = new AIFocusConditionDistance(5f, this.targetWorldPosition)
				});
			}
		}
		Vector3 vector = this.targetWorldPosition - Origin.position;
		Debug.DrawLine(vector, vector + Vector3.up * 2f, Color.blue);
		PathEntity path = this.theEntity.navigator.getPath();
		if (path != null)
		{
			for (int i = 0; i < path.getCurrentPathLength() - 1; i++)
			{
				Vector3 vector2 = path.getPathPointFromIndex(i).projectedLocation - Origin.position;
				Color color = new Color(1f, 1f, 0.1f);
				if (i < path.getCurrentPathIndex())
				{
					color = new Color(0.3f, 0.3f, 0f);
				}
				Utils.DrawLine(vector2, path.getPathPointFromIndex(i + 1).projectedLocation - Origin.position, color, color * 0.5f, 3, 0.1f);
			}
		}
	}

	// Token: 0x04001724 RID: 5924
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityBandit banditEntity;

	// Token: 0x04001725 RID: 5925
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetWorldPosition;

	// Token: 0x04001726 RID: 5926
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetYaw;

	// Token: 0x04001727 RID: 5927
	[PublicizedFrom(EAccessModifier.Private)]
	public bool newTarget;

	// Token: 0x04001728 RID: 5928
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancel;

	// Token: 0x04001729 RID: 5929
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canBreakBlocks;
}
