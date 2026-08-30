using System;
using UnityEngine;

// Token: 0x02001489 RID: 5257
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	// Token: 0x0600A4C5 RID: 42181 RVA: 0x003D54F4 File Offset: 0x003D36F4
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

	// Token: 0x0600A4C6 RID: 42182 RVA: 0x003D5577 File Offset: 0x003D3777
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDestroy()
	{
		if (this == SingletonMonoBehaviour<T>.Instance)
		{
			this.singletonDestroy();
		}
	}

	// Token: 0x0600A4C7 RID: 42183 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonAwake()
	{
	}

	// Token: 0x0600A4C8 RID: 42184 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonCreated()
	{
	}

	// Token: 0x0600A4C9 RID: 42185 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonDestroy()
	{
	}

	// Token: 0x04007AAD RID: 31405
	public static T Instance;

	// Token: 0x04007AAE RID: 31406
	public bool IsPersistant;
}
