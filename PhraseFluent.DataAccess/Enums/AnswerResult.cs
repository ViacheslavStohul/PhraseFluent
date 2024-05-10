using System.Text.Json.Serialization;

namespace PhraseFluent.DataAccess.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AnswerResult
{
    UnAnswered = 0,
    Correct = 1,
    PartiallyCorrect = 2,
    Wrong = 3
}