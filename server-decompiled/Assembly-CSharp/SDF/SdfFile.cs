using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x020016BD RID: 5821
	public class SdfFile
	{
		// Token: 0x0600B6A9 RID: 46761 RVA: 0x0043C792 File Offset: 0x0043A992
		public SdfFile(string path)
		{
			this.data = new SdfData();
			this.filePath = path;
		}

		// Token: 0x0600B6AA RID: 46762 RVA: 0x0043C7AC File Offset: 0x0043A9AC
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

		// Token: 0x0600B6AB RID: 46763 RVA: 0x0043C834 File Offset: 0x0043AA34
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

		// Token: 0x0600B6AC RID: 46764 RVA: 0x0043C8CC File Offset: 0x0043AACC
		public void Set(string name, int val)
		{
			this.data.Add(new SdfInt(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B6AD RID: 46765 RVA: 0x0043C8E8 File Offset: 0x0043AAE8
		public void Set(string name, float val)
		{
			this.data.Add(new SdfFloat(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B6AE RID: 46766 RVA: 0x0043C904 File Offset: 0x0043AB04
		public void Set(string name, string val)
		{
			this.Set(name, val, false);
		}

		// Token: 0x0600B6AF RID: 46767 RVA: 0x0043C90F File Offset: 0x0043AB0F
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

		// Token: 0x0600B6B0 RID: 46768 RVA: 0x0043C943 File Offset: 0x0043AB43
		public void Set(string name, byte[] byteArray)
		{
			this.data.Add(new SdfByteArray(name, byteArray));
		}

		// Token: 0x0600B6B1 RID: 46769 RVA: 0x0043C958 File Offset: 0x0043AB58
		public void Set(string name, bool val)
		{
			this.data.Add(new SdfBool(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B6B2 RID: 46770 RVA: 0x0043C974 File Offset: 0x0043AB74
		public float? GetFloat(string name)
		{
			return this.data.GetFloat(name);
		}

		// Token: 0x0600B6B3 RID: 46771 RVA: 0x0043C982 File Offset: 0x0043AB82
		public int? GetInt(string name)
		{
			return this.data.GetInt(name);
		}

		// Token: 0x0600B6B4 RID: 46772 RVA: 0x0043C990 File Offset: 0x0043AB90
		public string GetString(string name)
		{
			return this.GetString(name, false);
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x0043C99A File Offset: 0x0043AB9A
		public string GetString(string name, bool isBinary)
		{
			if (!isBinary)
			{
				return this.data.GetString(name);
			}
			return Utils.FromBase64(this.data.GetString(name));
		}

		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0043C9BD File Offset: 0x0043ABBD
		public byte[] GetByteArray(string name)
		{
			return this.data.GetByteArray(name);
		}

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x0043C9CB File Offset: 0x0043ABCB
		public bool? GetBool(string name)
		{
			return this.data.GetBool(name);
		}

		// Token: 0x0600B6B8 RID: 46776 RVA: 0x0043C9D9 File Offset: 0x0043ABD9
		public void Remove(string name)
		{
			this.data.Remove(name);
			this.valuesChanged = true;
		}

		// Token: 0x0600B6B9 RID: 46777 RVA: 0x0043C9F0 File Offset: 0x0043ABF0
		public string[] GetKeys()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B6BA RID: 46778 RVA: 0x0043CA28 File Offset: 0x0043AC28
		public string[] GetStoredGamePrefs()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B6BB RID: 46779 RVA: 0x0043CA60 File Offset: 0x0043AC60
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

		// Token: 0x0400887E RID: 34942
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SdfData data;

		// Token: 0x0400887F RID: 34943
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string filePath;

		// Token: 0x04008880 RID: 34944
		[PublicizedFrom(EAccessModifier.Private)]
		public bool valuesChanged;
	}
}
