using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001908 RID: 6408
	[Preserve]
	public class ChallengeObjectiveKillByTag : BaseChallengeObjective
	{
		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x0600C5E4 RID: 50660 RVA: 0x0005F529 File Offset: 0x0005D729
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.KillByTag;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x0600C5E5 RID: 50661 RVA: 0x0048D340 File Offset: 0x0048B540
		public override string DescriptionText
		{
			get
			{
				if (this.Biome == "")
				{
					return Localization.Get("challengeObjectiveKill", false, null) + " " + Localization.Get(this.targetName, false, null) + ":";
				}
				return string.Format(Localization.Get("challengeObjectiveKillIn", false, null), Localization.Get(this.targetName, false, null), Localization.Get("biome_" + this.Biome, false, null));
			}
		}

		// Token: 0x0600C5E6 RID: 50662 RVA: 0x0048D3BD File Offset: 0x0048B5BD
		public override void Init()
		{
			this.entityTags = FastTags<TagGroup.Global>.Parse(this.entityTag);
		}

		// Token: 0x0600C5E7 RID: 50663 RVA: 0x0048D3D0 File Offset: 0x0048B5D0
		public override void HandleAddHooks()
		{
			if (!EntityFactory.EnemySpawnMode && this.isEnemy)
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				this.Owner.HandleComplete(false, true);
				if (this.Owner.ChallengeState == Challenge.ChallengeStates.Completed)
				{
					this.Owner.AutoCompleted = true;
					this.Owner.ChallengeState = Challenge.ChallengeStates.Redeemed;
					return;
				}
			}
			else
			{
				QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
				QuestEventManager.Current.EntityKill += this.Current_EntityKill;
			}
		}

		// Token: 0x0600C5E8 RID: 50664 RVA: 0x0048D45F File Offset: 0x0048B65F
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C5E9 RID: 50665 RVA: 0x0048D478 File Offset: 0x0048B678
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_EntityKill(EntityAlive killedBy, EntityAlive killedEntity)
		{
			if (!this.entityTags.Test_AnySet(killedEntity.EntityClass.Tags))
			{
				return;
			}
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.isTwitchSpawn > -1)
			{
				if (this.isTwitchSpawn == 0 && killedEntity.spawnById != -1)
				{
					return;
				}
				if (this.isTwitchSpawn == 1 && killedEntity.spawnById == -1)
				{
					return;
				}
			}
			if (!this.killerHasBuffTag.IsEmpty && !killedBy.Buffs.HasBuffByTag(this.killerHasBuffTag))
			{
				return;
			}
			if (!this.killedHasBuffTag.IsEmpty && !killedEntity.Buffs.HasBuffByTag(this.killedHasBuffTag))
			{
				return;
			}
			int num = base.Current;
			base.Current = num + 1;
			this.CheckObjectiveComplete(true);
		}

		// Token: 0x0600C5EA RID: 50666 RVA: 0x0048D530 File Offset: 0x0048B730
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("entity_tags"))
			{
				this.entityTag = e.GetAttribute("entity_tags");
			}
			if (e.HasAttribute("target_name_key"))
			{
				this.targetName = Localization.Get(e.GetAttribute("target_name_key"), false, null);
			}
			else if (e.HasAttribute("target_name"))
			{
				this.targetName = e.GetAttribute("target_name");
			}
			if (e.HasAttribute("is_twitch_spawn"))
			{
				this.isTwitchSpawn = (StringParsers.ParseBool(e.GetAttribute("is_twitch_spawn"), 0, -1, true) ? 1 : 0);
			}
			if (e.HasAttribute("killer_has_bufftag"))
			{
				this.killerHasBuffTag = FastTags<TagGroup.Global>.Parse(e.GetAttribute("killer_has_bufftag"));
			}
			if (e.HasAttribute("killed_has_bufftag"))
			{
				this.killedHasBuffTag = FastTags<TagGroup.Global>.Parse(e.GetAttribute("killed_has_bufftag"));
			}
			e.ParseAttribute("is_enemy", ref this.isEnemy);
		}

		// Token: 0x0600C5EB RID: 50667 RVA: 0x0048D66C File Offset: 0x0048B86C
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveKillByTag
			{
				entityTag = this.entityTag,
				entityTags = this.entityTags,
				Biome = this.Biome,
				isEnemy = this.isEnemy,
				targetName = this.targetName,
				isTwitchSpawn = this.isTwitchSpawn,
				killerHasBuffTag = this.killerHasBuffTag,
				killedHasBuffTag = this.killedHasBuffTag
			};
		}

		// Token: 0x0400955D RID: 38237
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityTag = "";

		// Token: 0x0400955E RID: 38238
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> entityTags;

		// Token: 0x0400955F RID: 38239
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009560 RID: 38240
		[PublicizedFrom(EAccessModifier.Private)]
		public int isTwitchSpawn = -1;

		// Token: 0x04009561 RID: 38241
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> killerHasBuffTag;

		// Token: 0x04009562 RID: 38242
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> killedHasBuffTag;

		// Token: 0x04009563 RID: 38243
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
