using System;

// Token: 0x0200143D RID: 5181
public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
{
	// Token: 0x170012F4 RID: 4852
	// (get) Token: 0x0600A263 RID: 41571 RVA: 0x003CBC6F File Offset: 0x003C9E6F
	public TKey Key { get; }

	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x0600A264 RID: 41572 RVA: 0x003CBC77 File Offset: 0x003C9E77
	public TValue Value { get; }

	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x0600A265 RID: 41573 RVA: 0x003CBC7F File Offset: 0x003C9E7F
	public ObservableDictionary<TKey, TValue>.EChangeType Action { get; }

	// Token: 0x0600A266 RID: 41574 RVA: 0x003CBC87 File Offset: 0x003C9E87
	public DictionaryChangedEventArgs(TKey key, TValue value, ObservableDictionary<TKey, TValue>.EChangeType action)
	{
		this.Key = key;
		this.Value = value;
		this.Action = action;
	}
}
