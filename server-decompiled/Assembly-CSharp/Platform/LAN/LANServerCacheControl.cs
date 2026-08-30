using System;
using System.Collections.Generic;

namespace Platform.LAN
{
	// Token: 0x02001CD0 RID: 7376
	public class LANServerCacheControl
	{
		// Token: 0x0600DB03 RID: 56067 RVA: 0x004E4191 File Offset: 0x004E2391
		public LANServerCacheControl(TimeSpan updateInterval, TimeSpan timeout)
		{
			this.timeout = timeout;
			this.updateInterval = updateInterval;
		}

		// Token: 0x0600DB04 RID: 56068 RVA: 0x004E41B4 File Offset: 0x004E23B4
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

		// Token: 0x0600DB05 RID: 56069 RVA: 0x004E42A0 File Offset: 0x004E24A0
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

		// Token: 0x0600DB06 RID: 56070 RVA: 0x004E430D File Offset: 0x004E250D
		public void Clear()
		{
			this.lastServerUpdateTimes.Clear();
		}

		// Token: 0x0400A589 RID: 42377
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan timeout;

		// Token: 0x0400A58A RID: 42378
		[PublicizedFrom(EAccessModifier.Private)]
		public TimeSpan updateInterval;

		// Token: 0x0400A58B RID: 42379
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes> lastServerUpdateTimes = new Dictionary<LANServerCacheControl.ServerKey, LANServerCacheControl.UpdateTimes>();

		// Token: 0x02001CD1 RID: 7377
		[PublicizedFrom(EAccessModifier.Private)]
		public struct ServerKey : IEquatable<LANServerCacheControl.ServerKey>
		{
			// Token: 0x0600DB07 RID: 56071 RVA: 0x004E431A File Offset: 0x004E251A
			public ServerKey(string _ipAddress, int _port)
			{
				this.ipAddress = _ipAddress;
				this.port = _port;
			}

			// Token: 0x0600DB08 RID: 56072 RVA: 0x004E432A File Offset: 0x004E252A
			public override int GetHashCode()
			{
				return this.ipAddress.GetHashCode() ^ this.port;
			}

			// Token: 0x0600DB09 RID: 56073 RVA: 0x004E433E File Offset: 0x004E253E
			public bool Equals(LANServerCacheControl.ServerKey _other)
			{
				return this.ipAddress.Equals(_other.ipAddress) && this.port == _other.port;
			}

			// Token: 0x0400A58C RID: 42380
			public readonly string ipAddress;

			// Token: 0x0400A58D RID: 42381
			public readonly int port;
		}

		// Token: 0x02001CD2 RID: 7378
		[PublicizedFrom(EAccessModifier.Private)]
		public struct UpdateTimes
		{
			// Token: 0x0400A58E RID: 42382
			public DateTime lastServerCheckedTime;

			// Token: 0x0400A58F RID: 42383
			public DateTime lastRulesUpdateTime;
		}
	}
}
