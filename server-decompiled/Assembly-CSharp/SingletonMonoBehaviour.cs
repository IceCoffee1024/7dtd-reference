using System;
using UnityEngine;

// Token: 0x020014D6 RID: 5334
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	// Token: 0x0600A6C8 RID: 42696 RVA: 0x003E18C8 File Offset: 0x003DFAC8
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

	// Token: 0x0600A6C9 RID: 42697 RVA: 0x003E194B File Offset: 0x003DFB4B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDestroy()
	{
		if (this == SingletonMonoBehaviour<T>.Instance)
		{
			this.singletonDestroy();
		}
	}

	// Token: 0x0600A6CA RID: 42698 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonAwake()
	{
	}

	// Token: 0x0600A6CB RID: 42699 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonCreated()
	{
	}

	// Token: 0x0600A6CC RID: 42700 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonDestroy()
	{
	}

	// Token: 0x04007C42 RID: 31810
	public static T Instance;

	// Token: 0x04007C43 RID: 31811
	public bool IsPersistant;
}
