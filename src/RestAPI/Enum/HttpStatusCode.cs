﻿namespace RestAPI.Enum
{
    public enum HttpStatusCode
    {
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
        IMUsed = 226
    }
}
