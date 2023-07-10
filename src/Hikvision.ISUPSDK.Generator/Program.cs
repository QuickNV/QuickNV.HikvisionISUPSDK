﻿using Hikvision.ISUPSDK.Generator;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

//源文件编码
var sourceFileEncoding = CodePagesEncodingProvider.Instance.GetEncoding("gb2312");

//方法开始前缀
var methodStartLinePrefix = "[DllImport";
//方法结束前缀
var methodEndLinePrefix = "public static extern";
//错误开始内容
var errorStartContent = "全局错误码 begin";
//错误结束内容
var errorEndContent = "全局错误码 end";

//方法正则表达式
var methodRegex = new Regex(@"public static (?<ReturnType>.*?) (?<MethodName>.*?)\((?<Parameters>.*?)\)");
//错误正则表达式
var errorRegex = new Regex("public const int (?<Name>.*?) = (?<Value>.*?);[ ]*//(?<Description>.*)");

var sbDefines = new StringBuilder();
var sbMethods = new StringBuilder();
var errorList = new List<ErrorInfo>();
var methodList = new List<string>();

var linesHandler = new Action<string[]>(lines =>
{
    var methodStartLineIndex = -1;
    var errorStartLineIndex = -1;

    for (var i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        var lineTrimed = line.Trim();
        if (lineTrimed.StartsWith("namespace "))
            line = "namespace Hikvision.ISUPSDK";
        else if (lineTrimed.StartsWith("public class "))
            line = "    public class Defines";
        else if (lineTrimed.StartsWith("public CHCNetSDK()"))
            line = "        public Defines()";
        //如果是方法开始
        if (lineTrimed.StartsWith(methodStartLinePrefix))
        {
            methodStartLineIndex = i;
            line = "        [DllImport(DllPath)]";
        }
        line = line.Replace("HCEHomePublic.", string.Empty);
        line = line.Replace("HCEHomeCMS.", string.Empty);
        if (methodStartLineIndex >= 0)
            sbMethods.AppendLine(line);
        else
            sbDefines.AppendLine(line);

        //如果是方法结束
        if (lineTrimed.StartsWith(methodEndLinePrefix))
        {
            methodStartLineIndex = -1;
            methodList.Add(line);
        }

        //如果是错误结束
        if (lineTrimed.Contains(errorEndContent))
        {
            errorStartLineIndex = -1;
        }
        if (errorStartLineIndex >= 0)
        {
            var match = errorRegex.Match(lineTrimed);
            if (match.Success)
            {
                var name = match.Groups["Name"].Value;
                var value = match.Groups["Value"].Value;
                var description = match.Groups["Description"].Value;
                errorList.Add(new ErrorInfo()
                {
                    Name = name,
                    Value = value,
                    Description = description
                });
            }
        }
        //如果是错误开始
        if (lineTrimed.Contains(errorStartContent))
        {
            errorStartLineIndex = i;
        }
    }
});

//linesHandler(File.ReadAllLines("HCEHomePublic.cs", sourceFileEncoding));
//linesHandler(File.ReadAllLines("HCEHomeCMS.cs", sourceFileEncoding));
linesHandler(File.ReadAllLines("HCEHomeStream.cs", sourceFileEncoding));
//linesHandler(File.ReadAllLines("GlobalDefinition.cs", sourceFileEncoding));

var outFolder = "../../../../Hikvision.ISUPSDK";
Environment.CurrentDirectory = outFolder;
//Defines.cs
File.WriteAllText("Defines_Auto.cs", sbDefines.ToString());
//Methods_Win.cs
{
    var sb = new StringBuilder();
    sb.AppendLine(@"using System;
using System.Runtime.InteropServices;
using static Hikvision.ISUPSDK.Defines;

namespace Hikvision.ISUPSDK
{
    internal class Methods_Win
    {
        public const string DllPath = ""HCISUPCMS.dll"";
");
    sb.Append(sbMethods);
    sb.AppendLine("    }");
    sb.AppendLine("}");
    File.WriteAllText("Methods_Win_Auto.cs", sb.ToString());
}
//Methods_Linux.cs
{
    var sb = new StringBuilder();
    sb.AppendLine(@"using System;
using System.Runtime.InteropServices;
using static Hikvision.ISUPSDK.Defines;

namespace Hikvision.ISUPSDK
{
    internal class Methods_Linux
    {
        public const string DllPath = ""hcisupcms.so"";
");
    sb.Append(sbMethods);
    sb.AppendLine("    }");
    sb.AppendLine("}");
    File.WriteAllText("Methods_Linux_Auto.cs", sb.ToString());
}
//Method.cs
{
    var sb = new StringBuilder();
    sb.AppendLine(@"using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static Hikvision.ISUPSDK.Defines;

namespace Hikvision.ISUPSDK
{
    public partial class Methods
    {
");

    foreach (var method in methodList)
    {
        var line = method
            .Replace(" extern ", " ")
            .Replace("CHCNetSDK.", string.Empty)
            .Replace(";", string.Empty);
        sb.AppendLine(line);
        sb.AppendLine("        {");

        var match = methodRegex.Match(line);
        var methodReturnType = match.Groups["ReturnType"].Value;
        var returnStr = methodReturnType == "void" ? string.Empty : "return ";
        var methodMethodName = match.Groups["MethodName"].Value;
        var methodParameters = match.Groups["Parameters"].Value;
        var methodParameterList = new List<string>();
        foreach (var str1 in methodParameters.Split(','))
        {
            var strs = str1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length < 2)
                continue;
            else if (strs.Length == 2)
                methodParameterList.Add(strs[1]);
            else
                methodParameterList.Add(strs[0] + " " + strs[2]);
        }
        sb.AppendLine("            if (IsWindows)");
        sb.AppendLine($"                {returnStr}Methods_Win.{methodMethodName}({string.Join(", ", methodParameterList)});");
        sb.AppendLine("            else");
        sb.AppendLine($"                {returnStr}Methods_Linux.{methodMethodName}({string.Join(", ", methodParameterList)});");
        sb.AppendLine("        }");
    }
    sb.AppendLine("    }");
    sb.AppendLine("}");
    File.WriteAllText("Methods_Auto.cs", sb.ToString());
}
//Errors.cs
{
    var sb = new StringBuilder();
    sb.AppendLine(@"using System.ComponentModel;

namespace Hikvision.ISUPSDK
{
    public enum Errors
    {");

    for (var i = 0; i < errorList.Count; i++)
    {
        var error = errorList[i];
        sb.AppendLine($"        [Description(\"{error.Description}\")]");
        sb.Append($"        {error.Name} = {error.Value}");
        if (i == errorList.Count - 1)
            sb.AppendLine();
        else
            sb.AppendLine(",");
    }
    sb.AppendLine("    }");
    sb.AppendLine("}");
    File.WriteAllText("Errors_Auto.cs", sb.ToString());
}
Console.WriteLine("Done");
