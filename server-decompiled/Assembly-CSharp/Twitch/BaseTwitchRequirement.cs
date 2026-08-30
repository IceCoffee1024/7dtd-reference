using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001858 RID: 6232
	[Preserve]
	public class BaseTwitchRequirement
	{
		// Token: 0x0600C08F RID: 49295 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C090 RID: 49296 RVA: 0x00474723 File Offset: 0x00472923
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C091 RID: 49297 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual bool CanPerform(Entity player)
		{
			return true;
		}

		// Token: 0x0600C092 RID: 49298 RVA: 0x0047472B File Offset: 0x0047292B
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			properties.ParseBool(BaseTwitchRequirement.PropInvert, ref this.Invert);
			properties.ParseBool(BaseTwitchRequirement.PropHidesAction, ref this.HideAction);
		}

		// Token: 0x04009130 RID: 37168
		public BaseTwitchRequirement.OwnerTypes OwnerType;

		// Token: 0x04009131 RID: 37169
		public TwitchAction OwnerAction;

		// Token: 0x04009132 RID: 37170
		public TwitchVote OwnerVote;

		// Token: 0x04009133 RID: 37171
		public BaseTwitchEventEntry OwnerEvent;

		// Token: 0x04009134 RID: 37172
		public DynamicProperties Properties;

		// Token: 0x04009135 RID: 37173
		public bool HideAction;

		// Token: 0x04009136 RID: 37174
		public bool Invert;

		// Token: 0x04009137 RID: 37175
		public static string PropInvert = "invert";

		// Token: 0x04009138 RID: 37176
		public static string PropHidesAction = "hide_action";

		// Token: 0x02001859 RID: 6233
		public enum OwnerTypes
		{
			// Token: 0x0400913A RID: 37178
			Action,
			// Token: 0x0400913B RID: 37179
			Vote,
			// Token: 0x0400913C RID: 37180
			Event
		}
	}
}
