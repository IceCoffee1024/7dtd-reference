using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200045A RID: 1114
[Preserve]
public class EAISetNearestEntityAsTarget : EAITarget
{
	// Token: 0x060021CE RID: 8654 RVA: 0x000CCB3E File Offset: 0x000CAD3E
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 25f, true);
		this.MutexBits = 1;
		this.sorter = new EAISetNearestEntityAsTargetSorter(_theEntity);
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000CCB60 File Offset: 0x000CAD60
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		this.targetClasses = new List<EAISetNearestEntityAsTarget.TargetClass>();
		string text;
		if (data.TryGetValue("class", out text))
		{
			string[] array = text.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i += 3)
			{
				EAISetNearestEntityAsTarget.TargetClass targetClass;
				targetClass.type = EntityFactory.GetEntityType(array[i]);
				targetClass.hearDistMax = 0f;
				if (i + 1 < array.Length)
				{
					targetClass.hearDistMax = StringParsers.ParseFloat(array[i + 1], 0, -1, NumberStyles.Any);
				}
				if (targetClass.hearDistMax == 0f)
				{
					targetClass.hearDistMax = 50f;
				}
				targetClass.seeDistMax = 0f;
				if (i + 2 < array.Length)
				{
					targetClass.seeDistMax = StringParsers.ParseFloat(array[i + 2], 0, -1, NumberStyles.Any);
				}
				if (targetClass.type == typeof(EntityPlayer))
				{
					this.playerTargetClassIndex = this.targetClasses.Count;
				}
				this.targetClasses.Add(targetClass);
			}
		}
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x000CCC64 File Offset: 0x000CAE64
	public void SetTargetOnlyPlayers(float _distance)
	{
		this.targetClasses.Clear();
		EAISetNearestEntityAsTarget.TargetClass targetClass = default(EAISetNearestEntityAsTarget.TargetClass);
		targetClass.type = typeof(EntityPlayer);
		targetClass.hearDistMax = _distance;
		targetClass.seeDistMax = -_distance;
		this.targetClasses.Add(targetClass);
		this.playerTargetClassIndex = 0;
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000CCCBC File Offset: 0x000CAEBC
	public override bool CanExecute()
	{
		if (this.theEntity.distraction != null)
		{
			return false;
		}
		this.FindTarget();
		if (!this.closeTargetEntity)
		{
			return false;
		}
		this.targetEntity = this.closeTargetEntity;
		this.targetPlayer = this.closeTargetEntity as EntityPlayer;
		return true;
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000CCD14 File Offset: 0x000CAF14
	[PublicizedFrom(EAccessModifier.Private)]
	public void FindTarget()
	{
		this.closeTargetDist = float.MaxValue;
		this.closeTargetEntity = null;
		float seeDistance = this.theEntity.GetSeeDistance();
		for (int i = 0; i < this.targetClasses.Count; i++)
		{
			EAISetNearestEntityAsTarget.TargetClass targetClass = this.targetClasses[i];
			float num = seeDistance;
			if (targetClass.seeDistMax != 0f)
			{
				float num2 = ((targetClass.seeDistMax < 0f) ? (-targetClass.seeDistMax) : (targetClass.seeDistMax * this.theEntity.senseScale));
				num = Utils.FastMin(num, num2);
			}
			if (targetClass.type == typeof(EntityPlayer))
			{
				this.FindTargetPlayer(num);
				if (this.theEntity.noisePlayer && this.theEntity.noisePlayer != this.closeTargetEntity)
				{
					if (this.closeTargetEntity)
					{
						if (this.theEntity.noisePlayerVolume >= this.theEntity.sleeperNoiseToWake)
						{
							Vector3 position = this.theEntity.noisePlayer.position;
							float magnitude = (this.theEntity.position - position).magnitude;
							if (magnitude < this.closeTargetDist)
							{
								this.closeTargetDist = magnitude;
								this.closeTargetEntity = this.theEntity.noisePlayer;
							}
						}
					}
					else if (!this.theEntity.IsSleeping)
					{
						this.SeekNoise(this.theEntity.noisePlayer);
					}
				}
				if (this.closeTargetEntity)
				{
					EntityPlayer entityPlayer = (EntityPlayer)this.closeTargetEntity;
					if (entityPlayer.IsBloodMoonDead && entityPlayer.currentLife >= 0.5f)
					{
						Log.Out("Player {0}, living {1}, lost BM immunity", new object[]
						{
							entityPlayer.GetDebugName(),
							entityPlayer.currentLife * 60f
						});
						entityPlayer.IsBloodMoonDead = false;
					}
				}
				if (this.theEntity.attractPlayer && !this.closeTargetEntity && !this.theEntity.IsSleeping && !this.theEntity.HasInvestigatePosition)
				{
					this.SeekBreadcrumb(this.theEntity.attractPlayer, 15f);
				}
				if (this.theEntity.smellPlayer && !this.closeTargetEntity && !this.theEntity.IsSleeping && !this.theEntity.HasInvestigatePosition && this.theEntity.smellPlayer.currentLife > 1f)
				{
					this.SeekBreadcrumb(this.theEntity.smellPlayer, 24f);
				}
			}
			else if (!this.theEntity.IsSleeping && !this.theEntity.HasInvestigatePosition)
			{
				this.theEntity.world.GetEntitiesInBounds(targetClass.type, BoundsUtils.ExpandBounds(this.theEntity.boundingBox, num, 4f, num), EAISetNearestEntityAsTarget.list);
				EAISetNearestEntityAsTarget.list.Sort(this.sorter);
				int j = 0;
				while (j < EAISetNearestEntityAsTarget.list.Count)
				{
					EntityAlive entityAlive = (EntityAlive)EAISetNearestEntityAsTarget.list[j];
					if (!(entityAlive is EntityDrone) && base.check(entityAlive))
					{
						float distance = this.theEntity.GetDistance(entityAlive);
						if (distance < this.closeTargetDist)
						{
							this.closeTargetDist = distance;
							this.closeTargetEntity = entityAlive;
							this.lastSeenPos = entityAlive.position;
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
				EAISetNearestEntityAsTarget.list.Clear();
			}
		}
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000CD0A8 File Offset: 0x000CB2A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SeekNoise(EntityPlayer player)
	{
		float num = (player.position - this.theEntity.position).magnitude;
		if (this.playerTargetClassIndex >= 0)
		{
			float num2 = this.targetClasses[this.playerTargetClassIndex].hearDistMax;
			num2 *= this.theEntity.senseScale;
			num2 *= player.DetectUsScale(this.theEntity);
			if (num > num2)
			{
				return;
			}
		}
		num *= 0.9f;
		if (num > this.manager.noiseSeekDist)
		{
			num = this.manager.noiseSeekDist;
		}
		if (this.theEntity.IsBloodMoon)
		{
			num = this.manager.noiseSeekDist * 0.25f;
		}
		Vector3 breadcrumbPos = player.GetBreadcrumbPos(num * base.RandomFloat);
		int num3 = this.theEntity.CalcInvestigateTicks((int)(30f + base.RandomFloat * 30f) * 20, player);
		this.theEntity.SetInvestigatePosition(breadcrumbPos, num3, true);
		this.PlaySoundSense();
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000CD1A4 File Offset: 0x000CB3A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SeekBreadcrumb(EntityPlayer player, float _maxDist)
	{
		float num = Mathf.Pow(base.RandomFloat, 2.1f);
		Vector3 breadcrumbPos = player.GetBreadcrumbPos(1f + _maxDist * num);
		int num2 = this.theEntity.CalcInvestigateTicks((int)(10f + base.RandomFloat * 10f) * 20, player);
		this.theEntity.SetInvestigatePosition(breadcrumbPos, num2, true);
		this.PlaySoundSense();
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000CD20C File Offset: 0x000CB40C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlaySoundSense()
	{
		float time = Time.time;
		if (this.senseSoundTime - time < 0f)
		{
			this.senseSoundTime = time + 10f + base.RandomFloat * 10f;
			this.theEntity.PlayOneShot(this.theEntity.GetSoundSense(), false, false, false, null, 1f);
		}
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000CD268 File Offset: 0x000CB468
	[PublicizedFrom(EAccessModifier.Private)]
	public void FindTargetPlayer(float seeDist)
	{
		if (this.theEntity.IsSleeperPassive)
		{
			return;
		}
		this.theEntity.world.GetPlayersAround(this.theEntity.position, seeDist, EAISetNearestEntityAsTarget.playerList);
		if (this.theEntity.IsSleeping)
		{
			EAISetNearestEntityAsTarget.playerList.Sort(this.sorter);
			EntityPlayer entityPlayer = null;
			float num = float.MaxValue;
			bool flag = false;
			if (this.theEntity.noisePlayer != null)
			{
				if (this.theEntity.noisePlayerVolume >= this.theEntity.sleeperNoiseToWake)
				{
					entityPlayer = this.theEntity.noisePlayer;
					num = this.theEntity.noisePlayerDistance;
				}
				else if (this.theEntity.noisePlayerVolume >= this.theEntity.sleeperNoiseToSense)
				{
					flag = true;
				}
			}
			for (int i = 0; i < EAISetNearestEntityAsTarget.playerList.Count; i++)
			{
				EntityPlayer entityPlayer2 = EAISetNearestEntityAsTarget.playerList[i];
				if (this.theEntity.CanSee(entityPlayer2) && !entityPlayer2.IsIgnoredByAI())
				{
					float distance = this.theEntity.GetDistance(entityPlayer2);
					int sleeperDisturbedLevel = this.theEntity.GetSleeperDisturbedLevel(distance, entityPlayer2.Stealth.lightLevel);
					if (sleeperDisturbedLevel >= 2)
					{
						if (distance < num)
						{
							entityPlayer = entityPlayer2;
							num = distance;
						}
					}
					else if (sleeperDisturbedLevel >= 1)
					{
						flag = true;
					}
				}
			}
			if (entityPlayer != null)
			{
				this.closeTargetDist = num;
				this.closeTargetEntity = entityPlayer;
			}
			else if (flag)
			{
				this.theEntity.Groan();
			}
			else
			{
				this.theEntity.Snore();
			}
		}
		else
		{
			for (int j = 0; j < EAISetNearestEntityAsTarget.playerList.Count; j++)
			{
				EntityPlayer entityPlayer3 = EAISetNearestEntityAsTarget.playerList[j];
				if (entityPlayer3.IsAlive() && !entityPlayer3.IsIgnoredByAI())
				{
					float seeDistance = this.manager.GetSeeDistance(entityPlayer3);
					if (seeDistance < this.closeTargetDist && this.theEntity.CanSee(entityPlayer3) && this.theEntity.CanSeeStealth(seeDistance, entityPlayer3.Stealth.lightLevel))
					{
						this.closeTargetDist = seeDistance;
						this.closeTargetEntity = entityPlayer3;
					}
				}
			}
		}
		EAISetNearestEntityAsTarget.playerList.Clear();
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000CD482 File Offset: 0x000CB682
	public override void Start()
	{
		this.theEntity.SetAttackTarget(this.targetEntity, 200);
		this.theEntity.ConditionalTriggerSleeperWakeUp();
		this.PlaySoundSense();
		base.Start();
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000CD4B4 File Offset: 0x000CB6B4
	public override bool Continue()
	{
		if (this.targetEntity.IsDead() || this.theEntity.distraction != null)
		{
			if (this.theEntity.GetAttackTarget() == this.targetEntity)
			{
				this.theEntity.SetAttackTarget(null, 0);
			}
			return false;
		}
		this.findTime += 0.05f;
		if (this.findTime > 2f)
		{
			this.findTime = 0f;
			this.FindTarget();
			if (this.closeTargetEntity && this.closeTargetEntity != this.targetEntity)
			{
				return false;
			}
		}
		if (this.theEntity.GetAttackTarget() != this.targetEntity)
		{
			return false;
		}
		if (base.check(this.targetEntity) && (this.targetPlayer == null || this.theEntity.CanSeeStealth(this.manager.GetSeeDistance(this.targetEntity), this.targetPlayer.Stealth.lightLevel)))
		{
			this.theEntity.SetAttackTarget(this.targetEntity, 600);
			this.lastSeenPos = this.targetEntity.position;
			return true;
		}
		if (this.theEntity.GetDistanceSq(this.lastSeenPos) < 2.25f)
		{
			this.lastSeenPos = Vector3.zero;
		}
		this.theEntity.SetAttackTarget(null, 0);
		int num = this.theEntity.CalcInvestigateTicks(Constants.cEnemySenseMemory * 20, this.targetEntity);
		if (this.lastSeenPos != Vector3.zero)
		{
			this.theEntity.SetInvestigatePosition(this.lastSeenPos, num, true);
		}
		return false;
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000CD657 File Offset: 0x000CB857
	public override void Reset()
	{
		this.targetEntity = null;
		this.targetPlayer = null;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000CD667 File Offset: 0x000CB867
	public override string ToString()
	{
		return string.Format("{0}, {1}", base.ToString(), this.targetEntity ? this.targetEntity.EntityName : "");
	}

	// Token: 0x04001767 RID: 5991
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cHearDistMax = 50f;

	// Token: 0x04001768 RID: 5992
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAISetNearestEntityAsTarget.TargetClass> targetClasses;

	// Token: 0x04001769 RID: 5993
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerTargetClassIndex = -1;

	// Token: 0x0400176A RID: 5994
	[PublicizedFrom(EAccessModifier.Private)]
	public float closeTargetDist;

	// Token: 0x0400176B RID: 5995
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive closeTargetEntity;

	// Token: 0x0400176C RID: 5996
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetEntity;

	// Token: 0x0400176D RID: 5997
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer targetPlayer;

	// Token: 0x0400176E RID: 5998
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 lastSeenPos;

	// Token: 0x0400176F RID: 5999
	[PublicizedFrom(EAccessModifier.Private)]
	public float findTime;

	// Token: 0x04001770 RID: 6000
	[PublicizedFrom(EAccessModifier.Private)]
	public float senseSoundTime;

	// Token: 0x04001771 RID: 6001
	[PublicizedFrom(EAccessModifier.Private)]
	public EAISetNearestEntityAsTargetSorter sorter;

	// Token: 0x04001772 RID: 6002
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Entity> list = new List<Entity>();

	// Token: 0x04001773 RID: 6003
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<EntityPlayer> playerList = new List<EntityPlayer>();

	// Token: 0x0200045B RID: 1115
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TargetClass
	{
		// Token: 0x04001774 RID: 6004
		public Type type;

		// Token: 0x04001775 RID: 6005
		public float hearDistMax;

		// Token: 0x04001776 RID: 6006
		public float seeDistMax;
	}
}
