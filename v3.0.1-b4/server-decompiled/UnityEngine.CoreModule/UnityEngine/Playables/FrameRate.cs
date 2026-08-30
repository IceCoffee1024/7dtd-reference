using System;
using System.Globalization;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000493 RID: 1171
	[NativeHeader("Runtime/Director/Core/FrameRate.h")]
	[UsedByNativeCode("FrameRate")]
	internal struct FrameRate : IEquatable<FrameRate>
	{
		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x00045127 File Offset: 0x00043327
		public bool dropFrame
		{
			get
			{
				return this.m_Rate < 0;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x00045132 File Offset: 0x00043332
		public double rate
		{
			get
			{
				return this.dropFrame ? ((double)(-(double)this.m_Rate) * 0.999000999000999) : ((double)this.m_Rate);
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x00045157 File Offset: 0x00043357
		public FrameRate(uint frameRate = 0U, bool drop = false)
		{
			this.m_Rate = (int)((drop ? uint.MaxValue : 1U) * frameRate);
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x0004516C File Offset: 0x0004336C
		public bool IsValid()
		{
			return this.m_Rate != 0;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x00045188 File Offset: 0x00043388
		public bool Equals(FrameRate other)
		{
			return this.m_Rate == other.m_Rate;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000451A8 File Offset: 0x000433A8
		public override bool Equals(object obj)
		{
			return obj is FrameRate && this.Equals((FrameRate)obj);
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000451D1 File Offset: 0x000433D1
		public static bool operator ==(FrameRate a, FrameRate b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000451DB File Offset: 0x000433DB
		public static bool operator !=(FrameRate a, FrameRate b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000451E8 File Offset: 0x000433E8
		public static bool operator <(FrameRate a, FrameRate b)
		{
			return a.rate < b.rate;
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000451FA File Offset: 0x000433FA
		public static bool operator <=(FrameRate a, FrameRate b)
		{
			return a.rate <= b.rate;
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x0004520F File Offset: 0x0004340F
		public static bool operator >(FrameRate a, FrameRate b)
		{
			return a.rate > b.rate;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000451FA File Offset: 0x000433FA
		public static bool operator >=(FrameRate a, FrameRate b)
		{
			return a.rate <= b.rate;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00045224 File Offset: 0x00043424
		public override int GetHashCode()
		{
			return this.m_Rate;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x0004523C File Offset: 0x0004343C
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00045258 File Offset: 0x00043458
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00045274 File Offset: 0x00043474
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = string.IsNullOrEmpty(format);
			if (flag)
			{
				format = (this.dropFrame ? "F2" : "F0");
			}
			bool flag2 = formatProvider == null;
			if (flag2)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("{0} Fps", new object[] { this.rate.ToString(format, formatProvider) });
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000452E0 File Offset: 0x000434E0
		internal static int FrameRateToInt(FrameRate framerate)
		{
			return framerate.m_Rate;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000452F8 File Offset: 0x000434F8
		internal static FrameRate DoubleToFrameRate(double framerate)
		{
			uint num = (uint)Math.Ceiling(framerate);
			bool flag = num <= 0U;
			FrameRate frameRate;
			if (flag)
			{
				frameRate = new FrameRate(1U, false);
			}
			else
			{
				FrameRate frameRate2 = new FrameRate(num, true);
				bool flag2 = Math.Abs(framerate - frameRate2.rate) < Math.Abs(framerate - num);
				if (flag2)
				{
					frameRate = frameRate2;
				}
				else
				{
					frameRate = new FrameRate(num, false);
				}
			}
			return frameRate;
		}

		// Token: 0x04000F5B RID: 3931
		[Ignore]
		public static readonly FrameRate k_24Fps = new FrameRate(24U, false);

		// Token: 0x04000F5C RID: 3932
		[Ignore]
		public static readonly FrameRate k_23_976Fps = new FrameRate(24U, true);

		// Token: 0x04000F5D RID: 3933
		[Ignore]
		public static readonly FrameRate k_25Fps = new FrameRate(25U, false);

		// Token: 0x04000F5E RID: 3934
		[Ignore]
		public static readonly FrameRate k_30Fps = new FrameRate(30U, false);

		// Token: 0x04000F5F RID: 3935
		[Ignore]
		public static readonly FrameRate k_29_97Fps = new FrameRate(30U, true);

		// Token: 0x04000F60 RID: 3936
		[Ignore]
		public static readonly FrameRate k_50Fps = new FrameRate(50U, false);

		// Token: 0x04000F61 RID: 3937
		[Ignore]
		public static readonly FrameRate k_60Fps = new FrameRate(60U, false);

		// Token: 0x04000F62 RID: 3938
		[Ignore]
		public static readonly FrameRate k_59_94Fps = new FrameRate(60U, true);

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private int m_Rate;
	}
}
