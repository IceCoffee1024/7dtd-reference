using System;
using System.Collections.Generic;

namespace SDF
{
	// Token: 0x020016E6 RID: 5862
	public class SdfData
	{
		// Token: 0x0600B7A6 RID: 47014 RVA: 0x0043F984 File Offset: 0x0043DB84
		public SdfData()
		{
			this.Nodes = new Dictionary<string, SdfTag>();
		}

		// Token: 0x0600B7A7 RID: 47015 RVA: 0x0043F998 File Offset: 0x0043DB98
		public bool Add(SdfTag sdfTag)
		{
			if (this.Nodes.ContainsKey(sdfTag.Name))
			{
				this.Nodes[sdfTag.Name].Value = sdfTag.Value;
			}
			else
			{
				this.Nodes.Add(sdfTag.Name, sdfTag);
			}
			return true;
		}

		// Token: 0x0600B7A8 RID: 47016 RVA: 0x0043F9E9 File Offset: 0x0043DBE9
		public bool Remove(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return false;
			}
			this.Nodes.Remove(tagName);
			return true;
		}

		// Token: 0x0600B7A9 RID: 47017 RVA: 0x0043FA0C File Offset: 0x0043DC0C
		public int? GetInt(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Int)
			{
				return null;
			}
			return new int?(Convert.ToInt32(this.Nodes[tagName].Value));
		}

		// Token: 0x0600B7AA RID: 47018 RVA: 0x0043FA6C File Offset: 0x0043DC6C
		public float? GetFloat(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Float)
			{
				return null;
			}
			return new float?((float)this.Nodes[tagName].Value);
		}

		// Token: 0x0600B7AB RID: 47019 RVA: 0x0043FACA File Offset: 0x0043DCCA
		public string GetString(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.String)
			{
				return null;
			}
			return this.Nodes[tagName].Value.ToString();
		}

		// Token: 0x0600B7AC RID: 47020 RVA: 0x0043FB08 File Offset: 0x0043DD08
		public bool? GetBool(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Bool)
			{
				return null;
			}
			return new bool?((bool)this.Nodes[tagName].Value);
		}

		// Token: 0x0600B7AD RID: 47021 RVA: 0x0043FB66 File Offset: 0x0043DD66
		public string GetBinary(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Binary)
			{
				return null;
			}
			return (string)this.Nodes[tagName].Value;
		}

		// Token: 0x0600B7AE RID: 47022 RVA: 0x0043FBA4 File Offset: 0x0043DDA4
		public byte[] GetByteArray(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				throw new KeyNotFoundException();
			}
			if (this.Nodes[tagName].TagType == SdfTagType.ByteArray)
			{
				throw new InvalidCastException();
			}
			return (byte[])this.Nodes[tagName].Value;
		}

		// Token: 0x04008937 RID: 35127
		public Dictionary<string, SdfTag> Nodes;
	}
}
