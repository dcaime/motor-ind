using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace EvaluaIndividuos
{
    public partial class api_fp_credit_assessment_information_Client
    {
        static partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
        {
            settings.ContractResolver = new SafeContractResolver();
        }
    }

    public class SafeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.Required == Required.DisallowNull)
            {
                property.Required = Required.Default;
            }

            return property;
        }
    }
}