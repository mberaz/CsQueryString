using QueryStringHandler;
using System;

namespace QueryStringMain
{
    class Program
    {
        static void Main(string[] args)
        {

            var url = "htp://www.test.com?firstname=name&lastname=last&lastname=name";

            var concat = QueryString.ParseQueryString(url, DuplicateKeyMode.Concat);
            var replace = QueryString.ParseQueryString(url, DuplicateKeyMode.Replase);
            var keep = QueryString.ParseQueryString(url, DuplicateKeyMode.KeepOld);

            var queryStringWithAQuestionMark = QueryString.CreateQueryString(concat);
            var queryStringWithOutAQuestionMark = QueryString.CreateQueryString(concat,false);

            var qs = new QueryString(url, DuplicateKeyMode.Concat);

            var returnUrl = qs.ReturnQueryString();

            var value = QueryString.GetQueryStringValue(url, "firstname");
            var firstName = qs.GetValue("firstname");

            qs.InsertOrConcat("con", "first").InsertOrConcat("con", "last");

            qs.InsertOrReplace("replace", "first").InsertOrReplace("replace", "last");

            qs.InsertOrKeepOld("keep", "first").InsertOrKeepOld("keep", "last");
            qs.DeleteKey("firstname").RenameKey("lastname", "last");

            qs.InsertOrReplace("up", "down").UpdateValue("up", "up", DuplicateKeyMode.Replase);


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
