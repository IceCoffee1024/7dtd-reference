using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A99 RID: 6809
	public class ThreatLevelTracker
	{
		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x0600CD87 RID: 52615 RVA: 0x004AC06C File Offset: 0x004AA26C
		// (set) Token: 0x0600CD88 RID: 52616 RVA: 0x004AC074 File Offset: 0x004AA274
		public int DeadEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x0600CD89 RID: 52617 RVA: 0x004AC07D File Offset: 0x004AA27D
		// (set) Token: 0x0600CD8A RID: 52618 RVA: 0x004AC085 File Offset: 0x004AA285
		public int InactiveEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x0600CD8B RID: 52619 RVA: 0x004AC08E File Offset: 0x004AA28E
		// (set) Token: 0x0600CD8C RID: 52620 RVA: 0x004AC096 File Offset: 0x004AA296
		public int SleepingEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x0600CD8D RID: 52621 RVA: 0x004AC09F File Offset: 0x004AA29F
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

		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x0600CD8E RID: 52622 RVA: 0x004AC0CB File Offset: 0x004AA2CB
		// (set) Token: 0x0600CD8F RID: 52623 RVA: 0x004AC0D3 File Offset: 0x004AA2D3
		public float NumericalThreatLevel
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x0600CD90 RID: 52624 RVA: 0x004AC0DC File Offset: 0x004AA2DC
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

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x0600CD91 RID: 52625 RVA: 0x004AC0EE File Offset: 0x004AA2EE
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x0600CD92 RID: 52626 RVA: 0x004AC0FB File Offset: 0x004AA2FB
		public bool IsThreatLevelInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.NumericalThreatLevel < 0.25f;
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x0600CD93 RID: 52627 RVA: 0x004AC10A File Offset: 0x004AA30A
		public bool IsTargetInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget <= 0.25f;
			}
		}

		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x0600CD94 RID: 52628 RVA: 0x004AC11C File Offset: 0x004AA31C
		public bool IsTargetAboveThreatLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget > this.NumericalThreatLevel;
			}
		}

		// Token: 0x0600CD95 RID: 52629 RVA: 0x004AC12C File Offset: 0x004AA32C
		public static void Init(DynamicMusicManager _dmManager)
		{
			_dmManager.ThreatLevelTracker = new ThreatLevelTracker();
			_dmManager.ThreatLevelTracker.dynamicMusicManager = _dmManager;
			_dmManager.ThreatLevelTracker.somePlayer = GameManager.Instance.World.GetPrimaryPlayer();
			_dmManager.ThreatLevelTracker.epLocal = _dmManager.PrimaryLocalPlayer;
			_dmManager.ThreatLevelTracker.NumericalThreatLevel = 0f;
			_dmManager.ThreatLevelTracker.enemies = new List<Entity>();
		}

		// Token: 0x0600CD96 RID: 52630 RVA: 0x004AC19B File Offset: 0x004AA39B
		public void Tick()
		{
			if (GameTimer.Instance.ticks % 20UL == 0UL)
			{
				this.TickTrackThreatLevel();
			}
			this.TickMoveThreatLevel();
		}

		// Token: 0x0600CD97 RID: 52631 RVA: 0x004AC1BC File Offset: 0x004AA3BC
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

		// Token: 0x0600CD98 RID: 52632 RVA: 0x004AC250 File Offset: 0x004AA450
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

		// Token: 0x0600CD99 RID: 52633 RVA: 0x004AC3B0 File Offset: 0x004AA5B0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool EnemyIsTargetingPlayer(EntityEnemy _enemy)
		{
			EntityAlive attackTarget = _enemy.GetAttackTarget();
			return attackTarget != null && attackTarget.Equals(this.epLocal);
		}

		// Token: 0x0600CD9A RID: 52634 RVA: 0x000027FC File Offset: 0x000009FC
		public void Event(MinEventTypes _eventType, MinEventParams _eventParms)
		{
		}

		// Token: 0x04009BB8 RID: 39864
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009BB9 RID: 39865
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cSleeperIncrement = 0.03125f;

		// Token: 0x04009BBA RID: 39866
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cTargetIncrement = 0.25f;

		// Token: 0x04009BBB RID: 39867
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cAlertIncrement = 0.125f;

		// Token: 0x04009BBC RID: 39868
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cInactiveIncrement = 0.0625f;

		// Token: 0x04009BBD RID: 39869
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cBaseIncrement = 0.0015f;

		// Token: 0x04009BC2 RID: 39874
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal epLocal;

		// Token: 0x04009BC3 RID: 39875
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal somePlayer;

		// Token: 0x04009BC4 RID: 39876
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> enemies;

		// Token: 0x04009BC5 RID: 39877
		[PublicizedFrom(EAccessModifier.Private)]
		public float threatLevelTarget;

		// Token: 0x04009BC6 RID: 39878
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);
	}
}
