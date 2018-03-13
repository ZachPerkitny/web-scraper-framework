namespace RestFul.Enum
{
    public enum HttpStatusCode
    {
        /// <summary>
        /// For internal usage.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The initial part of a request has een received and has not yet
        /// been rejected by the server.
        /// </summary>
        Continue = 100,

        /// <summary>
        /// The server understands and is willing to comply with the client's request,
        /// via the upgrade header field.
        /// </summary>
        SwitchingProtocols = 101,

        /// <summary>
        /// The server has accepted the request, but has not yet been processed it.
        /// </summary>
        Processing = 102,

        /// <summary>
        /// 
        /// </summary>
        OK = 200,

        /// <summary>
        /// 
        /// </summary>
        Created = 201,

        /// <summary>
        /// 
        /// </summary>
        Accepted = 202,

        /// <summary>
        /// 
        /// </summary>
        NonAuthoritativeInformation = 203,

        /// <summary>
        /// 
        /// </summary>
        NoContent = 204,

        /// <summary>
        /// 
        /// </summary>
        ResetContent = 205,

        /// <summary>
        /// 
        /// </summary>
        PartialContent = 206,

        /// <summary>
        /// 
        /// </summary>
        MultiStatus = 207,

        /// <summary>
        /// 
        /// </summary>
        AlreadyReported = 208,

        /// <summary>
        /// 
        /// </summary>
        IMUsed = 226,

        /// <summary>
        /// 
        /// </summary>
        MultipleChoices = 300,

        /// <summary>
        /// 
        /// </summary>
        MovedPermanently = 301,

        /// <summary>
        /// 
        /// </summary>
        Found = 302,

        /// <summary>
        /// 
        /// </summary>
        SeeOther = 303,

        /// <summary>
        /// 
        /// </summary>
        NotModified = 304,

        /// <summary>
        /// 
        /// </summary>
        UseProxy = 305,

        /// <summary>
        /// 
        /// </summary>
        TemporaryRedirect = 307,

        /// <summary>
        /// 
        /// </summary>
        PermanentRedirect = 308,

        /// <summary>
        /// 
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 
        /// </summary>
        PaymentRequired = 402,

        /// <summary>
        /// 
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// 
        /// </summary>
        NotAcceptable = 406,

        /// <summary>
        /// 
        /// </summary>
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// 
        /// </summary>
        RequestTimeout = 408,

        /// <summary>
        /// 
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// 
        /// </summary>
        Gone = 410,

        /// <summary>
        /// 
        /// </summary>
        LengthRequired = 411,

        /// <summary>
        /// 
        /// </summary>
        PreconditionFailed = 412,

        /// <summary>
        /// 
        /// </summary>
        PayloadTooLarge = 413,

        /// <summary>
        /// 
        /// </summary>
        RequestUriTooLong = 414,

        /// <summary>
        /// 
        /// </summary>
        UnsupportedMediaType = 415,

        /// <summary>
        /// 
        /// </summary>
        RequestedRangeNotSatisfiable = 416,

        /// <summary>
        /// 
        /// </summary>
        ExpectationFailed = 417,

        /// <summary>
        /// 
        /// </summary>
        ImATeapot = 418,

        /// <summary>
        /// 
        /// </summary>
        MisdirectedRequest = 421,

        /// <summary>
        /// 
        /// </summary>
        UnprocessableEntity = 422,

        /// <summary>
        /// 
        /// </summary>
        Locked = 423,

        /// <summary>
        /// 
        /// </summary>
        FailedDependency = 424,

        /// <summary>
        /// 
        /// </summary>
        UpgradeRequired = 426,

        /// <summary>
        /// 
        /// </summary>
        PreconditionRequired = 428,

        /// <summary>
        /// 
        /// </summary>
        TooManyRequests = 429,

        /// <summary>
        /// 
        /// </summary>
        RequestHeaderFieldsTooLarge = 431,

        /// <summary>
        /// 
        /// </summary>
        ConnectionClosedWithoutResponse = 444,

        /// <summary>
        /// 
        /// </summary>
        UnavailableForLegalReasons = 451,

        /// <summary>
        /// 
        /// </summary>
        ClientClosedRequest = 499,


        /// <summary>
        /// 
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// 
        /// </summary>
        NotImplemented = 501,

        /// <summary>
        /// 
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// 
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// 
        /// </summary>
        GatewayTimeout = 504,

        /// <summary>
        /// 
        /// </summary>
        HttpVersionNotSupported = 505,

        /// <summary>
        /// 
        /// </summary>
        VariantAlsoNegotiates = 506,

        /// <summary>
        /// 
        /// </summary>
        InsufficientStorage = 507,

        /// <summary>
        /// 
        /// </summary>
        LoopDetected = 508,

        /// <summary>
        /// 
        /// </summary>
        NotExtended = 510,

        /// <summary>
        /// 
        /// </summary>
        NetworkAuthenticationRequired = 511,

        /// <summary>
        /// 
        /// </summary>
        NetworkConnectTimeoutError = 599
    }
}
