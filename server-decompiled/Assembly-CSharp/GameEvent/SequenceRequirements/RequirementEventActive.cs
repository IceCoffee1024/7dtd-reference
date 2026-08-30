using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001925 RID: 6437
	[Preserve]
	public class RequirementEventActive : BaseRequirement
	{
		// Token: 0x0600C6C4 RID: 50884 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6C5 RID: 50885 RVA: 0x00490A30 File Offset: 0x0048EC30
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

		// Token: 0x0600C6C6 RID: 50886 RVA: 0x00490A80 File Offset: 0x0048EC80
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementEventActive.PropEventName, ref this.EventName);
		}

		// Token: 0x0600C6C7 RID: 50887 RVA: 0x00490A9A File Offset: 0x0048EC9A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementEventActive
			{
				EventName = this.EventName,
				Invert = this.Invert
			};
		}

		// Token: 0x040095D1 RID: 38353
		[PublicizedFrom(EAccessModifier.Protected)]
		public string EventName = "";

		// Token: 0x040095D2 RID: 38354
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEventName = "event_name";
	}
}
