using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BF RID: 6591
	[Preserve]
	public class ActionPauseBuff : ActionBaseClientAction
	{
		// Token: 0x0600CA0A RID: 51722 RVA: 0x0049F9A4 File Offset: 0x0049DBA4
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive == null || entityAlive.Buffs.HasBuffByTag(this.buffTags);
		}

		// Token: 0x0600CA0B RID: 51723 RVA: 0x0049F9DC File Offset: 0x0049DBDC
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

		// Token: 0x0600CA0C RID: 51724 RVA: 0x0049FA44 File Offset: 0x0049DC44
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

		// Token: 0x0600CA0D RID: 51725 RVA: 0x0049FAAC File Offset: 0x0049DCAC
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

		// Token: 0x0600CA0E RID: 51726 RVA: 0x0049FB0E File Offset: 0x0049DD0E
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

		// Token: 0x040098CC RID: 39116
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040098CD RID: 39117
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x040098CE RID: 39118
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool pauseState = true;

		// Token: 0x040098CF RID: 39119
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";

		// Token: 0x040098D0 RID: 39120
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPauseState = "state";

		// Token: 0x040098D1 RID: 39121
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";
	}
}
