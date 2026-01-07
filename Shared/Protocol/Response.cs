using Shared.Models;
using System.Text.Json;

namespace Shared.Protocol
{
    public class Response
    {
        public string Status { get; set; } = "ok";
        public string Message { get; set; } = "";

        // Jedan Item ili lista Item-a
        public Character? Item { get; set; } = null;
        public List<Character>? Items { get; set; } = null;

        // Serializacija u JSON
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // Deserializacija iz JSON
        public static Response FromJson(string json)
        {
            return JsonSerializer.Deserialize<Response>(json)!;
        }

        // ---------------- Factory metode ----------------

        // READ, CREATE, UPDATE
        public static Response Ok(string message, Character item)
        {
            var resp = new Response();
            resp.Status = "ok";
            resp.Message = message;
            resp.Item = item;
            return resp;
        }

        // LIST
        public static Response Ok(string message, List<Character> items)
        {
            var resp = new Response();
            resp.Status = "ok";
            resp.Message = message;
            resp.Items = items;
            return resp;
        }

        // DELETE
        public static Response Ok(string message)
        {
            var resp = new Response();
            resp.Status = "ok";
            resp.Message = message;
            return resp;
        }


        public static Response Error(string message)
        {
            var resp = new Response();
            resp.Status = "error";
            resp.Message = message;
            return resp;
        }
    }
}
