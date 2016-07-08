# Overview
A couple additions to configuration in .NET.
Currently contains:

* AddJsonFileFromAbsolutePath : Adds back the ability to specify an absolute path to the configuration file
* AddJsonFileFromEnvironmentVariable : Builds on the above but takes a key to an environment variable that has a value for the absolute path to the configuration file

Check the official documentation on usage: https://docs.asp.net/en/latest/fundamentals/configuration.html

## Example Usage

## AddJsonFileFromAbsolutePath

```csharp
string path = "c:\\dev\\myproject\\appsettings.json";
IConfigurationBuilder builder = new ConfigurationBuilder();

builder.AddJsonFileFromAbsolutePath(path);
config = sut.Build();
```

Supports the usual *optional* and *reloadOnChange*.

## AddJsonFileFromEnvironmentVariable

Assuming we have an environment variable set of:

    myproject : c:\\dev\\myproject\\appsettings.json

```csharp
string key = "myproject";
IConfigurationBuilder builder = new ConfigurationBuilder();

builder.AddJsonFileFromEnvironmentVariable(key);
config = sut.Build();
```

Supports the usual *optional* and *reloadOnChange*.