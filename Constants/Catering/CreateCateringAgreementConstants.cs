using System.Text.Json.Serialization;
namespace camunda8_dotnet_poc_examples.Constants.Catering;

public record CreateCateringAgreementVariables
{
    [JsonPropertyName("agreementId")]
    public long AgreementId { get; set; }

    [JsonPropertyName("createRequestId")]
    public long CreateRequestId { get; set; }

    [JsonPropertyName("agreementStartDate")]
    public DateTime AgreementStartDate { get; set; }

    [JsonPropertyName("isApproved")]
    public bool? IsApproved { get; set; } = false;
}

public static class CreateCateringAgreementConstants
{
    public const string BPMN_PROCESS_ID = "cat_agreement_create_request";

    //Workers
    public const string Approve = "CAT_CREATE_APPROVE";
    public const string Reject = "CAT_CREATE_REJECT";
    public const string Withdraw = "CAT_CREATE_WITHDRAW";
    public const string Timeout = "CAT_CREATE_TIMEOUT";

    //Messages
    public static class Messages
    {
        public const string MSG_CATERING_CREATE_RESPONSE = "msg_catering_create_response";
        public const string MSG_CATERING_WITHDRAW_CREATE_RESPONSE = "msg_catering_withdraw_create_response";
    }
}

