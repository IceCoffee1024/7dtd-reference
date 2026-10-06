using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC5 RID: 6853
	public class ThreatLevelTracker
	{
		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x0600CEDF RID: 52959 RVA: 0x004B0538 File Offset: 0x004AE738
		// (set) Token: 0x0600CEE0 RID: 52960 RVA: 0x004B0540 File Offset: 0x004AE740
		public int DeadEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x0600CEE1 RID: 52961 RVA: 0x004B0549 File Offset: 0x004AE749
		// (set) Token: 0x0600CEE2 RID: 52962 RVA: 0x004B0551 File Offset: 0x004AE751
		public int InactiveEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x0600CEE3 RID: 52963 RVA: 0x004B055A File Offset: 0x004AE75A
		// (set) Token: 0x0600CEE4 RID: 52964 RVA: 0x004B0562 File Offset: 0x004AE762
		public int SleepingEnemies
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x0600CEE5 RID: 52965 RVA: 0x004B056B File Offset: 0x004AE76B
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

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x0600CEE6 RID: 52966 RVA: 0x004B0597 File Offset: 0x004AE797
		// (set) Token: 0x0600CEE7 RID: 52967 RVA: 0x004B059F File Offset: 0x004AE79F
		public float NumericalThreatLevel
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x0600CEE8 RID: 52968 RVA: 0x004B05A8 File Offset: 0x004AE7A8
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

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x0600CEE9 RID: 52969 RVA: 0x004B05BA File Offset: 0x004AE7BA
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x0600CEEA RID: 52970 RVA: 0x004B05C7 File Offset: 0x004AE7C7
		public bool IsThreatLevelInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.NumericalThreatLevel < 0.25f;
			}
		}

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x0600CEEB RID: 52971 RVA: 0x004B05D6 File Offset: 0x004AE7D6
		public bool IsTargetInExploration
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget <= 0.25f;
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x0600CEEC RID: 52972 RVA: 0x004B05E8 File Offset: 0x004AE7E8
		public bool IsTargetAboveThreatLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.threatLevelTarget > this.NumericalThreatLevel;
			}
		}

		// Token: 0x0600CEED RID: 52973 RVA: 0x004B05F8 File Offset: 0x004AE7F8
		public static void Init(DynamicMusicManager _dmManager)
		{
			_dmManager.ThreatLevelTracker = new ThreatLevelTracker();
			_dmManager.ThreatLevelTracker.dynamicMusicManager = _dmManager;
			_dmManager.ThreatLevelTracker.somePlayer = GameManager.Instance.World.GetPrimaryPlayer();
			_dmManager.ThreatLevelTracker.epLocal = _dmManager.PrimaryLocalPlayer;
			_dmManager.ThreatLevelTracker.NumericalThreatLevel = 0f;
			_dmManager.ThreatLevelTracker.enemies = new List<Entity>();
		}

		// Token: 0x0600CEEE RID: 52974 RVA: 0x004B0667 File Offset: 0x004AE867
		public void Tick()
		{
			if (GameTimer.Instance.ticks % 20UL == 0UL)
			{
				this.TickTrackThreatLevel();
			}
			this.TickMoveThreatLevel();
		}

		// Token: 0x0600CEEF RID: 52975 RVA: 0x004B0688 File Offset: 0x004AE888
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

		// Token: 0x0600CEF0 RID: 52976 RVA: 0x004B071C File Offset: 0x004AE91C
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

		// Token: 0x0600CEF1 RID: 52977 RVA: 0x004B087C File Offset: 0x004AEA7C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool EnemyIsTargetingPlayer(EntityEnemy _enemy)
		{
			EntityAlive attackTarget = _enemy.GetAttackTarget();
			return attackTarget != null && attackTarget.Equals(this.epLocal);
		}

		// Token: 0x0600CEF2 RID: 52978 RVA: 0x000027FC File Offset: 0x000009FC
		public void Event(MinEventTypes _eventType, MinEventParams _eventParms)
		{
		}

		// Token: 0x04009C7E RID: 40062
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009C7F RID: 40063
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cSleeperIncrement = 0.03125f;

		// Token: 0x04009C80 RID: 40064
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cTargetIncrement = 0.25f;

		// Token: 0x04009C81 RID: 40065
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cAlertIncrement = 0.125f;

		// Token: 0x04009C82 RID: 40066
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cInactiveIncrement = 0.0625f;

		// Token: 0x04009C83 RID: 40067
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cBaseIncrement = 0.0015f;

		// Token: 0x04009C88 RID: 40072
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal epLocal;

		// Token: 0x04009C89 RID: 40073
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal somePlayer;

		// Token: 0x04009C8A RID: 40074
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> enemies;

		// Token: 0x04009C8B RID: 40075
		[PublicizedFrom(EAccessModifier.Private)]
		public float threatLevelTarget;

		// Token: 0x04009C8C RID: 40076
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);
	}
}
