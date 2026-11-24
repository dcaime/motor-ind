namespace ESB
{
    using System.Collections.Generic;
    using MAPITools;
    using Newtonsoft.Json;

    public sealed class ESB
    {
        private ESB() { }

        public static string GetBaseURL()
        {
            switch (Herramientas.GetEnvironment())
            {
                case Ambiente.PROD:
                    return "https://esb.macro.com.ar";
                case Ambiente.PREP:
                    return "https://esbprep.macro.com.ar";
                default: // Testing y Desarrollo
                    return "https://esbtest.macro.com.ar";
            }
        }

        public static string GetErrorMessage(FaultResponse fault)
        {
            if (fault is null)
            {
                return "";
            }
            return fault.Fault.Detail.Message;
        }
    }

    public class FaultResponse
    {
        [JsonProperty("fault")]
        public Fault Fault { get; set; }
    }

    public class Fault
    {
        [JsonProperty("code")]
        public List<object> Code { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("detail")]
        public Detail Detail { get; set; }
    }

    public class Detail
    {
        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class Source
    {
        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
