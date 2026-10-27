using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000A89 RID: 2697
public class SignData
{
	// Token: 0x06005118 RID: 20760 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData()
	{
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x001F1A1C File Offset: 0x001EFC1C
	public static SignData Duplicate(SignData original, string copyName = "")
	{
		SignData signData = new SignData();
		signData.guid = Guid.NewGuid();
		signData.name = (string.IsNullOrEmpty(copyName) ? original.name : copyName);
		signData.layers = null;
		signData.lastModified = DateTime.Now;
		signData.sharing = SignData.SignSharing.Private;
		signData.nextPolyId = original.nextPolyId;
		signData.nextGroupId = original.nextGroupId;
		signData.nextTextId = original.nextTextId;
		signData.nextNoiseId = original.nextNoiseId;
		signData.CloneLayersFrom(original);
		return signData;
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x001F1AA0 File Offset: 0x001EFCA0
	public void CloneLayersFrom(SignData source)
	{
		if (source.layers == null)
		{
			Log.Error("Failed to clone layers from source with null layers list.");
			return;
		}
		if (this.layers != null)
		{
			this.layers.Clear();
		}
		else
		{
			this.layers = new List<SignData.SignLayer>();
		}
		foreach (SignData.SignLayer signLayer in source.layers)
		{
			this.layers.Add(signLayer.Clone());
		}
		this.nextPolyId = source.nextPolyId;
		this.nextTextId = source.nextTextId;
		this.nextNoiseId = source.nextNoiseId;
		this.nextGroupId = source.nextGroupId;
		this.lastModified = DateTime.Now;
	}

	// Token: 0x0600511B RID: 20763 RVA: 0x001F1B6C File Offset: 0x001EFD6C
	public void Write(PooledBinaryWriter bw)
	{
		bw.Write(3);
		StreamUtils.Write(bw, this.guid);
		bw.Write(this.name);
		bw.Write(this.lastModified.ToUniversalTime().Ticks);
		bw.Write(this.nextPolyId);
		bw.Write(this.nextTextId);
		bw.Write(this.nextNoiseId);
		bw.Write(this.nextGroupId);
		bw.Write((byte)this.sharing);
		List<SignData.SignLayer> list = this.layers;
		bw.Write((list != null) ? list.Count : 0);
		if (this.layers != null)
		{
			foreach (SignData.SignLayer signLayer in this.layers)
			{
				signLayer.Write(bw);
			}
		}
	}

	// Token: 0x0600511C RID: 20764 RVA: 0x001F1C54 File Offset: 0x001EFE54
	public static SignData Read(PooledBinaryReader br, out int binaryVersion)
	{
		binaryVersion = br.ReadInt32();
		if (binaryVersion <= 0 || binaryVersion > 3)
		{
			throw new InvalidDataException(string.Format("Sign blob version {0} is not readable by this build, which writes version {1}.", binaryVersion, 3));
		}
		SignData signData = new SignData
		{
			guid = StreamUtils.ReadGuid(br),
			name = br.ReadString(),
			lastModified = new DateTime(br.ReadInt64(), DateTimeKind.Utc)
		};
		signData.nextPolyId = br.ReadInt32();
		signData.nextTextId = br.ReadInt32();
		signData.nextNoiseId = br.ReadInt32();
		signData.nextGroupId = br.ReadInt32();
		signData.sharing = (SignData.SignSharing)br.ReadByte();
		int num = br.ReadInt32();
		signData.layers = new List<SignData.SignLayer>(num);
		for (int i = 0; i < num; i++)
		{
			signData.layers.Add(SignData.SignLayer.Read(br));
		}
		return signData;
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x001F1D30 File Offset: 0x001EFF30
	public void WriteXml(XmlElement parentElement)
	{
		XmlElement xmlElement = parentElement.AddXmlElement("sign");
		xmlElement.SetAttrib("guid", this.guid.ToString());
		xmlElement.SetAttrib("name", this.name.ToString());
		xmlElement.SetAttrib("modified", this.lastModified.ToString("u", Utils.StandardCulture));
		if (this.sharing != SignData.SignSharing.Private)
		{
			xmlElement.SetAttrib("sharing", this.sharing.ToString());
		}
		xmlElement.SetAttribute("next_poly_id", this.nextPolyId.ToString());
		xmlElement.SetAttribute("next_text_id", this.nextTextId.ToString());
		xmlElement.SetAttribute("next_noise_id", this.nextNoiseId.ToString());
		xmlElement.SetAttribute("next_group_id", this.nextGroupId.ToString());
		foreach (SignData.SignLayer signLayer in this.layers)
		{
			signLayer.WriteXml(xmlElement.AddXmlElement("layer"));
		}
	}

	// Token: 0x0600511E RID: 20766 RVA: 0x001F1E6C File Offset: 0x001F006C
	public static SignData ReadXML(XElement signElement)
	{
		SignData.<>c__DisplayClass33_0 CS$<>8__locals1;
		CS$<>8__locals1.signElement = signElement;
		SignData signData = new SignData
		{
			guid = Guid.Parse(CS$<>8__locals1.signElement.GetAttribute("guid")),
			name = CS$<>8__locals1.signElement.GetAttribute("name"),
			lastModified = DateTime.ParseExact(CS$<>8__locals1.signElement.GetAttribute("modified"), "u", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
			layers = new List<SignData.SignLayer>()
		};
		string text;
		SignData.SignSharing signSharing;
		if (CS$<>8__locals1.signElement.TryGetAttribute("sharing", out text) && Enum.TryParse<SignData.SignSharing>(text, true, out signSharing))
		{
			signData.sharing = signSharing;
		}
		signData.nextPolyId = SignData.<ReadXML>g__ReadNextIdAttribute|33_0("next_poly_id", ref CS$<>8__locals1);
		signData.nextTextId = SignData.<ReadXML>g__ReadNextIdAttribute|33_0("next_text_id", ref CS$<>8__locals1);
		signData.nextNoiseId = SignData.<ReadXML>g__ReadNextIdAttribute|33_0("next_noise_id", ref CS$<>8__locals1);
		signData.nextGroupId = SignData.<ReadXML>g__ReadNextIdAttribute|33_0("next_group_id", ref CS$<>8__locals1);
		foreach (XElement xelement in CS$<>8__locals1.signElement.Elements("layer"))
		{
			signData.layers.Add(SignData.SignLayer.LayerFromXml(xelement));
		}
		return signData;
	}

	// Token: 0x0600511F RID: 20767 RVA: 0x001F1FD0 File Offset: 0x001F01D0
	public void SetLayerDefaultName(SignData.SignLayer layer)
	{
		SignData.PolygonSignLayer polygonSignLayer = layer as SignData.PolygonSignLayer;
		if (polygonSignLayer != null)
		{
			this.nextPolyId++;
			polygonSignLayer.SetDefaultName(this.nextPolyId);
			return;
		}
		SignData.TextSignLayer textSignLayer = layer as SignData.TextSignLayer;
		if (textSignLayer != null)
		{
			this.nextTextId++;
			textSignLayer.SetDefaultName(this.nextTextId);
			return;
		}
		SignData.NoiseSignLayer noiseSignLayer = layer as SignData.NoiseSignLayer;
		if (noiseSignLayer != null)
		{
			this.nextNoiseId++;
			noiseSignLayer.SetDefaultName(this.nextNoiseId);
			return;
		}
		SignData.GroupSignLayer groupSignLayer = layer as SignData.GroupSignLayer;
		if (groupSignLayer == null)
		{
			return;
		}
		this.nextGroupId++;
		groupSignLayer.SetDefaultName(this.nextGroupId);
	}

	// Token: 0x06005120 RID: 20768 RVA: 0x001F2074 File Offset: 0x001F0274
	public void UnpackGroups(bool recursive)
	{
		for (int i = this.layers.Count - 1; i >= 0; i--)
		{
			int num;
			this.TryUnpackTopLevelGroup(i, recursive, out num);
		}
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x001F20A4 File Offset: 0x001F02A4
	public bool TryUnpackTopLevelGroup(int groupIndex, bool recursive, out int unpackedLayerCount)
	{
		unpackedLayerCount = 0;
		SignData.GroupSignLayer groupSignLayer = this.layers[groupIndex] as SignData.GroupSignLayer;
		if (groupSignLayer != null)
		{
			this.layers.RemoveAt(groupIndex);
			this.UnpackLayers(groupSignLayer.transform, groupSignLayer.layers, this.layers, groupIndex, recursive, ref unpackedLayerCount);
			return true;
		}
		return false;
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x001F20F4 File Offset: 0x001F02F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnpackLayers(SignData.SignTransform parentTransform, List<SignData.SignLayer> sourceLayers, List<SignData.SignLayer> destLayers, int destIndex, bool recursive, ref int unpackedLayerCount)
	{
		int i = sourceLayers.Count - 1;
		while (i >= 0)
		{
			SignData.SignLayer signLayer = sourceLayers[i];
			sourceLayers.RemoveAt(i);
			if (!recursive)
			{
				goto IL_0049;
			}
			SignData.GroupSignLayer groupSignLayer = signLayer as SignData.GroupSignLayer;
			if (groupSignLayer == null)
			{
				goto IL_0049;
			}
			this.UnpackLayers(parentTransform * groupSignLayer.transform, groupSignLayer.layers, destLayers, destIndex, recursive, ref unpackedLayerCount);
			IL_006C:
			i--;
			continue;
			IL_0049:
			signLayer.transform = parentTransform * signLayer.transform;
			destLayers.Insert(destIndex, signLayer);
			unpackedLayerCount++;
			goto IL_006C;
		}
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x001F2178 File Offset: 0x001F0378
	public static SignData GetTestSignData(string text = "Hello World!", float hueOffset = 0f)
	{
		SignData signData = new SignData();
		signData.guid = Guid.NewGuid();
		signData.name = text;
		List<SignData.SignLayer> list = new List<SignData.SignLayer>();
		for (int i = 0; i < 20; i++)
		{
			float num = (float)i / 20f;
			float num2 = 8f + Mathf.Lerp(360f, 0f, num);
			float num3 = (Mathf.Lerp(1f, 0f, num) + hueOffset) % 1f;
			list.Add(new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, num2, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(num3, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.5f));
		}
		list.Add(new SignData.GroupSignLayer("Group Sign Layer", new Vector2(-1f, 0.3f), 8f, Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), SignData.DefaultSignData.PolygonGroupLayers, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply));
		list.Add(new SignData.GroupSignLayer("Group Sign Layer", new Vector2(-0.7f, 0.45f), 8f, 0.5f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), SignData.DefaultSignData.PolygonGroupLayers, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply));
		list.Add(new SignData.GroupSignLayer("Group Sign Layer", new Vector2(0.5f, -0.5f), 8f, 1.25f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), SignData.DefaultSignData.PolygonGroupLayers, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", new Vector2(0.025f, -0.025f), 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.black, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 1f, 0.25f));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.5f));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(0f + hueOffset, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.15f));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", new Vector2(-0.005f, 0.005f), 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(0.05f + hueOffset, 0.9f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 1f, -0.5f));
		signData.layers = new List<SignData.SignLayer>
		{
			new SignData.GroupSignLayer("Group Sign Layer", Vector2.zero, 0f, 0.55f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), list, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply)
		};
		signData.lastModified = DateTime.Now;
		return signData;
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x001F24EC File Offset: 0x001F06EC
	public static SignData GetBasicTestSignData(string text = "Hello World!", float hueOffset = 0f)
	{
		return new SignData
		{
			guid = Guid.NewGuid(),
			name = text,
			layers = new List<SignData.SignLayer>
			{
				new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.85f * Vector2.one, new SignData.SignRenderSettings(Color.HSVToRGB(0.58f + hueOffset, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 8, 0f, 0f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
				new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, 8f, 0.55f * SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(0.16f + hueOffset, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.15f)
			},
			lastModified = DateTime.Now
		};
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x001F25F4 File Offset: 0x001F07F4
	public static SignData GetErrorSignData()
	{
		return new SignData
		{
			guid = Guid.NewGuid(),
			name = "Error",
			layers = new List<SignData.SignLayer>
			{
				new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 3f * Vector2.one, new SignData.SignRenderSettings(new Color(1f, 0f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 4, 1f, 0f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal)
			},
			lastModified = DateTime.Now
		};
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001F2697 File Offset: 0x001F0897
	public static SignData GetNewSignData(string signName)
	{
		return new SignData
		{
			guid = Guid.NewGuid(),
			name = signName,
			layers = new List<SignData.SignLayer>(),
			lastModified = DateTime.Now
		};
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x001F26C6 File Offset: 0x001F08C6
	public static SignData GetDefaultSignData()
	{
		SignData testSignData = SignData.GetTestSignData("Hello World!", 0f);
		testSignData.guid = SignData.defaultSignDataID.signGuid;
		return testSignData;
	}

	// Token: 0x06005129 RID: 20777 RVA: 0x001F2700 File Offset: 0x001F0900
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <ReadXML>g__ReadNextIdAttribute|33_0(string attribute, ref SignData.<>c__DisplayClass33_0 A_1)
	{
		string text;
		if (A_1.signElement.TryGetAttribute(attribute, out text))
		{
			return StringParsers.ParseSInt32(text, 0, -1, NumberStyles.Integer);
		}
		return 0;
	}

	// Token: 0x04003E87 RID: 16007
	public const int cCurrentBinaryVersion = 3;

	// Token: 0x04003E88 RID: 16008
	public Guid guid;

	// Token: 0x04003E89 RID: 16009
	public string name;

	// Token: 0x04003E8A RID: 16010
	public List<SignData.SignLayer> layers;

	// Token: 0x04003E8B RID: 16011
	public DateTime lastModified;

	// Token: 0x04003E8C RID: 16012
	public SignData.SignSharing sharing;

	// Token: 0x04003E8D RID: 16013
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPolyId;

	// Token: 0x04003E8E RID: 16014
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextTextId;

	// Token: 0x04003E8F RID: 16015
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextNoiseId;

	// Token: 0x04003E90 RID: 16016
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextGroupId;

	// Token: 0x04003E91 RID: 16017
	public static readonly GlobalSignId defaultSignDataID = new GlobalSignId("[D]", Guid.Empty);

	// Token: 0x02000A8A RID: 2698
	public struct SignTransform : IEquatable<SignData.SignTransform>
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600512A RID: 20778 RVA: 0x001F272D File Offset: 0x001F092D
		public bool HasUniformScale
		{
			get
			{
				return Mathf.Approximately(this.scale.x, this.scale.y);
			}
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001F274A File Offset: 0x001F094A
		public SignTransform(Vector2 position, float rotation, Vector2 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001F2764 File Offset: 0x001F0964
		public static SignData.SignTransform operator *(SignData.SignTransform parent, SignData.SignTransform child)
		{
			float num = parent.rotation * 0.017453292f;
			Vector2 vector = Vector2.Scale(child.position, parent.scale);
			Vector2 vector2 = new Vector2(vector.x * Mathf.Cos(num) - vector.y * Mathf.Sin(num), vector.x * Mathf.Sin(num) + vector.y * Mathf.Cos(num));
			Vector2 vector3 = parent.position + vector2;
			float num2 = parent.rotation + child.rotation;
			Vector2 vector4 = Vector2.Scale(parent.scale, child.scale);
			return new SignData.SignTransform(vector3, num2, vector4);
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x001F2802 File Offset: 0x001F0A02
		public bool Equals(SignData.SignTransform other)
		{
			return this.position == other.position && Mathf.Approximately(this.rotation, other.rotation) && this.scale == other.scale;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x001F2840 File Offset: 0x001F0A40
		public override bool Equals(object obj)
		{
			if (obj is SignData.SignTransform)
			{
				SignData.SignTransform signTransform = (SignData.SignTransform)obj;
				return this.Equals(signTransform);
			}
			return false;
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001F2865 File Offset: 0x001F0A65
		public override int GetHashCode()
		{
			return ((17 * 23 + this.position.GetHashCode()) * 23 + this.rotation.GetHashCode()) * 23 + this.scale.GetHashCode();
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x001F28A2 File Offset: 0x001F0AA2
		public static bool operator ==(SignData.SignTransform left, SignData.SignTransform right)
		{
			return left.Equals(right);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001F28AC File Offset: 0x001F0AAC
		public static bool operator !=(SignData.SignTransform left, SignData.SignTransform right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001F28BC File Offset: 0x001F0ABC
		public void WriteXml(XmlElement element)
		{
			element.SetAttrib("pos", string.Format("{0}, {1}", this.position.x, this.position.y));
			element.SetAttrib("rot", this.rotation.ToCultureInvariantString());
			element.SetAttrib("scale", string.Format("{0}, {1}", this.scale.x, this.scale.y));
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x001F294C File Offset: 0x001F0B4C
		public void ReadXml(XElement element)
		{
			this.position = StringParsers.ParseVector2(element.GetAttribute("pos"));
			this.rotation = float.Parse(element.GetAttribute("rot"), Utils.StandardCulture);
			this.scale = StringParsers.ParseVector2(element.GetAttribute("scale"));
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001F29AF File Offset: 0x001F0BAF
		public void Write(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.position);
			bw.Write(this.rotation);
			StreamUtils.Write(bw, this.scale);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001F29D8 File Offset: 0x001F0BD8
		public static SignData.SignTransform Read(PooledBinaryReader br)
		{
			Vector2 vector = StreamUtils.ReadVector2(br);
			float num = br.ReadSingle();
			Vector2 vector2 = StreamUtils.ReadVector2(br);
			return new SignData.SignTransform(vector, num, vector2);
		}

		// Token: 0x04003E92 RID: 16018
		public Vector2 position;

		// Token: 0x04003E93 RID: 16019
		public float rotation;

		// Token: 0x04003E94 RID: 16020
		public Vector2 scale;

		// Token: 0x04003E95 RID: 16021
		public static readonly SignData.SignTransform Defaults = new SignData.SignTransform(Vector2.zero, 0f, new Vector2(0.3f, 0.3f));

		// Token: 0x04003E96 RID: 16022
		public static readonly SignData.SignTransform Identity = new SignData.SignTransform(Vector2.zero, 0f, Vector2.one);
	}

	// Token: 0x02000A8B RID: 2699
	public struct SignRenderSettings
	{
		// Token: 0x06005137 RID: 20791 RVA: 0x001F2A3E File Offset: 0x001F0C3E
		public SignRenderSettings(Color color, SignData.SignRenderSettings.Mode mode = SignData.SignRenderSettings.Mode.ColorOnly)
		{
			this.color = color;
			this.mode = mode;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x001F2A4E File Offset: 0x001F0C4E
		public void WriteXml(XmlElement element)
		{
			element.SetAttrib("color", "#" + ColorUtility.ToHtmlStringRGBA(this.color));
			element.SetAttrib("mode", this.mode.ToString());
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x001F2A90 File Offset: 0x001F0C90
		public void ReadXml(XElement element)
		{
			string text = (element.HasAttribute("tint") ? element.GetAttribute("tint") : element.GetAttribute("color"));
			if (!ColorUtility.TryParseHtmlString(text, out this.color))
			{
				Log.Error("Invalid color string: " + text);
				this.color = Color.white;
			}
			if (element.HasAttribute("mode"))
			{
				string attribute = element.GetAttribute("mode");
				if (!Enum.TryParse<SignData.SignRenderSettings.Mode>(attribute, true, out this.mode))
				{
					Log.Error("Failed to parse SignRenderSettings mode from string: \"" + attribute + "\"");
					this.mode = SignData.SignRenderSettings.Mode.ColorAndMask;
				}
			}
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001F2B49 File Offset: 0x001F0D49
		public void Write(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.color);
			bw.Write((byte)this.mode);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x001F2B64 File Offset: 0x001F0D64
		public static SignData.SignRenderSettings Read(PooledBinaryReader br)
		{
			Color color = StreamUtils.ReadColor(br);
			SignData.SignRenderSettings.Mode mode = (SignData.SignRenderSettings.Mode)br.ReadByte();
			return new SignData.SignRenderSettings(color, mode);
		}

		// Token: 0x04003E97 RID: 16023
		public Color color;

		// Token: 0x04003E98 RID: 16024
		public SignData.SignRenderSettings.Mode mode;

		// Token: 0x02000A8C RID: 2700
		public enum Mode : byte
		{
			// Token: 0x04003E9A RID: 16026
			ColorOnly,
			// Token: 0x04003E9B RID: 16027
			ColorAndMask,
			// Token: 0x04003E9C RID: 16028
			MaskOnly,
			// Token: 0x04003E9D RID: 16029
			PunchOut
		}
	}

	// Token: 0x02000A8D RID: 2701
	public abstract class SignWarp
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600513C RID: 20796
		public abstract SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x0600513D RID: 20797
		public abstract SignData.SignWarp Clone();

		// Token: 0x0600513E RID: 20798
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void ReadXml(XElement element);

		// Token: 0x0600513F RID: 20799
		public abstract void WriteXml(XmlElement xmlElement);

		// Token: 0x06005140 RID: 20800
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalRead(PooledBinaryReader br);

		// Token: 0x06005141 RID: 20801
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalWrite(PooledBinaryWriter bw);

		// Token: 0x06005142 RID: 20802 RVA: 0x001F2B84 File Offset: 0x001F0D84
		public void Write(PooledBinaryWriter bw)
		{
			bw.Write((byte)this.TypeId);
			this.InternalWrite(bw);
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x001F2B9C File Offset: 0x001F0D9C
		public static SignData.SignWarp Read(PooledBinaryReader br)
		{
			SignData.SignWarp.WarpType warpType = (SignData.SignWarp.WarpType)br.ReadByte();
			SignData.SignWarp signWarp;
			switch (warpType)
			{
			case SignData.SignWarp.WarpType.Skew:
				signWarp = new SignData.SkewWarp();
				break;
			case SignData.SignWarp.WarpType.Bulge:
				signWarp = new SignData.BulgeWarp();
				break;
			case SignData.SignWarp.WarpType.Twirl:
				signWarp = new SignData.TwirlWarp();
				break;
			case SignData.SignWarp.WarpType.Kaleido:
				signWarp = new SignData.KaleidoWarp();
				break;
			case SignData.SignWarp.WarpType.Perspective:
				signWarp = new SignData.PerspectiveWarp();
				break;
			case SignData.SignWarp.WarpType.Arc:
				signWarp = new SignData.ArcWarp();
				break;
			case SignData.SignWarp.WarpType.Stretch:
				signWarp = new SignData.StretchWarp();
				break;
			case SignData.SignWarp.WarpType.Grid:
				signWarp = new SignData.GridWarp();
				break;
			default:
				throw new Exception(string.Format("Unknown warp type id: {0}", (byte)warpType));
			}
			SignData.SignWarp signWarp2 = signWarp;
			signWarp2.InternalRead(br);
			return signWarp2;
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x001F2C38 File Offset: 0x001F0E38
		public static SignData.SignWarp WarpFromXml(XElement element)
		{
			string attribute = element.GetAttribute("type");
			uint num = <PrivateImplementationDetails>.ComputeStringHash(attribute);
			SignData.SignWarp signWarp;
			if (num <= 2239152927U)
			{
				if (num <= 812224709U)
				{
					if (num != 542699362U)
					{
						if (num == 812224709U)
						{
							if (attribute == "ArcWarp")
							{
								signWarp = new SignData.ArcWarp();
								goto IL_015C;
							}
						}
					}
					else if (attribute == "KaleidoWarp")
					{
						signWarp = new SignData.KaleidoWarp();
						goto IL_015C;
					}
				}
				else if (num != 1765275369U)
				{
					if (num == 2239152927U)
					{
						if (attribute == "PerspectiveWarp")
						{
							signWarp = new SignData.PerspectiveWarp();
							goto IL_015C;
						}
					}
				}
				else if (attribute == "SkewWarp")
				{
					signWarp = new SignData.SkewWarp();
					goto IL_015C;
				}
			}
			else if (num <= 3251058595U)
			{
				if (num != 2846920778U)
				{
					if (num == 3251058595U)
					{
						if (attribute == "TwirlWarp")
						{
							signWarp = new SignData.TwirlWarp();
							goto IL_015C;
						}
					}
				}
				else if (attribute == "BulgeWarp")
				{
					signWarp = new SignData.BulgeWarp();
					goto IL_015C;
				}
			}
			else if (num != 3544788227U)
			{
				if (num == 4106962192U)
				{
					if (attribute == "StretchWarp")
					{
						signWarp = new SignData.StretchWarp();
						goto IL_015C;
					}
				}
			}
			else if (attribute == "GridWarp")
			{
				signWarp = new SignData.GridWarp();
				goto IL_015C;
			}
			throw new Exception("Unknown warp type: " + attribute);
			IL_015C:
			SignData.SignWarp signWarp2 = signWarp;
			signWarp2.ReadXml(element);
			return signWarp2;
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SignWarp()
		{
		}

		// Token: 0x02000A8E RID: 2702
		public enum WarpType : byte
		{
			// Token: 0x04003E9F RID: 16031
			Skew,
			// Token: 0x04003EA0 RID: 16032
			Bulge,
			// Token: 0x04003EA1 RID: 16033
			Twirl,
			// Token: 0x04003EA2 RID: 16034
			Kaleido,
			// Token: 0x04003EA3 RID: 16035
			Perspective,
			// Token: 0x04003EA4 RID: 16036
			Arc,
			// Token: 0x04003EA5 RID: 16037
			Stretch,
			// Token: 0x04003EA6 RID: 16038
			Grid
		}
	}

	// Token: 0x02000A8F RID: 2703
	public class SkewWarp : SignData.SignWarp
	{
		// Token: 0x06005146 RID: 20806 RVA: 0x001F2DA9 File Offset: 0x001F0FA9
		public SkewWarp()
		{
			this.amount = new Vector2(0f, 0.1f);
			this.rotation = 0f;
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x001F2DD1 File Offset: 0x001F0FD1
		public SkewWarp(Vector2 amount, float rotation)
		{
			this.amount = amount;
			this.rotation = rotation;
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001F2DE7 File Offset: 0x001F0FE7
		public override SignData.SignWarp Clone()
		{
			return new SignData.SkewWarp(this.amount, this.rotation);
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x001F2DFC File Offset: 0x001F0FFC
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("amount", string.Format("{0}, {1}", this.amount.x, this.amount.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x001F2E76 File Offset: 0x001F1076
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.amount = StringParsers.ParseVector2(element.GetAttribute("amount"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x0600514B RID: 20811 RVA: 0x00010E62 File Offset: 0x0000F062
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Skew;
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001F2EB3 File Offset: 0x001F10B3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.amount);
			bw.Write(this.rotation);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001F2ECD File Offset: 0x001F10CD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.amount = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
		}

		// Token: 0x04003EA7 RID: 16039
		public Vector2 amount;

		// Token: 0x04003EA8 RID: 16040
		public float rotation;
	}

	// Token: 0x02000A90 RID: 2704
	public class BulgeWarp : SignData.SignWarp
	{
		// Token: 0x0600514E RID: 20814 RVA: 0x001F2EE7 File Offset: 0x001F10E7
		public BulgeWarp()
		{
			this.offset = Vector2.zero;
			this.amount = 2f;
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001F2F05 File Offset: 0x001F1105
		public BulgeWarp(Vector2 offset, float amount)
		{
			this.offset = offset;
			this.amount = amount;
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x001F2F1B File Offset: 0x001F111B
		public override SignData.SignWarp Clone()
		{
			return new SignData.BulgeWarp(this.offset, this.amount);
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001F2F30 File Offset: 0x001F1130
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("amount", this.amount.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x001F2FAA File Offset: 0x001F11AA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.amount = float.Parse(element.GetAttribute("amount"), Utils.StandardCulture);
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x00020119 File Offset: 0x0001E319
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Bulge;
			}
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x001F2FE7 File Offset: 0x001F11E7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.amount);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x001F3001 File Offset: 0x001F1201
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.amount = br.ReadSingle();
		}

		// Token: 0x04003EA9 RID: 16041
		public Vector2 offset;

		// Token: 0x04003EAA RID: 16042
		public float amount;
	}

	// Token: 0x02000A91 RID: 2705
	public class TwirlWarp : SignData.SignWarp
	{
		// Token: 0x06005156 RID: 20822 RVA: 0x001F301B File Offset: 0x001F121B
		public TwirlWarp()
		{
			this.offset = Vector2.zero;
			this.amount = 0.2f;
			this.frequency = 0.2f;
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001F3044 File Offset: 0x001F1244
		public TwirlWarp(Vector2 offset, float amount, float frequency)
		{
			this.offset = offset;
			this.amount = amount;
			this.frequency = frequency;
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001F3061 File Offset: 0x001F1261
		public override SignData.SignWarp Clone()
		{
			return new SignData.TwirlWarp(this.offset, this.amount, this.frequency);
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001F307C File Offset: 0x001F127C
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("amount", this.amount.ToCultureInvariantString() ?? "");
			element.SetAttrib("frequency", this.frequency.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001F3118 File Offset: 0x001F1318
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.amount = float.Parse(element.GetAttribute("amount"), Utils.StandardCulture);
			this.frequency = float.Parse(element.GetAttribute("frequency"), Utils.StandardCulture);
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600515B RID: 20827 RVA: 0x0002F260 File Offset: 0x0002D460
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Twirl;
			}
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001F3180 File Offset: 0x001F1380
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.amount);
			bw.Write(this.frequency);
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001F31A6 File Offset: 0x001F13A6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.amount = br.ReadSingle();
			this.frequency = br.ReadSingle();
		}

		// Token: 0x04003EAB RID: 16043
		public Vector2 offset;

		// Token: 0x04003EAC RID: 16044
		public float amount;

		// Token: 0x04003EAD RID: 16045
		public float frequency;
	}

	// Token: 0x02000A92 RID: 2706
	public class KaleidoWarp : SignData.SignWarp
	{
		// Token: 0x0600515E RID: 20830 RVA: 0x001F31CC File Offset: 0x001F13CC
		public KaleidoWarp()
		{
			this.offset = Vector2.zero;
			this.sides = 6;
			this.rotation = 0f;
			this.offsetScale = 1f;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001F31FC File Offset: 0x001F13FC
		public KaleidoWarp(Vector2 offset, int sides, float rotation, float offsetScale)
		{
			this.offset = offset;
			this.sides = sides;
			this.rotation = rotation;
			this.offsetScale = offsetScale;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001F3221 File Offset: 0x001F1421
		public override SignData.SignWarp Clone()
		{
			return new SignData.KaleidoWarp(this.offset, this.sides, this.rotation, this.offsetScale);
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001F3240 File Offset: 0x001F1440
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("sides", string.Format("{0}", this.sides));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("offsetScale", this.offsetScale.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x001F32FC File Offset: 0x001F14FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.sides = int.Parse(element.GetAttribute("sides"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.offsetScale = (element.HasAttribute("offsetScale") ? float.Parse(element.GetAttribute("offsetScale"), Utils.StandardCulture) : 1f);
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x00047076 File Offset: 0x00045276
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Kaleido;
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001F3398 File Offset: 0x001F1598
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.sides);
			bw.Write(this.rotation);
			bw.Write(this.offsetScale);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001F33CA File Offset: 0x001F15CA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.sides = br.ReadInt32();
			this.rotation = br.ReadSingle();
			this.offsetScale = br.ReadSingle();
		}

		// Token: 0x04003EAE RID: 16046
		public Vector2 offset;

		// Token: 0x04003EAF RID: 16047
		public int sides;

		// Token: 0x04003EB0 RID: 16048
		public float rotation;

		// Token: 0x04003EB1 RID: 16049
		public float offsetScale;
	}

	// Token: 0x02000A93 RID: 2707
	public class PerspectiveWarp : SignData.SignWarp
	{
		// Token: 0x06005166 RID: 20838 RVA: 0x001F33FC File Offset: 0x001F15FC
		public PerspectiveWarp()
		{
			this.rotation = new Vector3(0f, 30f, 0f);
			this.strength = 0.1f;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x001F3429 File Offset: 0x001F1629
		public PerspectiveWarp(Vector3 rotation, float strength)
		{
			this.rotation = rotation;
			this.strength = strength;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x001F343F File Offset: 0x001F163F
		public override SignData.SignWarp Clone()
		{
			return new SignData.PerspectiveWarp(this.rotation, this.strength);
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001F3454 File Offset: 0x001F1654
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("rotation", string.Format("{0}, {1}, {2}", this.rotation.x, this.rotation.y, this.rotation.z));
			element.SetAttrib("strength", this.strength.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001F34DE File Offset: 0x001F16DE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.rotation = StringParsers.ParseVector3(element.GetAttribute("rotation"), 0, -1);
			this.strength = float.Parse(element.GetAttribute("strength"), Utils.StandardCulture);
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600516B RID: 20843 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Perspective;
			}
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x001F351D File Offset: 0x001F171D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.rotation);
			bw.Write(this.strength);
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x001F3537 File Offset: 0x001F1737
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.rotation = StreamUtils.ReadVector3(br);
			this.strength = br.ReadSingle();
		}

		// Token: 0x04003EB2 RID: 16050
		public Vector3 rotation;

		// Token: 0x04003EB3 RID: 16051
		public float strength;
	}

	// Token: 0x02000A94 RID: 2708
	public class ArcWarp : SignData.SignWarp
	{
		// Token: 0x0600516E RID: 20846 RVA: 0x001F3551 File Offset: 0x001F1751
		public ArcWarp()
		{
			this.rotation = 0f;
			this.radius = 2f;
			this.width = 0.1f;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x001F357A File Offset: 0x001F177A
		public ArcWarp(float rotation, float radius, float width)
		{
			this.rotation = rotation;
			this.radius = radius;
			this.width = width;
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x001F3597 File Offset: 0x001F1797
		public override SignData.SignWarp Clone()
		{
			return new SignData.ArcWarp(this.rotation, this.radius, this.width);
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x001F35B0 File Offset: 0x001F17B0
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("radius", this.radius.ToCultureInvariantString() ?? "");
			element.SetAttrib("width", this.width.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x001F3634 File Offset: 0x001F1834
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.radius = float.Parse(element.GetAttribute("radius"), Utils.StandardCulture);
			this.width = float.Parse(element.GetAttribute("width"), Utils.StandardCulture);
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06005173 RID: 20851 RVA: 0x00080BF2 File Offset: 0x0007EDF2
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Arc;
			}
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x001F36A1 File Offset: 0x001F18A1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			bw.Write(this.rotation);
			bw.Write(this.radius);
			bw.Write(this.width);
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x001F36C7 File Offset: 0x001F18C7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.rotation = br.ReadSingle();
			this.radius = br.ReadSingle();
			this.width = br.ReadSingle();
		}

		// Token: 0x04003EB4 RID: 16052
		public float rotation;

		// Token: 0x04003EB5 RID: 16053
		public float radius;

		// Token: 0x04003EB6 RID: 16054
		public float width;
	}

	// Token: 0x02000A95 RID: 2709
	public class StretchWarp : SignData.SignWarp
	{
		// Token: 0x06005176 RID: 20854 RVA: 0x001F36F0 File Offset: 0x001F18F0
		public StretchWarp()
		{
			this.offset = new Vector2(0f, 0f);
			this.rotation = 0f;
			this.distance = 0f;
			this.width = 0f;
			this.exponent = 0f;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x001F3744 File Offset: 0x001F1944
		public StretchWarp(Vector2 offset, float rotation, float distance, float width, float exponent)
		{
			this.offset = offset;
			this.rotation = rotation;
			this.distance = distance;
			this.width = width;
			this.exponent = exponent;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x001F3771 File Offset: 0x001F1971
		public override SignData.SignWarp Clone()
		{
			return new SignData.StretchWarp(this.offset, this.rotation, this.distance, this.width, this.exponent);
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x001F3798 File Offset: 0x001F1998
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("distance", this.distance.ToCultureInvariantString() ?? "");
			element.SetAttrib("width", this.width.ToCultureInvariantString() ?? "");
			element.SetAttrib("exponent", this.exponent.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x001F3874 File Offset: 0x001F1A74
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.distance = float.Parse(element.GetAttribute("distance"), Utils.StandardCulture);
			this.width = float.Parse(element.GetAttribute("width"), Utils.StandardCulture);
			this.exponent = float.Parse(element.GetAttribute("exponent"), Utils.StandardCulture);
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x0600517B RID: 20859 RVA: 0x00081CD4 File Offset: 0x0007FED4
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Stretch;
			}
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x001F391C File Offset: 0x001F1B1C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.rotation);
			bw.Write(this.distance);
			bw.Write(this.width);
			bw.Write(this.exponent);
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001F395A File Offset: 0x001F1B5A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
			this.distance = br.ReadSingle();
			this.width = br.ReadSingle();
			this.exponent = br.ReadSingle();
		}

		// Token: 0x04003EB7 RID: 16055
		public Vector2 offset;

		// Token: 0x04003EB8 RID: 16056
		public float rotation;

		// Token: 0x04003EB9 RID: 16057
		public float distance;

		// Token: 0x04003EBA RID: 16058
		public float width;

		// Token: 0x04003EBB RID: 16059
		public float exponent;
	}

	// Token: 0x02000A96 RID: 2710
	public class GridWarp : SignData.SignWarp
	{
		// Token: 0x0600517E RID: 20862 RVA: 0x001F3998 File Offset: 0x001F1B98
		public GridWarp()
		{
			this.mode = SignData.GridWarp.Mode.Rectangle;
			this.offset = Vector2.zero;
			this.rotation = 0f;
			this.scale = 1f;
			this.aspect = 1f;
			this.shift = 0f;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x001F39E9 File Offset: 0x001F1BE9
		public GridWarp(SignData.GridWarp.Mode mode, Vector2 offset, float rotation, float scale, float aspect, float shift)
		{
			this.mode = mode;
			this.offset = offset;
			this.rotation = rotation;
			this.scale = scale;
			this.aspect = aspect;
			this.shift = shift;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x001F3A1E File Offset: 0x001F1C1E
		public override SignData.SignWarp Clone()
		{
			return new SignData.GridWarp(this.mode, this.offset, this.rotation, this.scale, this.aspect, this.shift);
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x001F3A4C File Offset: 0x001F1C4C
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("mode", string.Format("{0}", (int)this.mode));
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("scale", this.scale.ToCultureInvariantString() ?? "");
			element.SetAttrib("aspect", this.aspect.ToCultureInvariantString() ?? "");
			element.SetAttrib("shift", this.shift.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x001F3B48 File Offset: 0x001F1D48
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.mode = (SignData.GridWarp.Mode)int.Parse(element.GetAttribute("mode"));
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.scale = float.Parse(element.GetAttribute("scale"), Utils.StandardCulture);
			this.aspect = (element.HasAttribute("aspect") ? float.Parse(element.GetAttribute("aspect"), Utils.StandardCulture) : 1f);
			this.shift = (element.HasAttribute("shift") ? float.Parse(element.GetAttribute("shift"), Utils.StandardCulture) : 0f);
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06005183 RID: 20867 RVA: 0x00060491 File Offset: 0x0005E691
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Grid;
			}
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001F3C40 File Offset: 0x001F1E40
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			bw.Write((int)this.mode);
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.rotation);
			bw.Write(this.scale);
			bw.Write(this.aspect);
			bw.Write(this.shift);
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x001F3C98 File Offset: 0x001F1E98
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.mode = (SignData.GridWarp.Mode)br.ReadInt32();
			this.offset = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
			this.scale = br.ReadSingle();
			this.aspect = br.ReadSingle();
			this.shift = br.ReadSingle();
		}

		// Token: 0x04003EBC RID: 16060
		public SignData.GridWarp.Mode mode;

		// Token: 0x04003EBD RID: 16061
		public Vector2 offset;

		// Token: 0x04003EBE RID: 16062
		public float rotation;

		// Token: 0x04003EBF RID: 16063
		public float scale;

		// Token: 0x04003EC0 RID: 16064
		public float aspect;

		// Token: 0x04003EC1 RID: 16065
		public float shift;

		// Token: 0x02000A97 RID: 2711
		public enum Mode
		{
			// Token: 0x04003EC3 RID: 16067
			Column,
			// Token: 0x04003EC4 RID: 16068
			Rectangle,
			// Token: 0x04003EC5 RID: 16069
			Hex
		}
	}

	// Token: 0x02000A98 RID: 2712
	public abstract class SignLayer
	{
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06005186 RID: 20870 RVA: 0x001F3CED File Offset: 0x001F1EED
		public bool HasWarps
		{
			get
			{
				List<SignData.SignWarp> list = this.warps;
				return list != null && list.Count > 0;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06005187 RID: 20871 RVA: 0x001F3D03 File Offset: 0x001F1F03
		public bool HasTransform
		{
			get
			{
				return this.transform != SignData.SignTransform.Identity;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x001F3D15 File Offset: 0x001F1F15
		public bool HasTransformOrWarps
		{
			get
			{
				return this.HasTransform || this.HasWarps;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06005189 RID: 20873
		public abstract SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x0600518A RID: 20874
		public abstract string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x0600518B RID: 20875
		public abstract SignData.SignLayer Clone();

		// Token: 0x0600518C RID: 20876
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalRead(PooledBinaryReader br);

		// Token: 0x0600518D RID: 20877
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalWrite(PooledBinaryWriter bw);

		// Token: 0x0600518E RID: 20878 RVA: 0x001F3D28 File Offset: 0x001F1F28
		public SignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps)
		{
			this.name = name;
			this.transform.position = position;
			this.transform.rotation = rotation;
			this.transform.scale = scale;
			this.renderSettings = renderSettings;
			this.warps = warps;
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x001F3D78 File Offset: 0x001F1F78
		public SignLayer()
		{
			this.name = string.Empty;
			this.transform.position = Vector2.zero;
			this.transform.rotation = 0f;
			this.transform.scale = new Vector2(0.3f, 0.3f);
			this.renderSettings = new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly);
			this.warps = new List<SignData.SignWarp>();
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x001F3DEC File Offset: 0x001F1FEC
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<SignData.SignWarp> CloneWarps()
		{
			List<SignData.SignWarp> list = new List<SignData.SignWarp>();
			foreach (SignData.SignWarp signWarp in this.warps)
			{
				list.Add(signWarp.Clone());
			}
			return list;
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001F3E4C File Offset: 0x001F204C
		public void Write(PooledBinaryWriter bw)
		{
			bw.Write((byte)this.TypeId);
			this.InternalWrite(bw);
			bw.Write(this.name ?? "");
			this.transform.Write(bw);
			this.renderSettings.Write(bw);
			List<SignData.SignWarp> list = this.warps;
			bw.Write((list != null) ? list.Count : 0);
			if (this.warps != null)
			{
				foreach (SignData.SignWarp signWarp in this.warps)
				{
					signWarp.Write(bw);
				}
			}
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x001F3F00 File Offset: 0x001F2100
		public static SignData.SignLayer Read(PooledBinaryReader br)
		{
			SignData.SignLayer.LayerType layerType = (SignData.SignLayer.LayerType)br.ReadByte();
			SignData.SignLayer signLayer;
			switch (layerType)
			{
			case SignData.SignLayer.LayerType.Group:
				signLayer = new SignData.GroupSignLayer();
				break;
			case SignData.SignLayer.LayerType.Text:
				signLayer = new SignData.TextSignLayer();
				break;
			case SignData.SignLayer.LayerType.Polygon:
				signLayer = new SignData.PolygonSignLayer();
				break;
			case SignData.SignLayer.LayerType.Noise:
				signLayer = new SignData.NoiseSignLayer();
				break;
			default:
				throw new Exception(string.Format("Unknown layer type id: {0}", (byte)layerType));
			}
			SignData.SignLayer signLayer2 = signLayer;
			signLayer2.InternalRead(br);
			signLayer2.name = br.ReadString();
			signLayer2.transform = SignData.SignTransform.Read(br);
			signLayer2.renderSettings = SignData.SignRenderSettings.Read(br);
			int num = br.ReadInt32();
			signLayer2.warps = new List<SignData.SignWarp>(num);
			for (int i = 0; i < num; i++)
			{
				signLayer2.warps.Add(SignData.SignWarp.Read(br));
			}
			return signLayer2;
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x001F3FC4 File Offset: 0x001F21C4
		public static SignData.SignLayer LayerFromXml(XElement element)
		{
			string attribute = element.GetAttribute("type");
			SignData.SignLayer signLayer;
			if (!(attribute == "TextSignLayer"))
			{
				if (!(attribute == "PolygonSignLayer"))
				{
					if (!(attribute == "GroupSignLayer"))
					{
						if (!(attribute == "NoiseSignLayer"))
						{
							throw new Exception("Unknown layer type: " + attribute);
						}
						signLayer = new SignData.NoiseSignLayer();
					}
					else
					{
						signLayer = new SignData.GroupSignLayer();
					}
				}
				else
				{
					signLayer = new SignData.PolygonSignLayer();
				}
			}
			else
			{
				signLayer = new SignData.TextSignLayer();
			}
			SignData.SignLayer signLayer2 = signLayer;
			signLayer2.ReadXml(element);
			return signLayer2;
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x001F4054 File Offset: 0x001F2254
		public virtual void WriteXml(XmlElement element)
		{
			element.SetAttribute("name", this.name);
			element.SetAttrib("type", base.GetType().Name);
			this.transform.WriteXml(element);
			this.renderSettings.WriteXml(element);
			foreach (SignData.SignWarp signWarp in this.warps)
			{
				signWarp.WriteXml(element.AddXmlElement("warp"));
			}
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x001F40F0 File Offset: 0x001F22F0
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ReadXml(XElement element)
		{
			element.TryGetAttribute("name", out this.name);
			this.transform.ReadXml(element);
			this.renderSettings.ReadXml(element);
			foreach (XElement xelement in element.Elements("warp"))
			{
				this.warps.Add(SignData.SignWarp.WarpFromXml(xelement));
			}
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x001F4180 File Offset: 0x001F2380
		public void SetDefaultName(int idx)
		{
			this.name = string.Format(this.defaultName + " " + idx.ToString("00"), Array.Empty<object>());
		}

		// Token: 0x04003EC6 RID: 16070
		public string name;

		// Token: 0x04003EC7 RID: 16071
		public SignData.SignTransform transform;

		// Token: 0x04003EC8 RID: 16072
		public SignData.SignRenderSettings renderSettings;

		// Token: 0x04003EC9 RID: 16073
		public List<SignData.SignWarp> warps;

		// Token: 0x02000A99 RID: 2713
		public enum LayerType : byte
		{
			// Token: 0x04003ECB RID: 16075
			Group,
			// Token: 0x04003ECC RID: 16076
			Text,
			// Token: 0x04003ECD RID: 16077
			Polygon,
			// Token: 0x04003ECE RID: 16078
			Noise
		}
	}

	// Token: 0x02000A9A RID: 2714
	public class GroupSignLayer : SignData.SignLayer
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x001F41AE File Offset: 0x001F23AE
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Group";
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x00010E62 File Offset: 0x0000F062
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Group;
			}
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001F41B5 File Offset: 0x001F23B5
		public GroupSignLayer()
		{
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001F41BD File Offset: 0x001F23BD
		public GroupSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, List<SignData.SignLayer> layers, SignData.GroupSignLayer.OffsetTarget offsetTarget = SignData.GroupSignLayer.OffsetTarget.All, float softnessOffset = 0f, float dilateOffset = 0f, SignData.GroupSignLayer.ColorMode colorMode = SignData.GroupSignLayer.ColorMode.Multiply)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.layers = layers;
			this.offsetTarget = offsetTarget;
			this.softnessOffset = softnessOffset;
			this.dilateOffset = dilateOffset;
			this.colorMode = colorMode;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001F41F8 File Offset: 0x001F23F8
		public override SignData.SignLayer Clone()
		{
			List<SignData.SignLayer> list = null;
			if (this.layers != null)
			{
				list = new List<SignData.SignLayer>();
				foreach (SignData.SignLayer signLayer in this.layers)
				{
					list.Add(signLayer.Clone());
				}
			}
			return new SignData.GroupSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), list, this.offsetTarget, this.softnessOffset, this.dilateOffset, this.colorMode);
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001F42B4 File Offset: 0x001F24B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			bw.Write((byte)this.offsetTarget);
			bw.Write(this.softnessOffset);
			bw.Write(this.dilateOffset);
			bw.Write((byte)this.colorMode);
			List<SignData.SignLayer> list = this.layers;
			bw.Write((list != null) ? list.Count : 0);
			if (this.layers != null)
			{
				foreach (SignData.SignLayer signLayer in this.layers)
				{
					signLayer.Write(bw);
				}
			}
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001F4358 File Offset: 0x001F2558
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.offsetTarget = (SignData.GroupSignLayer.OffsetTarget)br.ReadByte();
			this.softnessOffset = br.ReadSingle();
			this.dilateOffset = br.ReadSingle();
			this.colorMode = (SignData.GroupSignLayer.ColorMode)br.ReadByte();
			int num = br.ReadInt32();
			this.layers = new List<SignData.SignLayer>(num);
			for (int i = 0; i < num; i++)
			{
				this.layers.Add(SignData.SignLayer.Read(br));
			}
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001F43C8 File Offset: 0x001F25C8
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			if (this.offsetTarget != SignData.GroupSignLayer.OffsetTarget.All)
			{
				element.SetAttrib("offsetTarget", this.offsetTarget.ToString());
			}
			if (this.softnessOffset != 0f)
			{
				element.SetAttrib("softnessOffset", this.softnessOffset.ToCultureInvariantString());
			}
			if (this.dilateOffset != 0f)
			{
				element.SetAttrib("dilateOffset", this.dilateOffset.ToCultureInvariantString());
			}
			if (this.colorMode != SignData.GroupSignLayer.ColorMode.Multiply)
			{
				element.SetAttrib("colorMode", this.colorMode.ToString());
			}
			foreach (SignData.SignLayer signLayer in this.layers)
			{
				signLayer.WriteXml(element.AddXmlElement("layer"));
			}
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x001F44BC File Offset: 0x001F26BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			string text;
			SignData.GroupSignLayer.OffsetTarget offsetTarget;
			if (element.TryGetAttribute("offsetTarget", out text) && Enum.TryParse<SignData.GroupSignLayer.OffsetTarget>(text, out offsetTarget))
			{
				this.offsetTarget = offsetTarget;
			}
			else
			{
				this.offsetTarget = SignData.GroupSignLayer.OffsetTarget.All;
			}
			string text2;
			if (element.TryGetAttribute("softnessOffset", out text2))
			{
				this.softnessOffset = float.Parse(text2, Utils.StandardCulture);
			}
			string text3;
			if (element.TryGetAttribute("dilateOffset", out text3))
			{
				this.dilateOffset = float.Parse(text3, Utils.StandardCulture);
			}
			string text4;
			SignData.GroupSignLayer.ColorMode colorMode;
			if (element.TryGetAttribute("colorMode", out text4) && Enum.TryParse<SignData.GroupSignLayer.ColorMode>(text4, out colorMode))
			{
				this.colorMode = colorMode;
			}
			else
			{
				this.colorMode = SignData.GroupSignLayer.ColorMode.Multiply;
			}
			this.layers = new List<SignData.SignLayer>();
			foreach (XElement xelement in element.Elements("layer"))
			{
				this.layers.Add(SignData.SignLayer.LayerFromXml(xelement));
			}
		}

		// Token: 0x04003ECF RID: 16079
		public List<SignData.SignLayer> layers;

		// Token: 0x04003ED0 RID: 16080
		public SignData.GroupSignLayer.OffsetTarget offsetTarget;

		// Token: 0x04003ED1 RID: 16081
		public float softnessOffset;

		// Token: 0x04003ED2 RID: 16082
		public float dilateOffset;

		// Token: 0x04003ED3 RID: 16083
		public SignData.GroupSignLayer.ColorMode colorMode;

		// Token: 0x02000A9B RID: 2715
		public enum OffsetTarget : byte
		{
			// Token: 0x04003ED5 RID: 16085
			All,
			// Token: 0x04003ED6 RID: 16086
			Shapes,
			// Token: 0x04003ED7 RID: 16087
			Text,
			// Token: 0x04003ED8 RID: 16088
			Noise
		}

		// Token: 0x02000A9C RID: 2716
		public enum ColorMode : byte
		{
			// Token: 0x04003EDA RID: 16090
			Multiply,
			// Token: 0x04003EDB RID: 16091
			Blend,
			// Token: 0x04003EDC RID: 16092
			Override
		}
	}

	// Token: 0x02000A9D RID: 2717
	public class TextSignLayer : SignData.SignLayer
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x060051A0 RID: 20896 RVA: 0x001F45E0 File Offset: 0x001F27E0
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Text";
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x00020119 File Offset: 0x0001E319
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Text;
			}
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001F45E8 File Offset: 0x001F27E8
		public TextSignLayer()
		{
			this.name = "Text";
			this.font = "LiberationSans";
			this.text = Localization.Get("lblSignLayerTypeText", false, null);
			this.direction = 0f;
			this.spacing = 1f;
			this.softness = 0f;
			this.dilate = 0f;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x001F4650 File Offset: 0x001F2850
		public TextSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, string font, string text, float direction, float spacing, float softness, float dilate)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.font = font;
			this.text = text;
			this.direction = direction;
			this.spacing = spacing;
			this.softness = softness;
			this.dilate = dilate;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x001F469C File Offset: 0x001F289C
		public override SignData.SignLayer Clone()
		{
			return new SignData.TextSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.font, this.text, this.direction, this.spacing, this.softness, this.dilate);
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001F4708 File Offset: 0x001F2908
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			bw.Write(this.font);
			bw.Write(this.text);
			bw.Write(this.direction);
			bw.Write(this.spacing);
			bw.Write(this.softness);
			bw.Write(this.dilate);
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x001F4760 File Offset: 0x001F2960
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.font = br.ReadString();
			this.text = br.ReadString();
			this.direction = br.ReadSingle();
			this.spacing = br.ReadSingle();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x001F47B8 File Offset: 0x001F29B8
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("font", this.font);
			element.SetAttrib("text", this.text);
			element.SetAttrib("direction", this.direction.ToCultureInvariantString());
			element.SetAttrib("spacing", this.spacing.ToCultureInvariantString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001F484C File Offset: 0x001F2A4C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			this.font = element.GetAttribute("font");
			this.text = element.GetAttribute("text");
			this.direction = float.Parse(element.GetAttribute("direction"), Utils.StandardCulture);
			this.spacing = float.Parse(element.GetAttribute("spacing"), Utils.StandardCulture);
			this.softness = float.Parse(element.GetAttribute("softness"), Utils.StandardCulture);
			this.dilate = float.Parse(element.GetAttribute("dilate"), Utils.StandardCulture);
		}

		// Token: 0x04003EDD RID: 16093
		public string font;

		// Token: 0x04003EDE RID: 16094
		public string text;

		// Token: 0x04003EDF RID: 16095
		public float direction;

		// Token: 0x04003EE0 RID: 16096
		public float spacing;

		// Token: 0x04003EE1 RID: 16097
		public float softness;

		// Token: 0x04003EE2 RID: 16098
		public float dilate;
	}

	// Token: 0x02000A9E RID: 2718
	public class PolygonSignLayer : SignData.SignLayer
	{
		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001F490C File Offset: 0x001F2B0C
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Poly";
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060051AA RID: 20906 RVA: 0x0002F260 File Offset: 0x0002D460
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Polygon;
			}
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001F4914 File Offset: 0x001F2B14
		public PolygonSignLayer()
		{
			this.name = "Polygon";
			this.sides = 4;
			this.smoothness = 0f;
			this.starify = 0f;
			this.softness = 0f;
			this.dilate = 0f;
			this.frequency = 5f;
			this.shapeMode = SignData.PolygonSignLayer.ShapeMode.Normal;
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x001F4978 File Offset: 0x001F2B78
		public PolygonSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, int sides, float smoothness, float starify, float softness, float dilate, float frequency, SignData.PolygonSignLayer.ShapeMode shapeMode)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.sides = sides;
			this.smoothness = smoothness;
			this.starify = starify;
			this.softness = softness;
			this.dilate = dilate;
			this.frequency = frequency;
			this.shapeMode = shapeMode;
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x001F49CC File Offset: 0x001F2BCC
		public override SignData.SignLayer Clone()
		{
			return new SignData.PolygonSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.sides, this.smoothness, this.starify, this.softness, this.dilate, this.frequency, this.shapeMode);
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x001F4A3C File Offset: 0x001F2C3C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			bw.Write(this.sides);
			bw.Write(this.smoothness);
			bw.Write(this.starify);
			bw.Write(this.softness);
			bw.Write(this.dilate);
			bw.Write(this.frequency);
			bw.Write((byte)this.shapeMode);
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001F4AA0 File Offset: 0x001F2CA0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.sides = br.ReadInt32();
			this.smoothness = br.ReadSingle();
			this.starify = br.ReadSingle();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
			this.frequency = br.ReadSingle();
			this.shapeMode = (SignData.PolygonSignLayer.ShapeMode)br.ReadByte();
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001F4B04 File Offset: 0x001F2D04
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("sides", this.sides.ToString());
			element.SetAttrib("smoothness", this.smoothness.ToCultureInvariantString());
			element.SetAttrib("starify", this.starify.ToCultureInvariantString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
			element.SetAttrib("frequency", this.frequency.ToCultureInvariantString());
			element.SetAttrib("shapeMode", this.shapeMode.ToString());
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001F4BC0 File Offset: 0x001F2DC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			this.sides = int.Parse(element.GetAttribute("sides"));
			this.smoothness = float.Parse(element.GetAttribute("smoothness"), Utils.StandardCulture);
			this.starify = float.Parse(element.GetAttribute("starify"), Utils.StandardCulture);
			string text;
			string text2;
			if (element.TryGetAttribute("softness", out text) && element.TryGetAttribute("dilate", out text2))
			{
				this.softness = float.Parse(text, Utils.StandardCulture);
				this.dilate = float.Parse(text2, Utils.StandardCulture);
			}
			else
			{
				this.softness = 0f;
				this.dilate = 0f;
			}
			string text3;
			if (element.TryGetAttribute("frequency", out text3))
			{
				this.frequency = float.Parse(text3, Utils.StandardCulture);
			}
			else
			{
				this.frequency = 5f;
			}
			string text4;
			SignData.PolygonSignLayer.ShapeMode shapeMode;
			if (element.TryGetAttribute("shapeMode", out text4) && Enum.TryParse<SignData.PolygonSignLayer.ShapeMode>(text4, out shapeMode))
			{
				this.shapeMode = shapeMode;
				return;
			}
			this.shapeMode = SignData.PolygonSignLayer.ShapeMode.Normal;
		}

		// Token: 0x04003EE3 RID: 16099
		public int sides;

		// Token: 0x04003EE4 RID: 16100
		public float smoothness;

		// Token: 0x04003EE5 RID: 16101
		public float starify;

		// Token: 0x04003EE6 RID: 16102
		public float softness;

		// Token: 0x04003EE7 RID: 16103
		public float dilate;

		// Token: 0x04003EE8 RID: 16104
		public float frequency;

		// Token: 0x04003EE9 RID: 16105
		public SignData.PolygonSignLayer.ShapeMode shapeMode;

		// Token: 0x02000A9F RID: 2719
		public enum ShapeMode : byte
		{
			// Token: 0x04003EEB RID: 16107
			Normal,
			// Token: 0x04003EEC RID: 16108
			Invert,
			// Token: 0x04003EED RID: 16109
			Line,
			// Token: 0x04003EEE RID: 16110
			Ripple
		}
	}

	// Token: 0x02000AA0 RID: 2720
	public class NoiseSignLayer : SignData.SignLayer
	{
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060051B2 RID: 20914 RVA: 0x001F4CF1 File Offset: 0x001F2EF1
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Noise";
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060051B3 RID: 20915 RVA: 0x00047076 File Offset: 0x00045276
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Noise;
			}
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001F4CF8 File Offset: 0x001F2EF8
		public NoiseSignLayer()
		{
			this.name = "Noise";
			this.seed = 0;
			this.detail = 4;
			this.softness = 0f;
			this.dilate = 0f;
			this.fade = 0f;
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x001F4D45 File Offset: 0x001F2F45
		public NoiseSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, int seed, int detail, float softness, float dilate, float fade)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.seed = seed;
			this.detail = detail;
			this.softness = softness;
			this.dilate = dilate;
			this.fade = fade;
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x001F4D80 File Offset: 0x001F2F80
		public override SignData.SignLayer Clone()
		{
			return new SignData.NoiseSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.seed, this.detail, this.softness, this.dilate, this.fade);
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001F4DE4 File Offset: 0x001F2FE4
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("seed", this.seed.ToString());
			element.SetAttrib("detail", this.detail.ToString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
			element.SetAttrib("fade", this.fade.ToCultureInvariantString());
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x001F4E6C File Offset: 0x001F306C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			this.seed = int.Parse(element.GetAttribute("seed"));
			this.detail = int.Parse(element.GetAttribute("detail"));
			this.softness = float.Parse(element.GetAttribute("softness"), Utils.StandardCulture);
			this.dilate = float.Parse(element.GetAttribute("dilate"), Utils.StandardCulture);
			string text;
			if (element.TryGetAttribute("fade", out text))
			{
				this.fade = float.Parse(text, Utils.StandardCulture);
				return;
			}
			this.fade = 0f;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x001F4F27 File Offset: 0x001F3127
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(PooledBinaryWriter bw)
		{
			bw.Write(this.seed);
			bw.Write(this.detail);
			bw.Write(this.softness);
			bw.Write(this.dilate);
			bw.Write(this.fade);
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001F4F65 File Offset: 0x001F3165
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(PooledBinaryReader br)
		{
			this.seed = br.ReadInt32();
			this.detail = br.ReadInt32();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
			this.fade = br.ReadSingle();
		}

		// Token: 0x04003EEF RID: 16111
		public int seed;

		// Token: 0x04003EF0 RID: 16112
		public int detail;

		// Token: 0x04003EF1 RID: 16113
		public float softness;

		// Token: 0x04003EF2 RID: 16114
		public float dilate;

		// Token: 0x04003EF3 RID: 16115
		public float fade;
	}

	// Token: 0x02000AA1 RID: 2721
	public enum SignSharing : byte
	{
		// Token: 0x04003EF5 RID: 16117
		Private,
		// Token: 0x04003EF6 RID: 16118
		Selectable,
		// Token: 0x04003EF7 RID: 16119
		Copyable
	}

	// Token: 0x02000AA2 RID: 2722
	[PublicizedFrom(EAccessModifier.Private)]
	public static class DefaultSignData
	{
		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060051BB RID: 20923 RVA: 0x001F4FA4 File Offset: 0x001F31A4
		public static List<SignData.SignLayer> PolygonGroupLayers
		{
			get
			{
				return new List<SignData.SignLayer>
				{
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero + new Vector2(0.005f, -0.005f), 0f, 0.25f * Vector2.one, new SignData.SignRenderSettings(Color.black, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.2f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.25f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.2f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.23f * Vector2.one, new SignData.SignRenderSettings(Color.HSVToRGB(0.05f, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.165f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.18f * Vector2.one, new SignData.SignRenderSettings(Color.HSVToRGB(0.13f, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.15f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal)
				};
			}
		}

		// Token: 0x04003EF8 RID: 16120
		public static readonly Color color1 = new Color(0.318f, 0.396f, 0.686f);

		// Token: 0x04003EF9 RID: 16121
		public static readonly Color color2 = new Color(0.286f, 0.518f, 1f);

		// Token: 0x04003EFA RID: 16122
		public static readonly Color color3 = new Color(0.514f, 0.671f, 1f);

		// Token: 0x04003EFB RID: 16123
		public static readonly Vector2 textSize = 0.65f * Vector2.one;

		// Token: 0x04003EFC RID: 16124
		public const float textRotation = 8f;

		// Token: 0x04003EFD RID: 16125
		public const string font1 = "LiberationSans";

		// Token: 0x04003EFE RID: 16126
		public const string font2 = "AmaticSC";

		// Token: 0x04003EFF RID: 16127
		public const string text = "Hello World!";

		// Token: 0x04003F00 RID: 16128
		public const int polygonSides = 5;

		// Token: 0x04003F01 RID: 16129
		public const float polygonSmoothness = 0.2f;

		// Token: 0x04003F02 RID: 16130
		public const float polygonStarify = 0.5f;
	}
}
