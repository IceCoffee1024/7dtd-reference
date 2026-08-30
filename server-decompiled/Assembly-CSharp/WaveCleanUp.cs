using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020003C5 RID: 965
public class WaveCleanUp : MonoBehaviour
{
	// Token: 0x06001D07 RID: 7431 RVA: 0x000AEBB3 File Offset: 0x000ACDB3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		base.StartCoroutine(this.FormatHeader());
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x000AEBC2 File Offset: 0x000ACDC2
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

	// Token: 0x06001D09 RID: 7433 RVA: 0x000AEBD4 File Offset: 0x000ACDD4
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

	// Token: 0x06001D0A RID: 7434 RVA: 0x000AEC21 File Offset: 0x000ACE21
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

	// Token: 0x040012EC RID: 4844
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject PrefabWaveCleanUp;

	// Token: 0x040012ED RID: 4845
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] ChunkID = Encoding.ASCII.GetBytes("RIFF");

	// Token: 0x040012EE RID: 4846
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] Format = Encoding.ASCII.GetBytes("WAVE");

	// Token: 0x040012EF RID: 4847
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] Subchunk1ID = Encoding.ASCII.GetBytes("fmt ");

	// Token: 0x040012F0 RID: 4848
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte[] Subchunk2ID = Encoding.ASCII.GetBytes("data");

	// Token: 0x040012F1 RID: 4849
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short AudioFormat = 1;

	// Token: 0x040012F2 RID: 4850
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int SampleRate = 44100;

	// Token: 0x040012F3 RID: 4851
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short Channels = 2;

	// Token: 0x040012F4 RID: 4852
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short BitsPerSample = 16;

	// Token: 0x040012F5 RID: 4853
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int ByteRate = WaveCleanUp.SampleRate * (int)WaveCleanUp.Channels * (int)WaveCleanUp.BitsPerSample / 8;

	// Token: 0x040012F6 RID: 4854
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static short BlockAlign = WaveCleanUp.Channels * WaveCleanUp.BitsPerSample / 8;

	// Token: 0x040012F7 RID: 4855
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int Subchunk1Size = 16;

	// Token: 0x040012F8 RID: 4856
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool IsFinished;

	// Token: 0x040012F9 RID: 4857
	public string FilePath;
}
