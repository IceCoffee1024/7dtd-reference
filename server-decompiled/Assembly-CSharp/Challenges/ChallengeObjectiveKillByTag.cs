using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001934 RID: 6452
	[Preserve]
	public class ChallengeObjectiveKillByTag : BaseChallengeObjective
	{
		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x0600C73C RID: 51004 RVA: 0x0026ADCB File Offset: 0x00268FCB
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.KillByTag;
			}
		}

		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x0600C73D RID: 51005 RVA: 0x00491830 File Offset: 0x0048FA30
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

		// Token: 0x0600C73E RID: 51006 RVA: 0x004918AD File Offset: 0x0048FAAD
		public override void Init()
		{
			this.entityTags = FastTags<TagGroup.Global>.Parse(this.entityTag);
		}

		// Token: 0x0600C73F RID: 51007 RVA: 0x004918C0 File Offset: 0x0048FAC0
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

		// Token: 0x0600C740 RID: 51008 RVA: 0x0049194F File Offset: 0x0048FB4F
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C741 RID: 51009 RVA: 0x00491968 File Offset: 0x0048FB68
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

		// Token: 0x0600C742 RID: 51010 RVA: 0x00491A20 File Offset: 0x0048FC20
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

		// Token: 0x0600C743 RID: 51011 RVA: 0x00491B5C File Offset: 0x0048FD5C
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

		// Token: 0x04009623 RID: 38435
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityTag = "";

		// Token: 0x04009624 RID: 38436
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> entityTags;

		// Token: 0x04009625 RID: 38437
		[PublicizedFrom(EAccessModifier.Private)]
		public string targetName = "";

		// Token: 0x04009626 RID: 38438
		[PublicizedFrom(EAccessModifier.Private)]
		public int isTwitchSpawn = -1;

		// Token: 0x04009627 RID: 38439
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> killerHasBuffTag;

		// Token: 0x04009628 RID: 38440
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> killedHasBuffTag;

		// Token: 0x04009629 RID: 38441
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
