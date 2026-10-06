using System;

// Token: 0x02001478 RID: 5240
public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
{
	// Token: 0x17001372 RID: 4978
	// (get) Token: 0x0600A40C RID: 41996 RVA: 0x003D5BC3 File Offset: 0x003D3DC3
	public TKey Key { get; }

	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x0600A40D RID: 41997 RVA: 0x003D5BCB File Offset: 0x003D3DCB
	public TValue Value { get; }

	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x0600A40E RID: 41998 RVA: 0x003D5BD3 File Offset: 0x003D3DD3
	public ObservableDictionary<TKey, TValue>.EChangeType Action { get; }

	// Token: 0x0600A40F RID: 41999 RVA: 0x003D5BDB File Offset: 0x003D3DDB
	public DictionaryChangedEventArgs(TKey key, TValue value, ObservableDictionary<TKey, TValue>.EChangeType action)
	{
		this.Key = key;
		this.Value = value;
		this.Action = action;
	}
}
