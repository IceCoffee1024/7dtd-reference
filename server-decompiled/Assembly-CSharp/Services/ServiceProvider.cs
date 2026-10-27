using System;
using System.Collections.Generic;

namespace Services
{
	// Token: 0x020016AE RID: 5806
	public class ServiceProvider
	{
		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x0600B557 RID: 46423 RVA: 0x0043CEF0 File Offset: 0x0043B0F0
		// (set) Token: 0x0600B558 RID: 46424 RVA: 0x0043CEF7 File Offset: 0x0043B0F7
		public static ServiceProvider Instance
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B559 RID: 46425 RVA: 0x0043CEFF File Offset: 0x0043B0FF
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

		// Token: 0x0600B55A RID: 46426 RVA: 0x0043CF27 File Offset: 0x0043B127
		[PublicizedFrom(EAccessModifier.Private)]
		public void InternalInit()
		{
			this._services = new Dictionary<Type, object>();
		}

		// Token: 0x0600B55B RID: 46427 RVA: 0x0043CF34 File Offset: 0x0043B134
		public void Register(Type type, object obj)
		{
			this._services.Add(type, obj);
		}

		// Token: 0x0600B55C RID: 46428 RVA: 0x0043CF43 File Offset: 0x0043B143
		public T Get<T>()
		{
			return (T)((object)this._services[typeof(T)]);
		}

		// Token: 0x040087EF RID: 34799
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<Type, object> _services;
	}
}
