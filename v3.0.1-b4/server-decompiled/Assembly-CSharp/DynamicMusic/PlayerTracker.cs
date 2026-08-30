using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A30 RID: 6704
	public class PlayerTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x0600CAD7 RID: 51927 RVA: 0x0034A450 File Offset: 0x00348650
		public EntityPlayerLocal player
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.World.GetPrimaryPlayer();
			}
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x0600CAD8 RID: 51928 RVA: 0x004A3490 File Offset: 0x004A1690
		public bool isPlayerInTraderArea
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				TraderArea traderAreaAt = GameManager.Instance.World.GetTraderAreaAt(this.player.GetBlockPosition());
				return traderAreaAt != null && this.IsTraderAreaOpen(traderAreaAt);
			}
		}

		// Token: 0x0600CAD9 RID: 51929 RVA: 0x004A34C4 File Offset: 0x004A16C4
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

		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x0600CADA RID: 51930 RVA: 0x004A3570 File Offset: 0x004A1770
		public bool isPlayerHome
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return !this.player.GetSpawnPoint().IsUndef() && (this.player.GetSpawnPoint().position - this.player.position).magnitude < 50f;
			}
		}

		// Token: 0x0600CADB RID: 51931 RVA: 0x004A35C4 File Offset: 0x004A17C4
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

		// Token: 0x0600CADC RID: 51932 RVA: 0x004A36FD File Offset: 0x004A18FD
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPlayerInPOI()
		{
			return (this.player.prefab != null || GamePrefs.GetString(EnumGamePrefs.GameWorld).Equals("Playtesting")) && this.player.PlayerStats.LightInsidePer > 0.2f;
		}

		// Token: 0x0600CADD RID: 51933 RVA: 0x004A3738 File Offset: 0x004A1938
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

		// Token: 0x0600CADE RID: 51934 RVA: 0x004A37C0 File Offset: 0x004A19C0
		public override string ToString()
		{
			return "PlayerTracker:\n" + string.Format("Is Player in a trader station: {0}\n", this.isPlayerInTraderArea) + string.Format("Is Player home: {0}\n", this.isPlayerHome) + string.Format("Player Threat Level: {0}", this.player.ThreatLevel.Category);
		}

		// Token: 0x04009A0F RID: 39439
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMaxHomeDistance = 50f;

		// Token: 0x04009A10 RID: 39440
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> traders = new List<Entity>();

		// Token: 0x04009A11 RID: 39441
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(200f, 200f, 200f);

		// Token: 0x04009A12 RID: 39442
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Entity> npcs = new List<Entity>();
	}
}
