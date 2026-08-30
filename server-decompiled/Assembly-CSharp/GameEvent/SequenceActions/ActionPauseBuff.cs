using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001993 RID: 6547
	[Preserve]
	public class ActionPauseBuff : ActionBaseClientAction
	{
		// Token: 0x0600C8B2 RID: 51378 RVA: 0x0049B4E0 File Offset: 0x004996E0
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive == null || entityAlive.Buffs.HasBuffByTag(this.buffTags);
		}

		// Token: 0x0600C8B3 RID: 51379 RVA: 0x0049B518 File Offset: 0x00499718
		public override void OnClientPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				for (int i = 0; i < entityAlive.Buffs.ActiveBuffs.Count; i++)
				{
					BuffValue buffValue = entityAlive.Buffs.ActiveBuffs[i];
					if (buffValue.BuffClass.Tags.Test_AnySet(this.buffTags))
					{
						buffValue.Paused = this.pauseState;
					}
				}
			}
		}

		// Token: 0x0600C8B4 RID: 51380 RVA: 0x0049B580 File Offset: 0x00499780
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnServerPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				for (int i = 0; i < entityAlive.Buffs.ActiveBuffs.Count; i++)
				{
					BuffValue buffValue = entityAlive.Buffs.ActiveBuffs[i];
					if (buffValue.BuffClass.Tags.Test_AnySet(this.buffTags))
					{
						buffValue.Paused = this.pauseState;
					}
				}
			}
		}

		// Token: 0x0600C8B5 RID: 51381 RVA: 0x0049B5E8 File Offset: 0x004997E8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(ActionPauseBuff.PropBuffTags, ref text);
			if (text != "")
			{
				this.buffTags = FastTags<TagGroup.Global>.Parse(text);
			}
			properties.ParseBool(ActionPauseBuff.PropPauseState, ref this.pauseState);
			properties.ParseBool(ActionPauseBuff.PropCheckAlreadyExists, ref this.checkAlreadyExists);
		}

		// Token: 0x0600C8B6 RID: 51382 RVA: 0x0049B64A File Offset: 0x0049984A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPauseBuff
			{
				buffTags = this.buffTags,
				pauseState = this.pauseState,
				targetGroup = this.targetGroup,
				checkAlreadyExists = this.checkAlreadyExists
			};
		}

		// Token: 0x04009806 RID: 38918
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x04009807 RID: 38919
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x04009808 RID: 38920
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool pauseState = true;

		// Token: 0x04009809 RID: 38921
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";

		// Token: 0x0400980A RID: 38922
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPauseState = "state";

		// Token: 0x0400980B RID: 38923
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";
	}
}
