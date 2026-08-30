using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000A71 RID: 2673
public class SignData
{
	// Token: 0x06005100 RID: 20736 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData()
	{
	}

	// Token: 0x06005101 RID: 20737 RVA: 0x001EE9C4 File Offset: 0x001ECBC4
	public static SignData Duplicate(SignData original, string copyName = "")
	{
		SignData signData = new SignData();
		signData.guid = Guid.NewGuid();
		signData.name = (string.IsNullOrEmpty(copyName) ? original.name : copyName);
		signData.layers = null;
		signData.lastModified = DateTime.Now;
		signData.nextPolyId = original.nextPolyId;
		signData.nextGroupId = original.nextGroupId;
		signData.nextTextId = original.nextTextId;
		signData.nextNoiseId = original.nextNoiseId;
		signData.CloneLayersFrom(original);
		return signData;
	}

	// Token: 0x06005102 RID: 20738 RVA: 0x001EEA44 File Offset: 0x001ECC44
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

	// Token: 0x06005103 RID: 20739 RVA: 0x001EEB10 File Offset: 0x001ECD10
	public void Write(BinaryWriter bw)
	{
		StreamUtils.Write(bw, this.guid);
		bw.Write(this.name);
		bw.Write(this.lastModified.ToUniversalTime().Ticks);
		bw.Write(this.nextPolyId);
		bw.Write(this.nextTextId);
		bw.Write(this.nextNoiseId);
		bw.Write(this.nextGroupId);
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

	// Token: 0x06005104 RID: 20740 RVA: 0x001EEBE4 File Offset: 0x001ECDE4
	public static SignData Read(BinaryReader br)
	{
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
		int num = br.ReadInt32();
		signData.layers = new List<SignData.SignLayer>(num);
		for (int i = 0; i < num; i++)
		{
			signData.layers.Add(SignData.SignLayer.Read(br));
		}
		return signData;
	}

	// Token: 0x06005105 RID: 20741 RVA: 0x001EEC84 File Offset: 0x001ECE84
	public void WriteXml(XmlElement parentElement)
	{
		XmlElement xmlElement = parentElement.AddXmlElement("sign");
		xmlElement.SetAttrib("guid", this.guid.ToString());
		xmlElement.SetAttrib("name", this.name.ToString());
		xmlElement.SetAttrib("modified", this.lastModified.ToString("u", Utils.StandardCulture));
		xmlElement.SetAttribute("next_poly_id", this.nextPolyId.ToString());
		xmlElement.SetAttribute("next_text_id", this.nextTextId.ToString());
		xmlElement.SetAttribute("next_noise_id", this.nextNoiseId.ToString());
		xmlElement.SetAttribute("next_group_id", this.nextGroupId.ToString());
		foreach (SignData.SignLayer signLayer in this.layers)
		{
			signLayer.WriteXml(xmlElement.AddXmlElement("layer"));
		}
	}

	// Token: 0x06005106 RID: 20742 RVA: 0x001EED98 File Offset: 0x001ECF98
	public static SignData ReadXML(XElement signElement)
	{
		SignData.<>c__DisplayClass30_0 CS$<>8__locals1;
		CS$<>8__locals1.signElement = signElement;
		SignData signData = new SignData
		{
			guid = Guid.Parse(CS$<>8__locals1.signElement.GetAttribute("guid")),
			name = CS$<>8__locals1.signElement.GetAttribute("name"),
			lastModified = DateTime.ParseExact(CS$<>8__locals1.signElement.GetAttribute("modified"), "u", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
			layers = new List<SignData.SignLayer>()
		};
		signData.nextPolyId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_poly_id", ref CS$<>8__locals1);
		signData.nextTextId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_text_id", ref CS$<>8__locals1);
		signData.nextNoiseId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_noise_id", ref CS$<>8__locals1);
		signData.nextGroupId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_group_id", ref CS$<>8__locals1);
		foreach (XElement xelement in CS$<>8__locals1.signElement.Elements("layer"))
		{
			signData.layers.Add(SignData.SignLayer.LayerFromXml(xelement));
		}
		return signData;
	}

	// Token: 0x06005107 RID: 20743 RVA: 0x001EEEC8 File Offset: 0x001ED0C8
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

	// Token: 0x06005108 RID: 20744 RVA: 0x001EEF6C File Offset: 0x001ED16C
	public void UnpackGroups(bool recursive)
	{
		for (int i = this.layers.Count - 1; i >= 0; i--)
		{
			int num;
			this.TryUnpackTopLevelGroup(i, recursive, out num);
		}
	}

	// Token: 0x06005109 RID: 20745 RVA: 0x001EEF9C File Offset: 0x001ED19C
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

	// Token: 0x0600510A RID: 20746 RVA: 0x001EEFEC File Offset: 0x001ED1EC
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

	// Token: 0x0600510B RID: 20747 RVA: 0x001EF070 File Offset: 0x001ED270
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

	// Token: 0x0600510C RID: 20748 RVA: 0x001EF3E4 File Offset: 0x001ED5E4
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

	// Token: 0x0600510D RID: 20749 RVA: 0x001EF4EC File Offset: 0x001ED6EC
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

	// Token: 0x0600510E RID: 20750 RVA: 0x001EF58F File Offset: 0x001ED78F
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

	// Token: 0x0600510F RID: 20751 RVA: 0x001EF5BE File Offset: 0x001ED7BE
	public static SignData GetDefaultSignData()
	{
		SignData testSignData = SignData.GetTestSignData("Hello World!", 0f);
		testSignData.guid = SignData.defaultSignDataID.signGuid;
		return testSignData;
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001EF5F8 File Offset: 0x001ED7F8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <ReadXML>g__ReadNextIdAttribute|30_0(string attribute, ref SignData.<>c__DisplayClass30_0 A_1)
	{
		string text;
		if (A_1.signElement.TryGetAttribute(attribute, out text))
		{
			return StringParsers.ParseSInt32(text, 0, -1, NumberStyles.Integer);
		}
		return 0;
	}

	// Token: 0x04003E41 RID: 15937
	public Guid guid;

	// Token: 0x04003E42 RID: 15938
	public string name;

	// Token: 0x04003E43 RID: 15939
	public List<SignData.SignLayer> layers;

	// Token: 0x04003E44 RID: 15940
	public DateTime lastModified;

	// Token: 0x04003E45 RID: 15941
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPolyId;

	// Token: 0x04003E46 RID: 15942
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextTextId;

	// Token: 0x04003E47 RID: 15943
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextNoiseId;

	// Token: 0x04003E48 RID: 15944
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextGroupId;

	// Token: 0x04003E49 RID: 15945
	public static readonly GlobalSignId defaultSignDataID = new GlobalSignId("[D]", Guid.Empty);

	// Token: 0x02000A72 RID: 2674
	public struct SignTransform : IEquatable<SignData.SignTransform>
	{
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06005112 RID: 20754 RVA: 0x001EF625 File Offset: 0x001ED825
		public bool HasUniformScale
		{
			get
			{
				return Mathf.Approximately(this.scale.x, this.scale.y);
			}
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x001EF642 File Offset: 0x001ED842
		public SignTransform(Vector2 position, float rotation, Vector2 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x001EF65C File Offset: 0x001ED85C
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

		// Token: 0x06005115 RID: 20757 RVA: 0x001EF6FA File Offset: 0x001ED8FA
		public bool Equals(SignData.SignTransform other)
		{
			return this.position == other.position && Mathf.Approximately(this.rotation, other.rotation) && this.scale == other.scale;
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001EF738 File Offset: 0x001ED938
		public override bool Equals(object obj)
		{
			if (obj is SignData.SignTransform)
			{
				SignData.SignTransform signTransform = (SignData.SignTransform)obj;
				return this.Equals(signTransform);
			}
			return false;
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x001EF75D File Offset: 0x001ED95D
		public override int GetHashCode()
		{
			return ((17 * 23 + this.position.GetHashCode()) * 23 + this.rotation.GetHashCode()) * 23 + this.scale.GetHashCode();
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001EF79A File Offset: 0x001ED99A
		public static bool operator ==(SignData.SignTransform left, SignData.SignTransform right)
		{
			return left.Equals(right);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001EF7A4 File Offset: 0x001ED9A4
		public static bool operator !=(SignData.SignTransform left, SignData.SignTransform right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x001EF7B4 File Offset: 0x001ED9B4
		public void WriteXml(XmlElement element)
		{
			element.SetAttrib("pos", string.Format("{0}, {1}", this.position.x, this.position.y));
			element.SetAttrib("rot", this.rotation.ToCultureInvariantString());
			element.SetAttrib("scale", string.Format("{0}, {1}", this.scale.x, this.scale.y));
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001EF844 File Offset: 0x001EDA44
		public void ReadXml(XElement element)
		{
			this.position = StringParsers.ParseVector2(element.GetAttribute("pos"));
			this.rotation = float.Parse(element.GetAttribute("rot"), Utils.StandardCulture);
			this.scale = StringParsers.ParseVector2(element.GetAttribute("scale"));
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x001EF8A7 File Offset: 0x001EDAA7
		public void Write(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.position);
			bw.Write(this.rotation);
			StreamUtils.Write(bw, this.scale);
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x001EF8D0 File Offset: 0x001EDAD0
		public static SignData.SignTransform Read(BinaryReader br)
		{
			Vector2 vector = StreamUtils.ReadVector2(br);
			float num = br.ReadSingle();
			Vector2 vector2 = StreamUtils.ReadVector2(br);
			return new SignData.SignTransform(vector, num, vector2);
		}

		// Token: 0x04003E4A RID: 15946
		public Vector2 position;

		// Token: 0x04003E4B RID: 15947
		public float rotation;

		// Token: 0x04003E4C RID: 15948
		public Vector2 scale;

		// Token: 0x04003E4D RID: 15949
		public static readonly SignData.SignTransform Defaults = new SignData.SignTransform(Vector2.zero, 0f, new Vector2(0.3f, 0.3f));

		// Token: 0x04003E4E RID: 15950
		public static readonly SignData.SignTransform Identity = new SignData.SignTransform(Vector2.zero, 0f, Vector2.one);
	}

	// Token: 0x02000A73 RID: 2675
	public struct SignRenderSettings
	{
		// Token: 0x0600511F RID: 20767 RVA: 0x001EF936 File Offset: 0x001EDB36
		public SignRenderSettings(Color color, SignData.SignRenderSettings.Mode mode = SignData.SignRenderSettings.Mode.ColorOnly)
		{
			this.color = color;
			this.mode = mode;
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x001EF946 File Offset: 0x001EDB46
		public void WriteXml(XmlElement element)
		{
			element.SetAttrib("color", "#" + ColorUtility.ToHtmlStringRGBA(this.color));
			element.SetAttrib("mode", this.mode.ToString());
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x001EF988 File Offset: 0x001EDB88
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

		// Token: 0x06005122 RID: 20770 RVA: 0x001EFA41 File Offset: 0x001EDC41
		public void Write(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.color);
			bw.Write((byte)this.mode);
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x001EFA5C File Offset: 0x001EDC5C
		public static SignData.SignRenderSettings Read(BinaryReader br)
		{
			Color color = StreamUtils.ReadColor(br);
			SignData.SignRenderSettings.Mode mode = (SignData.SignRenderSettings.Mode)br.ReadByte();
			return new SignData.SignRenderSettings(color, mode);
		}

		// Token: 0x04003E4F RID: 15951
		public Color color;

		// Token: 0x04003E50 RID: 15952
		public SignData.SignRenderSettings.Mode mode;

		// Token: 0x02000A74 RID: 2676
		public enum Mode : byte
		{
			// Token: 0x04003E52 RID: 15954
			ColorOnly,
			// Token: 0x04003E53 RID: 15955
			ColorAndMask,
			// Token: 0x04003E54 RID: 15956
			MaskOnly,
			// Token: 0x04003E55 RID: 15957
			PunchOut
		}
	}

	// Token: 0x02000A75 RID: 2677
	public abstract class SignWarp
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06005124 RID: 20772
		public abstract SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x06005125 RID: 20773
		public abstract SignData.SignWarp Clone();

		// Token: 0x06005126 RID: 20774
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void ReadXml(XElement element);

		// Token: 0x06005127 RID: 20775
		public abstract void WriteXml(XmlElement xmlElement);

		// Token: 0x06005128 RID: 20776
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalRead(BinaryReader br);

		// Token: 0x06005129 RID: 20777
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalWrite(BinaryWriter bw);

		// Token: 0x0600512A RID: 20778 RVA: 0x001EFA7C File Offset: 0x001EDC7C
		public void Write(BinaryWriter bw)
		{
			bw.Write((byte)this.TypeId);
			this.InternalWrite(bw);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001EFA94 File Offset: 0x001EDC94
		public static SignData.SignWarp Read(BinaryReader br)
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

		// Token: 0x0600512C RID: 20780 RVA: 0x001EFB30 File Offset: 0x001EDD30
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

		// Token: 0x0600512D RID: 20781 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SignWarp()
		{
		}

		// Token: 0x02000A76 RID: 2678
		public enum WarpType : byte
		{
			// Token: 0x04003E57 RID: 15959
			Skew,
			// Token: 0x04003E58 RID: 15960
			Bulge,
			// Token: 0x04003E59 RID: 15961
			Twirl,
			// Token: 0x04003E5A RID: 15962
			Kaleido,
			// Token: 0x04003E5B RID: 15963
			Perspective,
			// Token: 0x04003E5C RID: 15964
			Arc,
			// Token: 0x04003E5D RID: 15965
			Stretch,
			// Token: 0x04003E5E RID: 15966
			Grid
		}
	}

	// Token: 0x02000A77 RID: 2679
	public class SkewWarp : SignData.SignWarp
	{
		// Token: 0x0600512E RID: 20782 RVA: 0x001EFCA1 File Offset: 0x001EDEA1
		public SkewWarp()
		{
			this.amount = new Vector2(0f, 0.1f);
			this.rotation = 0f;
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001EFCC9 File Offset: 0x001EDEC9
		public SkewWarp(Vector2 amount, float rotation)
		{
			this.amount = amount;
			this.rotation = rotation;
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x001EFCDF File Offset: 0x001EDEDF
		public override SignData.SignWarp Clone()
		{
			return new SignData.SkewWarp(this.amount, this.rotation);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001EFCF4 File Offset: 0x001EDEF4
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("amount", string.Format("{0}, {1}", this.amount.x, this.amount.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001EFD6E File Offset: 0x001EDF6E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.amount = StringParsers.ParseVector2(element.GetAttribute("amount"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x00010E62 File Offset: 0x0000F062
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Skew;
			}
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001EFDAB File Offset: 0x001EDFAB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.amount);
			bw.Write(this.rotation);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001EFDC5 File Offset: 0x001EDFC5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.amount = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
		}

		// Token: 0x04003E5F RID: 15967
		public Vector2 amount;

		// Token: 0x04003E60 RID: 15968
		public float rotation;
	}

	// Token: 0x02000A78 RID: 2680
	public class BulgeWarp : SignData.SignWarp
	{
		// Token: 0x06005136 RID: 20790 RVA: 0x001EFDDF File Offset: 0x001EDFDF
		public BulgeWarp()
		{
			this.offset = Vector2.zero;
			this.amount = 2f;
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x001EFDFD File Offset: 0x001EDFFD
		public BulgeWarp(Vector2 offset, float amount)
		{
			this.offset = offset;
			this.amount = amount;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x001EFE13 File Offset: 0x001EE013
		public override SignData.SignWarp Clone()
		{
			return new SignData.BulgeWarp(this.offset, this.amount);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x001EFE28 File Offset: 0x001EE028
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("amount", this.amount.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001EFEA2 File Offset: 0x001EE0A2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.amount = float.Parse(element.GetAttribute("amount"), Utils.StandardCulture);
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600513B RID: 20795 RVA: 0x00020021 File Offset: 0x0001E221
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Bulge;
			}
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x001EFEDF File Offset: 0x001EE0DF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.amount);
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x001EFEF9 File Offset: 0x001EE0F9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.amount = br.ReadSingle();
		}

		// Token: 0x04003E61 RID: 15969
		public Vector2 offset;

		// Token: 0x04003E62 RID: 15970
		public float amount;
	}

	// Token: 0x02000A79 RID: 2681
	public class TwirlWarp : SignData.SignWarp
	{
		// Token: 0x0600513E RID: 20798 RVA: 0x001EFF13 File Offset: 0x001EE113
		public TwirlWarp()
		{
			this.offset = Vector2.zero;
			this.amount = 0.2f;
			this.frequency = 0.2f;
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x001EFF3C File Offset: 0x001EE13C
		public TwirlWarp(Vector2 offset, float amount, float frequency)
		{
			this.offset = offset;
			this.amount = amount;
			this.frequency = frequency;
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x001EFF59 File Offset: 0x001EE159
		public override SignData.SignWarp Clone()
		{
			return new SignData.TwirlWarp(this.offset, this.amount, this.frequency);
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x001EFF74 File Offset: 0x001EE174
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("amount", this.amount.ToCultureInvariantString() ?? "");
			element.SetAttrib("frequency", this.frequency.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x001F0010 File Offset: 0x001EE210
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.amount = float.Parse(element.GetAttribute("amount"), Utils.StandardCulture);
			this.frequency = float.Parse(element.GetAttribute("frequency"), Utils.StandardCulture);
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06005143 RID: 20803 RVA: 0x0002F168 File Offset: 0x0002D368
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Twirl;
			}
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x001F0078 File Offset: 0x001EE278
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.amount);
			bw.Write(this.frequency);
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x001F009E File Offset: 0x001EE29E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.amount = br.ReadSingle();
			this.frequency = br.ReadSingle();
		}

		// Token: 0x04003E63 RID: 15971
		public Vector2 offset;

		// Token: 0x04003E64 RID: 15972
		public float amount;

		// Token: 0x04003E65 RID: 15973
		public float frequency;
	}

	// Token: 0x02000A7A RID: 2682
	public class KaleidoWarp : SignData.SignWarp
	{
		// Token: 0x06005146 RID: 20806 RVA: 0x001F00C4 File Offset: 0x001EE2C4
		public KaleidoWarp()
		{
			this.offset = Vector2.zero;
			this.sides = 6;
			this.rotation = 0f;
			this.offsetScale = 1f;
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x001F00F4 File Offset: 0x001EE2F4
		public KaleidoWarp(Vector2 offset, int sides, float rotation, float offsetScale)
		{
			this.offset = offset;
			this.sides = sides;
			this.rotation = rotation;
			this.offsetScale = offsetScale;
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001F0119 File Offset: 0x001EE319
		public override SignData.SignWarp Clone()
		{
			return new SignData.KaleidoWarp(this.offset, this.sides, this.rotation, this.offsetScale);
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x001F0138 File Offset: 0x001EE338
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("sides", string.Format("{0}", this.sides));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("offsetScale", this.offsetScale.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x001F01F4 File Offset: 0x001EE3F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.sides = int.Parse(element.GetAttribute("sides"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.offsetScale = (element.HasAttribute("offsetScale") ? float.Parse(element.GetAttribute("offsetScale"), Utils.StandardCulture) : 1f);
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600514B RID: 20811 RVA: 0x00046EDA File Offset: 0x000450DA
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Kaleido;
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001F0290 File Offset: 0x001EE490
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.sides);
			bw.Write(this.rotation);
			bw.Write(this.offsetScale);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001F02C2 File Offset: 0x001EE4C2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.sides = br.ReadInt32();
			this.rotation = br.ReadSingle();
			this.offsetScale = br.ReadSingle();
		}

		// Token: 0x04003E66 RID: 15974
		public Vector2 offset;

		// Token: 0x04003E67 RID: 15975
		public int sides;

		// Token: 0x04003E68 RID: 15976
		public float rotation;

		// Token: 0x04003E69 RID: 15977
		public float offsetScale;
	}

	// Token: 0x02000A7B RID: 2683
	public class PerspectiveWarp : SignData.SignWarp
	{
		// Token: 0x0600514E RID: 20814 RVA: 0x001F02F4 File Offset: 0x001EE4F4
		public PerspectiveWarp()
		{
			this.rotation = new Vector3(0f, 30f, 0f);
			this.strength = 0.1f;
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001F0321 File Offset: 0x001EE521
		public PerspectiveWarp(Vector3 rotation, float strength)
		{
			this.rotation = rotation;
			this.strength = strength;
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x001F0337 File Offset: 0x001EE537
		public override SignData.SignWarp Clone()
		{
			return new SignData.PerspectiveWarp(this.rotation, this.strength);
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001F034C File Offset: 0x001EE54C
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("rotation", string.Format("{0}, {1}, {2}", this.rotation.x, this.rotation.y, this.rotation.z));
			element.SetAttrib("strength", this.strength.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x001F03D6 File Offset: 0x001EE5D6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.rotation = StringParsers.ParseVector3(element.GetAttribute("rotation"), 0, -1);
			this.strength = float.Parse(element.GetAttribute("strength"), Utils.StandardCulture);
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x00080550 File Offset: 0x0007E750
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Perspective;
			}
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x001F0415 File Offset: 0x001EE615
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.rotation);
			bw.Write(this.strength);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x001F042F File Offset: 0x001EE62F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.rotation = StreamUtils.ReadVector3(br);
			this.strength = br.ReadSingle();
		}

		// Token: 0x04003E6A RID: 15978
		public Vector3 rotation;

		// Token: 0x04003E6B RID: 15979
		public float strength;
	}

	// Token: 0x02000A7C RID: 2684
	public class ArcWarp : SignData.SignWarp
	{
		// Token: 0x06005156 RID: 20822 RVA: 0x001F0449 File Offset: 0x001EE649
		public ArcWarp()
		{
			this.rotation = 0f;
			this.radius = 2f;
			this.width = 0.1f;
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001F0472 File Offset: 0x001EE672
		public ArcWarp(float rotation, float radius, float width)
		{
			this.rotation = rotation;
			this.radius = radius;
			this.width = width;
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001F048F File Offset: 0x001EE68F
		public override SignData.SignWarp Clone()
		{
			return new SignData.ArcWarp(this.rotation, this.radius, this.width);
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001F04A8 File Offset: 0x001EE6A8
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("radius", this.radius.ToCultureInvariantString() ?? "");
			element.SetAttrib("width", this.width.ToCultureInvariantString() ?? "");
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001F052C File Offset: 0x001EE72C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.radius = float.Parse(element.GetAttribute("radius"), Utils.StandardCulture);
			this.width = float.Parse(element.GetAttribute("width"), Utils.StandardCulture);
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600515B RID: 20827 RVA: 0x00080719 File Offset: 0x0007E919
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Arc;
			}
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001F0599 File Offset: 0x001EE799
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.rotation);
			bw.Write(this.radius);
			bw.Write(this.width);
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001F05BF File Offset: 0x001EE7BF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.rotation = br.ReadSingle();
			this.radius = br.ReadSingle();
			this.width = br.ReadSingle();
		}

		// Token: 0x04003E6C RID: 15980
		public float rotation;

		// Token: 0x04003E6D RID: 15981
		public float radius;

		// Token: 0x04003E6E RID: 15982
		public float width;
	}

	// Token: 0x02000A7D RID: 2685
	public class StretchWarp : SignData.SignWarp
	{
		// Token: 0x0600515E RID: 20830 RVA: 0x001F05E8 File Offset: 0x001EE7E8
		public StretchWarp()
		{
			this.offset = new Vector2(0f, 0f);
			this.rotation = 0f;
			this.distance = 0f;
			this.width = 0f;
			this.exponent = 0f;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001F063C File Offset: 0x001EE83C
		public StretchWarp(Vector2 offset, float rotation, float distance, float width, float exponent)
		{
			this.offset = offset;
			this.rotation = rotation;
			this.distance = distance;
			this.width = width;
			this.exponent = exponent;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001F0669 File Offset: 0x001EE869
		public override SignData.SignWarp Clone()
		{
			return new SignData.StretchWarp(this.offset, this.rotation, this.distance, this.width, this.exponent);
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001F0690 File Offset: 0x001EE890
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("distance", this.distance.ToCultureInvariantString() ?? "");
			element.SetAttrib("width", this.width.ToCultureInvariantString() ?? "");
			element.SetAttrib("exponent", this.exponent.ToCultureInvariantString() ?? "");
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x001F076C File Offset: 0x001EE96C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.distance = float.Parse(element.GetAttribute("distance"), Utils.StandardCulture);
			this.width = float.Parse(element.GetAttribute("width"), Utils.StandardCulture);
			this.exponent = float.Parse(element.GetAttribute("exponent"), Utils.StandardCulture);
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x000817FC File Offset: 0x0007F9FC
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Stretch;
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001F0814 File Offset: 0x001EEA14
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.rotation);
			bw.Write(this.distance);
			bw.Write(this.width);
			bw.Write(this.exponent);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001F0852 File Offset: 0x001EEA52
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
			this.distance = br.ReadSingle();
			this.width = br.ReadSingle();
			this.exponent = br.ReadSingle();
		}

		// Token: 0x04003E6F RID: 15983
		public Vector2 offset;

		// Token: 0x04003E70 RID: 15984
		public float rotation;

		// Token: 0x04003E71 RID: 15985
		public float distance;

		// Token: 0x04003E72 RID: 15986
		public float width;

		// Token: 0x04003E73 RID: 15987
		public float exponent;
	}

	// Token: 0x02000A7E RID: 2686
	public class GridWarp : SignData.SignWarp
	{
		// Token: 0x06005166 RID: 20838 RVA: 0x001F0890 File Offset: 0x001EEA90
		public GridWarp()
		{
			this.mode = SignData.GridWarp.Mode.Rectangle;
			this.offset = Vector2.zero;
			this.rotation = 0f;
			this.scale = 1f;
			this.aspect = 1f;
			this.shift = 0f;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x001F08E1 File Offset: 0x001EEAE1
		public GridWarp(SignData.GridWarp.Mode mode, Vector2 offset, float rotation, float scale, float aspect, float shift)
		{
			this.mode = mode;
			this.offset = offset;
			this.rotation = rotation;
			this.scale = scale;
			this.aspect = aspect;
			this.shift = shift;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x001F0916 File Offset: 0x001EEB16
		public override SignData.SignWarp Clone()
		{
			return new SignData.GridWarp(this.mode, this.offset, this.rotation, this.scale, this.aspect, this.shift);
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001F0944 File Offset: 0x001EEB44
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

		// Token: 0x0600516A RID: 20842 RVA: 0x001F0A40 File Offset: 0x001EEC40
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

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600516B RID: 20843 RVA: 0x000601DD File Offset: 0x0005E3DD
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Grid;
			}
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x001F0B38 File Offset: 0x001EED38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write((int)this.mode);
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.rotation);
			bw.Write(this.scale);
			bw.Write(this.aspect);
			bw.Write(this.shift);
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x001F0B90 File Offset: 0x001EED90
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.mode = (SignData.GridWarp.Mode)br.ReadInt32();
			this.offset = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
			this.scale = br.ReadSingle();
			this.aspect = br.ReadSingle();
			this.shift = br.ReadSingle();
		}

		// Token: 0x04003E74 RID: 15988
		public SignData.GridWarp.Mode mode;

		// Token: 0x04003E75 RID: 15989
		public Vector2 offset;

		// Token: 0x04003E76 RID: 15990
		public float rotation;

		// Token: 0x04003E77 RID: 15991
		public float scale;

		// Token: 0x04003E78 RID: 15992
		public float aspect;

		// Token: 0x04003E79 RID: 15993
		public float shift;

		// Token: 0x02000A7F RID: 2687
		public enum Mode
		{
			// Token: 0x04003E7B RID: 15995
			Column,
			// Token: 0x04003E7C RID: 15996
			Rectangle,
			// Token: 0x04003E7D RID: 15997
			Hex
		}
	}

	// Token: 0x02000A80 RID: 2688
	public abstract class SignLayer
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600516E RID: 20846 RVA: 0x001F0BE5 File Offset: 0x001EEDE5
		public bool HasWarps
		{
			get
			{
				List<SignData.SignWarp> list = this.warps;
				return list != null && list.Count > 0;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x001F0BFB File Offset: 0x001EEDFB
		public bool HasTransform
		{
			get
			{
				return this.transform != SignData.SignTransform.Identity;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x001F0C0D File Offset: 0x001EEE0D
		public bool HasTransformOrWarps
		{
			get
			{
				return this.HasTransform || this.HasWarps;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06005171 RID: 20849
		public abstract SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06005172 RID: 20850
		public abstract string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x06005173 RID: 20851
		public abstract SignData.SignLayer Clone();

		// Token: 0x06005174 RID: 20852
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalRead(BinaryReader br);

		// Token: 0x06005175 RID: 20853
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalWrite(BinaryWriter bw);

		// Token: 0x06005176 RID: 20854 RVA: 0x001F0C20 File Offset: 0x001EEE20
		public SignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps)
		{
			this.name = name;
			this.transform.position = position;
			this.transform.rotation = rotation;
			this.transform.scale = scale;
			this.renderSettings = renderSettings;
			this.warps = warps;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x001F0C70 File Offset: 0x001EEE70
		public SignLayer()
		{
			this.name = string.Empty;
			this.transform.position = Vector2.zero;
			this.transform.rotation = 0f;
			this.transform.scale = new Vector2(0.3f, 0.3f);
			this.renderSettings = new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly);
			this.warps = new List<SignData.SignWarp>();
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x001F0CE4 File Offset: 0x001EEEE4
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

		// Token: 0x06005179 RID: 20857 RVA: 0x001F0D44 File Offset: 0x001EEF44
		public void Write(BinaryWriter bw)
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

		// Token: 0x0600517A RID: 20858 RVA: 0x001F0DF8 File Offset: 0x001EEFF8
		public static SignData.SignLayer Read(BinaryReader br)
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

		// Token: 0x0600517B RID: 20859 RVA: 0x001F0EBC File Offset: 0x001EF0BC
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

		// Token: 0x0600517C RID: 20860 RVA: 0x001F0F4C File Offset: 0x001EF14C
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

		// Token: 0x0600517D RID: 20861 RVA: 0x001F0FE8 File Offset: 0x001EF1E8
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

		// Token: 0x0600517E RID: 20862 RVA: 0x001F1078 File Offset: 0x001EF278
		public void SetDefaultName(int idx)
		{
			this.name = string.Format(this.defaultName + " " + idx.ToString("00"), Array.Empty<object>());
		}

		// Token: 0x04003E7E RID: 15998
		public string name;

		// Token: 0x04003E7F RID: 15999
		public SignData.SignTransform transform;

		// Token: 0x04003E80 RID: 16000
		public SignData.SignRenderSettings renderSettings;

		// Token: 0x04003E81 RID: 16001
		public List<SignData.SignWarp> warps;

		// Token: 0x02000A81 RID: 2689
		public enum LayerType : byte
		{
			// Token: 0x04003E83 RID: 16003
			Group,
			// Token: 0x04003E84 RID: 16004
			Text,
			// Token: 0x04003E85 RID: 16005
			Polygon,
			// Token: 0x04003E86 RID: 16006
			Noise
		}
	}

	// Token: 0x02000A82 RID: 2690
	public class GroupSignLayer : SignData.SignLayer
	{
		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x0600517F RID: 20863 RVA: 0x001F10A6 File Offset: 0x001EF2A6
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Group";
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06005180 RID: 20864 RVA: 0x00010E62 File Offset: 0x0000F062
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Group;
			}
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x001F10AD File Offset: 0x001EF2AD
		public GroupSignLayer()
		{
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x001F10B5 File Offset: 0x001EF2B5
		public GroupSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, List<SignData.SignLayer> layers, SignData.GroupSignLayer.OffsetTarget offsetTarget = SignData.GroupSignLayer.OffsetTarget.All, float softnessOffset = 0f, float dilateOffset = 0f, SignData.GroupSignLayer.ColorMode colorMode = SignData.GroupSignLayer.ColorMode.Multiply)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.layers = layers;
			this.offsetTarget = offsetTarget;
			this.softnessOffset = softnessOffset;
			this.dilateOffset = dilateOffset;
			this.colorMode = colorMode;
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001F10F0 File Offset: 0x001EF2F0
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

		// Token: 0x06005184 RID: 20868 RVA: 0x001F11AC File Offset: 0x001EF3AC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
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

		// Token: 0x06005185 RID: 20869 RVA: 0x001F1250 File Offset: 0x001EF450
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
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

		// Token: 0x06005186 RID: 20870 RVA: 0x001F12C0 File Offset: 0x001EF4C0
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

		// Token: 0x06005187 RID: 20871 RVA: 0x001F13B4 File Offset: 0x001EF5B4
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

		// Token: 0x04003E87 RID: 16007
		public List<SignData.SignLayer> layers;

		// Token: 0x04003E88 RID: 16008
		public SignData.GroupSignLayer.OffsetTarget offsetTarget;

		// Token: 0x04003E89 RID: 16009
		public float softnessOffset;

		// Token: 0x04003E8A RID: 16010
		public float dilateOffset;

		// Token: 0x04003E8B RID: 16011
		public SignData.GroupSignLayer.ColorMode colorMode;

		// Token: 0x02000A83 RID: 2691
		public enum OffsetTarget : byte
		{
			// Token: 0x04003E8D RID: 16013
			All,
			// Token: 0x04003E8E RID: 16014
			Shapes,
			// Token: 0x04003E8F RID: 16015
			Text,
			// Token: 0x04003E90 RID: 16016
			Noise
		}

		// Token: 0x02000A84 RID: 2692
		public enum ColorMode : byte
		{
			// Token: 0x04003E92 RID: 16018
			Multiply,
			// Token: 0x04003E93 RID: 16019
			Blend,
			// Token: 0x04003E94 RID: 16020
			Override
		}
	}

	// Token: 0x02000A85 RID: 2693
	public class TextSignLayer : SignData.SignLayer
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x001F14D8 File Offset: 0x001EF6D8
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Text";
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06005189 RID: 20873 RVA: 0x00020021 File Offset: 0x0001E221
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Text;
			}
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x001F14E0 File Offset: 0x001EF6E0
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

		// Token: 0x0600518B RID: 20875 RVA: 0x001F1548 File Offset: 0x001EF748
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

		// Token: 0x0600518C RID: 20876 RVA: 0x001F1594 File Offset: 0x001EF794
		public override SignData.SignLayer Clone()
		{
			return new SignData.TextSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.font, this.text, this.direction, this.spacing, this.softness, this.dilate);
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x001F1600 File Offset: 0x001EF800
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.font);
			bw.Write(this.text);
			bw.Write(this.direction);
			bw.Write(this.spacing);
			bw.Write(this.softness);
			bw.Write(this.dilate);
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x001F1658 File Offset: 0x001EF858
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.font = br.ReadString();
			this.text = br.ReadString();
			this.direction = br.ReadSingle();
			this.spacing = br.ReadSingle();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x001F16B0 File Offset: 0x001EF8B0
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

		// Token: 0x06005190 RID: 20880 RVA: 0x001F1744 File Offset: 0x001EF944
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

		// Token: 0x04003E95 RID: 16021
		public string font;

		// Token: 0x04003E96 RID: 16022
		public string text;

		// Token: 0x04003E97 RID: 16023
		public float direction;

		// Token: 0x04003E98 RID: 16024
		public float spacing;

		// Token: 0x04003E99 RID: 16025
		public float softness;

		// Token: 0x04003E9A RID: 16026
		public float dilate;
	}

	// Token: 0x02000A86 RID: 2694
	public class PolygonSignLayer : SignData.SignLayer
	{
		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x001F1804 File Offset: 0x001EFA04
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Poly";
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x0002F168 File Offset: 0x0002D368
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Polygon;
			}
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x001F180C File Offset: 0x001EFA0C
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

		// Token: 0x06005194 RID: 20884 RVA: 0x001F1870 File Offset: 0x001EFA70
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

		// Token: 0x06005195 RID: 20885 RVA: 0x001F18C4 File Offset: 0x001EFAC4
		public override SignData.SignLayer Clone()
		{
			return new SignData.PolygonSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.sides, this.smoothness, this.starify, this.softness, this.dilate, this.frequency, this.shapeMode);
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x001F1934 File Offset: 0x001EFB34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.sides);
			bw.Write(this.smoothness);
			bw.Write(this.starify);
			bw.Write(this.softness);
			bw.Write(this.dilate);
			bw.Write(this.frequency);
			bw.Write((byte)this.shapeMode);
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001F1998 File Offset: 0x001EFB98
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.sides = br.ReadInt32();
			this.smoothness = br.ReadSingle();
			this.starify = br.ReadSingle();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
			this.frequency = br.ReadSingle();
			this.shapeMode = (SignData.PolygonSignLayer.ShapeMode)br.ReadByte();
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001F19FC File Offset: 0x001EFBFC
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

		// Token: 0x06005199 RID: 20889 RVA: 0x001F1AB8 File Offset: 0x001EFCB8
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

		// Token: 0x04003E9B RID: 16027
		public int sides;

		// Token: 0x04003E9C RID: 16028
		public float smoothness;

		// Token: 0x04003E9D RID: 16029
		public float starify;

		// Token: 0x04003E9E RID: 16030
		public float softness;

		// Token: 0x04003E9F RID: 16031
		public float dilate;

		// Token: 0x04003EA0 RID: 16032
		public float frequency;

		// Token: 0x04003EA1 RID: 16033
		public SignData.PolygonSignLayer.ShapeMode shapeMode;

		// Token: 0x02000A87 RID: 2695
		public enum ShapeMode : byte
		{
			// Token: 0x04003EA3 RID: 16035
			Normal,
			// Token: 0x04003EA4 RID: 16036
			Invert,
			// Token: 0x04003EA5 RID: 16037
			Line,
			// Token: 0x04003EA6 RID: 16038
			Ripple
		}
	}

	// Token: 0x02000A88 RID: 2696
	public class NoiseSignLayer : SignData.SignLayer
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x001F1BE9 File Offset: 0x001EFDE9
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Noise";
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x00046EDA File Offset: 0x000450DA
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Noise;
			}
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001F1BF0 File Offset: 0x001EFDF0
		public NoiseSignLayer()
		{
			this.name = "Noise";
			this.seed = 0;
			this.detail = 4;
			this.softness = 0f;
			this.dilate = 0f;
			this.fade = 0f;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001F1C3D File Offset: 0x001EFE3D
		public NoiseSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, int seed, int detail, float softness, float dilate, float fade)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.seed = seed;
			this.detail = detail;
			this.softness = softness;
			this.dilate = dilate;
			this.fade = fade;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001F1C78 File Offset: 0x001EFE78
		public override SignData.SignLayer Clone()
		{
			return new SignData.NoiseSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.seed, this.detail, this.softness, this.dilate, this.fade);
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x001F1CDC File Offset: 0x001EFEDC
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("seed", this.seed.ToString());
			element.SetAttrib("detail", this.detail.ToString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
			element.SetAttrib("fade", this.fade.ToCultureInvariantString());
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x001F1D64 File Offset: 0x001EFF64
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

		// Token: 0x060051A1 RID: 20897 RVA: 0x001F1E1F File Offset: 0x001F001F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.seed);
			bw.Write(this.detail);
			bw.Write(this.softness);
			bw.Write(this.dilate);
			bw.Write(this.fade);
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001F1E5D File Offset: 0x001F005D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.seed = br.ReadInt32();
			this.detail = br.ReadInt32();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
			this.fade = br.ReadSingle();
		}

		// Token: 0x04003EA7 RID: 16039
		public int seed;

		// Token: 0x04003EA8 RID: 16040
		public int detail;

		// Token: 0x04003EA9 RID: 16041
		public float softness;

		// Token: 0x04003EAA RID: 16042
		public float dilate;

		// Token: 0x04003EAB RID: 16043
		public float fade;
	}

	// Token: 0x02000A89 RID: 2697
	[PublicizedFrom(EAccessModifier.Private)]
	public static class DefaultSignData
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x001F1E9C File Offset: 0x001F009C
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

		// Token: 0x04003EAC RID: 16044
		public static readonly Color color1 = new Color(0.318f, 0.396f, 0.686f);

		// Token: 0x04003EAD RID: 16045
		public static readonly Color color2 = new Color(0.286f, 0.518f, 1f);

		// Token: 0x04003EAE RID: 16046
		public static readonly Color color3 = new Color(0.514f, 0.671f, 1f);

		// Token: 0x04003EAF RID: 16047
		public static readonly Vector2 textSize = 0.65f * Vector2.one;

		// Token: 0x04003EB0 RID: 16048
		public const float textRotation = 8f;

		// Token: 0x04003EB1 RID: 16049
		public const string font1 = "LiberationSans";

		// Token: 0x04003EB2 RID: 16050
		public const string font2 = "AmaticSC";

		// Token: 0x04003EB3 RID: 16051
		public const string text = "Hello World!";

		// Token: 0x04003EB4 RID: 16052
		public const int polygonSides = 5;

		// Token: 0x04003EB5 RID: 16053
		public const float polygonSmoothness = 0.2f;

		// Token: 0x04003EB6 RID: 16054
		public const float polygonStarify = 0.5f;
	}
}
