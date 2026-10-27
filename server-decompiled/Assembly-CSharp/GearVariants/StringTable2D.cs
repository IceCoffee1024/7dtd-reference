using System;
using System.Collections.Generic;

namespace GearVariants
{
	// Token: 0x0200174B RID: 5963
	[Serializable]
	public class StringTable2D
	{
		// Token: 0x0600B9AB RID: 47531 RVA: 0x004471C8 File Offset: 0x004453C8
		public void EnsureShapeSymmetric(IReadOnlyList<string> keys, IReadOnlyList<string> guids)
		{
			IReadOnlyList<string> readOnlyList = keys ?? Array.Empty<string>();
			IReadOnlyList<string> readOnlyList2 = guids ?? Array.Empty<string>();
			Dictionary<string, StringTable2D.Row> dictionary = new Dictionary<string, StringTable2D.Row>(StringComparer.Ordinal);
			Dictionary<string, StringTable2D.Row> dictionary2 = new Dictionary<string, StringTable2D.Row>(StringComparer.Ordinal);
			foreach (StringTable2D.Row row in this.rows)
			{
				if (!string.IsNullOrEmpty(row.rowGuid) && !dictionary.ContainsKey(row.rowGuid))
				{
					dictionary.Add(row.rowGuid, row);
				}
				if (!string.IsNullOrEmpty(row.rowKey) && !dictionary2.ContainsKey(row.rowKey))
				{
					dictionary2.Add(row.rowKey, row);
				}
			}
			this.rowKeys.Clear();
			this.columnKeys.Clear();
			this.rowKeys.AddRange(readOnlyList);
			this.columnKeys.AddRange(readOnlyList);
			int count = this.columnKeys.Count;
			List<StringTable2D.Row> list = new List<StringTable2D.Row>(readOnlyList.Count);
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				string text = readOnlyList[i];
				string text2 = ((i < readOnlyList2.Count) ? readOnlyList2[i] : string.Empty);
				StringTable2D.Row row2 = null;
				StringTable2D.Row row3;
				StringTable2D.Row row4;
				if (!string.IsNullOrEmpty(text2) && dictionary.TryGetValue(text2, out row3))
				{
					row2 = row3;
				}
				else if (!string.IsNullOrEmpty(text) && dictionary2.TryGetValue(text, out row4))
				{
					row2 = row4;
				}
				if (row2 == null)
				{
					row2 = new StringTable2D.Row
					{
						rowKey = text,
						rowGuid = text2,
						cellValues = new List<string>(count)
					};
				}
				else
				{
					row2.rowKey = text;
					row2.rowGuid = text2;
				}
				if (row2.cellValues == null)
				{
					row2.cellValues = new List<string>();
				}
				while (row2.cellValues.Count < count)
				{
					row2.cellValues.Add(string.Empty);
				}
				if (row2.cellValues.Count > count)
				{
					row2.cellValues.RemoveRange(count, row2.cellValues.Count - count);
				}
				list.Add(row2);
			}
			this.rows = list;
		}

		// Token: 0x0600B9AC RID: 47532 RVA: 0x0044740C File Offset: 0x0044560C
		public string Get(int rowIdx, int colIdx)
		{
			return this.rows[rowIdx].cellValues[colIdx];
		}

		// Token: 0x0600B9AD RID: 47533 RVA: 0x00447425 File Offset: 0x00445625
		public void Set(int rowIdx, int colIdx, string value)
		{
			this.rows[rowIdx].cellValues[colIdx] = value ?? string.Empty;
		}

		// Token: 0x0600B9AE RID: 47534 RVA: 0x00447448 File Offset: 0x00445648
		public void ClampValuesToValidOptions(Func<string, HashSet<string>> getValidOptionsForRowKey)
		{
			for (int i = 0; i < this.rows.Count; i++)
			{
				StringTable2D.Row row = this.rows[i];
				HashSet<string> hashSet = ((getValidOptionsForRowKey != null) ? getValidOptionsForRowKey(row.rowKey) : null);
				if (hashSet != null)
				{
					for (int j = 0; j < row.cellValues.Count; j++)
					{
						string text = row.cellValues[j] ?? string.Empty;
						if (!hashSet.Contains(text))
						{
							row.cellValues[j] = string.Empty;
						}
					}
				}
			}
		}

		// Token: 0x0600B9AF RID: 47535 RVA: 0x004474D8 File Offset: 0x004456D8
		public void ApplyReorder(IReadOnlyList<int> newToOld)
		{
			int count = this.rowKeys.Count;
			if (newToOld == null || newToOld.Count != count)
			{
				return;
			}
			List<StringTable2D.Row> list = this.rows;
			List<StringTable2D.Row> list2 = new List<StringTable2D.Row>(count);
			for (int i = 0; i < count; i++)
			{
				list2.Add(null);
			}
			for (int j = 0; j < count; j++)
			{
				int num = newToOld[j];
				list2[j] = list[num];
			}
			this.rows = list2;
			for (int k = 0; k < this.rows.Count; k++)
			{
				List<string> cellValues = this.rows[k].cellValues;
				List<string> list3 = new List<string>(new string[count]);
				for (int l = 0; l < count; l++)
				{
					int num2 = newToOld[l];
					list3[l] = ((num2 >= 0 && num2 < cellValues.Count) ? cellValues[num2] : string.Empty);
				}
				this.rows[k].cellValues = list3;
			}
			List<string> list4 = this.rowKeys;
			List<string> list5 = this.columnKeys;
			List<string> list6 = new List<string>(count);
			List<string> list7 = new List<string>(count);
			for (int m = 0; m < count; m++)
			{
				list6.Add(list4[newToOld[m]]);
				list7.Add(list5[newToOld[m]]);
			}
			this.rowKeys = list6;
			this.columnKeys = list7;
		}

		// Token: 0x04008AA0 RID: 35488
		public List<string> rowKeys = new List<string>();

		// Token: 0x04008AA1 RID: 35489
		public List<string> columnKeys = new List<string>();

		// Token: 0x04008AA2 RID: 35490
		public List<StringTable2D.Row> rows = new List<StringTable2D.Row>();

		// Token: 0x0200174C RID: 5964
		[Serializable]
		public class Row
		{
			// Token: 0x04008AA3 RID: 35491
			public string rowKey;

			// Token: 0x04008AA4 RID: 35492
			public string rowGuid;

			// Token: 0x04008AA5 RID: 35493
			public List<string> cellValues = new List<string>();
		}
	}
}
