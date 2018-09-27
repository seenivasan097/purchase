using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace InvoiceApp.Helpers.Extensions
{
    public class JsonNetResult : JsonResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }
        public bool CamelCaseContract { get; set; }
        public bool EnableTextContent { get; set; }
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
            SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "M/d/yyyy" });
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            try
            {
                if (EnableTextContent)
                {
                    response.ContentType = !string.IsNullOrEmpty(ContentType)
                                   ? ContentType
                                   : "text/plain";
                    //Need to check it throws error for types other than elfinder generated Json like image, thumbnail, download, etc.
                }
                else
                {
                    response.ContentType = !String.IsNullOrWhiteSpace(this.ContentType) ? this.ContentType : "application/json";
                }
            }
            catch (Exception)
            {

            }
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;
            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

            if (CamelCaseContract)
            {
                SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            var serializer = JsonSerializer.Create(SerializerSettings);
            serializer.Serialize(writer, Data);
            writer.Flush();
        }
    }
}