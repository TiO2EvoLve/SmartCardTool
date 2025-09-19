using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("card_personalization_file")]
public class CardPersonalizationFile
{
    [XmlElement("batch_information")]
    public BatchInformationWrapper BatchInformation { get; set; }
}

public class BatchInformationWrapper
{
    [XmlElement("batch_information")]
    public BatchInformation BatchInformation { get; set; }
    
    [XmlElement("card_data")]
    public List<CardData> CardDataList { get; set; }
}

public class BatchInformation
{
    [XmlAttribute("city")]
    public string City { get; set; }

    [XmlAttribute("count")]
    public int Count { get; set; }

    [XmlAttribute("uuid")]
    public string Uuid { get; set; }
}

public class CardData
{
    [XmlElement("Pamater")]
    public Pamater Pamater { get; set; }

    [XmlElement("EP")]
    public EP EP { get; set; }

    [XmlElement("TC")]
    public TC TC { get; set; }
}

public class Pamater
{
    [XmlElement("var")]
    public List<VarItem> Vars { get; set; }
}

public class VarItem
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("length")]
    public int Length { get; set; }

    [XmlText]
    public string Value { get; set; }
}

public class EP
{
    [XmlAttribute("AID")]
    public string AID { get; set; }

    [XmlAttribute("FID")]
    public string FID { get; set; }

    [XmlElement("key_group")]
    public KeyGroup KeyGroup { get; set; }

    [XmlElement("data_group")]
    public DataGroup DataGroup { get; set; }
}

public class TC
{
    [XmlAttribute("AID")]
    public string AID { get; set; }

    [XmlAttribute("FID")]
    public string FID { get; set; }

    [XmlElement("key_group")]
    public KeyGroup KeyGroup { get; set; }

    [XmlElement("data_group")]
    public DataGroup DataGroup { get; set; }
}

public class KeyGroup
{
    [XmlElement("key")]
    public List<KeyItem> Keys { get; set; }
}

public class KeyItem
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("md5")]
    public string Md5 { get; set; }

    [XmlText]
    public string Value { get; set; }
}

public class DataGroup
{
    [XmlElement("ef")]
    public List<EfItem> Efs { get; set; }
}

public class EfItem
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("length")]
    public int Length { get; set; }

    [XmlText]
    public string Value { get; set; }
}
