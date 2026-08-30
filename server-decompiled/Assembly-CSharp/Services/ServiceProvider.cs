using System;
using System.Collections.Generic;

namespace Services
{
	// Token: 0x0200167D RID: 5757
	public class ServiceProvider
	{
		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x0600B46D RID: 46189 RVA: 0x00436E10 File Offset: 0x00435010
		// (set) Token: 0x0600B46E RID: 46190 RVA: 0x00436E17 File Offset: 0x00435017
		public static ServiceProvider Instance
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B46F RID: 46191 RVA: 0x00436E1F File Offset: 0x0043501F
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

		// Token: 0x0600B470 RID: 46192 RVA: 0x00436E47 File Offset: 0x00435047
		[PublicizedFrom(EAccessModifier.Private)]
		public void InternalInit()
		{
			this._services = new Dictionary<Type, object>();
		}

		// Token: 0x0600B471 RID: 46193 RVA: 0x00436E54 File Offset: 0x00435054
		public void Register(Type type, object obj)
		{
			this._services.Add(type, obj);
		}

		// Token: 0x0600B472 RID: 46194 RVA: 0x00436E63 File Offset: 0x00435063
		public T Get<T>()
		{
			return (T)((object)this._services[typeof(T)]);
		}

		// Token: 0x04008758 RID: 34648
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<Type, object> _services;
	}
}
