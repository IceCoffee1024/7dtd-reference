using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001884 RID: 6276
	[Preserve]
	public class BaseTwitchRequirement
	{
		// Token: 0x0600C1E7 RID: 49639 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C1E8 RID: 49640 RVA: 0x00478C5F File Offset: 0x00476E5F
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C1E9 RID: 49641 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(Entity player)
		{
			return true;
		}

		// Token: 0x0600C1EA RID: 49642 RVA: 0x00478C67 File Offset: 0x00476E67
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			properties.ParseBool(BaseTwitchRequirement.PropInvert, ref this.Invert);
			properties.ParseBool(BaseTwitchRequirement.PropHidesAction, ref this.HideAction);
		}

		// Token: 0x040091F6 RID: 37366
		public BaseTwitchRequirement.OwnerTypes OwnerType;

		// Token: 0x040091F7 RID: 37367
		public TwitchAction OwnerAction;

		// Token: 0x040091F8 RID: 37368
		public TwitchVote OwnerVote;

		// Token: 0x040091F9 RID: 37369
		public BaseTwitchEventEntry OwnerEvent;

		// Token: 0x040091FA RID: 37370
		public DynamicProperties Properties;

		// Token: 0x040091FB RID: 37371
		public bool HideAction;

		// Token: 0x040091FC RID: 37372
		public bool Invert;

		// Token: 0x040091FD RID: 37373
		public static string PropInvert = "invert";

		// Token: 0x040091FE RID: 37374
		public static string PropHidesAction = "hide_action";

		// Token: 0x02001885 RID: 6277
		public enum OwnerTypes
		{
			// Token: 0x04009200 RID: 37376
			Action,
			// Token: 0x04009201 RID: 37377
			Vote,
			// Token: 0x04009202 RID: 37378
			Event
		}
	}
}
