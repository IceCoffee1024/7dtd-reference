using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000499 RID: 1177
	public class Notification : INotification
	{
		// Token: 0x0600286C RID: 10348 RVA: 0x000453D1 File Offset: 0x000435D1
		public Notification(string name)
		{
			this.id = new PropertyName(name);
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000453E7 File Offset: 0x000435E7
		public PropertyName id { get; }
	}
}
