using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
	// Token: 0x0200030F RID: 783
	internal class DictionarySerializationSurrogate<TKey, TValue> : ISerializationSurrogate
	{
		// Token: 0x0600201C RID: 8220 RVA: 0x000356EC File Offset: 0x000338EC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Dictionary<TKey, TValue> dictionary = (Dictionary<TKey, TValue>)obj;
			dictionary.GetObjectData(info, context);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0003570C File Offset: 0x0003390C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			IEqualityComparer<TKey> equalityComparer = (IEqualityComparer<TKey>)info.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(equalityComparer);
			bool flag = info.MemberCount > 3;
			if (flag)
			{
				KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])info.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
				bool flag2 = array != null;
				if (flag2)
				{
					foreach (KeyValuePair<TKey, TValue> keyValuePair in array)
					{
						dictionary.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
			return dictionary;
		}
	}
}
