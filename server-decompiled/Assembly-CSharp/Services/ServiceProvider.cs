using System;
using System.Collections.Generic;

namespace Services
{
	// Token: 0x0200169C RID: 5788
	public class ServiceProvider
	{
		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600B4FF RID: 46335 RVA: 0x0043AAEC File Offset: 0x00438CEC
		// (set) Token: 0x0600B500 RID: 46336 RVA: 0x0043AAF3 File Offset: 0x00438CF3
		public static ServiceProvider Instance
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B501 RID: 46337 RVA: 0x0043AAFB File Offset: 0x00438CFB
		public static void Init()
		{
			if (ServiceProvider.Instance != null)
			{
				Log.Out("ServiceProvider has already exists");
				return;
			}
			ServiceProvider.Instance = new ServiceProvider();
			ServiceProvider.Instance.InternalInit();
		}

		// Token: 0x0600B502 RID: 46338 RVA: 0x0043AB23 File Offset: 0x00438D23
		[PublicizedFrom(EAccessModifier.Private)]
		public void InternalInit()
		{
			this._services = new Dictionary<Type, object>();
		}

		// Token: 0x0600B503 RID: 46339 RVA: 0x0043AB30 File Offset: 0x00438D30
		public void Register(Type type, object obj)
		{
			this._services.Add(type, obj);
		}

		// Token: 0x0600B504 RID: 46340 RVA: 0x0043AB3F File Offset: 0x00438D3F
		public T Get<T>()
		{
			return (T)((object)this._services[typeof(T)]);
		}

		// Token: 0x040087AE RID: 34734
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<Type, object> _services;
	}
}
