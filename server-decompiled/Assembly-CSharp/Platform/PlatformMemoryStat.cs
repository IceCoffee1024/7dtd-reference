using System;
using System.Collections.Generic;
using System.Text;

namespace Platform
{
	// Token: 0x02001BE9 RID: 7145
	public sealed class PlatformMemoryStat<T> : IPlatformMemoryStat<T>, IPlatformMemoryStat
	{
		// Token: 0x0600D537 RID: 54583 RVA: 0x004CF393 File Offset: 0x004CD593
		[PublicizedFrom(EAccessModifier.Private)]
		public PlatformMemoryStat(string name)
		{
			this.m_columnValues = new EnumDictionary<MemoryStatColumn, T>();
			this.m_columnLastValues = new EnumDictionary<MemoryStatColumn, T>();
			this.Name = name;
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x0600D538 RID: 54584 RVA: 0x004CF3B8 File Offset: 0x004CD5B8
		public string Name { get; }

		// Token: 0x0600D539 RID: 54585 RVA: 0x004CF3C0 File Offset: 0x004CD5C0
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

		// Token: 0x0600D53A RID: 54586 RVA: 0x004CF42C File Offset: 0x004CD62C
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
		// (add) Token: 0x0600D53B RID: 54587 RVA: 0x004CF480 File Offset: 0x004CD680
		// (remove) Token: 0x0600D53C RID: 54588 RVA: 0x004CF4B8 File Offset: 0x004CD6B8
		public event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x0600D53D RID: 54589 RVA: 0x004CF4ED File Offset: 0x004CD6ED
		// (set) Token: 0x0600D53E RID: 54590 RVA: 0x004CF4F5 File Offset: 0x004CD6F5
		public PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x0600D53F RID: 54591 RVA: 0x004CF4FE File Offset: 0x004CD6FE
		// (set) Token: 0x0600D540 RID: 54592 RVA: 0x004CF506 File Offset: 0x004CD706
		public PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D541 RID: 54593 RVA: 0x004CF50F File Offset: 0x004CD70F
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

		// Token: 0x0600D542 RID: 54594 RVA: 0x004CF54B File Offset: 0x004CD74B
		public bool TryGet(MemoryStatColumn column, out T value)
		{
			return this.m_columnValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D543 RID: 54595 RVA: 0x004CF55A File Offset: 0x004CD75A
		public bool TryGetLast(MemoryStatColumn column, out T value)
		{
			return this.m_columnLastValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D544 RID: 54596 RVA: 0x004CF569 File Offset: 0x004CD769
		public static IPlatformMemoryStat<T> Create(string name)
		{
			return new PlatformMemoryStat<T>(name);
		}

		// Token: 0x0400A27D RID: 41597
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnValues;

		// Token: 0x0400A27E RID: 41598
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnLastValues;
	}
}
