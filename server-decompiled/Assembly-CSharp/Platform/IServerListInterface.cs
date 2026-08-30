using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B7B RID: 7035
	public interface IServerListInterface
	{
		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x0600D1A6 RID: 53670
		bool IsPrefiltered { get; }

		// Token: 0x0600D1A7 RID: 53671
		void Init(IPlatform _owner);

		// Token: 0x0600D1A8 RID: 53672
		void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback);

		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x0600D1A9 RID: 53673
		bool IsRefreshing { get; }

		// Token: 0x0600D1AA RID: 53674
		void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters);

		// Token: 0x0600D1AB RID: 53675
		void StopSearch();

		// Token: 0x0600D1AC RID: 53676
		void Disconnect();

		// Token: 0x0600D1AD RID: 53677
		void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback);

		// Token: 0x02001B7C RID: 7036
		public class ServerFilter
		{
			// Token: 0x0600D1AE RID: 53678 RVA: 0x004C1370 File Offset: 0x004BF570
			public ServerFilter(string _name, IServerListInterface.ServerFilter.EServerFilterType _type = IServerListInterface.ServerFilter.EServerFilterType.Any, int _intMinValue = 0, int _intMaxValue = 0, bool _boolValue = false, string _stringNeedle = null)
			{
				this.Name = _name;
				this.Type = _type;
				this.IntMinValue = _intMinValue;
				this.IntMaxValue = _intMaxValue;
				this.BoolValue = _boolValue;
				this.StringNeedle = _stringNeedle;
			}

			// Token: 0x04009FB1 RID: 40881
			public readonly string Name;

			// Token: 0x04009FB2 RID: 40882
			public readonly IServerListInterface.ServerFilter.EServerFilterType Type;

			// Token: 0x04009FB3 RID: 40883
			public readonly int IntMinValue;

			// Token: 0x04009FB4 RID: 40884
			public readonly int IntMaxValue;

			// Token: 0x04009FB5 RID: 40885
			public readonly bool BoolValue;

			// Token: 0x04009FB6 RID: 40886
			public readonly string StringNeedle;

			// Token: 0x02001B7D RID: 7037
			public enum EServerFilterType
			{
				// Token: 0x04009FB8 RID: 40888
				Any,
				// Token: 0x04009FB9 RID: 40889
				BoolValue,
				// Token: 0x04009FBA RID: 40890
				IntValue,
				// Token: 0x04009FBB RID: 40891
				IntNotValue,
				// Token: 0x04009FBC RID: 40892
				IntMin,
				// Token: 0x04009FBD RID: 40893
				IntMax,
				// Token: 0x04009FBE RID: 40894
				IntRange,
				// Token: 0x04009FBF RID: 40895
				StringValue,
				// Token: 0x04009FC0 RID: 40896
				StringContains
			}
		}
	}
}
