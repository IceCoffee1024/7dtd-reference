using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004BE RID: 1214
	public struct LinearColor
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002AB9 RID: 10937 RVA: 0x00047A54 File Offset: 0x00045C54
		// (set) Token: 0x06002ABA RID: 10938 RVA: 0x00047A6C File Offset: 0x00045C6C
		public float red
		{
			get
			{
				return this.m_red;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Red color (" + value.ToString() + ") must be in range [0;1].");
				}
				this.m_red = value;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x00047AB4 File Offset: 0x00045CB4
		// (set) Token: 0x06002ABC RID: 10940 RVA: 0x00047ACC File Offset: 0x00045CCC
		public float green
		{
			get
			{
				return this.m_green;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Green color (" + value.ToString() + ") must be in range [0;1].");
				}
				this.m_green = value;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x00047B14 File Offset: 0x00045D14
		// (set) Token: 0x06002ABE RID: 10942 RVA: 0x00047B2C File Offset: 0x00045D2C
		public float blue
		{
			get
			{
				return this.m_blue;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Blue color (" + value.ToString() + ") must be in range [0;1].");
				}
				this.m_blue = value;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x00047B74 File Offset: 0x00045D74
		// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x00047B8C File Offset: 0x00045D8C
		public float intensity
		{
			get
			{
				return this.m_intensity;
			}
			set
			{
				bool flag = value < 0f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Intensity (" + value.ToString() + ") must be positive.");
				}
				this.m_intensity = value;
			}
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x00047BCC File Offset: 0x00045DCC
		public static LinearColor Convert(Color color, float intensity)
		{
			Color color2 = (GraphicsSettings.lightsUseLinearIntensity ? color.linear.RGBMultiplied(intensity) : color.RGBMultiplied(intensity).linear);
			float maxColorComponent = color2.maxColorComponent;
			bool flag = color2.r < 0f || color2.g < 0f || color2.b < 0f;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Concat(new string[]
				{
					"The input color to be converted must not contain negative values (red: ",
					color2.r.ToString(),
					", green: ",
					color2.g.ToString(),
					", blue: ",
					color2.b.ToString(),
					")."
				}));
			}
			bool flag2 = maxColorComponent <= 1E-20f;
			LinearColor linearColor;
			if (flag2)
			{
				linearColor = LinearColor.Black();
			}
			else
			{
				float num = 1f / color2.maxColorComponent;
				LinearColor linearColor2;
				linearColor2.m_red = color2.r * num;
				linearColor2.m_green = color2.g * num;
				linearColor2.m_blue = color2.b * num;
				linearColor2.m_intensity = maxColorComponent;
				linearColor = linearColor2;
			}
			return linearColor;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x00047D00 File Offset: 0x00045F00
		public static LinearColor Black()
		{
			LinearColor linearColor;
			linearColor.m_red = (linearColor.m_green = (linearColor.m_blue = (linearColor.m_intensity = 0f)));
			return linearColor;
		}

		// Token: 0x04000FC4 RID: 4036
		private float m_red;

		// Token: 0x04000FC5 RID: 4037
		private float m_green;

		// Token: 0x04000FC6 RID: 4038
		private float m_blue;

		// Token: 0x04000FC7 RID: 4039
		private float m_intensity;
	}
}
