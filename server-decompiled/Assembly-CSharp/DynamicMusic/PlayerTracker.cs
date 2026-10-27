using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AB4 RID: 6836
	public class PlayerTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600CECC RID: 52940 RVA: 0x0035513C File Offset: 0x0035333C
		public EntityPlayerLocal player
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.World.GetPrimaryPlayer();
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x0600CECD RID: 52941 RVA: 0x004B184C File Offset: 0x004AFA4C
		public bool isPlayerInTraderArea
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				TraderArea traderAreaAt = GameManager.Instance.World.GetTraderAreaAt(this.player.GetBlockPosition());
				return traderAreaAt != null && this.IsTraderAreaOpen(traderAreaAt);
			}
		}

		// Token: 0x0600CECE RID: 52942 RVA: 0x004B1880 File Offset: 0x004AFA80
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsTraderAreaOpen(TraderArea _ta)
		{
			Vector3 vector = _ta.Position.ToVector3() + _ta.PrefabSize.ToVector3() / 2f;
			Bounds bounds = new Bounds(vector, _ta.PrefabSize.ToVector3());
			GameManager.Instance.World.GetEntitiesInBounds(typeof(EntityTrader), bounds, this.traders);
			if (this.traders.Count <= 0)
			{
				return false;
			}
			EntityTrader entityTrader = this.traders[0] as EntityTrader;
			if (entityTrader.TraderInfo == null)
			{
				return false;
			}
			this.traders.Clear();
			return entityTrader.TraderInfo.IsOpen;
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x0600CECF RID: 52943 RVA: 0x004B192C File Offset: 0x004AFB2C
		public bool isPlayerHome
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return !this.player.GetSpawnPoint().IsUndef() && (this.player.GetSpawnPoint().position - this.player.position).magnitude < 50f;
			}
		}

		// Token: 0x0600CED0 RID: 52944 RVA: 0x004B1980 File Offset: 0x004AFB80
		public override List<SectionType> Filter(List<SectionType> _sectionTypes)
		{
			if (this.player != null && this.player.IsAlive())
			{
				if (this.isPlayerInTraderArea)
				{
					_sectionTypes.Clear();
					_sectionTypes.Add(this.determineTrader());
					return _sectionTypes;
				}
				_sectionTypes.Remove(SectionType.TraderBob);
				_sectionTypes.Remove(SectionType.TraderHugh);
				_sectionTypes.Remove(SectionType.TraderJen);
				_sectionTypes.Remove(SectionType.TraderJoel);
				_sectionTypes.Remove(SectionType.TraderRekt);
				switch (this.player.ThreatLevel.Category)
				{
				case ThreatLevelType.Safe:
					_sectionTypes.Remove(SectionType.Suspense);
					_sectionTypes.Remove(SectionType.Combat);
					_sectionTypes.Remove(SectionType.Bloodmoon);
					if (this.isPlayerHome)
					{
						_sectionTypes.Remove(SectionType.Exploration);
					}
					else
					{
						_sectionTypes.Remove(SectionType.HomeDay);
						_sectionTypes.Remove(SectionType.HomeNight);
						if (this.isPlayerInPOI())
						{
							_sectionTypes.Remove(SectionType.Exploration);
						}
					}
					break;
				case ThreatLevelType.Spooked:
					_sectionTypes.Remove(SectionType.HomeDay);
					_sectionTypes.Remove(SectionType.HomeNight);
					_sectionTypes.Remove(SectionType.Exploration);
					_sectionTypes.Remove(SectionType.Combat);
					_sectionTypes.Remove(SectionType.Bloodmoon);
					break;
				case ThreatLevelType.Panicked:
					_sectionTypes.Clear();
					_sectionTypes.Add(SectionType.Combat);
					_sectionTypes.Add(SectionType.Bloodmoon);
					break;
				}
			}
			else
			{
				_sectionTypes.Clear();
				_sectionTypes.Add(SectionType.None);
			}
			return _sectionTypes;
		}

		// Token: 0x0600CED1 RID: 52945 RVA: 0x004B1AB9 File Offset: 0x004AFCB9
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPlayerInPOI()
		{
			return (this.player.prefab != null || GamePrefs.GetString(EnumGamePrefs.GameWorld).Equals("Playtesting")) && this.player.PlayerStats.LightInsidePer > 0.2f;
		}

		// Token: 0x0600CED2 RID: 52946 RVA: 0x004B1AF4 File Offset: 0x004AFCF4
		[PublicizedFrom(EAccessModifier.Private)]
		public SectionType determineTrader()
		{
			PlayerTracker.npcs.Clear();
			GameManager.Instance.World.GetEntitiesInBounds(typeof(EntityTrader), new Bounds(this.player.position, PlayerTracker.boundingBoxRange), PlayerTracker.npcs);
			if (PlayerTracker.npcs.Count > 0)
			{
				EntityTrader entityTrader = PlayerTracker.npcs[0] as EntityTrader;
				if (entityTrader != null)
				{
					NPCInfo npcinfo = entityTrader.NPCInfo;
					if (npcinfo == null)
					{
						return SectionType.None;
					}
					return npcinfo.DmsSectionType;
				}
			}
			return SectionType.None;
		}

		// Token: 0x0600CED3 RID: 52947 RVA: 0x004B1B7C File Offset: 0x004AFD7C
		public override string ToString()
		{
			return "PlayerTracker:\n" + string.Format("Is Player in a trader station: {0}\n", this.isPlayerInTraderArea) + string.Format("Is Player home: {0}\n", this.isPlayerHome) + string.Format("Player Threat Level: {0}", this.player.ThreatLevel.Category);
		}

		// Token: 0x04009CEF RID: 40175
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMaxHomeDistance = 50f;

		// Token: 0x04009CF0 RID: 40176
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> traders = new List<Entity>();

		// Token: 0x04009CF1 RID: 40177
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(200f, 200f, 200f);

		// Token: 0x04009CF2 RID: 40178
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Entity> npcs = new List<Entity>();
	}
}
