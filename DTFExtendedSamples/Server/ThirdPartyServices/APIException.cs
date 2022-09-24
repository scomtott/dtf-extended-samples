using System;

namespace DTFExtendedSamples.Server.ThirdPartyServices
{
    public class APIException: Exception
    {
        public APIException() : base("Contract was not honoured, the results were not deserialized")
        {
        }
    }
}