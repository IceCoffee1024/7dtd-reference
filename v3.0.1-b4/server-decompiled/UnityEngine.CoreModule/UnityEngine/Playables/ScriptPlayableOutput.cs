using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x020004AD RID: 1197
	[RequiredByNativeCode]
	public struct ScriptPlayableOutput : IPlayableOutput
	{
		// Token: 0x060029CB RID: 10699 RVA: 0x00046DAC File Offset: 0x00044FAC
		public static ScriptPlayableOutput Create(PlayableGraph graph, string name)
		{
			PlayableOutputHandle playableOutputHandle;
			bool flag = !graph.CreateScriptOutputInternal(name, out playableOutputHandle);
			ScriptPlayableOutput scriptPlayableOutput;
			if (flag)
			{
				scriptPlayableOutput = ScriptPlayableOutput.Null;
			}
			else
			{
				scriptPlayableOutput = new ScriptPlayableOutput(playableOutputHandle);
			}
			return scriptPlayableOutput;
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x00046DE0 File Offset: 0x00044FE0
		internal ScriptPlayableOutput(PlayableOutputHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOutputOfType<ScriptPlayableOutput>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not a ScriptPlayableOutput.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x00046E1C File Offset: 0x0004501C
		public static ScriptPlayableOutput Null
		{
			get
			{
				return new ScriptPlayableOutput(PlayableOutputHandle.Null);
			}
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x00046E38 File Offset: 0x00045038
		public PlayableOutputHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x00046E50 File Offset: 0x00045050
		public static implicit operator PlayableOutput(ScriptPlayableOutput output)
		{
			return new PlayableOutput(output.GetHandle());
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x00046E70 File Offset: 0x00045070
		public static explicit operator ScriptPlayableOutput(PlayableOutput output)
		{
			return new ScriptPlayableOutput(output.GetHandle());
		}

		// Token: 0x04000F8E RID: 3982
		private PlayableOutputHandle m_Handle;
	}
}
