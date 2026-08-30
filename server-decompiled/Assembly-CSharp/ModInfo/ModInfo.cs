using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace ModInfo
{
	// Token: 0x02001621 RID: 5665
	public class ModInfo : IXMLData
	{
		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x0600B21E RID: 45598 RVA: 0x004274E4 File Offset: 0x004256E4
		// (set) Token: 0x0600B21F RID: 45599 RVA: 0x004274EC File Offset: 0x004256EC
		public DataItem<string> Name
		{
			get
			{
				return this.pName;
			}
			set
			{
				this.pName = value;
			}
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x0600B220 RID: 45600 RVA: 0x004274F5 File Offset: 0x004256F5
		// (set) Token: 0x0600B221 RID: 45601 RVA: 0x004274FD File Offset: 0x004256FD
		public DataItem<string> Description
		{
			get
			{
				return this.pDescription;
			}
			set
			{
				this.pDescription = value;
			}
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x0600B222 RID: 45602 RVA: 0x00427506 File Offset: 0x00425706
		// (set) Token: 0x0600B223 RID: 45603 RVA: 0x0042750E File Offset: 0x0042570E
		public DataItem<string> Author
		{
			get
			{
				return this.pAuthor;
			}
			set
			{
				this.pAuthor = value;
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x0600B224 RID: 45604 RVA: 0x00427517 File Offset: 0x00425717
		// (set) Token: 0x0600B225 RID: 45605 RVA: 0x0042751F File Offset: 0x0042571F
		public DataItem<string> Version
		{
			get
			{
				return this.pVersion;
			}
			set
			{
				this.pVersion = value;
			}
		}

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x0600B226 RID: 45606 RVA: 0x00427528 File Offset: 0x00425728
		// (set) Token: 0x0600B227 RID: 45607 RVA: 0x00427530 File Offset: 0x00425730
		public DataItem<string> Website
		{
			get
			{
				return this.pWebsite;
			}
			set
			{
				this.pWebsite = value;
			}
		}

		// Token: 0x040085F0 RID: 34288
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pName;

		// Token: 0x040085F1 RID: 34289
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x040085F2 RID: 34290
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAuthor;

		// Token: 0x040085F3 RID: 34291
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pVersion;

		// Token: 0x040085F4 RID: 34292
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pWebsite;

		// Token: 0x02001622 RID: 5666
		public static class Parser
		{
			// Token: 0x0600B229 RID: 45609 RVA: 0x0042753C File Offset: 0x0042573C
			public static ModInfo Parse(XElement _elem, Dictionary<PositionXmlElement, DataItem<ModInfo>> _updateLater)
			{
				ModInfo modInfo = new ModInfo();
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (XElement xelement in _elem.Elements())
				{
					if (!ModInfo.Parser.knownAttributesMultiplicity.ContainsKey(xelement.Name.LocalName))
					{
						string text = "Unknown element \"";
						XName name = xelement.Name;
						throw new UnexpectedElementException(text + ((name != null) ? name.ToString() : null) + "\" found while parsing ModInfo", ((IXmlLineInfo)xelement).LineNumber);
					}
					string localName = xelement.Name.LocalName;
					if (!(localName == "Name"))
					{
						if (!(localName == "Description"))
						{
							if (!(localName == "Author"))
							{
								if (!(localName == "Version"))
								{
									if (localName == "Website")
									{
										ModInfo.Parser.ParseFieldAttributeWebsite(modInfo, dictionary, xelement);
									}
								}
								else
								{
									ModInfo.Parser.ParseFieldAttributeVersion(modInfo, dictionary, xelement);
								}
							}
							else
							{
								ModInfo.Parser.ParseFieldAttributeAuthor(modInfo, dictionary, xelement);
							}
						}
						else
						{
							ModInfo.Parser.ParseFieldAttributeDescription(modInfo, dictionary, xelement);
						}
					}
					else
					{
						ModInfo.Parser.ParseFieldAttributeName(modInfo, dictionary, xelement);
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in ModInfo.Parser.knownAttributesMultiplicity)
				{
					int num = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num > keyValuePair.Value.max))
					{
						string[] array = new string[6];
						array[0] = "Element has incorrect number of \"";
						array[1] = keyValuePair.Key;
						array[2] = "\" attribute instances, found ";
						array[3] = num.ToString();
						array[4] = ", expected ";
						int num2 = 5;
						Range<int> value = keyValuePair.Value;
						array[num2] = ((value != null) ? value.ToString() : null);
						throw new IncorrectAttributeOccurrenceException(string.Concat(array), ((IXmlLineInfo)_elem).LineNumber);
					}
				}
				return modInfo;
			}

			// Token: 0x0600B22A RID: 45610 RVA: 0x0042775C File Offset: 0x0042595C
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeName(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Name\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Name", text2);
				_entry.pName = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Name"))
					{
						_foundAttributes["Name"] = 0;
					}
					int num = _foundAttributes["Name"];
					_foundAttributes["Name"] = num + 1;
				}
			}

			// Token: 0x0600B22B RID: 45611 RVA: 0x00427808 File Offset: 0x00425A08
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeDescription(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Description\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Description", text2);
				_entry.pDescription = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Description"))
					{
						_foundAttributes["Description"] = 0;
					}
					int num = _foundAttributes["Description"];
					_foundAttributes["Description"] = num + 1;
				}
			}

			// Token: 0x0600B22C RID: 45612 RVA: 0x004278B4 File Offset: 0x00425AB4
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeAuthor(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Author\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Author", text2);
				_entry.pAuthor = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Author"))
					{
						_foundAttributes["Author"] = 0;
					}
					int num = _foundAttributes["Author"];
					_foundAttributes["Author"] = num + 1;
				}
			}

			// Token: 0x0600B22D RID: 45613 RVA: 0x00427960 File Offset: 0x00425B60
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeVersion(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Version\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Version", text2);
				_entry.pVersion = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Version"))
					{
						_foundAttributes["Version"] = 0;
					}
					int num = _foundAttributes["Version"];
					_foundAttributes["Version"] = num + 1;
				}
			}

			// Token: 0x0600B22E RID: 45614 RVA: 0x00427A0C File Offset: 0x00425C0C
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeWebsite(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Website\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Website", text2);
				_entry.pWebsite = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Website"))
					{
						_foundAttributes["Website"] = 0;
					}
					int num = _foundAttributes["Website"];
					_foundAttributes["Website"] = num + 1;
				}
			}

			// Token: 0x040085F5 RID: 34293
			[PublicizedFrom(EAccessModifier.Private)]
			public static readonly Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Name",
					new Range<int>(true, 1, true, 1)
				},
				{
					"Description",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Author",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Version",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Website",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
