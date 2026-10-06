using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Platform
{
	// Token: 0x02001BB9 RID: 7097
	public static class PlatformMemoryStat
	{
		// Token: 0x0600D431 RID: 54321 RVA: 0x004CBA49 File Offset: 0x004C9C49
		public static IPlatformMemoryStat<T> Create<T>(string name)
		{
			IPlatformMemoryStat<T> platformMemoryStat = PlatformMemoryStat<T>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<T>(PlatformMemoryStat.<Create>g__RenderValue|0_0<T>);
			return platformMemoryStat;
		}

		// Token: 0x0600D432 RID: 54322 RVA: 0x004CBA63 File Offset: 0x004C9C63
		public static IPlatformMemoryStat<long> CreateBytes(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat.CreateInt64(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateBytes>g__RenderValue|1_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateBytes>g__RenderDelta|1_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D433 RID: 54323 RVA: 0x004CBA8F File Offset: 0x004C9C8F
		public static IPlatformMemoryStat<int> CreateInt32(string name)
		{
			IPlatformMemoryStat<int> platformMemoryStat = PlatformMemoryStat<int>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<int>(PlatformMemoryStat.<CreateInt32>g__RenderValue|2_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<int>(PlatformMemoryStat.<CreateInt32>g__RenderDelta|2_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D434 RID: 54324 RVA: 0x004CBABB File Offset: 0x004C9CBB
		public static IPlatformMemoryStat<uint> CreateUInt32(string name)
		{
			IPlatformMemoryStat<uint> platformMemoryStat = PlatformMemoryStat<uint>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderValue|3_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<uint>(PlatformMemoryStat.<CreateUInt32>g__RenderDelta|3_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D435 RID: 54325 RVA: 0x004CBAE7 File Offset: 0x004C9CE7
		public static IPlatformMemoryStat<long> CreateInt64(string name)
		{
			IPlatformMemoryStat<long> platformMemoryStat = PlatformMemoryStat<long>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<long>(PlatformMemoryStat.<CreateInt64>g__RenderValue|4_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<long>(PlatformMemoryStat.<CreateInt64>g__RenderDelta|4_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D436 RID: 54326 RVA: 0x004CBB13 File Offset: 0x004C9D13
		public static IPlatformMemoryStat<ulong> CreateUInt64(string name)
		{
			IPlatformMemoryStat<ulong> platformMemoryStat = PlatformMemoryStat<ulong>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderValue|5_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<ulong>(PlatformMemoryStat.<CreateUInt64>g__RenderDelta|5_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D437 RID: 54327 RVA: 0x004CBB3F File Offset: 0x004C9D3F
		public static IPlatformMemoryStat<float> CreateFloat(string name)
		{
			IPlatformMemoryStat<float> platformMemoryStat = PlatformMemoryStat<float>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<float>(PlatformMemoryStat.<CreateFloat>g__RenderValue|6_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<float>(PlatformMemoryStat.<CreateFloat>g__RenderDelta|6_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D438 RID: 54328 RVA: 0x004CBB6B File Offset: 0x004C9D6B
		public static IPlatformMemoryStat<double> CreateDouble(string name)
		{
			IPlatformMemoryStat<double> platformMemoryStat = PlatformMemoryStat<double>.Create(name);
			platformMemoryStat.RenderValue = new PlatformMemoryRenderValue<double>(PlatformMemoryStat.<CreateDouble>g__RenderValue|7_0);
			platformMemoryStat.RenderDelta = new PlatformMemoryRenderDelta<double>(PlatformMemoryStat.<CreateDouble>g__RenderDelta|7_1);
			return platformMemoryStat;
		}

		// Token: 0x0600D439 RID: 54329 RVA: 0x004CBB97 File Offset: 0x004C9D97
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Create>g__RenderValue|0_0<T>(StringBuilder builder, T value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D43A RID: 54330 RVA: 0x004CBBAB File Offset: 0x004C9DAB
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateBytes>g__RenderValue|1_0(StringBuilder builder, long value)
		{
			PlatformMemoryStat.<CreateBytes>g__RenderSize|1_2(builder, value);
		}

		// Token: 0x0600D43B RID: 54331 RVA: 0x004CBBB4 File Offset: 0x004C9DB4
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

		// Token: 0x0600D43C RID: 54332 RVA: 0x004CBBC4 File Offset: 0x004C9DC4
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

		// Token: 0x0600D43D RID: 54333 RVA: 0x004CBC6C File Offset: 0x004C9E6C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt32>g__RenderValue|2_0(StringBuilder builder, int value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D43E RID: 54334 RVA: 0x004CBC80 File Offset: 0x004C9E80
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

		// Token: 0x0600D43F RID: 54335 RVA: 0x004CBCB4 File Offset: 0x004C9EB4
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt32>g__RenderValue|3_0(StringBuilder builder, uint value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D440 RID: 54336 RVA: 0x004CBCC8 File Offset: 0x004C9EC8
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

		// Token: 0x0600D441 RID: 54337 RVA: 0x004CBCFC File Offset: 0x004C9EFC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateInt64>g__RenderValue|4_0(StringBuilder builder, long value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D442 RID: 54338 RVA: 0x004CBD10 File Offset: 0x004C9F10
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

		// Token: 0x0600D443 RID: 54339 RVA: 0x004CBD44 File Offset: 0x004C9F44
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateUInt64>g__RenderValue|5_0(StringBuilder builder, ulong value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D444 RID: 54340 RVA: 0x004CBD58 File Offset: 0x004C9F58
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

		// Token: 0x0600D445 RID: 54341 RVA: 0x004CBD8C File Offset: 0x004C9F8C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateFloat>g__RenderValue|6_0(StringBuilder builder, float value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D446 RID: 54342 RVA: 0x004CBDA0 File Offset: 0x004C9FA0
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

		// Token: 0x0600D447 RID: 54343 RVA: 0x004CBDBB File Offset: 0x004C9FBB
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <CreateDouble>g__RenderValue|7_0(StringBuilder builder, double value)
		{
			builder.AppendFormat("{0}", value);
		}

		// Token: 0x0600D448 RID: 54344 RVA: 0x004CBDCF File Offset: 0x004C9FCF
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
