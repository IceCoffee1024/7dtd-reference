using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020018FF RID: 6399
	[Preserve]
	public class RequirementEventActive : BaseRequirement
	{
		// Token: 0x0600C53D RID: 50493 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C53E RID: 50494 RVA: 0x0048A6E4 File Offset: 0x004888E4
		public override bool CanPerform(Entity target)
		{
			if (!EventsFromXml.Events.ContainsKey(this.EventName))
			{
				return this.Invert;
			}
			if (EventsFromXml.Events[this.EventName].Active)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C53F RID: 50495 RVA: 0x0048A734 File Offset: 0x00488934
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementEventActive.PropEventName, ref this.EventName);
		}

		// Token: 0x0600C540 RID: 50496 RVA: 0x0048A74E File Offset: 0x0048894E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementEventActive
			{
				EventName = this.EventName,
				Invert = this.Invert
			};
		}

		// Token: 0x040094A8 RID: 38056
		[PublicizedFrom(EAccessModifier.Protected)]
		public string EventName = "";

		// Token: 0x040094A9 RID: 38057
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEventName = "event_name";
	}
}
