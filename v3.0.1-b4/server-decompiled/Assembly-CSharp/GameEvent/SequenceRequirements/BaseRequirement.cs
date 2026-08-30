using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020018FD RID: 6397
	[Preserve]
	public class BaseRequirement
	{
		// Token: 0x0600C52E RID: 50478 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C52F RID: 50479 RVA: 0x0048A566 File Offset: 0x00488766
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C530 RID: 50480 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600C531 RID: 50481 RVA: 0x0048A570 File Offset: 0x00488770
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			if (properties.Values.ContainsKey(BaseRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x0600C532 RID: 50482 RVA: 0x0048A5C0 File Offset: 0x004887C0
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

		// Token: 0x0600C533 RID: 50483 RVA: 0x0001FF1E File Offset: 0x0001E11E
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseRequirement CloneChildSettings()
		{
			return null;
		}

		// Token: 0x040094A0 RID: 38048
		public DynamicProperties Properties;

		// Token: 0x040094A1 RID: 38049
		public GameEventActionSequence Owner;

		// Token: 0x040094A2 RID: 38050
		public bool Invert;

		// Token: 0x040094A3 RID: 38051
		public static string PropInvert = "invert";
	}
}
