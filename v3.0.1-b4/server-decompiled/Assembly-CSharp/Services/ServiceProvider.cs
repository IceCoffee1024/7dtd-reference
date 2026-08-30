using System;
using System.Collections.Generic;

namespace Services
{
	// Token: 0x02001662 RID: 5730
	public class ServiceProvider
	{
		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x0600B370 RID: 45936 RVA: 0x00431AD0 File Offset: 0x0042FCD0
		// (set) Token: 0x0600B371 RID: 45937 RVA: 0x00431AD7 File Offset: 0x0042FCD7
		public static ServiceProvider Instance
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B372 RID: 45938 RVA: 0x00431ADF File Offset: 0x0042FCDF
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

		// Token: 0x0600B373 RID: 45939 RVA: 0x00431B07 File Offset: 0x0042FD07
		[PublicizedFrom(EAccessModifier.Private)]
		public void InternalInit()
		{
			this._services = new Dictionary<Type, object>();
		}

		// Token: 0x0600B374 RID: 45940 RVA: 0x00431B14 File Offset: 0x0042FD14
		public void Register(Type type, object obj)
		{
			this._services.Add(type, obj);
		}

		// Token: 0x0600B375 RID: 45941 RVA: 0x00431B23 File Offset: 0x0042FD23
		public T Get<T>()
		{
			return (T)((object)this._services[typeof(T)]);
		}

		// Token: 0x0400867F RID: 34431
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<Type, object> _services;
	}
}
