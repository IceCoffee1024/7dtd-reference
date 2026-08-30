using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A57 RID: 6743
	public class PlayerTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600CC60 RID: 52320 RVA: 0x0034EC74 File Offset: 0x0034CE74
		public EntityPlayerLocal player
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.World.GetPrimaryPlayer();
			}
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600CC61 RID: 52321 RVA: 0x004A9884 File Offset: 0x004A7A84
		public bool isPlayerInTraderArea
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				TraderArea traderAreaAt = GameManager.Instance.World.GetTraderAreaAt(this.player.GetBlockPosition());
				return traderAreaAt != null && this.IsTraderAreaOpen(traderAreaAt);
			}
		}

		// Token: 0x0600CC62 RID: 52322 RVA: 0x004A98B8 File Offset: 0x004A7AB8
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

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600CC63 RID: 52323 RVA: 0x004A9964 File Offset: 0x004A7B64
		public bool isPlayerHome
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return !this.player.GetSpawnPoint().IsUndef() && (this.player.GetSpawnPoint().position - this.player.position).magnitude < 50f;
			}
		}

		// Token: 0x0600CC64 RID: 52324 RVA: 0x004A99B8 File Offset: 0x004A7BB8
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

		// Token: 0x0600CC65 RID: 52325 RVA: 0x004A9AF1 File Offset: 0x004A7CF1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPlayerInPOI()
		{
			return (this.player.prefab != null || GamePrefs.GetString(EnumGamePrefs.GameWorld).Equals("Playtesting")) && this.player.PlayerStats.LightInsidePer > 0.2f;
		}

		// Token: 0x0600CC66 RID: 52326 RVA: 0x004A9B2C File Offset: 0x004A7D2C
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

		// Token: 0x0600CC67 RID: 52327 RVA: 0x004A9BB4 File Offset: 0x004A7DB4
		public override string ToString()
		{
			return "PlayerTracker:\n" + string.Format("Is Player in a trader station: {0}\n", this.isPlayerInTraderArea) + string.Format("Is Player home: {0}\n", this.isPlayerHome) + string.Format("Player Threat Level: {0}", this.player.ThreatLevel.Category);
		}

		// Token: 0x04009B40 RID: 39744
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMaxHomeDistance = 50f;

		// Token: 0x04009B41 RID: 39745
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> traders = new List<Entity>();

		// Token: 0x04009B42 RID: 39746
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(200f, 200f, 200f);

		// Token: 0x04009B43 RID: 39747
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Entity> npcs = new List<Entity>();
	}
}
