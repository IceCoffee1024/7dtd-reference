using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200194F RID: 6479
	[Preserve]
	public class BaseRequirement
	{
		// Token: 0x0600C80D RID: 51213 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C80E RID: 51214 RVA: 0x00494D9E File Offset: 0x00492F9E
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C80F RID: 51215 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600C810 RID: 51216 RVA: 0x00494DA8 File Offset: 0x00492FA8
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			if (properties.Values.ContainsKey(BaseRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x0600C811 RID: 51217 RVA: 0x00494DF8 File Offset: 0x00492FF8
		public virtual BaseRequirement Clone()
		{
			BaseRequirement baseRequirement = this.CloneChildSettings();
			if (this.Properties != null)
			{
				baseRequirement.Properties = new DynamicProperties();
				baseRequirement.Properties.CopyFrom(this.Properties, null);
			}
			return baseRequirement;
		}

		// Token: 0x0600C812 RID: 51218 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseRequirement CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0400968F RID: 38543
		public DynamicProperties Properties;

		// Token: 0x04009690 RID: 38544
		public GameEventActionSequence Owner;

		// Token: 0x04009691 RID: 38545
		public bool Invert;

		// Token: 0x04009692 RID: 38546
		public static string PropInvert = "invert";
	}
}
