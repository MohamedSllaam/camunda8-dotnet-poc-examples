
using System.Text.Json.Serialization;

namespace camunda8_dotnet_poc_examples.Constants.Catering;
public record CateringAgreementCancellationVariables
{
    [JsonPropertyName("agreementId")]
    public long AgreementId { get; set; }

    [JsonPropertyName("cancelRequestId")]
    public long CancelRequestId { get; set; }

    [JsonPropertyName("isApproved")]
    public bool? IsApproved { get; set; } = false;

    [JsonPropertyName("agreementStartDate")]
    public DateTime AgreementStartDate { get; set; }
}

public static class CancelCateringAgreementConstants
{
    public const string BPMN_PROCESS_ID = "cat_agreement_cancel_request";

    public static class Workers
    {
        public const string Approve = "CAT_CANCEL_APPROVE";
        public const string Reject = "CAT_CANCEL_REJECT";
        public const string Withdraw = "CAT_CANCEL_WITHDRAW";
        public const string Timeout = "CAT_CANCEL_TIMEOUT";

    }

    public static class Messages
    {
        public const string MSG_CATERING_CANCEL_RESPONSE = "msg_catering_cancel_response";
        public const string MSG_CATERING_WITHDRAW_CANCEL_RESPONSE = "msg_catering_withdraw_cancel_response";
    }
}
