using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BD4 RID: 7124
	public interface IServerListInterface
	{
		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600D4AA RID: 54442
		bool IsPrefiltered { get; }

		// Token: 0x0600D4AB RID: 54443
		void Init(IPlatform _owner);

		// Token: 0x0600D4AC RID: 54444
		void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback);

		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x0600D4AD RID: 54445
		bool IsRefreshing { get; }

		// Token: 0x0600D4AE RID: 54446
		void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600D4AF RID: 54447
		void StopSearch();

		// Token: 0x0600D4B0 RID: 54448
		void Disconnect();

		// Token: 0x0600D4B1 RID: 54449
		void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback);

		// Token: 0x02001BD5 RID: 7125
		public class ServerFilter
		{
			// Token: 0x0600D4B2 RID: 54450 RVA: 0x004CC24C File Offset: 0x004CA44C
			public ServerFilter(string _name, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			{
				this.Name = _name;
				this.Type = _type;
				this.IntMinValue = _intMinValue;
				this.IntMaxValue = _intMaxValue;
				this.BoolValue = _boolValue;
				this.StringNeedle = _stringNeedle;
			}

			// Token: 0x0400A1B7 RID: 41399
			public readonly string Name;

			// Token: 0x0400A1B8 RID: 41400
			public readonly IServerListInterface.ServerFilter.EServerFilterType Type;

			// Token: 0x0400A1B9 RID: 41401
			public readonly int IntMinValue;

			// Token: 0x0400A1BA RID: 41402
			public readonly int IntMaxValue;

			// Token: 0x0400A1BB RID: 41403
			public readonly bool BoolValue;

			// Token: 0x0400A1BC RID: 41404
			public readonly string StringNeedle;

			// Token: 0x02001BD6 RID: 7126
			public enum EServerFilterType
			{
				// Token: 0x0400A1BE RID: 41406
				Any,
				// Token: 0x0400A1BF RID: 41407
				BoolValue,
				// Token: 0x0400A1C0 RID: 41408
				IntValue,
				// Token: 0x0400A1C1 RID: 41409
				IntNotValue,
				// Token: 0x0400A1C2 RID: 41410
				IntMin,
				// Token: 0x0400A1C3 RID: 41411
				IntMax,
				// Token: 0x0400A1C4 RID: 41412
				IntRange,
				// Token: 0x0400A1C5 RID: 41413
				StringValue,
				// Token: 0x0400A1C6 RID: 41414
				StringContains
			}
		}
	}
}
