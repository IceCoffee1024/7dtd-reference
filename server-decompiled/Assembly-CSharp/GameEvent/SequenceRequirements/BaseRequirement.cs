using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001923 RID: 6435
	[Preserve]
	public class BaseRequirement
	{
		// Token: 0x0600C6B5 RID: 50869 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C6B6 RID: 50870 RVA: 0x004908B2 File Offset: 0x0048EAB2
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C6B7 RID: 50871 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600C6B8 RID: 50872 RVA: 0x004908BC File Offset: 0x0048EABC
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			if (properties.Values.ContainsKey(BaseRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x0600C6B9 RID: 50873 RVA: 0x0049090C File Offset: 0x0048EB0C
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

		// Token: 0x0600C6BA RID: 50874 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseRequirement CloneChildSettings()
		{
			return null;
		}

		// Token: 0x040095C9 RID: 38345
		public DynamicProperties Properties;

		// Token: 0x040095CA RID: 38346
		public GameEventActionSequence Owner;

		// Token: 0x040095CB RID: 38347
		public bool Invert;

		// Token: 0x040095CC RID: 38348
		public static string PropInvert = "invert";
	}
}
