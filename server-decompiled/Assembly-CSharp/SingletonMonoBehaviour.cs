using System;
using UnityEngine;

// Token: 0x020014C4 RID: 5316
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	// Token: 0x0600A670 RID: 42608 RVA: 0x003DF4C4 File Offset: 0x003DD6C4
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

	// Token: 0x0600A671 RID: 42609 RVA: 0x003DF547 File Offset: 0x003DD747
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDestroy()
	{
		if (this == SingletonMonoBehaviour<T>.Instance)
		{
			this.singletonDestroy();
		}
	}

	// Token: 0x0600A672 RID: 42610 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonAwake()
	{
	}

	// Token: 0x0600A673 RID: 42611 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonCreated()
	{
	}

	// Token: 0x0600A674 RID: 42612 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void singletonDestroy()
	{
	}

	// Token: 0x04007C01 RID: 31745
	public static T Instance;

	// Token: 0x04007C02 RID: 31746
	public bool IsPersistant;
}
