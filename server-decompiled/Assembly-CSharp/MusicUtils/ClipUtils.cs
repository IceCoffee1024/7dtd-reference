using System;
using System.Collections;
using UnityEngine;

namespace MusicUtils
{
	// Token: 0x02001A69 RID: 6761
	public static class ClipUtils
	{
		// Token: 0x0600CD62 RID: 52578 RVA: 0x004AD51B File Offset: 0x004AB71B
		public static AudioClip LoadClipImmediate(string _path)
		{
			return LoadManager.LoadAsset<AudioClip>(_path, null, null, false, true, false).Asset;
		}

		// Token: 0x0600CD63 RID: 52579 RVA: 0x004AD52D File Offset: 0x004AB72D
		public static IEnumerator LoadClipFrom(string _path, Func<AudioClip, float[]> _onAudioClipLoad, Action _onFinish)
		{
			LoadManager.AssetRequestTask<AudioClip> req = LoadManager.LoadAsset<AudioClip>(_path, null, null, false, false, false);
			yield return new WaitUntil(() => req.IsDone);
			if (req.Asset)
			{
				yield return ClipUtils.StripClip(req.Asset, _onAudioClipLoad(req.Asset));
				if (_onFinish != null)
				{
					_onFinish();
				}
			}
			yield break;
		}

		// Token: 0x0600CD64 RID: 52580 RVA: 0x004AD54A File Offset: 0x004AB74A
		public static IEnumerator StripClip(AudioClip _clip, float[] _data)
		{
			float[] buffer = MemoryPools.poolFloat.Alloc(44100);
			int cursor = 0;
			yield return null;
			while (cursor < _clip.samples)
			{
				_clip.GetData(buffer, cursor);
				yield return null;
				int num = 0;
				while (num < buffer.Length && cursor < _clip.samples)
				{
					_data[2 * cursor] = buffer[num];
					int num2 = 2;
					int num3 = cursor;
					cursor = num3 + 1;
					_data[num2 * num3 + 1] = buffer[num + 1];
					num += 2;
				}
				yield return null;
			}
			MemoryPools.poolFloat.Free(buffer);
			yield break;
		}
	}
}
