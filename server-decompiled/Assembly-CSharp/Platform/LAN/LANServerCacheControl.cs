using System;
using System.Collections.Generic;

namespace Platform.LAN
{
	// Token: 0x02001D02 RID: 7426
	public class LANServerCacheControl
	{
		// Token: 0x0600DC7E RID: 56446 RVA: 0x004E8CD9 File Offset: 0x004E6ED9
		public LANServerCacheControl(TimeSpan updateInterval, TimeSpan timeout)
		{
			this.timeout = timeout;
			this.updateInterval = updateInterval;
		}

		// Token: 0x0600DC7F RID: 56447 RVA: 0x004E8CFC File Offset: 0x004E6EFC
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

		// Token: 0x0600DC80 RID: 56448 RVA: 0x004E8DE8 File Offset: 0x004E6FE8
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

		// Token: 0x0600DC81 RID: 56449 RVA: 0x004E8E55 File Offset: 0x004E7055
		public void Clear()
		{
			this.lastServerUpdateTimes.Clear();
		}

		// Token: 0x0400A65E RID: 42590
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan timeout;

		// Token: 0x0400A65F RID: 42591
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan updateInterval;

		// Token: 0x0400A660 RID: 42592
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes> lastServerUpdateTimes = new Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes>();

		// Token: 0x02001D03 RID: 7427
		[PublicizedFrom(EAccessModifier.Private)]
		public struct ServerKey : IEquatable<LANServerCacheControl.ServerKey>
		{
			// Token: 0x0600DC82 RID: 56450 RVA: 0x004E8E62 File Offset: 0x004E7062
			public ServerKey(string _ipAddress, int _port)
			{
				this.ipAddress = _ipAddress;
				this.port = _port;
			}

			// Token: 0x0600DC83 RID: 56451 RVA: 0x004E8E72 File Offset: 0x004E7072
			public override int GetHashCode()
			{
				return this.ipAddress.GetHashCode() ^ this.port;
			}

			// Token: 0x0600DC84 RID: 56452 RVA: 0x004E8E86 File Offset: 0x004E7086
			public bool Equals(LANServerCacheControl.ServerKey _other)
			{
				return this.ipAddress.Equals(_other.ipAddress) && this.port == _other.port;
			}

			// Token: 0x0400A661 RID: 42593
			public readonly string ipAddress;

			// Token: 0x0400A662 RID: 42594
			public readonly int port;
		}

		// Token: 0x02001D04 RID: 7428
		[PublicizedFrom(EAccessModifier.Private)]
		public struct UpdateTimes
		{
			// Token: 0x0400A663 RID: 42595
			public DateTime lastServerCheckedTime;

			// Token: 0x0400A664 RID: 42596
			public DateTime lastRulesUpdateTime;
		}
	}
}
