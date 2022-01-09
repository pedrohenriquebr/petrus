# Petrus

## Description

HttpClient Wrapper for C#

## Usage


Making a GET request

```csharp
var response =Petrus.Get("https://api.publicapis.org/entries");

```

Making a GET request with query string

```csharp
var result = await Petrus.Get("https://google.com/search", new PetrusOptions
{
    Params = new List<(string, string)>()
    {
        ("q","oops")
    }
});
```

Making a POST request with body 

```csharp
var response = await Petrus.Post("https://myapi.com", new
{
    param = "Value",
    param2 = "Value2"
});

```

Get the response data


```csharp
PetrusResult result = await Petrus.Get("https://pokeapi.co/api/v2/pokemon/pikachu");

Console.WriteLine(result.Data.name); // Shows pikachu

```
> The Data Property is dynamic


### Petrus Instance

```csharp
var api = Petrus.Create(new ()
{
    BaseURL = "https://pokeapi.co/api/v2/"
});
var result  = await api.Get("pokemon/pikachu");

```




