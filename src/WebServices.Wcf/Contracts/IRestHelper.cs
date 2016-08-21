using System.ServiceModel.Channels;

namespace WebServices.Wcf.Contracts
{
    public interface IRestHelper
    {
        Message Ok(object value);

        Message NoContent();

        Message BadRequest();

        Message NotFound();
    }
}