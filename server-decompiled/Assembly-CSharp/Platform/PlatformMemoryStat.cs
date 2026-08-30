using System;
using System.Collections.Generic;
using System.Text;

namespace Platform
{
	// Token: 0x02001B5F RID: 7007
	public sealed class PlatformMemoryStat<T> : IPlatformMemoryStat<T>, IPlatformMemoryStat
	{
		// Token: 0x0600D120 RID: 53536 RVA: 0x004C098F File Offset: 0x004BEB8F
		[PublicizedFrom(EAccessModifier.Private)]
		public PlatformMemoryStat(string name)
		{
			this.m_columnValues = new EnumDictionary<MemoryStatColumn, T>();
			this.m_columnLastValues = new EnumDictionary<MemoryStatColumn, T>();
			this.Name = name;
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x0600D121 RID: 53537 RVA: 0x004C09B4 File Offset: 0x004BEBB4
		public string Name { get; }

		// Token: 0x0600D122 RID: 53538 RVA: 0x004C09BC File Offset: 0x004BEBBC
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

		// Token: 0x0600D123 RID: 53539 RVA: 0x004C0A28 File Offset: 0x004BEC28
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

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x0600D124 RID: 53540 RVA: 0x004C0A7C File Offset: 0x004BEC7C
		// (remove) Token: 0x0600D125 RID: 53541 RVA: 0x004C0AB4 File Offset: 0x004BECB4
		public event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x0600D126 RID: 53542 RVA: 0x004C0AE9 File Offset: 0x004BECE9
		// (set) Token: 0x0600D127 RID: 53543 RVA: 0x004C0AF1 File Offset: 0x004BECF1
		public PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x0600D128 RID: 53544 RVA: 0x004C0AFA File Offset: 0x004BECFA
		// (set) Token: 0x0600D129 RID: 53545 RVA: 0x004C0B02 File Offset: 0x004BED02
		public PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D12A RID: 53546 RVA: 0x004C0B0B File Offset: 0x004BED0B
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

		// Token: 0x0600D12B RID: 53547 RVA: 0x004C0B47 File Offset: 0x004BED47
		public bool TryGet(MemoryStatColumn column, out T value)
		{
			return this.m_columnValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D12C RID: 53548 RVA: 0x004C0B56 File Offset: 0x004BED56
		public bool TryGetLast(MemoryStatColumn column, out T value)
		{
			return this.m_columnLastValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D12D RID: 53549 RVA: 0x004C0B65 File Offset: 0x004BED65
		public static IPlatformMemoryStat<T> Create(string name)
		{
			return new PlatformMemoryStat<T>(name);
		}

		// Token: 0x04009F8E RID: 40846
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnValues;

		// Token: 0x04009F8F RID: 40847
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnLastValues;
	}
}
