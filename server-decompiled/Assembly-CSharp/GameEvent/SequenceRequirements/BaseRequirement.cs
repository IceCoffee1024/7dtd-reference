using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001980 RID: 6528
	[Preserve]
	public class BaseRequirement
	{
		// Token: 0x0600C921 RID: 51489 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C922 RID: 51490 RVA: 0x00498792 File Offset: 0x00496992
		public void Init()
		{
			this.OnInit();
		}

		// Token: 0x0600C923 RID: 51491 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600C924 RID: 51492 RVA: 0x0049879C File Offset: 0x0049699C
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			if (properties.Values.ContainsKey(BaseRequirement.PropInvert))
			{
				this.Invert = StringParsers.ParseBool(properties.Values[BaseRequirement.PropInvert], 0, -1, true);
			}
		}

		// Token: 0x0600C925 RID: 51493 RVA: 0x004987EC File Offset: 0x004969EC
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

		// Token: 0x0600C926 RID: 51494 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseRequirement CloneChildSettings()
		{
			return null;
		}

		// Token: 0x04009778 RID: 38776
		public DynamicProperties Properties;

		// Token: 0x04009779 RID: 38777
		public GameEventActionSequence Owner;

		// Token: 0x0400977A RID: 38778
		public bool Invert;

		// Token: 0x0400977B RID: 38779
		public static string PropInvert = "invert";
	}
}
