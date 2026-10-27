using System;
using System.Collections.Generic;

namespace Platform.LAN
{
	// Token: 0x02001D33 RID: 7475
	public class LANServerCacheControl
	{
		// Token: 0x0600DD92 RID: 56722 RVA: 0x004EC879 File Offset: 0x004EAA79
		public LANServerCacheControl(TimeSpan updateInterval, TimeSpan timeout)
		{
			this.timeout = timeout;
			this.updateInterval = updateInterval;
		}

		// Token: 0x0600DD93 RID: 56723 RVA: 0x004EC89C File Offset: 0x004EAA9C
		public bool IsUpdateRequired(string addressString, int port)
		{
			LANServerCacheControl.ServerKey serverKey = new LANServerCacheControl.ServerKey(addressString, port);
			LANServerCacheControl.UpdateTimes updateTimes;
			if (!this.lastServerUpdateTimes.TryGetValue(serverKey, out updateTimes))
			{
				this.lastServerUpdateTimes[serverKey] = new LANServerCacheControl.UpdateTimes
				{
					lastServerCheckedTime = DateTime.Now
				};
				return true;
			}
			TimeSpan timeSpan = DateTime.Now - updateTimes.lastServerCheckedTime;
			updateTimes.lastServerCheckedTime = DateTime.Now;
			this.lastServerUpdateTimes[serverKey] = updateTimes;
			if (timeSpan > this.timeout)
			{
				Log.Out(string.Format("[{0}] server timed out, update needed. Last checked {1:F2} seconds ago", "LANServerCacheControl", timeSpan.TotalSeconds));
				return true;
			}
			TimeSpan timeSpan2 = DateTime.Now - updateTimes.lastRulesUpdateTime;
			if (timeSpan2 > this.updateInterval)
			{
				Log.Out(string.Format("[{0}] found known server, last updated {1:F2} seconds ago", "LANServerCacheControl", timeSpan2.TotalSeconds));
				return true;
			}
			return false;
		}

		// Token: 0x0600DD94 RID: 56724 RVA: 0x004EC988 File Offset: 0x004EAB88
		public void SetUpdated(string addressString, int port)
		{
			LANServerCacheControl.ServerKey serverKey = new LANServerCacheControl.ServerKey(addressString, port);
			LANServerCacheControl.UpdateTimes updateTimes;
			if (!this.lastServerUpdateTimes.TryGetValue(serverKey, out updateTimes))
			{
				this.lastServerUpdateTimes[serverKey] = new LANServerCacheControl.UpdateTimes
				{
					lastServerCheckedTime = DateTime.Now,
					lastRulesUpdateTime = DateTime.Now
				};
				return;
			}
			updateTimes.lastRulesUpdateTime = DateTime.Now;
			this.lastServerUpdateTimes[serverKey] = updateTimes;
		}

		// Token: 0x0600DD95 RID: 56725 RVA: 0x004EC9F5 File Offset: 0x004EABF5
		public void Clear()
		{
			this.lastServerUpdateTimes.Clear();
		}

		// Token: 0x0400A747 RID: 42823
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan timeout;

		// Token: 0x0400A748 RID: 42824
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan updateInterval;

		// Token: 0x0400A749 RID: 42825
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes> lastServerUpdateTimes = new Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes>();

		// Token: 0x02001D34 RID: 7476
		[PublicizedFrom(EAccessModifier.Private)]
		public struct ServerKey : IEquatable<LANServerCacheControl.ServerKey>
		{
			// Token: 0x0600DD96 RID: 56726 RVA: 0x004ECA02 File Offset: 0x004EAC02
			public ServerKey(string _ipAddress, int _port)
			{
				this.ipAddress = _ipAddress;
				this.port = _port;
			}

			// Token: 0x0600DD97 RID: 56727 RVA: 0x004ECA12 File Offset: 0x004EAC12
			public override int GetHashCode()
			{
				return this.ipAddress.GetHashCode() ^ this.port;
			}

			// Token: 0x0600DD98 RID: 56728 RVA: 0x004ECA26 File Offset: 0x004EAC26
			public bool Equals(LANServerCacheControl.ServerKey _other)
			{
				return this.ipAddress.Equals(_other.ipAddress) && this.port == _other.port;
			}

			// Token: 0x0400A74A RID: 42826
			public readonly string ipAddress;

			// Token: 0x0400A74B RID: 42827
			public readonly int port;
		}

		// Token: 0x02001D35 RID: 7477
		[PublicizedFrom(EAccessModifier.Private)]
		public struct UpdateTimes
		{
			// Token: 0x0400A74C RID: 42828
			public DateTime lastServerCheckedTime;

			// Token: 0x0400A74D RID: 42829
			public DateTime lastRulesUpdateTime;
		}
	}
}
