using System.Text.Json.Serialization;

namespace camunda8_dotnet_poc_examples.Constants.Catering;
public record CateringModifyVariables
{
    [JsonPropertyName("agreementId")]
    public long AgreementId { get; set; }

    [JsonPropertyName("modifyRequestId")]
    public long ModifyRequestId { get; set; }
    
    [JsonPropertyName("isApproved")]
    public bool? IsApproved { get; set; } = false;

    [JsonPropertyName("agreementStartDate")]
    public DateTime AgreementStartDate { get; set; }
}

public static class ModifyCateringAgreementConstants
{
    public const string BPMN_PROCESS_ID = "cat_agreement_modify_request";

    public static class Workers
    {
        public const string Approve = "CAT_MODIFY_APPROVE";
        public const string Reject = "CAT_MODIFY_REJECT";
        public const string Withdraw = "CAT_MODIFY_WITHDRAW";
        public const string Timeout = "CAT_MODIFY_TIMEOUT";
    }

    public static class Messages
    {
        public const string PROVIDER_RESPONSE = "msg_catering_modify_response";
        public const string WITHDRAW = "msg_catering_modify_withdraw";
    }
}
