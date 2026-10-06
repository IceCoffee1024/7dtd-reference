using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D6B RID: 7531
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	public class GlitterLight : MonoBehaviour
	{
		// Token: 0x0600DF18 RID: 57112 RVA: 0x004F91F0 File Offset: 0x004F73F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			this.lght = base.GetComponent<Light>();
		}

		// Token: 0x0600DF19 RID: 57113 RVA: 0x004F91F0 File Offset: 0x004F73F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			this.lght = base.GetComponent<Light>();
		}

		// Token: 0x0600DF1A RID: 57114 RVA: 0x004F9200 File Offset: 0x004F7400
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			Shader.SetGlobalVector("_gGlitterLightDir", -base.transform.forward);
			Shader.SetGlobalVector("_gGlitterLightWorldPos", base.transform.position);
			if (this.lght != null)
			{
				Shader.SetGlobalColor("_gGlitterLightColor", this.lght.color);
			}
		}

		// Token: 0x0400A838 RID: 43064
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Light lght;
	}
}
