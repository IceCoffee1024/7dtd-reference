using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A83 RID: 6787
	public class PlayerTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x0600CDB8 RID: 52664 RVA: 0x0035308C File Offset: 0x0035128C
		public EntityPlayerLocal player
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.World.GetPrimaryPlayer();
			}
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x0600CDB9 RID: 52665 RVA: 0x004ADD50 File Offset: 0x004ABF50
		public bool isPlayerInTraderArea
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				TraderArea traderAreaAt = GameManager.Instance.World.GetTraderAreaAt(this.player.GetBlockPosition());
				return traderAreaAt != null && this.IsTraderAreaOpen(traderAreaAt);
			}
		}

		// Token: 0x0600CDBA RID: 52666 RVA: 0x004ADD84 File Offset: 0x004ABF84
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

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x0600CDBB RID: 52667 RVA: 0x004ADE30 File Offset: 0x004AC030
		public bool isPlayerHome
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return !this.player.GetSpawnPoint().IsUndef() && (this.player.GetSpawnPoint().position - this.player.position).magnitude < 50f;
			}
		}

		// Token: 0x0600CDBC RID: 52668 RVA: 0x004ADE84 File Offset: 0x004AC084
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

		// Token: 0x0600CDBD RID: 52669 RVA: 0x004ADFBD File Offset: 0x004AC1BD
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPlayerInPOI()
		{
			return (this.player.prefab != null || GamePrefs.GetString(EnumGamePrefs.GameWorld).Equals("Playtesting")) && this.player.PlayerStats.LightInsidePer > 0.2f;
		}

		// Token: 0x0600CDBE RID: 52670 RVA: 0x004ADFF8 File Offset: 0x004AC1F8
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

		// Token: 0x0600CDBF RID: 52671 RVA: 0x004AE080 File Offset: 0x004AC280
		public override string ToString()
		{
			return "PlayerTracker:\n" + string.Format("Is Player in a trader station: {0}\n", this.isPlayerInTraderArea) + string.Format("Is Player home: {0}\n", this.isPlayerHome) + string.Format("Player Threat Level: {0}", this.player.ThreatLevel.Category);
		}

		// Token: 0x04009C06 RID: 39942
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMaxHomeDistance = 50f;

		// Token: 0x04009C07 RID: 39943
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> traders = new List<Entity>();

		// Token: 0x04009C08 RID: 39944
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 boundingBoxRange = new Vector3(200f, 200f, 200f);

		// Token: 0x04009C09 RID: 39945
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Entity> npcs = new List<Entity>();
	}
}
