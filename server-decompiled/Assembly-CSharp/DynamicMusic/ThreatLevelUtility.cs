using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A6A RID: 6762
	public static class ThreatLevelUtility
	{
		// Token: 0x0600CCCC RID: 52428 RVA: 0x004AA834 File Offset: 0x004A8A34
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

		// Token: 0x0600CCCD RID: 52429 RVA: 0x004AAA14 File Offset: 0x004A8C14
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

		// Token: 0x0600CCCE RID: 52430 RVA: 0x004AAAA4 File Offset: 0x004A8CA4
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

		// Token: 0x0600CCCF RID: 52431 RVA: 0x004AAAF0 File Offset: 0x004A8CF0
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

		// Token: 0x0600CCD0 RID: 52432 RVA: 0x004AAB50 File Offset: 0x004A8D50
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsPlayerHome(EntityPlayerLocal _player)
		{
			SpawnPosition spawnPoint = _player.GetSpawnPoint();
			return !spawnPoint.IsUndef() && (spawnPoint.position - _player.position).magnitude <= 50f;
		}

		// Token: 0x0600CCD1 RID: 52433 RVA: 0x004AAB92 File Offset: 0x004A8D92
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsPlayerInSpookyBiome(EntityPlayerLocal _player)
		{
			return _player.biomeStandingOn != null && (_player.biomeStandingOn.m_sBiomeName.Equals("burnt_forest") || _player.biomeStandingOn.m_sBiomeName.Equals("wasteland"));
		}

		// Token: 0x04009B6D RID: 39789
		[PublicizedFrom(EAccessModifier.Private)]
		public const int ZOMBIE_COMBAT_QUANTITY = 4;

		// Token: 0x04009B6E RID: 39790
		[PublicizedFrom(EAccessModifier.Private)]
		public const float PLAYER_HOME_MINIMUM_DISTANCE = 50f;

		// Token: 0x04009B6F RID: 39791
		[PublicizedFrom(EAccessModifier.Private)]
		public const float THREAT_PER_ENEMY = 0.033333335f;

		// Token: 0x04009B70 RID: 39792
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Entity> enemies = new List<Entity>();

		// Token: 0x04009B71 RID: 39793
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);

		// Token: 0x04009B72 RID: 39794
		public static int Zombies;

		// Token: 0x04009B73 RID: 39795
		public static int Targeting;

		// Token: 0x04009B74 RID: 39796
		[PublicizedFrom(EAccessModifier.Private)]
		public static Queue<float> threatLevels = new Queue<float>();

		// Token: 0x04009B75 RID: 39797
		[PublicizedFrom(EAccessModifier.Private)]
		public const int LOOKBACK = 300;
	}
}
