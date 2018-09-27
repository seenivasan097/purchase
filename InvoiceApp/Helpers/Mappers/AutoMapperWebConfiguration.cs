using AutoMapper;

namespace InvoiceApp.Helpers.Mappers
{
    /// <summary>
    /// this class used for mapping the fields in Business entity to Datacontract feild
    /// </summary>
    public class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
               
            });
        }
    }
}
