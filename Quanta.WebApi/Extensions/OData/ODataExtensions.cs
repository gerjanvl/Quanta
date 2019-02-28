using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.OData.UriParser;
using Newtonsoft.Json.Linq;

namespace Quanta.WebApi.Extensions.OData
{
    public static class ODataExtensions
    {
        public static Guid GetUserAdId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.Claims.FirstOrDefault(o => o.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value);
        }

        public static async Task<KeyValuePair<string, T>> GetODataRefIdAsync<T>(this HttpRequest request) where T : IConvertible
        {
            using (var sr = new StreamReader(request.Body, Encoding.UTF8))
            {
                var requestBody = await sr.ReadToEndAsync();

                var data = JObject.Parse(requestBody);

                var odataId = new Uri(data["@odata.id"].ToString());

                var pathHandler = request.GetPathHandler();

                var serviceRoot = request
                    .GetUrlHelper()
                    .CreateODataLink(request.ODataFeature().RouteName, pathHandler, new List<ODataPathSegment>() { });

                var odataPath = pathHandler.Parse(serviceRoot, odataId.ToString().Replace(serviceRoot, ""), request.GetRequestContainer());

                if (odataPath.PathTemplate == "~/entityset/key")
                {
                    var navigationPropertySegment = (EntitySetSegment)odataPath.Segments.First(o => o is EntitySetSegment);
                    var keySegment = (KeySegment)odataPath.Segments.First(o => o is KeySegment);

                    var key = keySegment.Keys.First();

                    return new KeyValuePair<string, T>(navigationPropertySegment.EntitySet.Name, (T) key.Value);
                }

                throw new Exception("Invalid request");
            }
        }
    }
}
