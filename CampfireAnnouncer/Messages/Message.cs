using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace CampfireAnnouncer.Messages {

    [XmlType(TypeName="message")]
    public class Message {
        [XmlElement(ElementName = "type", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        [XmlElement(ElementName = "body", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Body { get; set; }

        public Message() : this(@"TextMessage", @"") {}

        public Message(string type, string body) {
            this.Type = type;
            this.Body = body;
        }
    }
}
