using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018B5 RID: 6325
	[Preserve]
	public class BaseTwitchRequirement
	{
		// Token: 0x0600C2FB RID: 49915 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C2FC RID: 49916 RVA: 0x0047C60B File Offset: 0x0047A80B
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C2FD RID: 49917 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(Entity player)
		{
			return true;
		}

		// Token: 0x0600C2FE RID: 49918 RVA: 0x0047C613 File Offset: 0x0047A813
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			properties.ParseBool(BaseTwitchRequirement.PropInvert, ref this.Invert);
			properties.ParseBool(BaseTwitchRequirement.PropHidesAction, ref this.HideAction);
		}

		// Token: 0x040092DF RID: 37599
		public BaseTwitchRequirement.OwnerTypes OwnerType;

		// Token: 0x040092E0 RID: 37600
		public TwitchAction OwnerAction;

		// Token: 0x040092E1 RID: 37601
		public TwitchVote OwnerVote;

		// Token: 0x040092E2 RID: 37602
		public BaseTwitchEventEntry OwnerEvent;

		// Token: 0x040092E3 RID: 37603
		public DynamicProperties Properties;

		// Token: 0x040092E4 RID: 37604
		public bool HideAction;

		// Token: 0x040092E5 RID: 37605
		public bool Invert;

		// Token: 0x040092E6 RID: 37606
		public static string PropInvert = "invert";

		// Token: 0x040092E7 RID: 37607
		public static string PropHidesAction = "hide_action";

		// Token: 0x020018B6 RID: 6326
		public enum OwnerTypes
		{
			// Token: 0x040092E9 RID: 37609
			Action,
			// Token: 0x040092EA RID: 37610
			Vote,
			// Token: 0x040092EB RID: 37611
			Event
		}
	}
}
