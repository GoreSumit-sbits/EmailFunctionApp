using System.Collections.Generic;
using Newtonsoft.Json;

public class PayloadData
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("place")]
    public string Place { get; set; }

    [JsonProperty("invisibility")]
    public List<InvisibilityType> Invisibility { get; set; }
}

public class Recipient
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("invisibility")]
    public List<InvisibilityType> Invisibility { get; set; }
}

public class InvisibilityType
{
    [JsonProperty("type1")]
    public bool Type1 { get; set; }
}

public class Payload
{
    [JsonProperty("data")]
    public PayloadData Data { get; set; }

    [JsonProperty("recipient")]
    public List<Recipient> Recipients { get; set; }
}
