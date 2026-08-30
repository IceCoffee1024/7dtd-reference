using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BA2 RID: 7074
	public interface IServerListInterface
	{
		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x0600D332 RID: 54066
		bool IsPrefiltered { get; }

		// Token: 0x0600D333 RID: 54067
		void Init(IPlatform _owner);

		// Token: 0x0600D334 RID: 54068
		void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback);

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x0600D335 RID: 54069
		bool IsRefreshing { get; }

		// Token: 0x0600D336 RID: 54070
		void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600D337 RID: 54071
		void StopSearch();

		// Token: 0x0600D338 RID: 54072
		void Disconnect();

		// Token: 0x0600D339 RID: 54073
		void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback);

		// Token: 0x02001BA3 RID: 7075
		public class ServerFilter
		{
			// Token: 0x0600D33A RID: 54074 RVA: 0x004C7764 File Offset: 0x004C5964
			public ServerFilter(string _name, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			{
				this.Name = _name;
				this.Type = _type;
				this.IntMinValue = _intMinValue;
				this.IntMaxValue = _intMaxValue;
				this.BoolValue = _boolValue;
				this.StringNeedle = _stringNeedle;
			}

			// Token: 0x0400A0E2 RID: 41186
			public readonly string Name;

			// Token: 0x0400A0E3 RID: 41187
			public readonly IServerListInterface.ServerFilter.EServerFilterType Type;

			// Token: 0x0400A0E4 RID: 41188
			public readonly int IntMinValue;

			// Token: 0x0400A0E5 RID: 41189
			public readonly int IntMaxValue;

			// Token: 0x0400A0E6 RID: 41190
			public readonly bool BoolValue;

			// Token: 0x0400A0E7 RID: 41191
			public readonly string StringNeedle;

			// Token: 0x02001BA4 RID: 7076
			public enum EServerFilterType
			{
				// Token: 0x0400A0E9 RID: 41193
				Any,
				// Token: 0x0400A0EA RID: 41194
				BoolValue,
				// Token: 0x0400A0EB RID: 41195
				IntValue,
				// Token: 0x0400A0EC RID: 41196
				IntNotValue,
				// Token: 0x0400A0ED RID: 41197
				IntMin,
				// Token: 0x0400A0EE RID: 41198
				IntMax,
				// Token: 0x0400A0EF RID: 41199
				IntRange,
				// Token: 0x0400A0F0 RID: 41200
				StringValue,
				// Token: 0x0400A0F1 RID: 41201
				StringContains
			}
		}
	}
}
