using RestFul.Http;

namespace RestFul.Result
{
    public interface IResult
    {
        /// <summary>
        /// Handles the result based on the given context.
        /// </summary>
        /// <returns></returns>
        void Execute(HttpContext context);
    }
}
