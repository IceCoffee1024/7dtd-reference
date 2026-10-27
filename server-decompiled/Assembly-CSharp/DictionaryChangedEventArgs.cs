using System;

// Token: 0x0200148A RID: 5258
public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
{
	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x0600A464 RID: 42084 RVA: 0x003D7FC7 File Offset: 0x003D61C7
	public TKey Key { get; }

	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x0600A465 RID: 42085 RVA: 0x003D7FCF File Offset: 0x003D61CF
	public TValue Value { get; }

	// Token: 0x17001380 RID: 4992
	// (get) Token: 0x0600A466 RID: 42086 RVA: 0x003D7FD7 File Offset: 0x003D61D7
	public ObservableDictionary<TKey, TValue>.EChangeType Action { get; }

	// Token: 0x0600A467 RID: 42087 RVA: 0x003D7FDF File Offset: 0x003D61DF
	public DictionaryChangedEventArgs(TKey key, TValue value, ObservableDictionary<TKey, TValue>.EChangeType action)
	{
		this.Key = key;
		this.Value = value;
		this.Action = action;
	}
}
