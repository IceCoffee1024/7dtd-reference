using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Platform
{
	// Token: 0x02001BEA RID: 7146
	public static class PlatformMemoryStat
	{
		// Token: 0x0600D545 RID: 54597 RVA: 0x004CF571 File Offset: 0x004CD771
		public static IPlatformMemoryStat<T> Create<T>(string name)
		{
			IPlatformMemoryStat<T> platformMemoryStat = PlatformMemoryStat<T>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<T>(PlatformMemoryStat.<Create>g__RenderValue|0_0<T>);
			return platformMemoryStat;
		}

		// Token: 0x0600D546 RID: 54598 RVA: 0x004CF58B File Offset: 0x004CD78B
		public static IPlatformMemoryStat<long> CreateBytes(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat.CreateInt64(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateBytes>g__RenderValue|1_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateBytes>g__RenderDelta|1_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D547 RID: 54599 RVA: 0x004CF5B7 File Offset: 0x004CD7B7
		public static IPlatformMemoryStat<int> CreateInt32(string name)
		{
			IPlatformMemoryStat<int> platformMemoryStat = PlatformMemoryStat<int>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<int>(PlatformMemoryStat.<CreateInt32>g__RenderValue|2_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<int>(PlatformMemoryStat.<CreateInt32>g__RenderDelta|2_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D548 RID: 54600 RVA: 0x004CF5E3 File Offset: 0x004CD7E3
		public static IPlatformMemoryStat<uint> CreateUInt32(string name)
		{
			IPlatformMemoryStat<uint> platformMemoryStat = PlatformMemoryStat<uint>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderValue|3_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderDelta|3_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D549 RID: 54601 RVA: 0x004CF60F File Offset: 0x004CD80F
		public static IPlatformMemoryStat<long> CreateInt64(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat<long>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateInt64>g__RenderValue|4_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateInt64>g__RenderDelta|4_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D54A RID: 54602 RVA: 0x004CF63B File Offset: 0x004CD83B
		public static IPlatformMemoryStat<ulong> CreateUInt64(string name)
		{
			IPlatformMemoryStat<ulong> platformMemoryStat = PlatformMemoryStat<ulong>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderValue|5_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderDelta|5_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D54B RID: 54603 RVA: 0x004CF667 File Offset: 0x004CD867
		public static IPlatformMemoryStat<float> CreateFloat(string name)
		{
			IPlatformMemoryStat<float> platformMemoryStat = PlatformMemoryStat<float>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<float>(PlatformMemoryStat.<CreateFloat>g__RenderValue|6_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<float>(PlatformMemoryStat.<CreateFloat>g__RenderDelta|6_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D54C RID: 54604 RVA: 0x004CF693 File Offset: 0x004CD893
		public static IPlatformMemoryStat<double> CreateDouble(string name)
		{
			IPlatformMemoryStat<double> platformMemoryStat = PlatformMemoryStat<double>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<double>(PlatformMemoryStat.<CreateDouble>g__RenderValue|7_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<double>(PlatformMemoryStat.<CreateDouble>g__RenderDelta|7_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D54D RID: 54605 RVA: 0x004CF6BF File Offset: 0x004CD8BF
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Create>g__RenderValue|0_0<T>(StringBuilder builder, T value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D54E RID: 54606 RVA: 0x004CF6D3 File Offset: 0x004CD8D3
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateBytes>g__RenderValue|1_0(StringBuilder builder, long value)
		{
			PlatformMemoryStat.<CreateBytes>g__RenderSize|1_2(builder, value);
		}

		// Token: 0x0600D54F RID: 54607 RVA: 0x004CF6DC File Offset: 0x004CD8DC
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

		// Token: 0x0600D550 RID: 54608 RVA: 0x004CF6EC File Offset: 0x004CD8EC
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

		// Token: 0x0600D551 RID: 54609 RVA: 0x004CF794 File Offset: 0x004CD994
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt32>g__RenderValue|2_0(StringBuilder builder, int value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D552 RID: 54610 RVA: 0x004CF7A8 File Offset: 0x004CD9A8
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

		// Token: 0x0600D553 RID: 54611 RVA: 0x004CF7DC File Offset: 0x004CD9DC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt32>g__RenderValue|3_0(StringBuilder builder, uint value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D554 RID: 54612 RVA: 0x004CF7F0 File Offset: 0x004CD9F0
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

		// Token: 0x0600D555 RID: 54613 RVA: 0x004CF824 File Offset: 0x004CDA24
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt64>g__RenderValue|4_0(StringBuilder builder, long value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D556 RID: 54614 RVA: 0x004CF838 File Offset: 0x004CDA38
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

		// Token: 0x0600D557 RID: 54615 RVA: 0x004CF86C File Offset: 0x004CDA6C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt64>g__RenderValue|5_0(StringBuilder builder, ulong value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D558 RID: 54616 RVA: 0x004CF880 File Offset: 0x004CDA80
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

		// Token: 0x0600D559 RID: 54617 RVA: 0x004CF8B4 File Offset: 0x004CDAB4
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateFloat>g__RenderValue|6_0(StringBuilder builder, float value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D55A RID: 54618 RVA: 0x004CF8C8 File Offset: 0x004CDAC8
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

		// Token: 0x0600D55B RID: 54619 RVA: 0x004CF8E3 File Offset: 0x004CDAE3
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateDouble>g__RenderValue|7_0(StringBuilder builder, double value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D55C RID: 54620 RVA: 0x004CF8F7 File Offset: 0x004CDAF7
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
