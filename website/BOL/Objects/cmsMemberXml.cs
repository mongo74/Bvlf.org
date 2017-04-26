using System.Xml.Schema;
using System.Xml.Serialization;

namespace bvlf_v2.BOL.Objects
{
    [XmlRoot(ElementName = "node")]
    public class cmsMemberXml
    {
        [XmlElement("naamschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Naam { get; set; }

        [XmlElement("lidnr", Form = XmlSchemaForm.Unqualified)]
        public string LidNr { get; set; }

        [XmlElement("fipfnr", Form = XmlSchemaForm.Unqualified)]
        public string FipfNr { get; set; }

        [XmlElement("profiel", Form = XmlSchemaForm.Unqualified)]
        public string Profiel { get; set; }

        [XmlElement("gender", Form = XmlSchemaForm.Unqualified)]
        public string Gender { get; set; }

        [XmlElement("emailschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Email { get; set; }

        [XmlElement("country", Form = XmlSchemaForm.Unqualified)]
        public string Country { get; set; }

        [XmlElement("bvlfMemberStatus", Form = XmlSchemaForm.Unqualified)]
        public string BvlfMemberStatus { get; set; }

        [XmlElement("subscriptionDate", Form = XmlSchemaForm.Unqualified)]
        public string SubscriptionDate { get; set; }

        [XmlElement("subscriptionExpiration", Form = XmlSchemaForm.Unqualified)]
        public string SubscriptionExpiration { get; set; }

        [XmlElement("telschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Phone { get; set; }

        [XmlElement("naam", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement("voornaam", Form = XmlSchemaForm.Unqualified)]
        public string FirstName { get; set; }

        [XmlElement("busschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Box { get; set; }

        [XmlElement("straat", Form = XmlSchemaForm.Unqualified)]
        public string Street { get; set; }

        [XmlElement("straatschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Street { get; set; }

        [XmlElement("nrSchool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Nr { get; set; }

        [XmlElement("nr", Form = XmlSchemaForm.Unqualified)]
        public string Nr { get; set; }

        [XmlElement("bus", Form = XmlSchemaForm.Unqualified)]
        public string Box { get; set; }

        [XmlElement("zipcodeschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Zipcode { get; set; }

        [XmlElement("postcode", Form = XmlSchemaForm.Unqualified)]
        public string Zipcode { get; set; }

        [XmlElement("plaatsschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1_Naam { get; set; }

        [XmlElement("emailschool_1", Form = XmlSchemaForm.Unqualified)]
        public string School1Email { get; set; }

        [XmlElement("plaats", Form = XmlSchemaForm.Unqualified)]
        public string Location { get; set; }

        [XmlElement("tel", Form = XmlSchemaForm.Unqualified)]
        public string Phone { get; set; }

        [XmlElement("naamschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Name { get; set; }

        [XmlElement("straatschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Street { get; set; }

        [XmlElement("mobile", Form = XmlSchemaForm.Unqualified)]
        public string Mobile { get; set; }

        [XmlElement("nrschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Nr { get; set; }

        [XmlElement("busschool2", Form = XmlSchemaForm.Unqualified)]
        public string School2Box { get; set; }

        [XmlElement("zipcodeschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Zipcode { get; set; }

        [XmlElement("plaatsschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Plaats { get; set; }

        [XmlElement("telschool_2", Form = XmlSchemaForm.Unqualified)]
        public string School2Tel { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("parentID")]
        public string ParentId { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("writerID")]
        public string WriterId { get; set; }

        [XmlAttribute("nodeType")]
        public string NodeType { get; set; }

        [XmlAttribute("template")]
        public string Template { get; set; }

        [XmlAttribute("sortOrder")]
        public string SortOrder { get; set; }

        [XmlAttribute("createDate")]
        public string CreateDate { get; set; }

        [XmlAttribute("updateDate")]
        public string UpdateDate { get; set; }

        [XmlAttribute("nodeName")]
        public string NodeName { get; set; }

        [XmlAttribute("urlName")]
        public string UrlName { get; set; }

        [XmlAttribute("writerName")]
        public string WriterName { get; set; }

        [XmlAttribute("nodeTypeAlias")]
        public string NodeTypeAlias { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("loginName")]
        public string LoginName { get; set; }

        [XmlAttribute("email")]
        public string Email { get; set; }
    }
}