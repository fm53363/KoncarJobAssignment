using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Protocol
{
    public class Request
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestType Operation { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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

        public static Request GetById(int id)
        {
            var req = new Request();
            req.Operation = RequestType.GetById;
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

        public static Request GetAll()
        {
            var req = new Request();
            req.Operation = RequestType.GetAll;
            return req;
        }


        public override string ToString()
        {
            switch (Operation)
            {
                case RequestType.GetAll:
                    return "GetAll()";
                case RequestType.GetById:
                    return $"GetById({Id})";
                case RequestType.Create:
                    return $"Create('{Title}', '{Desc}')";
                case RequestType.Update:
                    return $"Update({Id}, '{Title}', '{Desc}')";
                case RequestType.Delete:
                    return $"Delete({Id})";
                default:
                    return "Unknown";
            }
        }
    }
}
