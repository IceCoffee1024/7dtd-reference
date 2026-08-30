using System;

// Token: 0x02001458 RID: 5208
public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
{
	// Token: 0x1700132D RID: 4909
	// (get) Token: 0x0600A360 RID: 41824 RVA: 0x003D0FB3 File Offset: 0x003CF1B3
	public TKey Key { get; }

	// Token: 0x1700132E RID: 4910
	// (get) Token: 0x0600A361 RID: 41825 RVA: 0x003D0FBB File Offset: 0x003CF1BB
	public TValue Value { get; }

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x0600A362 RID: 41826 RVA: 0x003D0FC3 File Offset: 0x003CF1C3
	public ObservableDictionary<TKey, TValue>.EChangeType Action { get; }

	// Token: 0x0600A363 RID: 41827 RVA: 0x003D0FCB File Offset: 0x003CF1CB
	public DictionaryChangedEventArgs(TKey key, TValue value, ObservableDictionary<TKey, TValue>.EChangeType action)
	{
		this.Key = key;
		this.Value = value;
		this.Action = action;
	}
}
