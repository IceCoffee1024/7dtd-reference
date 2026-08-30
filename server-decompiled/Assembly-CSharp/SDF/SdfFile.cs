using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x0200166B RID: 5739
	public class SdfFile
	{
		// Token: 0x0600B3CF RID: 46031 RVA: 0x004324FA File Offset: 0x004306FA
		public SdfFile(string path)
		{
			this.data = new SdfData();
			this.filePath = path;
		}

		// Token: 0x0600B3D0 RID: 46032 RVA: 0x00432514 File Offset: 0x00430714
		public void Load()
		{
			try
			{
				this.valuesChanged = false;
				if (SdFile.Exists(this.filePath))
				{
					using (Stream stream = SdFile.Open(this.filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						this.data.Nodes = SdfReader.Read(stream);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error opening SDF file: " + ex.Message);
			}
		}

		// Token: 0x0600B3D1 RID: 46033 RVA: 0x0043259C File Offset: 0x0043079C
		public void Save()
		{
			try
			{
				if (this.valuesChanged)
				{
					if (!SdDirectory.Exists(Path.GetDirectoryName(this.filePath)))
					{
						SdDirectory.CreateDirectory(Path.GetDirectoryName(this.filePath));
					}
					using (Stream stream = SdFile.Open(this.filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
					{
						SdfWriter.Write(stream, this.data.Nodes);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error opening SDF file:");
				Log.Exception(ex);
			}
		}

		// Token: 0x0600B3D2 RID: 46034 RVA: 0x00432634 File Offset: 0x00430834
		public void Set(string name, int val)
		{
			this.data.Add(new SdfInt(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B3D3 RID: 46035 RVA: 0x00432650 File Offset: 0x00430850
		public void Set(string name, float val)
		{
			this.data.Add(new SdfFloat(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B3D4 RID: 46036 RVA: 0x0043266C File Offset: 0x0043086C
		public void Set(string name, string val)
		{
			this.Set(name, val, false);
		}

		// Token: 0x0600B3D5 RID: 46037 RVA: 0x00432677 File Offset: 0x00430877
		public void Set(string name, string val, bool isBinary)
		{
			if (!isBinary)
			{
				this.data.Add(new SdfString(name, val));
			}
			else
			{
				this.data.Add(new SdfBinary(name, val));
			}
			this.valuesChanged = true;
		}

		// Token: 0x0600B3D6 RID: 46038 RVA: 0x004326AB File Offset: 0x004308AB
		public void Set(string name, byte[] byteArray)
		{
			this.data.Add(new SdfByteArray(name, byteArray));
		}

		// Token: 0x0600B3D7 RID: 46039 RVA: 0x004326C0 File Offset: 0x004308C0
		public void Set(string name, bool val)
		{
			this.data.Add(new SdfBool(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B3D8 RID: 46040 RVA: 0x004326DC File Offset: 0x004308DC
		public float? GetFloat(string name)
		{
			return this.data.GetFloat(name);
		}

		// Token: 0x0600B3D9 RID: 46041 RVA: 0x004326EA File Offset: 0x004308EA
		public int? GetInt(string name)
		{
			return this.data.GetInt(name);
		}

		// Token: 0x0600B3DA RID: 46042 RVA: 0x004326F8 File Offset: 0x004308F8
		public string GetString(string name)
		{
			return this.GetString(name, false);
		}

		// Token: 0x0600B3DB RID: 46043 RVA: 0x00432702 File Offset: 0x00430902
		public string GetString(string name, bool isBinary)
		{
			if (!isBinary)
			{
				return this.data.GetString(name);
			}
			return Utils.FromBase64(this.data.GetString(name));
		}

		// Token: 0x0600B3DC RID: 46044 RVA: 0x00432725 File Offset: 0x00430925
		public byte[] GetByteArray(string name)
		{
			return this.data.GetByteArray(name);
		}

		// Token: 0x0600B3DD RID: 46045 RVA: 0x00432733 File Offset: 0x00430933
		public bool? GetBool(string name)
		{
			return this.data.GetBool(name);
		}

		// Token: 0x0600B3DE RID: 46046 RVA: 0x00432741 File Offset: 0x00430941
		public void Remove(string name)
		{
			this.data.Remove(name);
			this.valuesChanged = true;
		}

		// Token: 0x0600B3DF RID: 46047 RVA: 0x00432758 File Offset: 0x00430958
		public string[] GetKeys()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B3E0 RID: 46048 RVA: 0x00432790 File Offset: 0x00430990
		public string[] GetStoredGamePrefs()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B3E1 RID: 46049 RVA: 0x004327C8 File Offset: 0x004309C8
		public void CopyFrom(SdfFile other)
		{
			foreach (KeyValuePair<string, SdfTag> keyValuePair in other.data.Nodes)
			{
				string text;
				SdfTag sdfTag;
				keyValuePair.Deconstruct(out text, out sdfTag);
				string text2 = text;
				SdfTag sdfTag2 = sdfTag;
				this.data.Nodes[text2] = sdfTag2;
			}
			this.valuesChanged = true;
		}

		// Token: 0x0400869D RID: 34461
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SdfData data;

		// Token: 0x0400869E RID: 34462
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string filePath;

		// Token: 0x0400869F RID: 34463
		[PublicizedFrom(EAccessModifier.Private)]
		public bool valuesChanged;
	}
}
