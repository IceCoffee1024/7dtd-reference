using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001982 RID: 6530
	[Preserve]
	public class RequirementEventActive : BaseRequirement
	{
		// Token: 0x0600C930 RID: 51504 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C931 RID: 51505 RVA: 0x00498910 File Offset: 0x00496B10
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

		// Token: 0x0600C932 RID: 51506 RVA: 0x00498960 File Offset: 0x00496B60
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementEventActive.PropEventName, ref this.EventName);
		}

		// Token: 0x0600C933 RID: 51507 RVA: 0x0049897A File Offset: 0x00496B7A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementEventActive
			{
				EventName = this.EventName,
				Invert = this.Invert
			};
		}

		// Token: 0x04009780 RID: 38784
		[PublicizedFrom(EAccessModifier.Protected)]
		public string EventName = "";

		// Token: 0x04009781 RID: 38785
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEventName = "event_name";
	}
}
