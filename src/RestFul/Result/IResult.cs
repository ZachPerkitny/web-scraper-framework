using RestFul.Http;
using RestFul.Serializer;

namespace RestFul.Result
{
    public interface IResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void Execute(IHttpContext context, ISerializer serializer);
    }
}
