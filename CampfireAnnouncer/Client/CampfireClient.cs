using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace CampfireAnnouncer.Client {
    public class CampfireClient {

        public string Token { get; set; }
        public string Room {get; set; }
        public string Domain { get; set; }
        public string LastResponse { get; private set; }

        public CampfireClient(string domain, string room, string token) {
            this.Domain = domain;
            this.Room = room;
            this.Token = token;
        }

        public void PostTextMessage(string message) {
            HttpWebRequest req = GetAuthenticatedRequest(string.Format(@"/room/{0}/speak.xml", this.Room));
            req.Method = @"POST";
            req.ContentType = @"application/xml";

            Messages.Message msg = new Messages.Message(@"TextMessage", message);

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(msg.GetType());
            xs.Serialize(req.GetRequestStream(), msg);

            HandleXMLResponse(req);
        }

        private string GetRequestUrl(string domain, string requestPath) {
            return string.Format(@"https://{0}.campfirenow.com{1}", domain, requestPath);
        }

        private HttpWebRequest GetAuthenticatedRequest(string requestPath) {
            string url = GetRequestUrl(this.Domain, requestPath);
            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;

            string basicauth = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Token + @":X"));
            req.Headers.Add(@"Authorization", @"Basic " + basicauth);
            return req;
        }

        public XElement HandleXMLResponse(HttpWebRequest req) {
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse) {
                if (resp == null) throw new NullReferenceException(@"The web request to " + req.RequestUri + @" did not receive a response");
                if (resp.StatusCode >= HttpStatusCode.MultipleChoices) throw new Exception(@"The web request to " + req.RequestUri + " returned HTTP Status " + resp.StatusCode.ToString());
                using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                    this.LastResponse = sr.ReadToEnd();
                    return XElement.Parse(this.LastResponse);
                }
            }
        }

        public XElement RequestToXMLResponse(string requestPath) {
            return HandleXMLResponse(GetAuthenticatedRequest(requestPath));
        }

    }
}
