using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001C05 RID: 7173
	public interface IServerListInterface
	{
		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x0600D5BE RID: 54718
		bool IsPrefiltered { get; }

		// Token: 0x0600D5BF RID: 54719
		void Init(IPlatform _owner);

		// Token: 0x0600D5C0 RID: 54720
		void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback);

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x0600D5C1 RID: 54721
		bool IsRefreshing { get; }

		// Token: 0x0600D5C2 RID: 54722
		void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600D5C3 RID: 54723
		void StopSearch();

		// Token: 0x0600D5C4 RID: 54724
		void Disconnect();

		// Token: 0x0600D5C5 RID: 54725
		void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback);

		// Token: 0x02001C06 RID: 7174
		public class ServerFilter
		{
			// Token: 0x0600D5C6 RID: 54726 RVA: 0x004CFD74 File Offset: 0x004CDF74
			public ServerFilter(string _name, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			{
				this.Name = _name;
				this.Type = _type;
				this.IntMinValue = _intMinValue;
				this.IntMaxValue = _intMaxValue;
				this.BoolValue = _boolValue;
				this.StringNeedle = _stringNeedle;
			}

			// Token: 0x0400A2A0 RID: 41632
			public readonly string Name;

			// Token: 0x0400A2A1 RID: 41633
			public readonly IServerListInterface.ServerFilter.EServerFilterType Type;

			// Token: 0x0400A2A2 RID: 41634
			public readonly int IntMinValue;

			// Token: 0x0400A2A3 RID: 41635
			public readonly int IntMaxValue;

			// Token: 0x0400A2A4 RID: 41636
			public readonly bool BoolValue;

			// Token: 0x0400A2A5 RID: 41637
			public readonly string StringNeedle;

			// Token: 0x02001C07 RID: 7175
			public enum EServerFilterType
			{
				// Token: 0x0400A2A7 RID: 41639
				Any,
				// Token: 0x0400A2A8 RID: 41640
				BoolValue,
				// Token: 0x0400A2A9 RID: 41641
				IntValue,
				// Token: 0x0400A2AA RID: 41642
				IntNotValue,
				// Token: 0x0400A2AB RID: 41643
				IntMin,
				// Token: 0x0400A2AC RID: 41644
				IntMax,
				// Token: 0x0400A2AD RID: 41645
				IntRange,
				// Token: 0x0400A2AE RID: 41646
				StringValue,
				// Token: 0x0400A2AF RID: 41647
				StringContains
			}
		}
	}
}
