using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF6 RID: 6902
	public class ThreatLevelTracker
	{
		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x0600CFF3 RID: 53235 RVA: 0x004B4034 File Offset: 0x004B2234
		// (set) Token: 0x0600CFF4 RID: 53236 RVA: 0x004B403C File Offset: 0x004B223C
		public int DeadEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x0600CFF5 RID: 53237 RVA: 0x004B4045 File Offset: 0x004B2245
		// (set) Token: 0x0600CFF6 RID: 53238 RVA: 0x004B404D File Offset: 0x004B224D
		public int InactiveEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x0600CFF7 RID: 53239 RVA: 0x004B4056 File Offset: 0x004B2256
		// (set) Token: 0x0600CFF8 RID: 53240 RVA: 0x004B405E File Offset: 0x004B225E
		public int SleepingEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x0600CFF9 RID: 53241 RVA: 0x004B4067 File Offset: 0x004B2267
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

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x0600CFFA RID: 53242 RVA: 0x004B4093 File Offset: 0x004B2293
		// (set) Token: 0x0600CFFB RID: 53243 RVA: 0x004B409B File Offset: 0x004B229B
		public float NumericalThreatLevel
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x0600CFFC RID: 53244 RVA: 0x004B40A4 File Offset: 0x004B22A4
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

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x0600CFFD RID: 53245 RVA: 0x004B40B6 File Offset: 0x004B22B6
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x0600CFFE RID: 53246 RVA: 0x004B40C3 File Offset: 0x004B22C3
		public bool IsThreatLevelInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.NumericalThreatLevel < 0.25f;
			}
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x0600CFFF RID: 53247 RVA: 0x004B40D2 File Offset: 0x004B22D2
		public bool IsTargetInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget <= 0.25f;
			}
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x0600D000 RID: 53248 RVA: 0x004B40E4 File Offset: 0x004B22E4
		public bool IsTargetAboveThreatLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget > this.NumericalThreatLevel;
			}
		}

		// Token: 0x0600D001 RID: 53249 RVA: 0x004B40F4 File Offset: 0x004B22F4
		public static void Init(DynamicMusicManager _dmManager)
		{
			_dmManager.ThreatLevelTracker = new ThreatLevelTracker();
			_dmManager.ThreatLevelTracker.dynamicMusicManager = _dmManager;
			_dmManager.ThreatLevelTracker.somePlayer = GameManager.Instance.World.GetPrimaryPlayer();
			_dmManager.ThreatLevelTracker.epLocal = _dmManager.PrimaryLocalPlayer;
			_dmManager.ThreatLevelTracker.NumericalThreatLevel = 0f;
			_dmManager.ThreatLevelTracker.enemies = new List<Entity>();
		}

		// Token: 0x0600D002 RID: 53250 RVA: 0x004B4163 File Offset: 0x004B2363
		public void Tick()
		{
			if (GameTimer.Instance.ticks % 20UL == 0UL)
			{
				this.TickTrackThreatLevel();
			}
			this.TickMoveThreatLevel();
		}

		// Token: 0x0600D003 RID: 53251 RVA: 0x004B4184 File Offset: 0x004B2384
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

		// Token: 0x0600D004 RID: 53252 RVA: 0x004B4218 File Offset: 0x004B2418
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

		// Token: 0x0600D005 RID: 53253 RVA: 0x004B4378 File Offset: 0x004B2578
		[PublicizedFrom(EAccessModifier.Private)]
		public bool EnemyIsTargetingPlayer(EntityEnemy _enemy)
		{
			EntityAlive attackTarget = _enemy.GetAttackTarget();
			return attackTarget != null && attackTarget.Equals(this.epLocal);
		}

		// Token: 0x0600D006 RID: 53254 RVA: 0x000027FC File Offset: 0x000009FC
		public void Event(MinEventTypes _eventType, MinEventParams _eventParms)
		{
		}

		// Token: 0x04009D67 RID: 40295
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009D68 RID: 40296
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cSleeperIncrement = 0.03125f;

		// Token: 0x04009D69 RID: 40297
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cTargetIncrement = 0.25f;

		// Token: 0x04009D6A RID: 40298
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cAlertIncrement = 0.125f;

		// Token: 0x04009D6B RID: 40299
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cInactiveIncrement = 0.0625f;

		// Token: 0x04009D6C RID: 40300
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cBaseIncrement = 0.0015f;

		// Token: 0x04009D71 RID: 40305
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal epLocal;

		// Token: 0x04009D72 RID: 40306
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal somePlayer;

		// Token: 0x04009D73 RID: 40307
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> enemies;

		// Token: 0x04009D74 RID: 40308
		[PublicizedFrom(EAccessModifier.Private)]
		public float threatLevelTarget;

		// Token: 0x04009D75 RID: 40309
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);
	}
}
