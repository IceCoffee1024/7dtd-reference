using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020003CA RID: 970
public class WaveCleanUp : MonoBehaviour
{
	// Token: 0x06001D03 RID: 7427 RVA: 0x000AEFCF File Offset: 0x000AD1CF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		base.StartCoroutine(this.FormatHeader());
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x000AEFDE File Offset: 0x000AD1DE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator FormatHeader()
	{
		yield return new WaitUntil(() => this.FilePath != null);
		UnityWebRequest getAudioFile = UnityWebRequestMultimedia.GetAudioClip("file://" + this.FilePath, AudioType.WAV);
		getAudioFile.disposeDownloadHandlerOnDispose = true;
		yield return getAudioFile.SendWebRequest();
		if (getAudioFile.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.Log(getAudioFile.error);
		}
		else
		{
			AudioClip content = DownloadHandlerAudioClip.GetContent(getAudioFile);
			float[] array = new float[content.samples * content.channels];
			content.GetData(array, 0);
			byte[] array2 = WaveCleanUp.PCMDataToByteArray(array);
			int num = content.samples * (int)WaveCleanUp.Channels * (int)WaveCleanUp.BitsPerSample / 8;
			int num2 = 36 + num;
			using (Stream stream = SdFile.Open(this.FilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				stream.Write(WaveCleanUp.ChunkID, 0, 4);
				stream.Write(BitConverter.GetBytes(num2), 0, 4);
				stream.Write(WaveCleanUp.Format, 0, 4);
				stream.Write(WaveCleanUp.Subchunk1ID, 0, 4);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.Subchunk1Size), 0, 4);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.AudioFormat), 0, 2);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.Channels), 0, 2);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.SampleRate), 0, 4);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.ByteRate), 0, 4);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.BlockAlign), 0, 2);
				stream.Write(BitConverter.GetBytes(WaveCleanUp.BitsPerSample), 0, 2);
				stream.Write(WaveCleanUp.Subchunk2ID, 0, 4);
				stream.Write(BitConverter.GetBytes(num), 0, 4);
				stream.Write(array2, 0, array2.Length);
			}
		}
		getAudioFile.Dispose();
		Log.Out("Cleaned up: " + this.FilePath);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x000AEFF0 File Offset: 0x000AD1F0
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] PCMDataToByteArray(float[] _pcmData)
	{
		byte[] array = new byte[2 * _pcmData.Length];
		for (int i = 0; i < _pcmData.Length; i++)
		{
			byte[] bytes = BitConverter.GetBytes((short)(_pcmData[i] * 32767f));
			for (int j = 0; j < 2; j++)
			{
				array[2 * i + j] = bytes[j];
			}
		}
		return array;
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x000AF03D File Offset: 0x000AD23D
	public static GameObject Create()
	{
		if (WaveCleanUp.PrefabWaveCleanUp == null)
		{
			WaveCleanUp.PrefabWaveCleanUp = Resources.Load<GameObject>("Prefabs/prefabDMSWaveCleanup");
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(WaveCleanUp.PrefabWaveCleanUp);
		gameObject.name = "WaveCleanUp";
		return gameObject;
	}

	// Token: 0x040012F5 RID: 4853
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject PrefabWaveCleanUp;

	// Token: 0x040012F6 RID: 4854
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] ChunkID = Encoding.ASCII.GetBytes("RIFF");

	// Token: 0x040012F7 RID: 4855
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] Format = Encoding.ASCII.GetBytes("WAVE");

	// Token: 0x040012F8 RID: 4856
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] Subchunk1ID = Encoding.ASCII.GetBytes("fmt ");

	// Token: 0x040012F9 RID: 4857
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] Subchunk2ID = Encoding.ASCII.GetBytes("data");

	// Token: 0x040012FA RID: 4858
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short AudioFormat = 1;

	// Token: 0x040012FB RID: 4859
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int SampleRate = 44100;

	// Token: 0x040012FC RID: 4860
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short Channels = 2;

	// Token: 0x040012FD RID: 4861
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short BitsPerSample = 16;

	// Token: 0x040012FE RID: 4862
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int ByteRate = WaveCleanUp.SampleRate * (int)WaveCleanUp.Channels * (int)WaveCleanUp.BitsPerSample / 8;

	// Token: 0x040012FF RID: 4863
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short BlockAlign = WaveCleanUp.Channels * WaveCleanUp.BitsPerSample / 8;

	// Token: 0x04001300 RID: 4864
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int Subchunk1Size = 16;

	// Token: 0x04001301 RID: 4865
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool IsFinished;

	// Token: 0x04001302 RID: 4866
	public string FilePath;
}
