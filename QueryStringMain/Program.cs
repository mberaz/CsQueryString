using QueryStringHandler;
using System;

namespace QueryStringMain
{
    class Program
    {
        static void Main(string[] args)
        {

            var url = "htp://www.test.com?firstname=name&lastname=last&lastname=name";

            var concat = QueryString.ParseQueryString(url);

            var queryStringWithAQuestionMark = QueryString.CreateQueryString(concat);
            var queryStringWithOutAQuestionMark = QueryString.CreateQueryString(concat,false);

            var qs = new QueryString(url);

            var returnUrl = qs.ReturnQueryString();

            var value = QueryString.GetQueryStringValue(url, "firstname");
            var firstName = qs.GetValue("firstname");

            qs.Insert("replace", "first").Insert("replace", "last");

            qs.DeleteKey("firstname").RenameKey("lastname", "last");

            qs.Insert("up", "down").UpdateValue("up", "up");

            returnUrl = qs.ReturnQueryString();

            var testReturn = qs.ReturnQueryString((pair) => $"{pair.Key}={ ReverseString (pair.Value)}");

            Console.Read();
        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
