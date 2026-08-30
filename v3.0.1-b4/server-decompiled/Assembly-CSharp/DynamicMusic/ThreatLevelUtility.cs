using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A43 RID: 6723
	public static class ThreatLevelUtility
	{
		// Token: 0x0600CB43 RID: 52035 RVA: 0x004A4440 File Offset: 0x004A2640
		public static float GetThreatLevelOn(EntityPlayerLocal _player)
		{
			GameManager.Instance.World.GetEntitiesInBounds(typeof(EntityEnemy), new Bounds(_player.position, ThreatLevelUtility.boundingBoxRange), ThreatLevelUtility.enemies);
			float num = 0f;
			int num2 = (ThreatLevelUtility.Zombies = ThreatLevelUtility.zombiesContributingThreat());
			int num3 = (ThreatLevelUtility.Targeting = ThreatLevelUtility.EnemiesTargeting());
			if (num3 > 0)
			{
				_player.LastTargetEventTime = Time.time;
			}
			if ((num2 >= 4 && num3 > 0) || (_player.ThreatLevel.Category == ThreatLevelType.Panicked && num2 > 0 && Time.time - _player.LastTargetEventTime < 15f) || GameUtils.IsBloodMoonTime(GameManager.Instance.World.worldTime, GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength)), GameStats.GetInt(EnumGameStats.BloodMoonDay)))
			{
				int num4 = GamePrefs.GetInt(EnumGamePrefs.BloodMoonEnemyCount) * ((_player.Party != null) ? _player.Party.MemberList.Count : 1);
				float num5 = 0.3f / (float)num4;
				float num6 = 0.15f / (float)num4;
				float num7 = MathUtils.Clamp(0.7f + num5 * (float)num3 + num6 * (float)(num2 - num3), 0.7f, 1f);
				ThreatLevelUtility.threatLevels.Enqueue(num7);
				if (ThreatLevelUtility.threatLevels.Count > 300)
				{
					ThreatLevelUtility.threatLevels.Dequeue();
				}
				num = MathUtils.Clamp(ThreatLevelUtility.threatLevels.Average(), 0.7f, 1f);
			}
			else
			{
				num += (GameManager.Instance.World.IsDark() ? 0.1f : 0f);
				num += (ThreatLevelUtility.isPlayerInUnclearedPOI(_player) ? 0.2f : 0f);
				num += (ThreatLevelUtility.IsPlayerHome(_player) ? 0f : 0.2f);
				num += (ThreatLevelUtility.IsPlayerInSpookyBiome(_player) ? 0.1f : 0f);
				num += (float)num2 * 0.033333335f;
			}
			ThreatLevelUtility.enemies.Clear();
			return num;
		}

		// Token: 0x0600CB44 RID: 52036 RVA: 0x004A4620 File Offset: 0x004A2820
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isPlayerInUnclearedPOI(EntityPlayerLocal _player)
		{
			if (_player.PlayerStats.LightInsidePer > 0.2f)
			{
				if (GamePrefs.GetString(EnumGamePrefs.GameWorld).Equals("Playtesting"))
				{
					return true;
				}
				if (_player.prefab != null)
				{
					using (List<SleeperVolume>.Enumerator enumerator = _player.prefab.sleeperVolumes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.wasCleared)
							{
								return true;
							}
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x0600CB45 RID: 52037 RVA: 0x004A46B0 File Offset: 0x004A28B0
		[PublicizedFrom(EAccessModifier.Private)]
		public static int zombiesContributingThreat()
		{
			int num = 0;
			for (int i = 0; i < ThreatLevelUtility.enemies.Count; i++)
			{
				EntityEnemy entityEnemy = ThreatLevelUtility.enemies[i] as EntityEnemy;
				if (entityEnemy.IsAlive() && !entityEnemy.IsSleeping)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600CB46 RID: 52038 RVA: 0x004A46FC File Offset: 0x004A28FC
		[PublicizedFrom(EAccessModifier.Private)]
		public static int EnemiesTargeting()
		{
			int num = 0;
			for (int i = 0; i < ThreatLevelUtility.enemies.Count; i++)
			{
				EntityEnemy entityEnemy = ThreatLevelUtility.enemies[i] as EntityEnemy;
				if (entityEnemy != null && entityEnemy.IsAlive() && entityEnemy.GetAttackTargetLocal() as EntityPlayer != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600CB47 RID: 52039 RVA: 0x004A475C File Offset: 0x004A295C
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsPlayerHome(EntityPlayerLocal _player)
		{
			SpawnPosition spawnPoint = _player.GetSpawnPoint();
			return !spawnPoint.IsUndef() && (spawnPoint.position - _player.position).magnitude <= 50f;
		}

		// Token: 0x0600CB48 RID: 52040 RVA: 0x004A479E File Offset: 0x004A299E
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsPlayerInSpookyBiome(EntityPlayerLocal _player)
		{
			return _player.biomeStandingOn != null && (_player.biomeStandingOn.m_sBiomeName.Equals("burnt_forest") || _player.biomeStandingOn.m_sBiomeName.Equals("wasteland"));
		}

		// Token: 0x04009A3C RID: 39484
		[PublicizedFrom(EAccessModifier.Private)]
		public const int ZOMBIE_COMBAT_QUANTITY = 4;

		// Token: 0x04009A3D RID: 39485
		[PublicizedFrom(EAccessModifier.Private)]
		public const float PLAYER_HOME_MINIMUM_DISTANCE = 50f;

		// Token: 0x04009A3E RID: 39486
		[PublicizedFrom(EAccessModifier.Private)]
		public const float THREAT_PER_ENEMY = 0.033333335f;

		// Token: 0x04009A3F RID: 39487
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Entity> enemies = new List<Entity>();

		// Token: 0x04009A40 RID: 39488
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);

		// Token: 0x04009A41 RID: 39489
		public static int Zombies;

		// Token: 0x04009A42 RID: 39490
		public static int Targeting;

		// Token: 0x04009A43 RID: 39491
		[PublicizedFrom(EAccessModifier.Private)]
		public static Queue<float> threatLevels = new Queue<float>();

		// Token: 0x04009A44 RID: 39492
		[PublicizedFrom(EAccessModifier.Private)]
		public const int LOOKBACK = 300;
	}
}
