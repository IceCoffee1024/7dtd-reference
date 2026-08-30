using System;
using System.Collections.Generic;

namespace Platform.LAN
{
	// Token: 0x02001CA9 RID: 7337
	public class LANServerCacheControl
	{
		// Token: 0x0600D968 RID: 55656 RVA: 0x004DDB7D File Offset: 0x004DBD7D
		public LANServerCacheControl(TimeSpan updateInterval, TimeSpan timeout)
		{
			this.timeout = timeout;
			this.updateInterval = updateInterval;
		}

		// Token: 0x0600D969 RID: 55657 RVA: 0x004DDBA0 File Offset: 0x004DBDA0
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

		// Token: 0x0600D96A RID: 55658 RVA: 0x004DDC8C File Offset: 0x004DBE8C
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

		// Token: 0x0600D96B RID: 55659 RVA: 0x004DDCF9 File Offset: 0x004DBEF9
		public void Clear()
		{
			this.lastServerUpdateTimes.Clear();
		}

		// Token: 0x0400A456 RID: 42070
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan timeout;

		// Token: 0x0400A457 RID: 42071
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan updateInterval;

		// Token: 0x0400A458 RID: 42072
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes> lastServerUpdateTimes = new Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes>();

		// Token: 0x02001CAA RID: 7338
		[PublicizedFrom(EAccessModifier.Private)]
		public struct ServerKey : IEquatable<LANServerCacheControl.ServerKey>
		{
			// Token: 0x0600D96C RID: 55660 RVA: 0x004DDD06 File Offset: 0x004DBF06
			public ServerKey(string _ipAddress, int _port)
			{
				this.ipAddress = _ipAddress;
				this.port = _port;
			}

			// Token: 0x0600D96D RID: 55661 RVA: 0x004DDD16 File Offset: 0x004DBF16
			public override int GetHashCode()
			{
				return this.ipAddress.GetHashCode() ^ this.port;
			}

			// Token: 0x0600D96E RID: 55662 RVA: 0x004DDD2A File Offset: 0x004DBF2A
			public bool Equals(LANServerCacheControl.ServerKey _other)
			{
				return this.ipAddress.Equals(_other.ipAddress) && this.port == _other.port;
			}

			// Token: 0x0400A459 RID: 42073
			public readonly string ipAddress;

			// Token: 0x0400A45A RID: 42074
			public readonly int port;
		}

		// Token: 0x02001CAB RID: 7339
		[PublicizedFrom(EAccessModifier.Private)]
		public struct UpdateTimes
		{
			// Token: 0x0400A45B RID: 42075
			public DateTime lastServerCheckedTime;

			// Token: 0x0400A45C RID: 42076
			public DateTime lastRulesUpdateTime;
		}
	}
}
