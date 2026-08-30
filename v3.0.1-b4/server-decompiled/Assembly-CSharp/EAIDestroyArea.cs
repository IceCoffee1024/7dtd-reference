using System;
using System.Diagnostics;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000433 RID: 1075
[Preserve]
public class EAIDestroyArea : EAIBase
{
	// Token: 0x060020FA RID: 8442 RVA: 0x000C6E31 File Offset: 0x000C5031
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 0.9f + base.RandomFloat * 0.6f;
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000C6E5C File Offset: 0x000C505C
	public override bool CanExecute()
	{
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		if (!moveHelper.CanBreakBlocks)
		{
			return false;
		}
		EntityAlive attackTarget = this.theEntity.GetAttackTarget();
		if (!attackTarget)
		{
			return false;
		}
		if (this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		bool flag = this.isLookFar;
		if (moveHelper.IsDestroyAreaTryUnreachable)
		{
			moveHelper.IsDestroyAreaTryUnreachable = false;
			float num = moveHelper.UnreachablePercent;
			if (num > 0f)
			{
				if (base.RandomFloat < num)
				{
					flag = true;
					num = 0f;
				}
				moveHelper.UnreachablePercent = num * 0.5f;
			}
		}
		if (this.manager.pathCostScale < 0.65f)
		{
			float num2 = (1f - this.manager.pathCostScale * 1.5384616f) * 0.6f;
			if (base.RandomFloat < num2)
			{
				PathEntity path = this.theEntity.navigator.getPath();
				if (path != null && path.NodeCountRemaining() > 18 && (attackTarget.position - this.theEntity.position).sqrMagnitude <= 81f)
				{
					flag = true;
				}
			}
		}
		if (!flag && !moveHelper.IsUnreachableAbove)
		{
			return false;
		}
		moveHelper.IsDestroyAreaTryUnreachable = false;
		Vector3 position = this.theEntity.position;
		this.targetPos = (moveHelper.IsUnreachableSide ? moveHelper.UnreachablePos : attackTarget.position);
		this.targetPos.y = Utils.FastMoveTowards(position.y, this.targetPos.y, 2f);
		this.seekPos = Vector3.MoveTowards(position, this.targetPos, 5f);
		this.seekPos.x = this.seekPos.x + (float)(base.Random.RandomRange(11) - 5);
		this.seekPos.z = this.seekPos.z + (float)(base.Random.RandomRange(11) - 5);
		this.findDir = (int)(Mathf.Atan2(this.targetPos.x - position.x, this.targetPos.z - position.z) * 0.6366198f + 4f) & 3;
		this.findCount = ((base.Random.RandomFloat < 0.1f) ? 4 : 5);
		this.state = EAIDestroyArea.eState.FindExistingPos;
		if (base.RandomFloat < 0.18f)
		{
			this.state = EAIDestroyArea.eState.FindPos;
		}
		return true;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000C70A2 File Offset: 0x000C52A2
	public override void Start()
	{
		this.isAtPathEnd = false;
		this.delayTime = 3f;
		this.attackTimeout = 0;
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x000C70C0 File Offset: 0x000C52C0
	public void Stop()
	{
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		if (moveHelper != null)
		{
			moveHelper.IsDestroyAreaTryUnreachable = false;
		}
		this.delayTime = 0f;
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x000C70F0 File Offset: 0x000C52F0
	public override bool Continue()
	{
		if (this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		if (this.delayTime <= 0f)
		{
			return false;
		}
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		if (this.state == EAIDestroyArea.eState.FindExistingPos)
		{
			if (!moveHelper.FindExistingDestroyPos(ref this.seekPos))
			{
				this.state = EAIDestroyArea.eState.FindPos;
				return true;
			}
			this.state = EAIDestroyArea.eState.FindPath;
		}
		if (this.state == EAIDestroyArea.eState.FindPos)
		{
			Vector3 vector;
			if (this.findCount > 4)
			{
				this.findCount--;
				vector = this.targetPos;
				int num = 4;
				if (base.RandomFloat < 0.65f)
				{
					vector.x += (float)(base.Random.RandomRange(5) - 2);
					vector.z += (float)(base.Random.RandomRange(5) - 2);
					num = 7;
				}
				if (!moveHelper.FindDestroyPos(ref vector, num, this.isLookFar))
				{
					return true;
				}
			}
			else
			{
				vector = this.seekPos;
				int num2 = this.findDir * 2;
				vector.x += EAIDestroyArea.quadrants[num2];
				vector.z += EAIDestroyArea.quadrants[num2 + 1];
				if (!moveHelper.FindDestroyPos(ref vector, 8, this.isLookFar))
				{
					this.findDir = (this.findDir + 1) & 3;
					int num3 = this.findCount - 1;
					this.findCount = num3;
					if (num3 <= 0)
					{
						this.isLookFar = !this.isLookFar;
						return false;
					}
					return true;
				}
			}
			this.seekPos = vector;
			this.state = EAIDestroyArea.eState.FindPath;
		}
		if (this.state == EAIDestroyArea.eState.FindPath)
		{
			this.seekBlockPos = World.worldToBlockPos(this.seekPos);
			this.isLookFar = false;
			this.state = EAIDestroyArea.eState.WaitForPath;
			this.theEntity.navigator.clearPath();
			this.theEntity.FindPath(this.seekPos, this.theEntity.GetMoveSpeedAggro(), true, this);
			moveHelper.IsDestroyArea = true;
			return true;
		}
		if (this.state == EAIDestroyArea.eState.WaitForPath && this.theEntity.navigator.HasPath())
		{
			this.theEntity.navigator.ShortenEnd(0f);
			moveHelper.IsUnreachableAbove = true;
			this.state = EAIDestroyArea.eState.HasPath;
			this.delayTime = 15f;
		}
		if (this.state == EAIDestroyArea.eState.HasPath)
		{
			PathEntity path = this.theEntity.navigator.getPath();
			if (path != null && path.NodeCountRemaining() <= 1)
			{
				this.theEntity.navigator.clearPath();
				this.state = EAIDestroyArea.eState.EndPath;
				this.isAtPathEnd = true;
				this.delayTime = 5f + base.RandomFloat * 6f;
				moveHelper.BlockedFlags = 0;
				this.seekPos.y = Utils.FastMin(this.seekPos.y, this.theEntity.position.y + 2.1f);
				this.seekBlockPos.y = Utils.Fastfloor(this.seekPos.y);
				return true;
			}
		}
		if (this.state == EAIDestroyArea.eState.EndPath && moveHelper.BlockedFlags == 0)
		{
			if (!Voxel.BlockHit(this.hitInfo, this.seekBlockPos))
			{
				this.isLookFar = true;
				return false;
			}
			this.state = EAIDestroyArea.eState.Attack;
			this.theEntity.SeekYawToPos(this.seekPos, 10f);
		}
		return this.isAtPathEnd || !this.theEntity.navigator.noPathAndNotPlanningOne();
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000C7434 File Offset: 0x000C5634
	public override void Update()
	{
		this.delayTime -= 0.05f;
		if (this.state == EAIDestroyArea.eState.Attack)
		{
			int num = this.attackTimeout - 1;
			this.attackTimeout = num;
			if (num <= 0)
			{
				ItemActionAttackData itemActionAttackData = this.theEntity.inventory.holdingItemData.actionData[0] as ItemActionAttackData;
				if (itemActionAttackData != null)
				{
					this.theEntity.SetLookPosition(Vector3.zero);
					if (this.theEntity.Attack(false))
					{
						this.attackTimeout = this.theEntity.GetAttackTimeoutTicks();
						itemActionAttackData.hitDelegate = new ItemActionAttackData.HitDelegate(this.GetHitInfo);
						this.theEntity.Attack(true);
						this.state = EAIDestroyArea.eState.EndPath;
					}
				}
			}
		}
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000C74EC File Offset: 0x000C56EC
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldRayHitInfo GetHitInfo(out float damageScale)
	{
		damageScale = 1f;
		return this.hitInfo;
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000C74FB File Offset: 0x000C56FB
	public override void Reset()
	{
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		moveHelper.Stop();
		moveHelper.IsUnreachableAbove = false;
		moveHelper.IsDestroyArea = false;
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000C751C File Offset: 0x000C571C
	public override string ToString()
	{
		return string.Format("{0}, {1}, delayTime {2}, findC {3}", new object[]
		{
			base.ToString(),
			this.state.ToStringCached<EAIDestroyArea.eState>(),
			this.delayTime.ToCultureInvariantString("0.00"),
			this.findCount
		});
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000C7574 File Offset: 0x000C5774
	[Conditional("DEBUG_AIDESTROY")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogDestroy(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} EAIDestroyArea {1} {2}, {3}", new object[]
		{
			GameManager.frameCount,
			this.theEntity.EntityName,
			this.theEntity.entityId,
			_format
		});
		Log.Warning(_format, _args);
	}

	// Token: 0x0400169E RID: 5790
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDumbDistance = 9f;

	// Token: 0x0400169F RID: 5791
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTowardsMeMaxDist = 5f;

	// Token: 0x040016A0 RID: 5792
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFindDirOffset = 8;

	// Token: 0x040016A1 RID: 5793
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] quadrants = new float[] { 9f, 9f, 9f, -8f, -8f, -8f, -8f, 9f };

	// Token: 0x040016A2 RID: 5794
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetPos;

	// Token: 0x040016A3 RID: 5795
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 seekPos;

	// Token: 0x040016A4 RID: 5796
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i seekBlockPos;

	// Token: 0x040016A5 RID: 5797
	[PublicizedFrom(EAccessModifier.Private)]
	public int findDir;

	// Token: 0x040016A6 RID: 5798
	[PublicizedFrom(EAccessModifier.Private)]
	public int findCount;

	// Token: 0x040016A7 RID: 5799
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLookFar;

	// Token: 0x040016A8 RID: 5800
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAtPathEnd;

	// Token: 0x040016A9 RID: 5801
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayTime;

	// Token: 0x040016AA RID: 5802
	[PublicizedFrom(EAccessModifier.Private)]
	public int attackTimeout;

	// Token: 0x040016AB RID: 5803
	[PublicizedFrom(EAccessModifier.Private)]
	public EAIDestroyArea.eState state;

	// Token: 0x040016AC RID: 5804
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldRayHitInfo hitInfo = new WorldRayHitInfo();

	// Token: 0x02000434 RID: 1076
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eState
	{
		// Token: 0x040016AE RID: 5806
		FindExistingPos,
		// Token: 0x040016AF RID: 5807
		FindPos,
		// Token: 0x040016B0 RID: 5808
		FindPath,
		// Token: 0x040016B1 RID: 5809
		WaitForPath,
		// Token: 0x040016B2 RID: 5810
		HasPath,
		// Token: 0x040016B3 RID: 5811
		EndPath,
		// Token: 0x040016B4 RID: 5812
		Attack
	}
}
