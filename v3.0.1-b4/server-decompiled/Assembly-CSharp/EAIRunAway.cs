using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044C RID: 1100
[Preserve]
public abstract class EAIRunAway : EAIBase
{
	// Token: 0x06002192 RID: 8594 RVA: 0x000CB20F File Offset: 0x000C940F
	public EAIRunAway()
	{
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000CB21F File Offset: 0x000C941F
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "fleeDistance", ref this.fleeDistance);
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x000CB23A File Offset: 0x000C943A
	public override bool CanExecute()
	{
		return this.FindFleePos(this.GetFleeFromPos());
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000CB248 File Offset: 0x000C9448
	public override void Start()
	{
		this.timeoutTicks = (30 + base.GetRandom(20)) * 20;
		PathFinderThread.Instance.RemovePathsFor(this.theEntity.entityId);
		string soundAlert = this.theEntity.GetSoundAlert();
		if (soundAlert != null)
		{
			this.theEntity.PlayOneShot(soundAlert, false, false, false, null, 1f);
		}
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000CB2A2 File Offset: 0x000C94A2
	public override bool Continue()
	{
		return this.timeoutTicks > 0;
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000CB2B0 File Offset: 0x000C94B0
	public override void Update()
	{
		this.timeoutTicks--;
		PathEntity path = this.theEntity.navigator.getPath();
		if (!this.checkedPath && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
		{
			this.checkedPath = true;
			if (path != null)
			{
				Vector3 rawEndPos = path.rawEndPos;
				if (this.theEntity.GetDistanceSq(rawEndPos) < 1.21f)
				{
					this.FindRandomPos();
				}
			}
		}
		if (this.checkedPath && path != null && path.getCurrentPathLength() >= 2 && path.NodeCountRemaining() <= 2)
		{
			this.fleeTicks = 0;
		}
		int num = this.fleeTicks - 1;
		this.fleeTicks = num;
		if (num <= 0)
		{
			Vector3 fleeFromPos = this.GetFleeFromPos();
			this.FindFleePos(fleeFromPos);
		}
		num = this.pathTicks - 1;
		this.pathTicks = num;
		if (num <= 0 && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
		{
			this.pathTicks = 60;
			this.theEntity.FindPath(this.targetPos, this.theEntity.GetMoveSpeed(), false, this);
			this.checkedPath = false;
		}
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x000CB3C5 File Offset: 0x000C95C5
	public override void Reset()
	{
		this.enemy = null;
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x000CB3D0 File Offset: 0x000C95D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool FindFleePos(Vector3 fleeFromPos)
	{
		Vector3 vector = this.theEntity.position - fleeFromPos;
		Vector3 vector2 = RandomPositionGenerator.CalcPositionInDirection(this.theEntity, this.theEntity.position, vector, (float)this.fleeDistance, 80f);
		if (vector2.y == 0f)
		{
			return false;
		}
		this.targetPos = vector2;
		this.fleeTicks = 80;
		this.pathTicks = 0;
		this.checkedPath = false;
		return true;
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000CB440 File Offset: 0x000C9640
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool FindRandomPos()
	{
		Vector3 vector = RandomPositionGenerator.CalcAround(this.theEntity, this.fleeDistance, 0);
		if (vector.y == 0f)
		{
			this.fleeTicks = 0;
			return false;
		}
		this.targetPos = vector;
		this.fleeTicks = 80;
		this.pathTicks = 0;
		this.checkedPath = false;
		return true;
	}

	// Token: 0x0600219B RID: 8603
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Vector3 GetFleeFromPos();

	// Token: 0x0600219C RID: 8604 RVA: 0x000CB494 File Offset: 0x000C9694
	public override string ToString()
	{
		return string.Format("{0}, flee {1}, timeout {2}, {3}", new object[]
		{
			base.ToString(),
			this.fleeTicks,
			this.timeoutTicks,
			(this.enemy != null) ? this.enemy.GetDebugName() : ""
		});
	}

	// Token: 0x04001733 RID: 5939
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive enemy;

	// Token: 0x04001734 RID: 5940
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetPos;

	// Token: 0x04001735 RID: 5941
	[PublicizedFrom(EAccessModifier.Private)]
	public int timeoutTicks;

	// Token: 0x04001736 RID: 5942
	[PublicizedFrom(EAccessModifier.Private)]
	public int fleeTicks;

	// Token: 0x04001737 RID: 5943
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathTicks;

	// Token: 0x04001738 RID: 5944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkedPath;

	// Token: 0x04001739 RID: 5945
	[PublicizedFrom(EAccessModifier.Private)]
	public int fleeDistance = 20;
}
