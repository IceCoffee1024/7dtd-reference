using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D39 RID: 7481
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	public class GlitterLight : MonoBehaviour
	{
		// Token: 0x0600DD9D RID: 56733 RVA: 0x004F45A8 File Offset: 0x004F27A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			this.lght = base.GetComponent<Light>();
		}

		// Token: 0x0600DD9E RID: 56734 RVA: 0x004F45A8 File Offset: 0x004F27A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			this.lght = base.GetComponent<Light>();
		}

		// Token: 0x0600DD9F RID: 56735 RVA: 0x004F45B8 File Offset: 0x004F27B8
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

		// Token: 0x0400A761 RID: 42849
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Light lght;
	}
}
