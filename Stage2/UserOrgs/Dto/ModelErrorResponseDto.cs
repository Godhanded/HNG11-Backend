namespace UserOrgs.Dto
{
        public record ModelError(string field, string message);

        public record ModelErrorResponseDto(List<ModelError> errors);

}
