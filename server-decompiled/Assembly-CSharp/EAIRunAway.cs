using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000454 RID: 1108
[Preserve]
public abstract class EAIRunAway : EAIBase
{
	// Token: 0x060021A3 RID: 8611 RVA: 0x000CBE7B File Offset: 0x000CA07B
	public EAIRunAway()
	{
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x000CBE8B File Offset: 0x000CA08B
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "fleeDistance", ref this.fleeDistance);
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000CBEA6 File Offset: 0x000CA0A6
	public override bool CanExecute()
	{
		return this.FindFleePos(this.GetFleeFromPos());
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000CBEB4 File Offset: 0x000CA0B4
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

	// Token: 0x060021A7 RID: 8615 RVA: 0x000CBF0E File Offset: 0x000CA10E
	public override bool Continue()
	{
		return this.timeoutTicks > 0;
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000CBF1C File Offset: 0x000CA11C
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

	// Token: 0x060021A9 RID: 8617 RVA: 0x000CC031 File Offset: 0x000CA231
	public override void Reset()
	{
		this.enemy = null;
	}

	// Token: 0x060021AA RID: 8618 RVA: 0x000CC03C File Offset: 0x000CA23C
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

	// Token: 0x060021AB RID: 8619 RVA: 0x000CC0AC File Offset: 0x000CA2AC
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

	// Token: 0x060021AC RID: 8620
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Vector3 GetFleeFromPos();

	// Token: 0x060021AD RID: 8621 RVA: 0x000CC100 File Offset: 0x000CA300
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

	// Token: 0x0400174F RID: 5967
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive enemy;

	// Token: 0x04001750 RID: 5968
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetPos;

	// Token: 0x04001751 RID: 5969
	[PublicizedFrom(EAccessModifier.Private)]
	public int timeoutTicks;

	// Token: 0x04001752 RID: 5970
	[PublicizedFrom(EAccessModifier.Private)]
	public int fleeTicks;

	// Token: 0x04001753 RID: 5971
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathTicks;

	// Token: 0x04001754 RID: 5972
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkedPath;

	// Token: 0x04001755 RID: 5973
	[PublicizedFrom(EAccessModifier.Private)]
	public int fleeDistance = 20;
}
