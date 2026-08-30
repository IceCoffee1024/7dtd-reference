using System;
using UnityEngine;

// Token: 0x020014A4 RID: 5284
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	// Token: 0x0600A5C2 RID: 42434 RVA: 0x003DA838 File Offset: 0x003D8A38
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (this.IsPersistant)
		{
			if (!SingletonMonoBehaviour<T>.Instance)
			{
				SingletonMonoBehaviour<T>.Instance = (T)((object)this);
				SingletonMonoBehaviour<T>.Instance.singletonCreated();
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else
		{
			SingletonMonoBehaviour<T>.Instance = (T)((object)this);
			SingletonMonoBehaviour<T>.Instance.singletonCreated();
		}
		SingletonMonoBehaviour<T>.Instance.singletonAwake();
	}

	// Token: 0x0600A5C3 RID: 42435 RVA: 0x003DA8BB File Offset: 0x003D8ABB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDestroy()
	{
		if (this == SingletonMonoBehaviour<T>.Instance)
		{
			this.singletonDestroy();
		}
	}

	// Token: 0x0600A5C4 RID: 42436 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonAwake()
	{
	}

	// Token: 0x0600A5C5 RID: 42437 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonCreated()
	{
	}

	// Token: 0x0600A5C6 RID: 42438 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonDestroy()
	{
	}

	// Token: 0x04007B86 RID: 31622
	public static T Instance;

	// Token: 0x04007B87 RID: 31623
	public bool IsPersistant;
}
