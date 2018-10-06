using CarRentalNovility.Entities.Exceptions;
using Newtonsoft.Json;

namespace CarRentalNovility.Web.Infrastructure
{
    public class ErrorDetails
    {
        public ErrorCode StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

}
