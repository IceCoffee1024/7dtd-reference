using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F0 RID: 6640
	[Preserve]
	public class ActionPauseBuff : ActionBaseClientAction
	{
		// Token: 0x0600CB1E RID: 51998 RVA: 0x004A34A4 File Offset: 0x004A16A4
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive == null || entityAlive.Buffs.HasBuffByTag(this.buffTags);
		}

		// Token: 0x0600CB1F RID: 51999 RVA: 0x004A34DC File Offset: 0x004A16DC
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

		// Token: 0x0600CB20 RID: 52000 RVA: 0x004A3544 File Offset: 0x004A1744
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

		// Token: 0x0600CB21 RID: 52001 RVA: 0x004A35AC File Offset: 0x004A17AC
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

		// Token: 0x0600CB22 RID: 52002 RVA: 0x004A360E File Offset: 0x004A180E
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

		// Token: 0x040099B5 RID: 39349
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040099B6 RID: 39350
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x040099B7 RID: 39351
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool pauseState = true;

		// Token: 0x040099B8 RID: 39352
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";

		// Token: 0x040099B9 RID: 39353
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPauseState = "state";

		// Token: 0x040099BA RID: 39354
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";
	}
}
