using System;
using System.Collections.Generic;
using System.Text;

namespace Platform
{
	// Token: 0x02001B86 RID: 7046
	public sealed class PlatformMemoryStat<T> : IPlatformMemoryStat<T>, IPlatformMemoryStat
	{
		// Token: 0x0600D2AB RID: 53931 RVA: 0x004C6D83 File Offset: 0x004C4F83
		[PublicizedFrom(EAccessModifier.Private)]
		public PlatformMemoryStat(string name)
		{
			this.m_columnValues = new EnumDictionary<MemoryStatColumn, T>();
			this.m_columnLastValues = new EnumDictionary<MemoryStatColumn, T>();
			this.Name = name;
		}

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x0600D2AC RID: 53932 RVA: 0x004C6DA8 File Offset: 0x004C4FA8
		public string Name { get; }

		// Token: 0x0600D2AD RID: 53933 RVA: 0x004C6DB0 File Offset: 0x004C4FB0
		public void UpdateLast()
		{
			foreach (KeyValuePair<MemoryStatColumn, T> keyValuePair in this.m_columnValues)
			{
				MemoryStatColumn memoryStatColumn;
				T t;
				keyValuePair.Deconstruct(out memoryStatColumn, out t);
				MemoryStatColumn memoryStatColumn2 = memoryStatColumn;
				T t2 = t;
				this.m_columnLastValues[memoryStatColumn2] = t2;
			}
		}

		// Token: 0x0600D2AE RID: 53934 RVA: 0x004C6E1C File Offset: 0x004C501C
		public void RenderColumn(StringBuilder builder, MemoryStatColumn column, bool delta)
		{
			T t;
			if (!this.m_columnValues.TryGetValue(column, out t))
			{
				return;
			}
			if (!delta)
			{
				PlatformMemoryRenderValue<T> renderValue = this.RenderValue;
				if (renderValue == null)
				{
					return;
				}
				renderValue(builder, t);
				return;
			}
			else
			{
				T t2 = this.m_columnLastValues[column];
				PlatformMemoryRenderDelta<T> renderDelta = this.RenderDelta;
				if (renderDelta == null)
				{
					return;
				}
				renderDelta(builder, t, t2);
				return;
			}
		}

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x0600D2AF RID: 53935 RVA: 0x004C6E70 File Offset: 0x004C5070
		// (remove) Token: 0x0600D2B0 RID: 53936 RVA: 0x004C6EA8 File Offset: 0x004C50A8
		public event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x0600D2B1 RID: 53937 RVA: 0x004C6EDD File Offset: 0x004C50DD
		// (set) Token: 0x0600D2B2 RID: 53938 RVA: 0x004C6EE5 File Offset: 0x004C50E5
		public PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x0600D2B3 RID: 53939 RVA: 0x004C6EEE File Offset: 0x004C50EE
		// (set) Token: 0x0600D2B4 RID: 53940 RVA: 0x004C6EF6 File Offset: 0x004C50F6
		public PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D2B5 RID: 53941 RVA: 0x004C6EFF File Offset: 0x004C50FF
		public void Set(MemoryStatColumn column, T value)
		{
			this.m_columnValues[column] = value;
			if (!this.m_columnLastValues.ContainsKey(column))
			{
				this.m_columnLastValues[column] = value;
			}
			PlatformMemoryColumnChangedHandler<T> columnSetAfter = this.ColumnSetAfter;
			if (columnSetAfter == null)
			{
				return;
			}
			columnSetAfter(column, value);
		}

		// Token: 0x0600D2B6 RID: 53942 RVA: 0x004C6F3B File Offset: 0x004C513B
		public bool TryGet(MemoryStatColumn column, out T value)
		{
			return this.m_columnValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D2B7 RID: 53943 RVA: 0x004C6F4A File Offset: 0x004C514A
		public bool TryGetLast(MemoryStatColumn column, out T value)
		{
			return this.m_columnLastValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D2B8 RID: 53944 RVA: 0x004C6F59 File Offset: 0x004C5159
		public static IPlatformMemoryStat<T> Create(string name)
		{
			return new PlatformMemoryStat<T>(name);
		}

		// Token: 0x0400A0BF RID: 41151
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnValues;

		// Token: 0x0400A0C0 RID: 41152
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnLastValues;
	}
}
