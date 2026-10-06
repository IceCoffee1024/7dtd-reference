using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001951 RID: 6481
	[Preserve]
	public class RequirementEventActive : BaseRequirement
	{
		// Token: 0x0600C81C RID: 51228 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C81D RID: 51229 RVA: 0x00494F1C File Offset: 0x0049311C
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

		// Token: 0x0600C81E RID: 51230 RVA: 0x00494F6C File Offset: 0x0049316C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementEventActive.PropEventName, ref this.EventName);
		}

		// Token: 0x0600C81F RID: 51231 RVA: 0x00494F86 File Offset: 0x00493186
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementEventActive
			{
				EventName = this.EventName,
				Invert = this.Invert
			};
		}

		// Token: 0x04009697 RID: 38551
		[PublicizedFrom(EAccessModifier.Protected)]
		public string EventName = "";

		// Token: 0x04009698 RID: 38552
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEventName = "event_name";
	}
}
