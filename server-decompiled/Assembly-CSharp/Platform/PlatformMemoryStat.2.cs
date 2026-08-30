using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Platform
{
	// Token: 0x02001B87 RID: 7047
	public static class PlatformMemoryStat
	{
		// Token: 0x0600D2B9 RID: 53945 RVA: 0x004C6F61 File Offset: 0x004C5161
		public static IPlatformMemoryStat<T> Create<T>(string name)
		{
			IPlatformMemoryStat<T> platformMemoryStat = PlatformMemoryStat<T>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<T>(PlatformMemoryStat.<Create>g__RenderValue|0_0<T>);
			return platformMemoryStat;
		}

		// Token: 0x0600D2BA RID: 53946 RVA: 0x004C6F7B File Offset: 0x004C517B
		public static IPlatformMemoryStat<long> CreateBytes(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat.CreateInt64(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateBytes>g__RenderValue|1_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateBytes>g__RenderDelta|1_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2BB RID: 53947 RVA: 0x004C6FA7 File Offset: 0x004C51A7
		public static IPlatformMemoryStat<int> CreateInt32(string name)
		{
			IPlatformMemoryStat<int> platformMemoryStat = PlatformMemoryStat<int>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<int>(PlatformMemoryStat.<CreateInt32>g__RenderValue|2_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<int>(PlatformMemoryStat.<CreateInt32>g__RenderDelta|2_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2BC RID: 53948 RVA: 0x004C6FD3 File Offset: 0x004C51D3
		public static IPlatformMemoryStat<uint> CreateUInt32(string name)
		{
			IPlatformMemoryStat<uint> platformMemoryStat = PlatformMemoryStat<uint>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderValue|3_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderDelta|3_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2BD RID: 53949 RVA: 0x004C6FFF File Offset: 0x004C51FF
		public static IPlatformMemoryStat<long> CreateInt64(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat<long>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateInt64>g__RenderValue|4_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateInt64>g__RenderDelta|4_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2BE RID: 53950 RVA: 0x004C702B File Offset: 0x004C522B
		public static IPlatformMemoryStat<ulong> CreateUInt64(string name)
		{
			IPlatformMemoryStat<ulong> platformMemoryStat = PlatformMemoryStat<ulong>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderValue|5_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderDelta|5_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2BF RID: 53951 RVA: 0x004C7057 File Offset: 0x004C5257
		public static IPlatformMemoryStat<float> CreateFloat(string name)
		{
			IPlatformMemoryStat<float> platformMemoryStat = PlatformMemoryStat<float>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<float>(PlatformMemoryStat.<CreateFloat>g__RenderValue|6_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<float>(PlatformMemoryStat.<CreateFloat>g__RenderDelta|6_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2C0 RID: 53952 RVA: 0x004C7083 File Offset: 0x004C5283
		public static IPlatformMemoryStat<double> CreateDouble(string name)
		{
			IPlatformMemoryStat<double> platformMemoryStat = PlatformMemoryStat<double>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<double>(PlatformMemoryStat.<CreateDouble>g__RenderValue|7_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<double>(PlatformMemoryStat.<CreateDouble>g__RenderDelta|7_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D2C1 RID: 53953 RVA: 0x004C70AF File Offset: 0x004C52AF
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Create>g__RenderValue|0_0<T>(StringBuilder builder, T value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2C2 RID: 53954 RVA: 0x004C70C3 File Offset: 0x004C52C3
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateBytes>g__RenderValue|1_0(StringBuilder builder, long value)
		{
			PlatformMemoryStat.<CreateBytes>g__RenderSize|1_2(builder, value);
		}

		// Token: 0x0600D2C3 RID: 53955 RVA: 0x004C70CC File Offset: 0x004C52CC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateBytes>g__RenderDelta|1_1(StringBuilder builder, long current, long last)
		{
			if (current == last)
			{
				return;
			}
			PlatformMemoryStat.<CreateBytes>g__RenderSize|1_2(builder, current - last);
		}

		// Token: 0x0600D2C4 RID: 53956 RVA: 0x004C70DC File Offset: 0x004C52DC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateBytes>g__RenderSize|1_2(StringBuilder builder, long sizeBytes)
		{
			if (Math.Abs(sizeBytes) < 1024L)
			{
				builder.Append(sizeBytes).Append("  ").Append('B');
				return;
			}
			double num = (double)sizeBytes / 1024.0;
			foreach (char c in "kMGTPE")
			{
				if (Math.Abs(num) < 1024.0)
				{
					builder.AppendFormat("{0:F3} ", num).Append(c).Append('B');
					return;
				}
				num /= 1024.0;
			}
			throw new InvalidOperationException("Should not be reachable... Are there enough prefixes?");
		}

		// Token: 0x0600D2C5 RID: 53957 RVA: 0x004C7184 File Offset: 0x004C5384
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt32>g__RenderValue|2_0(StringBuilder builder, int value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2C6 RID: 53958 RVA: 0x004C7198 File Offset: 0x004C5398
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt32>g__RenderDelta|2_1(StringBuilder builder, int current, int last)
		{
			if (current == last)
			{
				return;
			}
			if (current >= last)
			{
				builder.AppendFormat("{0}", current - last);
				return;
			}
			builder.AppendFormat("-{0}", last - current);
		}

		// Token: 0x0600D2C7 RID: 53959 RVA: 0x004C71CC File Offset: 0x004C53CC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt32>g__RenderValue|3_0(StringBuilder builder, uint value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2C8 RID: 53960 RVA: 0x004C71E0 File Offset: 0x004C53E0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt32>g__RenderDelta|3_1(StringBuilder builder, uint current, uint last)
		{
			if (current == last)
			{
				return;
			}
			if (current >= last)
			{
				builder.AppendFormat("{0}", current - last);
				return;
			}
			builder.AppendFormat("-{0}", last - current);
		}

		// Token: 0x0600D2C9 RID: 53961 RVA: 0x004C7214 File Offset: 0x004C5414
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt64>g__RenderValue|4_0(StringBuilder builder, long value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2CA RID: 53962 RVA: 0x004C7228 File Offset: 0x004C5428
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt64>g__RenderDelta|4_1(StringBuilder builder, long current, long last)
		{
			if (current == last)
			{
				return;
			}
			if (current >= last)
			{
				builder.AppendFormat("{0}", current - last);
				return;
			}
			builder.AppendFormat("-{0}", last - current);
		}

		// Token: 0x0600D2CB RID: 53963 RVA: 0x004C725C File Offset: 0x004C545C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt64>g__RenderValue|5_0(StringBuilder builder, ulong value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2CC RID: 53964 RVA: 0x004C7270 File Offset: 0x004C5470
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt64>g__RenderDelta|5_1(StringBuilder builder, ulong current, ulong last)
		{
			if (current == last)
			{
				return;
			}
			if (current >= last)
			{
				builder.AppendFormat("{0}", current - last);
				return;
			}
			builder.AppendFormat("-{0}", last - current);
		}

		// Token: 0x0600D2CD RID: 53965 RVA: 0x004C72A4 File Offset: 0x004C54A4
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateFloat>g__RenderValue|6_0(StringBuilder builder, float value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2CE RID: 53966 RVA: 0x004C72B8 File Offset: 0x004C54B8
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateFloat>g__RenderDelta|6_1(StringBuilder builder, float current, float last)
		{
			if (current == last)
			{
				return;
			}
			builder.AppendFormat("{0}", current - last);
		}

		// Token: 0x0600D2CF RID: 53967 RVA: 0x004C72D3 File Offset: 0x004C54D3
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateDouble>g__RenderValue|7_0(StringBuilder builder, double value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D2D0 RID: 53968 RVA: 0x004C72E7 File Offset: 0x004C54E7
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateDouble>g__RenderDelta|7_1(StringBuilder builder, double current, double last)
		{
			if (current == last)
			{
				return;
			}
			builder.AppendFormat("{0}", current - last);
		}
	}
}
