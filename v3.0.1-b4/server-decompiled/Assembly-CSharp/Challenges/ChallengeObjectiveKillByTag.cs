using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E2 RID: 6370
	[Preserve]
	public class ChallengeObjectiveKillByTag : BaseChallengeObjective
	{
		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x0600C45D RID: 50269 RVA: 0x0005EFE1 File Offset: 0x0005D1E1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.KillByTag;
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x0600C45E RID: 50270 RVA: 0x00486FF4 File Offset: 0x004851F4
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

		// Token: 0x0600C45F RID: 50271 RVA: 0x00487071 File Offset: 0x00485271
		public override void Init()
		{
			this.entityTags = FastTags<TagGroup.Global>.Parse(this.entityTag);
		}

		// Token: 0x0600C460 RID: 50272 RVA: 0x00487084 File Offset: 0x00485284
		public override void HandleAddHooks()
		{
			if (!EntityFactory.EnemySpawnMode && this.isEnemy)
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				this.Owner.HandleComplete(false);
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

		// Token: 0x0600C461 RID: 50273 RVA: 0x00487112 File Offset: 0x00485312
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C462 RID: 50274 RVA: 0x0048712C File Offset: 0x0048532C
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

		// Token: 0x0600C463 RID: 50275 RVA: 0x004871E4 File Offset: 0x004853E4
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

		// Token: 0x0600C464 RID: 50276 RVA: 0x00487320 File Offset: 0x00485520
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

		// Token: 0x04009434 RID: 37940
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityTag = "";

		// Token: 0x04009435 RID: 37941
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> entityTags;

		// Token: 0x04009436 RID: 37942
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009437 RID: 37943
		[PublicizedFrom(EAccessModifier.Private)]
		public int isTwitchSpawn = -1;

		// Token: 0x04009438 RID: 37944
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> killerHasBuffTag;

		// Token: 0x04009439 RID: 37945
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> killedHasBuffTag;

		// Token: 0x0400943A RID: 37946
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
