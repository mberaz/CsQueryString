using QueryStringHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryStringMain
{
    class Program
    {
        //https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package
        //https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package
        //https://github.com/NuGetPackageExplorer/NuGetPackageExplorer/blob/master/README.md
        static void Main(string[] args)
        {

            var url = "htp://www.test.com?firstname=name&lastname=last&lastname=name";

            var concat = QueryString.ParseQueryString(url, DuplicateKeyMode.Concat);
            var replace = QueryString.ParseQueryString(url, DuplicateKeyMode.Replase);
            var keep = QueryString.ParseQueryString(url, DuplicateKeyMode.KeepOld);

            var queryStringWithAQuestionMark = QueryString.CreateQueryString(concat);
            var queryStringWithOutAQuestionMark = QueryString.CreateQueryString(concat,false);

            var qs = new QueryString(url, DuplicateKeyMode.Concat);

            var returnUrl = qs.RetunrQueryString();

            var value = QueryString.GetQueryStringValue(url, "firstname");


            qs.InsertOrConcat("con", "first").InsertOrConcat("con", "last");

            qs.InsertOrReplase("replase", "first").InsertOrReplase("replase", "last");

            qs.InsertOrKeepOld("keep", "first").InsertOrKeepOld("keep", "last");
            qs.DeleteKey("firstname").RenameKey("lastname", "last");

            qs.InsertOrReplase("up", "down").UpdateValue("up", "up", DuplicateKeyMode.Replase);


            returnUrl = qs.RetunrQueryString();

            var testReturn = qs.RetunrQueryString((pair) =>
             {
                 return $"{pair.Key}={ ReverseString (pair.Value)}";
             });

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
