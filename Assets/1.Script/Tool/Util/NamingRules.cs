using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingRules 
{
    //아스키 코드로 범위 지정
    //첫단어 한글과 영어인지 확인
    public static bool IsFirstTextisNum(string name)
    {
        int firstLetter = (int)char.Parse(name.Substring(0,1));
        if (
            //영단어 범위
            (firstLetter >= 65 && firstLetter <= 90) ||
            (firstLetter >= 97 && firstLetter <= 122) ||
            //한글범위
            (firstLetter >= 44032 && firstLetter <= 55203) 
            )
        {
            return false;
        }
        Debug.Log("첫글자가 한글과 영어가 아닙니다.");
        return true;
    }
    // 한글과 영어와 숫자로만 이루어져있는지 확인
    public static bool IsOnlyNumKorEng(string name)
    {
        int firstLetter;
        for (int i =0; i<name.Length; i++)
        {
            firstLetter = (int)char.Parse(name.Substring(i, 1));
            if (
                    //영단어 범위
                    !(
                    (firstLetter >= 65 && firstLetter <= 90) ||
                    (firstLetter >= 97 && firstLetter <= 122) ||
                    //한글범위
                    (firstLetter >= 44032 && firstLetter <= 55203) ||
                    //숫자 범위
                    (firstLetter >= 48 && firstLetter <= 57) 
                    )
                )
            {
                Debug.Log("한글 영어 숫자가 아닌것이 있습니다.");
                return false;
            }
        } 
        return true;
    }
    
    public static bool IsSpecialSymbol(string name)
    {
        if (
            name.Contains("`") || name.Contains("~") || name.Contains("!") ||
            name.Contains("@") || name.Contains("#") || name.Contains("$") ||
            name.Contains("%") || name.Contains("^") || name.Contains("&") ||
            name.Contains("*") || name.Contains("(") || name.Contains(")") ||
            name.Contains("_") || name.Contains("+") || name.Contains("-") ||
            name.Contains("=") || name.Contains("{") || name.Contains("}") ||
            name.Contains("|") || name.Contains("[") || name.Contains("]") ||
            name.Contains("\\") || name.Contains(":") || name.Contains("\"") ||
            name.Contains(";") || name.Contains("'") || name.Contains("<") ||
            name.Contains(">") || name.Contains("?") || name.Contains(",") ||
            name.Contains(".") || name.Contains("/") 
            )
        {
            Debug.Log("특수기호가 존재합니다.");
            return true;
        }
        return false;
    }
    public static bool IsNamingBlank(string name)
    {
        if(name.Contains(" "))
        {
            Debug.Log("공백이 존재합니다.");
            return true;
        }
        return false;
    }
    public static bool IsReservedWord(string name)
    {
        //예약어참조 사이트
        //https://learn.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/
        if (name.Equals("abstract") || name.Equals("event") ||
            name.Equals("namespace") || name.Equals("static") || name.Equals("as") ||
            name.Equals("explicit") || name.Equals("new") || name.Equals("string") ||
            name.Equals("base") || name.Equals("extern") || name.Equals("null") ||
            name.Equals("struct") || name.Equals("bool") || name.Equals("false") ||
            name.Equals("object") || name.Equals("switch") || name.Equals("break") ||
            name.Equals("finally") || name.Equals("operator") || name.Equals("this") ||
            name.Equals("byte") || name.Equals("fixed") || name.Equals("out") ||
            name.Equals("throw") || name.Equals("case") || name.Equals("float") ||
            name.Equals("override") || name.Equals("true") || name.Equals("catch") ||
            name.Equals("for") || name.Equals("params") || name.Equals("try") ||
            name.Equals("char") || name.Equals("foreach") || name.Equals("private") ||
            name.Equals("typeof") || name.Equals("checked") || name.Equals("goto") ||
            name.Equals("protected") || name.Equals("unit") || name.Equals("class") ||
            name.Equals("if") || name.Equals("public") || name.Equals("ulong") ||
            name.Equals("const") || name.Equals("implicit") || name.Equals("readonly") ||
            name.Equals("unchecked") || name.Equals("continue") || name.Equals("in") ||
            name.Equals("ref") || name.Equals("unsafe") || name.Equals("decimal") ||
            name.Equals("int") || name.Equals("return") || name.Equals("ushort") ||
            name.Equals("default") || name.Equals("interface") || name.Equals("sbyte") ||
            name.Equals("using") || name.Equals("delegate") || name.Equals("internal") ||
            name.Equals("sealed") || name.Equals("virtual") || name.Equals("do") ||
            name.Equals("is") || name.Equals("short") || name.Equals("void") ||
            name.Equals("double") || name.Equals("lock") || name.Equals("sizeof") ||
            name.Equals("volatile") || name.Equals("else") || name.Equals("long") ||
            name.Equals("stackalloc") || name.Equals("while") || name.Equals("enum") ||
            //상황별 키워드
            name.Equals("add") || name.Equals("get") || name.Equals("notnull") ||
            name.Equals("set") || name.Equals("and") || name.Equals("global") ||
            name.Equals("nuint") || name.Equals("unmanaged") || name.Equals("alias") ||
            name.Equals("group") || name.Equals("on") || name.Equals("ascending") ||
            name.Equals("init") || name.Equals("or") || name.Equals("args") ||
            name.Equals("into") || name.Equals("orderby") || name.Equals("async") ||
            name.Equals("join") || name.Equals("partial") || name.Equals("value") ||
            name.Equals("await") || name.Equals("let") || name.Equals("partial") ||
            name.Equals("var") || name.Equals("by") || name.Equals("managed") ||
            name.Equals("record") || name.Equals("when") || name.Equals("descending") ||
            name.Equals("remove") || name.Equals("where") || name.Equals("dynamic") ||
            name.Equals("nameof") || name.Equals("select") || name.Equals("equals") ||
            name.Equals("nint") || name.Equals("from") || name.Equals("not") ||
            name.Equals("with") || name.Equals("yield") )
        {
            Debug.Log("예약어가 존재합니다.");
            return true;
        }
        return false;
    }
    //같은 단어입니다.

}
