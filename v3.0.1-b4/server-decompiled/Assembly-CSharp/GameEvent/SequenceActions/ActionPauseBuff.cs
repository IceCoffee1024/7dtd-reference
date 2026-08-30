using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200196D RID: 6509
	[Preserve]
	public class ActionPauseBuff : ActionBaseClientAction
	{
		// Token: 0x0600C72B RID: 50987 RVA: 0x0049519C File Offset: 0x0049339C
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive == null || entityAlive.Buffs.HasBuffByTag(this.buffTags);
		}

		// Token: 0x0600C72C RID: 50988 RVA: 0x004951D4 File Offset: 0x004933D4
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

		// Token: 0x0600C72D RID: 50989 RVA: 0x0049523C File Offset: 0x0049343C
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

		// Token: 0x0600C72E RID: 50990 RVA: 0x004952A4 File Offset: 0x004934A4
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

		// Token: 0x0600C72F RID: 50991 RVA: 0x00495306 File Offset: 0x00493506
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

		// Token: 0x040096DD RID: 38621
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040096DE RID: 38622
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x040096DF RID: 38623
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool pauseState = true;

		// Token: 0x040096E0 RID: 38624
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";

		// Token: 0x040096E1 RID: 38625
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPauseState = "state";

		// Token: 0x040096E2 RID: 38626
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";
	}
}
