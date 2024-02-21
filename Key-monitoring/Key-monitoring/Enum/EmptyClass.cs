using System.Text.Json.Serialization;

namespace Key_monitoring.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleEnum
	{
		Student,
		Teacher
	}
}