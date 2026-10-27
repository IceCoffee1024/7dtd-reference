using System;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D9C RID: 7580
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	public class GlitterLight : MonoBehaviour
	{
		// Token: 0x0600E02C RID: 57388 RVA: 0x004FCD90 File Offset: 0x004FAF90
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			this.lght = base.GetComponent<Light>();
		}

		// Token: 0x0600E02D RID: 57389 RVA: 0x004FCD90 File Offset: 0x004FAF90
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			this.lght = base.GetComponent<Light>();
		}

		// Token: 0x0600E02E RID: 57390 RVA: 0x004FCDA0 File Offset: 0x004FAFA0
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

		// Token: 0x0400A921 RID: 43297
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Light lght;
	}
}
