using RestFul.Http;

namespace RestFul.Result
{
    public interface IResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void Execute(IHttpContext context);
    }
}
