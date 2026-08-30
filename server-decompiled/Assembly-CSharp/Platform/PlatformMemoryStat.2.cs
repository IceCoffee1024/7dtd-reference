using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Platform
{
	// Token: 0x02001B60 RID: 7008
	public static class PlatformMemoryStat
	{
		// Token: 0x0600D12E RID: 53550 RVA: 0x004C0B6D File Offset: 0x004BED6D
		public static IPlatformMemoryStat<T> Create<T>(string name)
		{
			IPlatformMemoryStat<T> platformMemoryStat = PlatformMemoryStat<T>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<T>(PlatformMemoryStat.<Create>g__RenderValue|0_0<T>);
			return platformMemoryStat;
		}

		// Token: 0x0600D12F RID: 53551 RVA: 0x004C0B87 File Offset: 0x004BED87
		public static IPlatformMemoryStat<long> CreateBytes(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat.CreateInt64(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateBytes>g__RenderValue|1_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateBytes>g__RenderDelta|1_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D130 RID: 53552 RVA: 0x004C0BB3 File Offset: 0x004BEDB3
		public static IPlatformMemoryStat<int> CreateInt32(string name)
		{
			IPlatformMemoryStat<int> platformMemoryStat = PlatformMemoryStat<int>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<int>(PlatformMemoryStat.<CreateInt32>g__RenderValue|2_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<int>(PlatformMemoryStat.<CreateInt32>g__RenderDelta|2_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D131 RID: 53553 RVA: 0x004C0BDF File Offset: 0x004BEDDF
		public static IPlatformMemoryStat<uint> CreateUInt32(string name)
		{
			IPlatformMemoryStat<uint> platformMemoryStat = PlatformMemoryStat<uint>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderValue|3_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderDelta|3_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D132 RID: 53554 RVA: 0x004C0C0B File Offset: 0x004BEE0B
		public static IPlatformMemoryStat<long> CreateInt64(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat<long>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateInt64>g__RenderValue|4_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateInt64>g__RenderDelta|4_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D133 RID: 53555 RVA: 0x004C0C37 File Offset: 0x004BEE37
		public static IPlatformMemoryStat<ulong> CreateUInt64(string name)
		{
			IPlatformMemoryStat<ulong> platformMemoryStat = PlatformMemoryStat<ulong>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderValue|5_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderDelta|5_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D134 RID: 53556 RVA: 0x004C0C63 File Offset: 0x004BEE63
		public static IPlatformMemoryStat<float> CreateFloat(string name)
		{
			IPlatformMemoryStat<float> platformMemoryStat = PlatformMemoryStat<float>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<float>(PlatformMemoryStat.<CreateFloat>g__RenderValue|6_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<float>(PlatformMemoryStat.<CreateFloat>g__RenderDelta|6_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D135 RID: 53557 RVA: 0x004C0C8F File Offset: 0x004BEE8F
		public static IPlatformMemoryStat<double> CreateDouble(string name)
		{
			IPlatformMemoryStat<double> platformMemoryStat = PlatformMemoryStat<double>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<double>(PlatformMemoryStat.<CreateDouble>g__RenderValue|7_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<double>(PlatformMemoryStat.<CreateDouble>g__RenderDelta|7_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D136 RID: 53558 RVA: 0x004C0CBB File Offset: 0x004BEEBB
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Create>g__RenderValue|0_0<T>(StringBuilder builder, T value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D137 RID: 53559 RVA: 0x004C0CCF File Offset: 0x004BEECF
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateBytes>g__RenderValue|1_0(StringBuilder builder, long value)
		{
			PlatformMemoryStat.<CreateBytes>g__RenderSize|1_2(builder, value);
		}

		// Token: 0x0600D138 RID: 53560 RVA: 0x004C0CD8 File Offset: 0x004BEED8
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

		// Token: 0x0600D139 RID: 53561 RVA: 0x004C0CE8 File Offset: 0x004BEEE8
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

		// Token: 0x0600D13A RID: 53562 RVA: 0x004C0D90 File Offset: 0x004BEF90
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt32>g__RenderValue|2_0(StringBuilder builder, int value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D13B RID: 53563 RVA: 0x004C0DA4 File Offset: 0x004BEFA4
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

		// Token: 0x0600D13C RID: 53564 RVA: 0x004C0DD8 File Offset: 0x004BEFD8
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt32>g__RenderValue|3_0(StringBuilder builder, uint value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D13D RID: 53565 RVA: 0x004C0DEC File Offset: 0x004BEFEC
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

		// Token: 0x0600D13E RID: 53566 RVA: 0x004C0E20 File Offset: 0x004BF020
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt64>g__RenderValue|4_0(StringBuilder builder, long value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D13F RID: 53567 RVA: 0x004C0E34 File Offset: 0x004BF034
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

		// Token: 0x0600D140 RID: 53568 RVA: 0x004C0E68 File Offset: 0x004BF068
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt64>g__RenderValue|5_0(StringBuilder builder, ulong value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D141 RID: 53569 RVA: 0x004C0E7C File Offset: 0x004BF07C
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

		// Token: 0x0600D142 RID: 53570 RVA: 0x004C0EB0 File Offset: 0x004BF0B0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateFloat>g__RenderValue|6_0(StringBuilder builder, float value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D143 RID: 53571 RVA: 0x004C0EC4 File Offset: 0x004BF0C4
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

		// Token: 0x0600D144 RID: 53572 RVA: 0x004C0EDF File Offset: 0x004BF0DF
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateDouble>g__RenderValue|7_0(StringBuilder builder, double value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D145 RID: 53573 RVA: 0x004C0EF3 File Offset: 0x004BF0F3
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
