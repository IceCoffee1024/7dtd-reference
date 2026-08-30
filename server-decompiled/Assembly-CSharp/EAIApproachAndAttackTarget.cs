using System;
using System.Collections.Generic;
using System.Globalization;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000428 RID: 1064
[Preserve]
public class EAIApproachAndAttackTarget : EAIBase
{
	// Token: 0x060020B8 RID: 8376 RVA: 0x000C5027 File Offset: 0x000C3227
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 0.1f;
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000C5044 File Offset: 0x000C3244
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		this.targetClasses = new List<EAIApproachAndAttackTarget.TargetClass>();
		string text;
		if (data.TryGetValue("class", out text))
		{
			string[] array = text.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i += 2)
			{
				EAIApproachAndAttackTarget.TargetClass targetClass = default(EAIApproachAndAttackTarget.TargetClass);
				targetClass.type = EntityFactory.GetEntityType(array[i]);
				targetClass.chaseTimeMax = 0f;
				if (i + 1 < array.Length)
				{
					targetClass.chaseTimeMax = StringParsers.ParseFloat(array[i + 1], 0, -1, NumberStyles.Any);
				}
				this.targetClasses.Add(targetClass);
				if (targetClass.type == typeof(EntityEnemyAnimal))
				{
					targetClass.type = typeof(EntityAnimalSnake);
					this.targetClasses.Add(targetClass);
				}
			}
		}
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x000C5118 File Offset: 0x000C3318
	public void SetTargetOnlyPlayers()
	{
		this.targetClasses.Clear();
		EAIApproachAndAttackTarget.TargetClass targetClass = default(EAIApproachAndAttackTarget.TargetClass);
		targetClass.type = typeof(EntityPlayer);
		this.targetClasses.Add(targetClass);
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x000C5158 File Offset: 0x000C3358
	public override bool CanExecute()
	{
		if (this.theEntity.sleepingOrWakingUp || this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None || (this.theEntity.Jumping && !this.theEntity.isSwimming))
		{
			return false;
		}
		this.entityTarget = this.theEntity.GetAttackTarget();
		if (this.entityTarget == null)
		{
			return false;
		}
		Type type = this.entityTarget.GetType();
		for (int i = 0; i < this.targetClasses.Count; i++)
		{
			EAIApproachAndAttackTarget.TargetClass targetClass = this.targetClasses[i];
			if (targetClass.type != null && targetClass.type.IsAssignableFrom(type))
			{
				this.chaseTimeMax = targetClass.chaseTimeMax;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x000C521C File Offset: 0x000C341C
	public override void Start()
	{
		this.entityTargetPos = this.entityTarget.position;
		this.entityTargetVel = Vector3.zero;
		this.isTargetToEat = this.entityTarget.IsDead();
		this.isEating = false;
		this.theEntity.IsEating = false;
		this.homeTimeout = (this.theEntity.IsSleeper ? 90f : this.chaseTimeMax);
		this.hasHome = this.homeTimeout > 0f;
		this.isGoingHome = false;
		if (this.theEntity.ChaseReturnLocation == Vector3.zero)
		{
			this.theEntity.ChaseReturnLocation = (this.theEntity.IsSleeper ? this.theEntity.SleeperSpawnPosition : this.theEntity.position);
		}
		this.pathCounter = 0;
		this.relocateTicks = 0;
		this.attackTimeout = 5;
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x000C5300 File Offset: 0x000C3500
	public override bool Continue()
	{
		if (this.theEntity.sleepingOrWakingUp || this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		EntityAlive attackTarget = this.theEntity.GetAttackTarget();
		if (this.isGoingHome)
		{
			return !attackTarget && this.theEntity.ChaseReturnLocation != Vector3.zero;
		}
		return attackTarget && !(attackTarget != this.entityTarget) && attackTarget.IsDead() == this.isTargetToEat;
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x000C538D File Offset: 0x000C358D
	public override void Reset()
	{
		this.theEntity.IsEating = false;
		this.theEntity.moveHelper.Stop();
		if (this.blockTargetTask != null)
		{
			this.blockTargetTask.canExecute = false;
		}
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x000C53C0 File Offset: 0x000C35C0
	public override void Update()
	{
		if (this.hasHome && !this.isTargetToEat)
		{
			if (this.isGoingHome)
			{
				Vector3 vector = this.theEntity.ChaseReturnLocation - this.theEntity.position;
				float y = vector.y;
				vector.y = 0f;
				if (vector.sqrMagnitude <= 0.16000001f && Utils.FastAbs(y) < 2f)
				{
					Vector3 chaseReturnLocation = this.theEntity.ChaseReturnLocation;
					chaseReturnLocation.y = this.theEntity.position.y;
					this.theEntity.SetPosition(chaseReturnLocation, true);
					this.theEntity.ChaseReturnLocation = Vector3.zero;
					if (this.theEntity.IsSleeper)
					{
						this.theEntity.ResumeSleeperPose();
					}
					return;
				}
				int num = this.pathCounter - 1;
				this.pathCounter = num;
				if (num <= 0 && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
				{
					this.pathCounter = 60;
					float num2 = this.theEntity.GetMoveSpeedAggro() * 0.8f;
					this.theEntity.FindPath(this.theEntity.ChaseReturnLocation, num2, false, this);
				}
				return;
			}
			else
			{
				this.homeTimeout -= 0.05f;
				if (this.homeTimeout <= 0f)
				{
					if (this.blockTargetTask == null)
					{
						List<EAIBlockingTargetTask> targetTasks = this.manager.GetTargetTasks<EAIBlockingTargetTask>();
						if (targetTasks != null)
						{
							this.blockTargetTask = targetTasks[0];
						}
					}
					if (this.blockTargetTask != null)
					{
						this.blockTargetTask.canExecute = true;
					}
					this.theEntity.SetAttackTarget(null, 0);
					this.theEntity.SetLookPosition(Vector3.zero);
					this.theEntity.PlayGiveUpSound();
					this.pathCounter = 0;
					this.isGoingHome = true;
					return;
				}
			}
		}
		if (this.entityTarget == null)
		{
			return;
		}
		if (this.relocateTicks > 0)
		{
			if (!this.theEntity.navigator.noPathAndNotPlanningOne())
			{
				this.relocateTicks--;
				this.theEntity.moveHelper.SetFocusPos(this.entityTarget.position);
				return;
			}
			this.relocateTicks = 0;
		}
		Vector3 vector2 = this.entityTarget.position;
		if (this.isTargetToEat)
		{
			vector2 = this.entityTarget.getBellyPosition();
		}
		Vector3 vector3 = vector2 - this.entityTargetPos;
		if (vector3.sqrMagnitude < 1f)
		{
			this.entityTargetVel = this.entityTargetVel * 0.7f + vector3 * 0.3f;
		}
		this.entityTargetPos = vector2;
		this.attackTimeout--;
		if (this.isEating)
		{
			if (this.theEntity.bodyDamage.HasLimbs)
			{
				this.theEntity.RotateTo(vector2.x, vector2.y, vector2.z, 8f, 5f);
			}
			if (this.attackTimeout <= 0)
			{
				this.attackTimeout = 25 + base.GetRandom(10);
				if ((this.eatCount & 1) == 0)
				{
					this.theEntity.PlayOneShot("eat_player", false, false, false, null, 1f);
					this.entityTarget.DamageEntity(DamageSource.eat, 35, false, 1f);
				}
				Vector3 vector4 = new Vector3(0f, 0.04f, 0.08f);
				ParticleEffect particleEffect = new ParticleEffect("blood_eat", vector4, 1f, Color.white, null, this.theEntity.entityId, ParticleEffect.Attachment.Head);
				GameManager.Instance.SpawnParticleEffectServer(particleEffect, this.theEntity.entityId, false, false);
				this.eatCount++;
			}
			return;
		}
		this.theEntity.moveHelper.CalcIfUnreachablePos();
		float num4;
		float num5;
		if (!this.isTargetToEat)
		{
			ItemValue holdingItemItemValue = this.theEntity.inventory.holdingItemItemValue;
			int num3 = 0;
			ItemAction itemAction = holdingItemItemValue.ItemClass.Actions[num3];
			num4 = 1.095f;
			if (itemAction != null)
			{
				num4 = itemAction.Range;
				if (num4 == 0f)
				{
					num4 = EffectManager.GetItemValue(PassiveEffects.MaxRange, holdingItemItemValue, 0f);
				}
			}
			num5 = num4;
			if (num4 >= 2.5f)
			{
				num5 -= 0.3f;
			}
			num5 = Utils.FastMax(0.7f, num5 - 0.35f);
		}
		else
		{
			num4 = this.theEntity.GetHeight() * 0.9f;
			num5 = num4 - 0.05f;
		}
		float num6 = num5 * num5;
		float num7 = 4f;
		if (this.theEntity.IsFeral)
		{
			num7 = 8f;
		}
		num7 = base.RandomFloat * num7;
		float targetXZDistanceSq = this.GetTargetXZDistanceSq(num7);
		float num8 = vector2.y - this.theEntity.position.y;
		float num9 = Utils.FastAbs(num8);
		bool flag = targetXZDistanceSq <= num6 && num9 < 1f;
		if (!flag)
		{
			if (num9 < 3f && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
			{
				PathEntity path = this.theEntity.navigator.getPath();
				if (path != null && path.NodeCountRemaining() <= 2)
				{
					this.pathCounter = 0;
				}
			}
			int num = this.pathCounter - 1;
			this.pathCounter = num;
			if (num <= 0 && this.theEntity.CanNavigatePath() && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
			{
				this.pathCounter = 6 + base.GetRandom(10);
				Vector3 moveToLocation = this.GetMoveToLocation(num5);
				if (moveToLocation.y - this.theEntity.position.y < -8f)
				{
					this.pathCounter += 40;
					if (base.RandomFloat < 0.2f)
					{
						this.seekPosOffset.x = this.seekPosOffset.x + (base.RandomFloat * 0.6f - 0.3f);
						this.seekPosOffset.y = this.seekPosOffset.y + (base.RandomFloat * 0.6f - 0.3f);
					}
					moveToLocation.x += this.seekPosOffset.x;
					moveToLocation.z += this.seekPosOffset.y;
				}
				else
				{
					float num10 = (moveToLocation - this.theEntity.position).magnitude - 5f;
					if (num10 > 0f)
					{
						if (num10 > 60f)
						{
							num10 = 60f;
						}
						this.pathCounter += (int)(num10 / 20f * 20f);
					}
				}
				this.theEntity.FindPath(moveToLocation, this.theEntity.GetMoveSpeedAggro(), true, this);
			}
		}
		if (this.theEntity.Climbing)
		{
			return;
		}
		bool flag2 = this.theEntity.CanSee(this.entityTarget);
		this.theEntity.SetLookPosition((flag2 && !this.theEntity.IsBreakingBlocks) ? this.entityTarget.getHeadPosition() : Vector3.zero);
		if (!flag)
		{
			if (this.theEntity.navigator.noPathAndNotPlanningOne() && this.pathCounter > 0 && num8 < 2.1f)
			{
				Vector3 moveToLocation2 = this.GetMoveToLocation(num5);
				this.theEntity.moveHelper.SetMoveTo(moveToLocation2, true);
			}
		}
		else
		{
			this.theEntity.moveHelper.Stop();
			this.pathCounter = 0;
		}
		float num11 = (this.isTargetToEat ? num4 : (num4 - 0.1f));
		float num12 = num11 * num11;
		if (targetXZDistanceSq > num12)
		{
			return;
		}
		if (num8 < -1.25f || num8 - this.theEntity.GetHeight() > 0.95f)
		{
			return;
		}
		this.theEntity.IsBreakingBlocks = false;
		this.theEntity.IsBreakingDoors = false;
		if (this.theEntity.bodyDamage.HasLimbs && !this.theEntity.Electrocuted)
		{
			this.theEntity.RotateTo(vector2.x, vector2.y, vector2.z, 30f, 30f);
		}
		if (this.isTargetToEat)
		{
			this.isEating = true;
			this.theEntity.IsEating = true;
			this.attackTimeout = 20;
			this.eatCount = 0;
			return;
		}
		if (this.theEntity.GetDamagedTarget() == this.entityTarget || (this.entityTarget != null && this.entityTarget.GetDamagedTarget() == this.theEntity))
		{
			this.homeTimeout = (this.theEntity.IsSleeper ? 90f : this.chaseTimeMax);
			if (this.blockTargetTask != null)
			{
				this.blockTargetTask.canExecute = false;
			}
			this.theEntity.ClearDamagedTarget();
			if (this.entityTarget)
			{
				this.entityTarget.ClearDamagedTarget();
			}
		}
		if (this.attackTimeout > 0)
		{
			return;
		}
		if (this.manager.groupCircle > 0f)
		{
			Entity targetIfAttackedNow = this.theEntity.GetTargetIfAttackedNow();
			if (targetIfAttackedNow != this.entityTarget && (!this.entityTarget.AttachedToEntity || this.entityTarget.AttachedToEntity != targetIfAttackedNow))
			{
				if (targetIfAttackedNow != null)
				{
					this.relocateTicks = 46;
					Vector3 vector5 = (this.theEntity.position - vector2).normalized * (num11 + 1.1f);
					float num13 = base.RandomFloat * 28f + 18f;
					if (base.RandomFloat < 0.5f)
					{
						num13 = -num13;
					}
					vector5 = Quaternion.Euler(0f, num13, 0f) * vector5;
					Vector3 vector6 = vector2 + vector5;
					this.theEntity.FindPath(vector6, this.theEntity.GetMoveSpeedAggro(), true, this);
				}
				return;
			}
		}
		this.theEntity.SleeperSupressLivingSounds = false;
		if (this.theEntity.Attack(false))
		{
			this.attackTimeout = this.theEntity.GetAttackTimeoutTicks();
			this.theEntity.Attack(true);
		}
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x000C5D90 File Offset: 0x000C3F90
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetTargetXZDistanceSq(float estimatedTicks)
	{
		Vector3 vector = this.entityTarget.position;
		vector += this.entityTargetVel * estimatedTicks;
		if (this.isTargetToEat)
		{
			vector = this.entityTarget.getBellyPosition();
		}
		Vector3 vector2 = this.theEntity.position + this.theEntity.motion * estimatedTicks - vector;
		vector2.y = 0f;
		return vector2.sqrMagnitude;
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x000C5E0C File Offset: 0x000C400C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetMoveToLocation(float maxDist)
	{
		Vector3 vector = this.entityTarget.position;
		vector += this.entityTargetVel * 6f;
		if (this.isTargetToEat)
		{
			vector = this.entityTarget.getBellyPosition();
		}
		if (!this.entityTarget.onGround && !this.entityTarget.IsInElevator())
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(vector - Origin.position, Vector3.down, out raycastHit, 3.9f, 1082195968))
			{
				vector.y -= raycastHit.distance;
			}
			else
			{
				vector.y -= 3.9f;
			}
			maxDist *= 0.8f;
		}
		vector = this.entityTarget.world.FindSupportingBlockPos(vector);
		if (maxDist > 0f)
		{
			Vector3 vector2 = new Vector3(this.theEntity.position.x, vector.y, this.theEntity.position.z);
			Vector3 vector3 = vector - vector2;
			float magnitude = vector3.magnitude;
			if (magnitude < 3f)
			{
				if (magnitude <= maxDist)
				{
					float num = vector.y - this.theEntity.position.y;
					if (num < -3f || num > 1.5f)
					{
						return vector;
					}
					return vector2;
				}
				else
				{
					vector3 *= maxDist / magnitude;
					Vector3 vector4 = vector - vector3;
					vector4.y += 0.51f;
					Vector3i vector3i = World.worldToBlockPos(vector4);
					BlockValue block = this.entityTarget.world.GetBlock(vector3i);
					Block block2 = block.Block;
					if (block2.PathType <= 0)
					{
						RaycastHit raycastHit2;
						if (Physics.Raycast(vector4 - Origin.position, Vector3.down, out raycastHit2, 1.02f, 1082195968))
						{
							vector4.y = raycastHit2.point.y + Origin.position.y;
							return vector4;
						}
						if (block2.IsElevator((int)block.rotation))
						{
							vector4.y = vector.y;
							return vector4;
						}
					}
				}
			}
		}
		return vector;
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x000C6010 File Offset: 0x000C4210
	public override string ToString()
	{
		ItemValue holdingItemItemValue = this.theEntity.inventory.holdingItemItemValue;
		ItemAction itemAction = holdingItemItemValue.ItemClass.Actions[0];
		float num = 1.095f;
		if (!this.isTargetToEat && itemAction != null)
		{
			num = itemAction.Range;
			if (num == 0f)
			{
				num = EffectManager.GetItemValue(PassiveEffects.MaxRange, holdingItemItemValue, 0f);
			}
		}
		float num2 = (this.isTargetToEat ? num : (num - 0.1f));
		float targetXZDistanceSq = this.GetTargetXZDistanceSq(0f);
		return string.Format("{0}, {1}{2}{3}{4}{5} dist {6} rng {7} timeout {8}", new object[]
		{
			base.ToString(),
			this.entityTarget ? this.entityTarget.EntityName : "",
			this.theEntity.CanSee(this.entityTarget) ? "(see)" : "",
			this.theEntity.navigator.noPathAndNotPlanningOne() ? "(-path)" : (this.theEntity.navigator.noPath() ? "(!path)" : ""),
			this.isTargetToEat ? "(eat)" : "",
			this.isGoingHome ? "(home)" : "",
			Mathf.Sqrt(targetXZDistanceSq).ToCultureInvariantString("0.000"),
			num2.ToCultureInvariantString("0.000"),
			this.homeTimeout.ToCultureInvariantString("0.00")
		});
	}

	// Token: 0x04001665 RID: 5733
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSleeperChaseTime = 90f;

	// Token: 0x04001666 RID: 5734
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAIApproachAndAttackTarget.TargetClass> targetClasses;

	// Token: 0x04001667 RID: 5735
	[PublicizedFrom(EAccessModifier.Private)]
	public float chaseTimeMax;

	// Token: 0x04001668 RID: 5736
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasHome;

	// Token: 0x04001669 RID: 5737
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isGoingHome;

	// Token: 0x0400166A RID: 5738
	[PublicizedFrom(EAccessModifier.Private)]
	public float homeTimeout;

	// Token: 0x0400166B RID: 5739
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entityTarget;

	// Token: 0x0400166C RID: 5740
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 entityTargetPos;

	// Token: 0x0400166D RID: 5741
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 entityTargetVel;

	// Token: 0x0400166E RID: 5742
	[PublicizedFrom(EAccessModifier.Private)]
	public int attackTimeout;

	// Token: 0x0400166F RID: 5743
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathCounter;

	// Token: 0x04001670 RID: 5744
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 seekPosOffset;

	// Token: 0x04001671 RID: 5745
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTargetToEat;

	// Token: 0x04001672 RID: 5746
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEating;

	// Token: 0x04001673 RID: 5747
	[PublicizedFrom(EAccessModifier.Private)]
	public int eatCount;

	// Token: 0x04001674 RID: 5748
	[PublicizedFrom(EAccessModifier.Private)]
	public EAIBlockingTargetTask blockTargetTask;

	// Token: 0x04001675 RID: 5749
	[PublicizedFrom(EAccessModifier.Private)]
	public int relocateTicks;

	// Token: 0x02000429 RID: 1065
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TargetClass
	{
		// Token: 0x04001676 RID: 5750
		public Type type;

		// Token: 0x04001677 RID: 5751
		public float chaseTimeMax;
	}
}
