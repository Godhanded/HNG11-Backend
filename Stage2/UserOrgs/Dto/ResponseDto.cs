namespace UserOrgs.Dto
{
        public record SuccessResponse<T>(string message, T? data, string status = "success");

        public record FailiureResponse(string message,int statusCode, string status = "Bad request");

        public record AuthResponsedto(string accessToken,UserDataDto user);
}
