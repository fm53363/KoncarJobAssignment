using Server.Services;
using Shared.Models;
using Shared.Protocol;

namespace Server.Networking
{
    internal class RequestHandler(ICharacterService service)
    {
        private readonly ICharacterService _service = service;

        public async Task<Response> Handle(Request request)
        {

            return request.Operation switch
            {
                RequestType.GetAll => await HandleGetAll(request),
                RequestType.GetById => await HandleGetById(request),
                RequestType.Create => await HandleCreate(request),
                RequestType.Update => await HandleUpdate(request),
                RequestType.Delete => await HandleDelete(request),
                _ => Response.Error("Unknown operation"),
            };
        }



        private async Task<Response> HandleGetAll(Request request)
        {
            var collection = await _service.GetAllAsync();
            Response response = Response.Ok("Characters retrieved", collection.ToList());
            return response;
        }


        private async Task<Response> HandleGetById(Request request)
        {
            var character = await _service.GetByIdAsync(request.Id ?? -1);
            if (character != null)
            {
                return Response.Ok("Character found", character);
            }
            return Response.Error("Id not present");

        }


        private async Task<Response> HandleCreate(Request request)
        {

            var c = new Character()
            {
                Title = request.Title ?? "",
                Desc = request.Desc ?? ""
            };
            var character = await _service.CreateAsync(c);

            return Response.Ok("Character created", character);

        }

        private async Task<Response> HandleUpdate(Request request)
        {
            if (!request.Id.HasValue)
                return Response.Error("Id is required");

            var c = new Character()
            {
                Id = request.Id.Value,
                Title = request.Title ?? "",
                Desc = request.Desc ?? ""
            };

            var result = await _service.UpdateAsync(c);
            if (result)
            {
                return Response.Ok("Character updated", c);
            }
            return Response.Error("Id not present");

        }

        private async Task<Response> HandleDelete(Request request)
        {
            if (!request.Id.HasValue)
                return Response.Error("Id is required");

            var result = await _service.DeleteAsync(request.Id.Value);
            if (result)
            {
                return Response.Ok("Character deleted");
            }
            return Response.Error("Id not present");

        }

    }
}
