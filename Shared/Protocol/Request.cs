using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Protocol
{
    public class Request
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestType Operation { get; set; }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Desc { get; set; }

        // Serializacija u JSON
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // Deserializacija iz JSON
        public static Request FromJson(string json)
        {
            return JsonSerializer.Deserialize<Request>(json)!;
        }

        // ---------------- Factory metode ----------------

        public static Request Create(string title, string desc)
        {
            var req = new Request();
            req.Operation = RequestType.Create;
            req.Title = title;
            req.Desc = desc;
            return req;
        }

        public static Request Read(int id)
        {
            var req = new Request();
            req.Operation = RequestType.Read;
            req.Id = id;
            return req;
        }

        public static Request Update(int id, string title, string desc)
        {
            var req = new Request();
            req.Operation = RequestType.Update;
            req.Id = id;
            req.Title = title;
            req.Desc = desc;
            return req;
        }

        public static Request Delete(int id)
        {
            var req = new Request();
            req.Operation = RequestType.Delete;
            req.Id = id;
            return req;
        }

        public static Request List()
        {
            var req = new Request();
            req.Operation = RequestType.List;
            return req;
        }
    }
}
