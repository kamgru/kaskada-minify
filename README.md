# Kaskada Minify

---
.NET 6 CSS Minifier

## What is this
Kaskada Minifier performs an operation on css known as 'minification'. Basically it takes in your cleanly structured css and produces a compressed version.

The input
```css
body {
    color: red;
    padding: 10px 20px;
    /* border: 1px; */
}
```
becomes:
```css
body{color:red;padding:10px 20px}
```


## Installing

The easiest way is to use a nuget package (hosted on nuget.org [here](https://www.nuget.org/packages/Kaskada.Minify)):
```
dotnet add package kaskada.minify
```

You can also build the code yourself:
1. Make sure you have .NET 6 SDK installed (grab it [here](https://dotnet.microsoft.com/download/dotnet/6.0))
2. Clone the repo: `git clone https://github.com/kamgru/kaskada-minify.git`
3. Optionally test the code: `dotnet test`
4. Build: `dotnet build`

## Usage

First of all, add usings:
```c#
using Kaskada.Minify;
using Kaskada.Minify.Extensions;
```

There are four ways to use the minifier, depending on input and output:

1. Read a file and output to file
```c#
Minifier.FromFile("input.css").ToFile("output.css");
```
2. Read a file and output to string
```c#
string output = Minifier.FromFile("input.css").ToText();
```
3. Minify from string to file
```c#
string input = "body { color: red; } ";
Minifier.FromText(input).ToFile("output.css");
```
4. Minify from string to string
```c#
string input = "body { color: red; } ";
string output = Minifier.FromText(input).ToText();
```

## Current limitations
Kaskada Minifier does not currently replace hashes (i.e. `#ffffff` to `#fff`)