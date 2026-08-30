using System;

namespace UnityEngine.Playables
{
	// Token: 0x020004AC RID: 1196
	public static class ScriptPlayableBinding
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x00046D64 File Offset: 0x00044F64
		public static PlayableBinding Create(string name, Object key, Type type)
		{
			return PlayableBinding.CreateInternal(name, key, type, new PlayableBinding.CreateOutputMethod(ScriptPlayableBinding.CreateScriptOutput));
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x00046D8C File Offset: 0x00044F8C
		private static PlayableOutput CreateScriptOutput(PlayableGraph graph, string name)
		{
			return ScriptPlayableOutput.Create(graph, name);
		}
	}
}
