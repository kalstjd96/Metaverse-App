using Newtonsoft.Json;
namespace SCM.Service.TaxSquare.Consultation
{
    public class ConnectionRequestDto
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("body")]
        public BodyDto Body { get; set; }
    }

    public class BodyDto
    {
        [JsonProperty("counselor")]
        public CounselorDto Counselor { get; set; }
    }

    public class CounselorDto
    {
        [JsonProperty("acntSn")]
        public int AcntSn { get; set; }

        [JsonProperty("nickNm")]
        public string NickName { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("consultationId")]
        public string ConsultationId { get; set; }

        [JsonProperty("socketId")]
        public string SocketId { get; set; }

        [JsonProperty("prfImgAddr")]
        public string ProfileImageAddress { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("standbyDt")]
        public string StandbyDate { get; set; }
    }

    public class MessageDto
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("from")]
        public string from { get; set; }

        [JsonProperty("to")]
        public string to { get; set; }

        [JsonProperty("body")]
        public MsgBodyDto body { get; set; }
    }

    public class MsgBodyDto
    {
        [JsonProperty("data")]
        public string data { get; set; }
    }

    public class SendMsgDto
    {
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class ResponseDto
    {
        [JsonProperty("acceptYn")]
        public int acceptYn { get; set; }

        [JsonProperty("counselorAcntSn")]
        public int counselorAcntSn { get; set; }
    }
}
