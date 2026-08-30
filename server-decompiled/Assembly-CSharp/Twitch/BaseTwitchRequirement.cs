using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001832 RID: 6194
	[Preserve]
	public class BaseTwitchRequirement
	{
		// Token: 0x0600BF0A RID: 48906 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600BF0B RID: 48907 RVA: 0x0046E987 File Offset: 0x0046CB87
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600BF0C RID: 48908 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual bool CanPerform(Entity player)
		{
			return true;
		}

		// Token: 0x0600BF0D RID: 48909 RVA: 0x0046E98F File Offset: 0x0046CB8F
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			properties.ParseBool(BaseTwitchRequirement.PropInvert, ref this.Invert);
			properties.ParseBool(BaseTwitchRequirement.PropHidesAction, ref this.HideAction);
		}

		// Token: 0x04009014 RID: 36884
		public BaseTwitchRequirement.OwnerTypes OwnerType;

		// Token: 0x04009015 RID: 36885
		public TwitchAction OwnerAction;

		// Token: 0x04009016 RID: 36886
		public TwitchVote OwnerVote;

		// Token: 0x04009017 RID: 36887
		public BaseTwitchEventEntry OwnerEvent;

		// Token: 0x04009018 RID: 36888
		public DynamicProperties Properties;

		// Token: 0x04009019 RID: 36889
		public bool HideAction;

		// Token: 0x0400901A RID: 36890
		public bool Invert;

		// Token: 0x0400901B RID: 36891
		public static string PropInvert = "invert";

		// Token: 0x0400901C RID: 36892
		public static string PropHidesAction = "hide_action";

		// Token: 0x02001833 RID: 6195
		public enum OwnerTypes
		{
			// Token: 0x0400901E RID: 36894
			Action,
			// Token: 0x0400901F RID: 36895
			Vote,
			// Token: 0x04009020 RID: 36896
			Event
		}
	}
}
