﻿namespace UserOrgs.Dto
{
    public class ResponseDto
    {
        public record SuccessResponse(string message, dynamic data, string status = "success");

        public record FailiureResponse(string message,int statusCode, string status = "Bad request");

    }
}