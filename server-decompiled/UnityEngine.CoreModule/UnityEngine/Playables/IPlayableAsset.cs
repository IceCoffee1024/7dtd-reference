using System;
using System.Collections.Generic;

namespace UnityEngine.Playables
{
	// Token: 0x0200049C RID: 1180
	public interface IPlayableAsset
	{
		// Token: 0x06002876 RID: 10358
		Playable CreatePlayable(PlayableGraph graph, GameObject owner);

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002877 RID: 10359
		double duration { get; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002878 RID: 10360
		IEnumerable<PlayableBinding> outputs { get; }
	}
}
