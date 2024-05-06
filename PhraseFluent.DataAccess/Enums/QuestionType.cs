﻿using System.Text.Json.Serialization;

namespace PhraseFluent.DataAccess.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestionType
{
    None = 0,
    Text = 1,
    TestOneAnswer = 2,
    TestManyAnswers = 3
}