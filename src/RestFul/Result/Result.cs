using RestFul.Http;

namespace RestFul.Result
{
    /// <summary>
    /// Base Class of <see cref="IResult"/>
    /// </summary>
    public abstract class Result : IResult
    {
        public virtual void Execute(HttpContext context) { }
    }
}
