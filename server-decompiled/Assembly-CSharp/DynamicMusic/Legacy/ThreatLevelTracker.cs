using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A72 RID: 6770
	public class ThreatLevelTracker
	{
		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x0600CBFE RID: 52222 RVA: 0x004A5C78 File Offset: 0x004A3E78
		// (set) Token: 0x0600CBFF RID: 52223 RVA: 0x004A5C80 File Offset: 0x004A3E80
		public int DeadEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x0600CC00 RID: 52224 RVA: 0x004A5C89 File Offset: 0x004A3E89
		// (set) Token: 0x0600CC01 RID: 52225 RVA: 0x004A5C91 File Offset: 0x004A3E91
		public int InactiveEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x0600CC02 RID: 52226 RVA: 0x004A5C9A File Offset: 0x004A3E9A
		// (set) Token: 0x0600CC03 RID: 52227 RVA: 0x004A5CA2 File Offset: 0x004A3EA2
		public int SleepingEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x0600CC04 RID: 52228 RVA: 0x004A5CAB File Offset: 0x004A3EAB
		public int ActiveEnemies
		{
			get
			{
				if (this.enemies == null)
				{
					return 0;
				}
				return this.enemies.Count - this.DeadEnemies - this.InactiveEnemies - this.SleepingEnemies;
			}
		}

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x0600CC05 RID: 52229 RVA: 0x004A5CD7 File Offset: 0x004A3ED7
		// (set) Token: 0x0600CC06 RID: 52230 RVA: 0x004A5CDF File Offset: 0x004A3EDF
		public float NumericalThreatLevel
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x0600CC07 RID: 52231 RVA: 0x004A5CE8 File Offset: 0x004A3EE8
		public ThreatLevelLegacyType ThreatLevel
		{
			get
			{
				if (this.NumericalThreatLevel < 0.25f)
				{
					return ThreatLevelLegacyType.Exploration;
				}
				return ThreatLevelLegacyType.Suspense;
			}
		}

		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x0600CC08 RID: 52232 RVA: 0x004A5CFA File Offset: 0x004A3EFA
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x0600CC09 RID: 52233 RVA: 0x004A5D07 File Offset: 0x004A3F07
		public bool IsThreatLevelInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.NumericalThreatLevel < 0.25f;
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x0600CC0A RID: 52234 RVA: 0x004A5D16 File Offset: 0x004A3F16
		public bool IsTargetInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget <= 0.25f;
			}
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x0600CC0B RID: 52235 RVA: 0x004A5D28 File Offset: 0x004A3F28
		public bool IsTargetAboveThreatLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget > this.NumericalThreatLevel;
			}
		}

		// Token: 0x0600CC0C RID: 52236 RVA: 0x004A5D38 File Offset: 0x004A3F38
		public static void Init(DynamicMusicManager _dmManager)
		{
			_dmManager.ThreatLevelTracker = new ThreatLevelTracker();
			_dmManager.ThreatLevelTracker.dynamicMusicManager = _dmManager;
			_dmManager.ThreatLevelTracker.somePlayer = GameManager.Instance.World.GetPrimaryPlayer();
			_dmManager.ThreatLevelTracker.epLocal = _dmManager.PrimaryLocalPlayer;
			_dmManager.ThreatLevelTracker.NumericalThreatLevel = 0f;
			_dmManager.ThreatLevelTracker.enemies = new List<Entity>();
		}

		// Token: 0x0600CC0D RID: 52237 RVA: 0x004A5DA7 File Offset: 0x004A3FA7
		public void Tick()
		{
			if (GameTimer.Instance.ticks % 20UL == 0UL)
			{
				this.TickTrackThreatLevel();
			}
			this.TickMoveThreatLevel();
		}

		// Token: 0x0600CC0E RID: 52238 RVA: 0x004A5DC8 File Offset: 0x004A3FC8
		[PublicizedFrom(EAccessModifier.Private)]
		public void TickMoveThreatLevel()
		{
			if (!this.IsMusicPlayingThisTick || (this.IsTargetInExploration && this.IsThreatLevelInExploration))
			{
				this.NumericalThreatLevel = this.threatLevelTarget;
				return;
			}
			if (!this.IsTargetAboveThreatLevel)
			{
				this.NumericalThreatLevel = Utils.FastClamp(this.NumericalThreatLevel - 0.003f, this.threatLevelTarget, this.NumericalThreatLevel);
				return;
			}
			if (this.IsThreatLevelInExploration)
			{
				this.NumericalThreatLevel = 0.25f;
				return;
			}
			this.NumericalThreatLevel = Utils.FastClamp(this.NumericalThreatLevel + 0.0015f, 0f, this.threatLevelTarget);
		}

		// Token: 0x0600CC0F RID: 52239 RVA: 0x004A5E5C File Offset: 0x004A405C
		[PublicizedFrom(EAccessModifier.Private)]
		public void TickTrackThreatLevel()
		{
			GameManager.Instance.World.GetEntitiesInBounds(typeof(EntityEnemy), new Bounds(this.epLocal.position, ThreatLevelTracker.boundingBoxRange), this.enemies);
			this.DeadEnemies = (this.SleepingEnemies = (this.InactiveEnemies = 0));
			this.threatLevelTarget = 0f;
			for (int i = 0; i < this.enemies.Count; i++)
			{
				EntityEnemy entityEnemy = this.enemies[i] as EntityEnemy;
				if (entityEnemy.IsDead())
				{
					int num = this.DeadEnemies;
					this.DeadEnemies = num + 1;
				}
				else if (entityEnemy.IsSleeping)
				{
					this.threatLevelTarget += 0.03125f;
					int num = this.SleepingEnemies;
					this.SleepingEnemies = num + 1;
				}
				else if (this.EnemyIsTargetingPlayer(entityEnemy))
				{
					this.threatLevelTarget += 0.25f;
				}
				else if (entityEnemy.IsAlert)
				{
					this.threatLevelTarget += 0.125f;
				}
				else
				{
					this.threatLevelTarget += 0.0625f;
					int num = this.InactiveEnemies;
					this.InactiveEnemies = num + 1;
				}
			}
			this.threatLevelTarget = Utils.FastClamp(this.threatLevelTarget, 0f, 0.5f);
			this.enemies.Clear();
		}

		// Token: 0x0600CC10 RID: 52240 RVA: 0x004A5FBC File Offset: 0x004A41BC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool EnemyIsTargetingPlayer(EntityEnemy _enemy)
		{
			EntityAlive attackTarget = _enemy.GetAttackTarget();
			return attackTarget != null && attackTarget.Equals(this.epLocal);
		}

		// Token: 0x0600CC11 RID: 52241 RVA: 0x000027FC File Offset: 0x000009FC
		public void Event(MinEventTypes _eventType, MinEventParams _eventParms)
		{
		}

		// Token: 0x04009A87 RID: 39559
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009A88 RID: 39560
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cSleeperIncrement = 0.03125f;

		// Token: 0x04009A89 RID: 39561
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cTargetIncrement = 0.25f;

		// Token: 0x04009A8A RID: 39562
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cAlertIncrement = 0.125f;

		// Token: 0x04009A8B RID: 39563
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cInactiveIncrement = 0.0625f;

		// Token: 0x04009A8C RID: 39564
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cBaseIncrement = 0.0015f;

		// Token: 0x04009A91 RID: 39569
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal epLocal;

		// Token: 0x04009A92 RID: 39570
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal somePlayer;

		// Token: 0x04009A93 RID: 39571
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> enemies;

		// Token: 0x04009A94 RID: 39572
		[PublicizedFrom(EAccessModifier.Private)]
		public float threatLevelTarget;

		// Token: 0x04009A95 RID: 39573
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);
	}
}
