﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhraseFluent.API.ExceptionHandling;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;


    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}