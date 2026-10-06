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
	// Token: 0x02001640 RID: 5696
	public class ModInfo : IXMLData
	{
		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x0600B2B0 RID: 45744 RVA: 0x0042B1C0 File Offset: 0x004293C0
		// (set) Token: 0x0600B2B1 RID: 45745 RVA: 0x0042B1C8 File Offset: 0x004293C8
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

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x0600B2B2 RID: 45746 RVA: 0x0042B1D1 File Offset: 0x004293D1
		// (set) Token: 0x0600B2B3 RID: 45747 RVA: 0x0042B1D9 File Offset: 0x004293D9
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

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x0600B2B4 RID: 45748 RVA: 0x0042B1E2 File Offset: 0x004293E2
		// (set) Token: 0x0600B2B5 RID: 45749 RVA: 0x0042B1EA File Offset: 0x004293EA
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

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x0600B2B6 RID: 45750 RVA: 0x0042B1F3 File Offset: 0x004293F3
		// (set) Token: 0x0600B2B7 RID: 45751 RVA: 0x0042B1FB File Offset: 0x004293FB
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

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x0600B2B8 RID: 45752 RVA: 0x0042B204 File Offset: 0x00429404
		// (set) Token: 0x0600B2B9 RID: 45753 RVA: 0x0042B20C File Offset: 0x0042940C
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

		// Token: 0x04008646 RID: 34374
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pName;

		// Token: 0x04008647 RID: 34375
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x04008648 RID: 34376
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAuthor;

		// Token: 0x04008649 RID: 34377
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pVersion;

		// Token: 0x0400864A RID: 34378
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pWebsite;

		// Token: 0x02001641 RID: 5697
		public static class Parser
		{
			// Token: 0x0600B2BB RID: 45755 RVA: 0x0042B218 File Offset: 0x00429418
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

			// Token: 0x0600B2BC RID: 45756 RVA: 0x0042B438 File Offset: 0x00429638
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

			// Token: 0x0600B2BD RID: 45757 RVA: 0x0042B4E4 File Offset: 0x004296E4
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

			// Token: 0x0600B2BE RID: 45758 RVA: 0x0042B590 File Offset: 0x00429790
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

			// Token: 0x0600B2BF RID: 45759 RVA: 0x0042B63C File Offset: 0x0042983C
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

			// Token: 0x0600B2C0 RID: 45760 RVA: 0x0042B6E8 File Offset: 0x004298E8
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

			// Token: 0x0400864B RID: 34379
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
